using System;
using System.Collections.Generic;
using LargestGap.Interfaces;

namespace LargestGap.Sort
{
    public class Sorter : ISorter
    {
        private readonly LayoutConfig _layout;
        private readonly ILayoutProvider _layoutProvider;

        public Sorter(LayoutConfig layout, ILayoutProvider layoutProvider)
        {
            _layout = layout;
            _layoutProvider = layoutProvider;
        }

        public List<Item> Sort(List<Item> data)
        {
            ISort sort;
            if (_layout.ClockWise)
            {
                if (_layout.StartFromBottomOfRoute)
                    sort = new SortClockWiseBottomDepot(_layout, _layoutProvider);
                else
                    sort = new SortClockwiseTopDepot(_layout, _layoutProvider);
            }
            else
            {
                if (_layout.StartFromBottomOfRoute)
                    sort = new SortCoClockWiseBottomDepot(_layout, _layoutProvider);
                else
                    sort = new SortCoClockWiseTopDepot(_layout, _layoutProvider);
            }
            if (sort == null)
                throw new Exception("Not Implemented");
            return sort.Sorting(data);
        }
    }
}