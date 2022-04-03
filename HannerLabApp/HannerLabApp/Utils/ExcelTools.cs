using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using HannerLabApp.Extensions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HannerLabApp.Utils
{
    /// <summary>
    /// Helper tools to work with excel files
    /// </summary>
    public static class ExcelTools
    {
        /// <summary>
        /// Converts a list of objects into a data table based on its properties. Column names can optionally be set using the ExcelColumn property attribute/decorator. Sheet name can be set using the attribute ExcelSheet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The list of objects</param>
        /// <param name="name">Specify the name</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> items, string name = "")
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                var c = prop.Name.ToString();

                // If the property is decorated with "ExcelColumn" then use that instead of the property name.
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ExcelColumn authAttr = attr as ExcelColumn;
                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        string auth = authAttr.Name;

                        if (!string.IsNullOrEmpty(propName) && !string.IsNullOrEmpty(auth))
                            c = auth;
                    }
                }

                tb.Columns.Add(c, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            // Set Table name to provided value or attribute
            if (!string.IsNullOrEmpty(name))
            {
                tb.TableName = name;
            }
            else
            {
                MemberInfo info = typeof(T);
                object[] attrs = info.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    ExcelSheet authAttr = attr as ExcelSheet;
                    if (authAttr != null)
                    {
                        string auth = authAttr.Name;

                        if (!string.IsNullOrEmpty(auth))
                            tb.TableName = auth;
                    }
                }
            }

            return tb;
        }

        /// <summary>
        /// Converts a list of data tables into an excel file. The data table names are used as sheet names. Return file type is base64 string.
        /// </summary>
        /// <param name="sheets"></param>
        /// <returns></returns>
        public static string GenerateExcelFileFromDataTables(List<DataTable> sheets)
        {
            var file64 = string.Empty;

            // Build Workbook
            using (var ms = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();

                foreach (var dt in sheets)
                {
                    ISheet excelSheet = workbook.CreateSheet(dt.TableName);

                    List<string> columns = new List<string>();
                    IRow row = excelSheet.CreateRow(0);
                    int columnIndex = 0;

                    foreach (System.Data.DataColumn column in dt.Columns)
                    {
                        columns.Add(column.ColumnName);
                        row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                        columnIndex++;
                    }

                    int rowIndex = 1;
                    foreach (DataRow dsrow in dt.Rows)
                    {
                        row = excelSheet.CreateRow(rowIndex);
                        int cellIndex = 0;
                        foreach (String col in columns)
                        {
                            var obj = dsrow[col];

                            row.CreateCell(cellIndex).SetCellValue(obj.ToString());
                            cellIndex++;
                        }
                        rowIndex++;
                    }
                }


                workbook.Write(ms);

                file64 = Convert.ToBase64String(ms.ToArray());
            }

            return file64;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
    }
}
