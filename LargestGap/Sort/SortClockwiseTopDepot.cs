using System;
using System.Linq;

namespace LargestGap.Sort
{
    public class SortClockwiseTopDepot : SortClockWise
    {
        public SortClockwiseTopDepot(LayoutConfig layout, ILayoutProvider layoutProvider) : base(layout, layoutProvider)
        {
            Console.WriteLine("SortClockwiseTopDepot");
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
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            var items = Data.Where(
                e =>
                    (LayoutProvider.GetRouteOfAisle(e.Aisle).Index > startingRoute.Index) &&
                    LayoutProvider.GetRouteOfAisle(e.Aisle).Index <= maxRoute.Index &&
                    (e.Bin > Layout.HaftBin || maxRoute.Aisles.Any(f => f.Aisle == e.Aisle)));
            return items
                .OrderBy(e => LayoutProvider.GetRouteOfAisle(e.Aisle).Index)
                .ThenByDescending(e => e.Bin)
                .ThenBy(e => e.Aisle).FirstOrDefault();
        }
    }
}