using System;
using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
    public interface ILayoutProvider
    {
        Route GetRouteOfAisle(int aisle);
        Route GetRoute(int routeIndex);
        List<Item> ClockWiseOrderItem(List<Item> itemNeedFormat, bool leftRoute);
        List<Item> CounterClockWiseOrderItem(List<Item> itemNeedFormat, bool leftRoute);
        List<Item> GetItemNeedFormat(List<Item> data, int routeIndex, bool leftRoute);
        List<Item> GetItemNeedFormat(List<Item> data, int routeIndex);
        int GetMinAisle(List<Item> data);
        int GetMaxAisle(List<Item> data);
        Route GetMinRoute(List<Item> data);
        Route GetMaxRoute(List<Item> data);
    }

    public class LayoutProvider : ILayoutProvider
    {
        private readonly LayoutConfig _layout;

        public LayoutProvider(LayoutConfig layout)
        {
            _layout = layout;
        }

        public Route GetRouteOfAisle(int aisle)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Aisles.Any(f => f.Aisle == aisle));
            if (route == null)
                throw new Exception("No any route for this aisle");
            return route;
        }

        public Route GetRoute(int routeIndex)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Index == routeIndex);
            if (route == null)
                throw new Exception("No any route for this aisle");
            return route;
        }

        public List<Item> ClockWiseOrderItem(List<Item> itemNeedFormat, bool leftRoute)
        {
            if (leftRoute)
                return itemNeedFormat.OrderByDescending(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            return itemNeedFormat.OrderBy(e => e.Bin).ThenByDescending(e => e.Aisle).ToList();
        }

        public List<Item> CounterClockWiseOrderItem(List<Item> itemNeedFormat, bool leftRoute)
        {
            if (leftRoute)
                return itemNeedFormat.OrderBy(e => e.Bin).ThenBy(e => e.Aisle).ToList();
            return itemNeedFormat.OrderByDescending(e => e.Bin).ThenByDescending(e => e.Aisle).ToList();
        }

        public List<Item> GetItemNeedFormat(List<Item> data, int routeIndex, bool leftRoute)
        {
            var route = GetRoute(routeIndex);
            return data.Where(e => (leftRoute && route.Aisles.Any(f => f.LeftSide && f.Aisle == e.Aisle)) || (!leftRoute && route.Aisles.Any(f => !f.LeftSide && f.Aisle == e.Aisle))).ToList();
        }


        public List<Item> GetItemNeedFormat(List<Item> data, int routeIndex)
        {
            var route = GetRoute(routeIndex);
            return data.Where(e => route.Aisles.Any(f => f.Aisle == e.Aisle)).ToList();
        }
        public int GetMinAisle(List<Item> data)
        {
            return data.Min(e => e.Aisle);
        }
        public int GetMaxAisle(List<Item> data)
        {
            return data.Max(e => e.Aisle);
        }
        public Route GetMinRoute(List<Item> data)
        {
            var minAisle = GetMinAisle(data);
            return GetRouteOfAisle(minAisle);
        }
        public Route GetMaxRoute(List<Item> data)
        {
            var maxAisleAisle = GetMaxAisle(data);
            return GetRouteOfAisle(maxAisleAisle);
        }
    }
}
