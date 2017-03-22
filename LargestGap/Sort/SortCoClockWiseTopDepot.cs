using System;
using System.Linq;

namespace LargestGap.Sort
{
    public class SortCoClockWiseTopDepot : SortCoClockWise
    {
        public SortCoClockWiseTopDepot(LayoutConfig layout, ILayoutProvider layoutProvider) : base(layout, layoutProvider)
        {
            Console.WriteLine("SortCoClockWiseTopDepot");
        }
        public override void SortForCustomDepot()
        {
            Item startItem;
            if (NeedReformatForNewDepotLocation(out startItem))
            {
                UpdateSortListForNewDepotLocation(startItem);
            }
        }


        public bool NeedReformatForNewDepotLocation(out Item item)
        {
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            item = null;
            if ((Layout.StartRoute <= minRoute.Index && Layout.StartFromBottomOfRoute) ||
                (Layout.StartRoute > maxRoute.Index && Layout.StartFromBottomOfRoute))
                return false;
            // get start item of layout
            var startItem = GetStartingItem();
            if (startItem == null)
                return false;
            item = startItem;
            return true;
        }

        public void UpdateSortListForNewDepotLocation(Item startItem)
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
                    e.Aisle >= minRoute.Aisles.Min(f => f.Aisle) &&
                    e.Aisle <= startingRoute.Aisles.Max(f => f.Aisle) &&
                    (e.Bin > Layout.HaftBin || minRoute.Aisles.Any(f => f.Aisle == e.Aisle)));
            return items
                .OrderByDescending(e => LayoutProvider.GetRouteOfAisle(e.Aisle).Index)
                .ThenByDescending(e => e.Bin)
                .ThenByDescending(e => e.Aisle).FirstOrDefault();
        }
    }
}