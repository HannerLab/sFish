namespace HannerLabApp.Extensions
{
    public class ExcelColumn : System.Attribute
    {
        private readonly string _name;
        public string Name => _name;

        public ExcelColumn(string name)
        {
            this._name = name;
        }
    }

    public class ExcelSheet : System.Attribute
    {
        private readonly string _name;
        public string Name => _name;

        public ExcelSheet(string name)
        {
            this._name = name;
        }
    }
}
