using System.Collections.Generic;
using System.Linq;
using LargestGap.Interfaces;

namespace LargestGap.Sort
{
    public abstract class SortClockWise : Sort, ISortDirection
    {
        protected SortClockWise(LayoutConfig layout, ILayoutProvider layoutProvider) : base(layout, layoutProvider)
        {
        }
        public override void SortWithNormalDepot()
        {
            // for starting point is route 1
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            // min route
            SortForMinRoute();
            // route between min & max
            for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
            {
                SortForTopRoute(i);
            }
            // for max
            SortForMaxRoute();
            // route between max & min
            for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
            {
                SortForBottomRoute(i);
            }
        }
        public void SortForMaxRoute()
        {
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, maxRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderByDescending(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            Formated.AddRange(itemNeedFormat);
        }

        public void SortForMinRoute()
        {
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, minRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderBy(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            Formated.AddRange(itemNeedFormat);
        }
        public void SortForBottomRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForBottomRoute(i, false);
            Formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForBottomRoute(i, true);
            Formated.AddRange(itemNeedFormat);
        }

        public void SortForTopRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForTopRoute(i, true);
            Formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForTopRoute(i, false);
            Formated.AddRange(itemNeedFormat);
        }
        private List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, index, leftRoute).Where(e => e.Bin > Layout.HaftBin).ToList();
            return LayoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
        private List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, index, leftRoute).Where(e => e.Bin <= Layout.HaftBin).ToList();
            return LayoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
    }
}