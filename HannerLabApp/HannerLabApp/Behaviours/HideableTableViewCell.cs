using Xamarin.Forms;

namespace HannerLabApp.Behaviours
{
    /// <summary>
    /// Adds an IsVisible type property to a table view cell
    /// Source: http://www.bryancook.net/2017/09/dynamically-hiding-cells-in-tableview.html
    /// </summary>
    public class HideableTableViewCell
    {
        public static BindableProperty CollapsedProperty =
            BindableProperty.CreateAttached(
                "Collapsed",
                typeof(bool?),
                typeof(HideableTableViewCell),
                default(bool?),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnCollapsedChanged);

        public static bool GetCollapsed(BindableObject target)
        {
            return (bool)target.GetValue(CollapsedProperty);
        }

        public static void SetCollapsed(BindableObject target, bool value)
        {
            target.SetValue(CollapsedProperty, value);
        }

        private static void ToggleViewCellCollapsedState(Cell cell, bool isVisible)
        {
            var table = (TableView)cell.Parent;
            TableSection container = FindContainingTableSection(table, cell);
            if (container != null)
            {
                if (!isVisible)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // remove the cell from the section
                        container.Remove(cell);

                        // remove the section from the table if it's empty
                        if (container.Count == 0)
                        {
                            table.Root.Remove(container);
                        }
                    });
                }
            }
        }

        private static TableSection FindContainingTableSection(TableView table, Cell cell)
        {
            foreach (var section in table.Root)
            {
                foreach (var child in section)
                {
                    if (child == cell)
                    {
                        return section;
                    }
                }
            }

            return null;
        }

        private static void OnCollapsedChanged(BindableObject sender, object oldValue, object newValue)
        {
            var view = sender as Cell;
            bool isVisible = (bool)newValue;
            if (view != null)
            {
                // the parent isn't available until the page has loaded.
                if (view.Parent == null)
                {
                    view.Appearing += (o, e) =>
                    {
                        ToggleViewCellCollapsedState(view, isVisible);
                    };
                }
                else
                {
                    ToggleViewCellCollapsedState(view, isVisible);
                }
            }
        }
    }
}
