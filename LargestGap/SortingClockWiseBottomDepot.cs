using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
    public class SortingClockWiseBottomDepot : ISorting
    {
        private readonly LayoutConfig _layout;
        private readonly ILayoutProvider _layoutProvider;
        private List<Item> _formated;
        private List<Item> _data;
        public SortingClockWiseBottomDepot(LayoutConfig layout, ILayoutProvider layoutProvider)
        {
            _layout = layout;
            _layoutProvider = layoutProvider;
        }

        public List<Item> Sort(List<Item> data)
        {
            _data = data;
            _formated = new List<Item>();
            SortWithNormalDepot();
            SortForCustomDepot();
            return _formated;
        }

        public void SortForCustomDepot()
        {
            Item startItem;
            if (NeedReformatForNewDepotLocation(out startItem))
            {
                UpdateSortListForNewDepotLocation(startItem);
            }
        }

        public void SortWithNormalDepot()
        {
            // for starting point is route 1
            var minRoute = _layoutProvider.GetMinRoute(_data);
            var maxRoute = _layoutProvider.GetMaxRoute(_data);
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
            var maxRoute = _layoutProvider.GetMaxRoute(_data);
            var itemNeedFormat = _layoutProvider.GetItemNeedFormat(_data, maxRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderByDescending(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            _formated.AddRange(itemNeedFormat);
        }

        public void SortForMinRoute()
        {
            var minRoute = _layoutProvider.GetMinRoute(_data);
            var itemNeedFormat = _layoutProvider.GetItemNeedFormat(_data, minRoute.Index);
            itemNeedFormat = itemNeedFormat.OrderBy(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            _formated.AddRange(itemNeedFormat);
        }

        public void SortForBottomRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForBottomRoute(i, false);
            _formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForBottomRoute(i, true);
            _formated.AddRange(itemNeedFormat);
        }

        public void SortForTopRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForTopRoute(i, true);
            _formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForTopRoute(i, false);
            _formated.AddRange(itemNeedFormat);
        }

        private bool NeedReformatForNewDepotLocation(out Item item)
        {
            var minRoute = _layoutProvider.GetMinRoute(_data);
            var maxRoute = _layoutProvider.GetMaxRoute(_data);
            item = null;
            if (_layout.StartRoute < minRoute.Index ||
                _layout.StartRoute > maxRoute.Index ||
                (_layout.StartRoute == minRoute.Index && _layout.StartFromBottomOfRoute)
                )
                return false;
            // default start route (1)
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
            var index = _formated.FindIndex(e => e.Aisle == startItem.Aisle && e.Bin == startItem.Bin);
            // move it to top
            var itemNeedFormat = _formated.GetRange(index, _formated.Count - index);
            foreach (var item in itemNeedFormat)
            {
                _formated.Remove(item);
            }
            _formated.InsertRange(0, itemNeedFormat);
        }

        private Item GetStartingItem()
        {
            return GetStartingItemForBottomOfRoute();
        }

        private Item GetStartingItemForBottomOfRoute()
        {
            var startingRoute = _layoutProvider.GetRoute(_layout.StartRoute);
            var minRoute = _layoutProvider.GetMinRoute(_data);
            var items = _data.Where(
                e =>
                    e.Aisle > minRoute.Aisles.Min(f => f.Aisle) &&
                    e.Aisle <= startingRoute.Aisles.Max(f => f.Aisle) &&
                    e.Bin <= _layout.HaftBin);
            return items.OrderByDescending(e => e.Aisle).ThenBy(e => e.Bin).FirstOrDefault();
        }


        private List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = _layoutProvider.GetItemNeedFormat(_data, index, leftRoute).Where(e => e.Bin > _layout.HaftBin).ToList();
            return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
        private List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = _layoutProvider.GetItemNeedFormat(_data, index, leftRoute).Where(e => e.Bin <= _layout.HaftBin).ToList();
            return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
    }
}