using System;
using System.Linq;

namespace LargestGap.Sort
{
    public class SortClockWiseBottomDepot : SortClockWise
    {
        public SortClockWiseBottomDepot(LayoutConfig layout, ILayoutProvider layoutProvider) : base(layout, layoutProvider)
        {
            Console.WriteLine("SortClockWiseBottomDepot");
        }

        public override void SortForCustomDepot()
        {
            Item startItem;
            if (NeedReformatForNewDepotLocation(out startItem))
            {
                UpdateSortListForNewDepotLocation(startItem);
            }
        }

        private bool NeedReformatForNewDepotLocation(out Item item)
        {
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            item = null;
            if (Layout.StartRoute < minRoute.Index ||
                Layout.StartRoute > maxRoute.Index ||
                (Layout.StartRoute == minRoute.Index && Layout.StartFromBottomOfRoute)
                )
                return false;
            // get start item of layout
            var startItem = GetStartingItem();
            if (startItem == null)
                return false;
            item = startItem;
            return true;
        }

        private void UpdateSortListForNewDepotLocation(Item startItem)
        {
            // index to start item
            var index = Formated.FindIndex(e => e.Aisle == startItem.Aisle && e.Bin == startItem.Bin);
            // move it to top
            var itemNeedFormat = Formated.GetRange(index, Formated.Count - index);
            foreach (var item in itemNeedFormat)
            {
                Formated.Remove(item);
            }
            Formated.InsertRange(0, itemNeedFormat);
        }

        private Item GetStartingItem()
        {
            var startingRoute = LayoutProvider.GetRoute(Layout.StartRoute);
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var items = Data.Where(
                e =>
                    LayoutProvider.GetRouteOfAisle(e.Aisle).Index > minRoute.Index &&
                    LayoutProvider.GetRouteOfAisle(e.Aisle).Index <= startingRoute.Index &&
                    e.Bin <= Layout.HaftBin);
            return items.
                OrderByDescending(e => LayoutProvider.GetRouteOfAisle(e.Aisle).Index)
                .ThenBy(e => e.Bin)
                .ThenByDescending(e => e.Aisle)
                .FirstOrDefault();
        }
    }
}