using System.Collections.Generic;
using LargestGap.Interfaces;

namespace LargestGap.Sort
{
    public abstract class Sort : ISort
    {
        protected readonly LayoutConfig Layout;
        protected readonly ILayoutProvider LayoutProvider;
        protected List<Item> Formated;
        protected List<Item> Data;

        protected Sort(LayoutConfig layout, ILayoutProvider layoutProvider)
        {
            Layout = layout;
            LayoutProvider = layoutProvider;
        }
        public List<Item> Sorting(List<Item> data)
        {
            Data = data;
            Formated = new List<Item>();
            SortWithNormalDepot();
            SortForCustomDepot();
            return Formated;
        }

        public abstract void SortWithNormalDepot();

        public abstract void SortForCustomDepot();
    }
    //public class Sort : ISort
    //{
    //    private readonly LayoutConfig _layout;
    //    private readonly ILayoutProvider _layoutProvider;
    //    private List<Item> _formated;
    //    private List<Item> _data;

    //    public Sort(LayoutConfig layout, ILayoutProvider layoutProvider)
    //    {
    //        _layout = layout;
    //        _layoutProvider = layoutProvider;
    //    }

    //    public List<Item> Sorting(List<Item> data)
    //    {
    //        _data = data;
    //        _formated = new List<Item>();
    //        SortWithNormalDepot();
    //        SortForCustomDepot();
    //        return _formated;
    //    }

    //    public void SortWithNormalDepot()
    //    {
    //        if (_layout.ClockWise)
    //            SortClockWise();
    //        else
    //            SortForCounterClockWise();
    //    }

    //    public void SortForCustomDepot()
    //    {
    //        Item startItem;
    //        if (NeedReformatForNewDepotLocation(out startItem))
    //        {
    //            UpdateSortListForNewDepotLocation(startItem);
    //        }
    //    }

    //    private void SortClockWise()
    //    {
    //        // for starting point is route 1
    //        var minRoute = GetMinRoute();
    //        var maxRoute = GetMaxRoute();
    //        // min route
    //        SortForMinRoute();
    //        // route between min & max
    //        for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
    //        {
    //            SortForTopRoute(i);
    //        }
    //        // for max
    //        SortForMaxRoute();
    //        // route between max & min
    //        for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
    //        {
    //            SortForBottomRoute(i);
    //        }
    //    }
    //    private void SortForCounterClockWise()
    //    {
    //        var minRoute = GetMinRoute();
    //        var maxRoute = GetMaxRoute();
    //        // between min & max
    //        for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
    //        {
    //            SortForBottomRoute(i);
    //        }
    //        // for max
    //        SortForMaxRoute();
    //        // for route between max - min
    //        for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
    //        {
    //            SortForTopRoute(i);
    //        }
    //        // for min
    //        SortForMinRoute();
    //    }

    //    public void SortForMaxRoute()
    //    {
    //        var maxRoute = GetMaxRoute();
    //        var itemNeedFormat = GetItemNeedFormat(maxRoute.Index);
    //        itemNeedFormat = _layout.ClockWise ? itemNeedFormat.OrderByDescending(e => e.Bin).ThenBy(e => e.Aisle).ToList() : itemNeedFormat.OrderBy(e => e.Bin).ThenByDescending(e => e.Aisle).ToList();
    //        _formated.AddRange(itemNeedFormat);
    //    }

    //    public void SortForMinRoute()
    //    {
    //        var minRoute = GetMinRoute();
    //        var itemNeedFormat = GetItemNeedFormat(minRoute.Index);
    //        itemNeedFormat = _layout.ClockWise ? itemNeedFormat.OrderBy(e => e.Bin).ThenBy(e => e.Aisle).ToList() : itemNeedFormat.OrderByDescending(e => e.Bin).ThenByDescending(e => e.Aisle).ToList();
    //        _formated.AddRange(itemNeedFormat);
    //    }

    //    public void SortForBottomRoute(int i)
    //    {
    //        var itemNeedFormat = GetItemNeedFormatForBottomRoute(i, !_layout.ClockWise);
    //        _formated.AddRange(itemNeedFormat);

    //        itemNeedFormat = GetItemNeedFormatForBottomRoute(i, _layout.ClockWise);
    //        _formated.AddRange(itemNeedFormat);
    //    }

    //    public void SortForTopRoute(int i)
    //    {
    //        var itemNeedFormat = GetItemNeedFormatForTopRoute(i, _layout.ClockWise);
    //        _formated.AddRange(itemNeedFormat);

    //        itemNeedFormat = GetItemNeedFormatForTopRoute(i, !_layout.ClockWise);
    //        _formated.AddRange(itemNeedFormat);
    //    }

    //    private bool NeedReformatForNewDepotLocation(out Item item)
    //    {
    //        var minRoute = GetMinRoute();
    //        var maxRoute = GetMaxRoute();
    //        item = null;
    //        if (_layout.StartRoute < minRoute.Index ||
    //            _layout.StartRoute > maxRoute.Index ||
    //            (_layout.StartRoute == minRoute.Index && _layout.StartFromBottomOfRoute)
    //            )
    //            return false;
    //        // default start route (1)
    //        // get start item of layout
    //        var startItem = GetStartingItem();
    //        if (startItem == null)
    //            return false;
    //        item = startItem;
    //        return true;
    //    }

    //    private void UpdateSortListForNewDepotLocation(Item startItem)
    //    {
    //        // index to start item
    //        var index = _formated.FindIndex(e => e.Aisle == startItem.Aisle && e.Bin == startItem.Bin);
    //        // move it to top
    //        var itemNeedFormat = _formated.GetRange(index, _formated.Count - index);
    //        foreach (var item in itemNeedFormat)
    //        {
    //            _formated.Remove(item);
    //        }
    //        _formated.InsertRange(0, itemNeedFormat);
    //    }

    //    private Route GetMinRoute()
    //    {
    //        var minAisle = GetMinAisle();
    //        return _layoutProvider.GetRouteOfAisle(minAisle);
    //    }
    //    private Route GetMaxRoute()
    //    {
    //        var maxAisleAisle = GetMaxAisle();
    //        return _layoutProvider.GetRouteOfAisle(maxAisleAisle);
    //    }

    //    private Item GetStartingItem()
    //    {
    //        if (_layout.StartFromBottomOfRoute)
    //        {
    //            return GetStartingItemForBottomOfRoute();
    //        }
    //        return GetStartingItemForTopOfRoute();
    //    }

    //    #region Get item for top of route

    //    private Item GetStartingItemForTopOfRoute()
    //    {
    //        if (!_layout.ClockWise)
    //        {
    //            return GetStartingItemForTopOfRouteWithCoClockWise();
    //        }
    //        return GetStartingItemForTopOfRouteWithClockWise();
    //    }

    //    private Item GetStartingItemForTopOfRouteWithCoClockWise()
    //    {
    //        var startingRoute = _layoutProvider.GetRoute(_layout.StartRoute);
    //        var minRoute = GetMinRoute();
    //        var items = _data.Where(
    //            e =>
    //                e.Aisle >= minRoute.Aisles.Min(f => f.Aisle) &&
    //                e.Aisle <= startingRoute.Aisles.Max(f => f.Aisle) &&
    //                (e.Bin > _layout.HaftBin || minRoute.Aisles.Any(f => f.Aisle == e.Aisle)));
    //        return items.OrderByDescending(e => _layoutProvider.GetRouteOfAisle(e.Aisle).Index)
    //            .ThenByDescending(e => e.Bin)
    //            .ThenByDescending(e => e.Aisle).FirstOrDefault();
    //    }

    //    private Item GetStartingItemForTopOfRouteWithClockWise()
    //    {
    //        var startingRoute = _layoutProvider.GetRoute(_layout.StartRoute);
    //        var maxRoute = GetMaxRoute();
    //        var items = _data.Where(
    //            e =>
    //                (e.Aisle > startingRoute.Aisles.Min(f => f.Aisle)) &&
    //                e.Aisle <= maxRoute.Aisles.Max(f => f.Aisle) &&
    //                (e.Bin > _layout.HaftBin || maxRoute.Aisles.Any(f => f.Aisle == e.Aisle)));
    //        return items
    //            .OrderBy(e => _layoutProvider.GetRouteOfAisle(e.Aisle).Index)
    //            .ThenByDescending(e => e.Bin)
    //            .ThenBy(e => e.Aisle).FirstOrDefault();
    //    }

    //    #endregion


    //    #region Get Item for bottom of route

    //    private Item GetStartingItemForBottomOfRoute()
    //    {
    //        if (_layout.ClockWise)
    //        {
    //            return GetStartingItemForBottomOfRouteWithClockwise();
    //        }
    //        return GetStartingItemForBottomOfRouteWithCoClockwise();
    //    }

    //    private Item GetStartingItemForBottomOfRouteWithCoClockwise()
    //    {
    //        var startingRoute = _layoutProvider.GetRoute(_layout.StartRoute);
    //        var maxRoute = GetMaxRoute();
    //        var items = _data.Where(
    //            e =>
    //                e.Aisle >= startingRoute.Aisles.Min(f => f.Aisle) &&
    //                e.Aisle <= maxRoute.Aisles.Max(f => f.Aisle) &&
    //                (e.Bin <= _layout.HaftBin || maxRoute.Aisles.Any(f => f.Aisle == e.Aisle)));
    //        return items.OrderBy(e => e.Aisle).ThenBy(e => e.Bin).FirstOrDefault();
    //    }

    //    private Item GetStartingItemForBottomOfRouteWithClockwise()
    //    {
    //        var startingRoute = _layoutProvider.GetRoute(_layout.StartRoute);
    //        var minRoute = GetMinRoute();
    //        var items = _data.Where(
    //            e =>
    //                e.Aisle > minRoute.Aisles.Min(f => f.Aisle) &&
    //                e.Aisle <= startingRoute.Aisles.Max(f => f.Aisle) &&
    //                e.Bin <= _layout.HaftBin);
    //        return items.OrderByDescending(e => e.Aisle).ThenBy(e => e.Bin).FirstOrDefault();
    //    }

    //    #endregion


    //    private List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
    //    {
    //        var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin > _layout.HaftBin).ToList();
    //        if (_layout.ClockWise)
    //        {
    //            return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
    //        }
    //        return _layoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
    //    }
    //    private List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
    //    {
    //        var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin <= _layout.HaftBin).ToList();
    //        if (_layout.ClockWise)
    //        {
    //            return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
    //        }
    //        return _layoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
    //    }


    //    private List<Item> GetItemNeedFormat(int routeIndex, bool leftRoute)
    //    {
    //        var route = _layoutProvider.GetRoute(routeIndex);
    //        return _data.Where(e => (leftRoute && route.Aisles.Any(f => f.LeftSide && f.Aisle == e.Aisle)) || (!leftRoute && route.Aisles.Any(f => !f.LeftSide && f.Aisle == e.Aisle))).ToList();
    //    }


    //    private List<Item> GetItemNeedFormat(int routeIndex)
    //    {
    //        var route = _layoutProvider.GetRoute(routeIndex);
    //        return _data.Where(e => route.Aisles.Any(f => f.Aisle == e.Aisle)).ToList();
    //    }

    //    private int GetMinAisle()
    //    {
    //        return _data.Min(e => e.Aisle);
    //    }
    //    private int GetMaxAisle()
    //    {
    //        return _data.Max(e => e.Aisle);
    //    }
    //}
}