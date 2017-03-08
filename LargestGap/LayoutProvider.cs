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
                return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
            return itemNeedFormat.OrderBy(e => e.Bin).ToList();
        }

        public List<Item> CounterClockWiseOrderItem(List<Item> itemNeedFormat, bool leftRoute)
        {
            if (leftRoute)
                return itemNeedFormat.OrderBy(e => e.Bin).ToList();
            return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
        }
    }
}
