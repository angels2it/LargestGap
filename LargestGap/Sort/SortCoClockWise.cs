using System.Collections.Generic;
using System.Linq;
using LargestGap.Interfaces;

namespace LargestGap.Sort
{
    public abstract class SortCoClockWise : Sort, ISortDirection
    {
        protected SortCoClockWise(LayoutConfig layout, ILayoutProvider layoutProvider) : base(layout, layoutProvider)
        {
        }
        public override void SortWithNormalDepot()
        {
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            // between min & max
            for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
            {
                SortForBottomRoute(i);
            }
            // for max
            SortForMaxRoute();
            // for route between max - min
            for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
            {
                SortForTopRoute(i);
            }
            // for min
            SortForMinRoute();
        }
        public void SortForMaxRoute()
        {
            var maxRoute = LayoutProvider.GetMaxRoute(Data);
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, maxRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderBy(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            Formated.AddRange(itemNeedFormat);
        }

        public void SortForMinRoute()
        {
            var minRoute = LayoutProvider.GetMinRoute(Data);
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, minRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderByDescending(e => e.Bin).ThenByDescending(e => e.Aisle).ToList();
            Formated.AddRange(itemNeedFormat);
        }

        public void SortForBottomRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForBottomRoute(i, true);
            Formated.AddRange(itemNeedFormat);

            //itemNeedFormat = GetItemNeedFormatForBottomRoute(i, Layout.ClockWise);
            //Formated.AddRange(itemNeedFormat);
        }

        public void SortForTopRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForTopRoute(i, false);
            Formated.AddRange(itemNeedFormat);

            //itemNeedFormat = GetItemNeedFormatForTopRoute(i, !Layout.ClockWise);
            //Formated.AddRange(itemNeedFormat);
        }
        private List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, index).Where(e => e.Bin > Layout.HaftBin).ToList();
            return LayoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
        private List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = LayoutProvider.GetItemNeedFormat(Data, index).Where(e => e.Bin <= Layout.HaftBin).ToList();
            return LayoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
    }
}
