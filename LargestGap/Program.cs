using System;
using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
    class Program
    {
        private static LayoutConfig _layout;
        private static ILayoutProvider _layoutProvider;
        private static List<Item> _data;
        private static List<Item> _formated;
        static void Main(string[] args)
        {
            InitLayout();
            _layoutProvider = new LayoutProvider(_layout);
            InitData();
            Sort();
            Print();
            Console.ReadLine();
        }

        private static void Print()
        {
            foreach (var item in _formated)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void Sort()
        {
            _formated = new List<Item>();
            // for min
            if (_layout.ClockWise)
                SortClockWise();
            else
                SortForCounterClockWise();
            var minRoute = GetMinRoute();
            var maxRoute = GetMaxRoute();
            if (_layout.StartRoute <= minRoute.Index || _layout.StartRoute >= maxRoute.Index)
                return;
            // default start route (1)
            var defaultStartRoute = _layout.Routes.First(e => e.Index == _layout.Routes.Min(f => f.Index));
            // get start item of layout
            var startItem = GetStartingItem(_layout.StartRoute);
            if (startItem == null)
                return;
            // if it's same with default option - do nothing
            if (defaultStartRoute.Aisles.Any(e => e.Aisle == startItem.Aisle))
                return;
            UpdateSortListForNewDepotLocation(startItem);
        }

        private static void UpdateSortListForNewDepotLocation(Item startItem)
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


        private static void SortForCounterClockWise()
        {
            var minRoute = GetMinRoute();
            var maxRoute = GetMaxRoute();
            for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
            {
                SortForBottomRoute(i);
            }
            // for min
            SortForMaxRoute();
            // for aisle between max - min
            for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
            {
                SortForTopRoute(i);
            }
            SortForMinRoute();
        }

        private static Route GetMinRoute()
        {
            var minAisle = GetMinAisle();
            return _layoutProvider.GetRouteOfAisle(minAisle);
        }
        private static Route GetMaxRoute()
        {
            var maxAisleAisle = GetMaxAisle();
            return _layoutProvider.GetRouteOfAisle(maxAisleAisle);
        }
        private static void SortClockWise()
        {
            // for starting point is route 1
            var minRoute = GetMinRoute();
            var maxRoute = GetMaxRoute();
            SortForMinRoute();
            for (int i = minRoute.Index + 1; i < maxRoute.Index; i++)
            {
                SortForTopRoute(i);
            }
            // for max
            SortForMaxRoute();
            for (int i = maxRoute.Index - 1; i > minRoute.Index; i--)
            {
                SortForBottomRoute(i);
            }
        }

        private static void SortForMaxRoute()
        {
            var maxRoute = GetMaxRoute();
            var itemNeedFormat = GetItemNeedFormat(maxRoute.Index);
            itemNeedFormat = _layout.ClockWise ? itemNeedFormat.OrderByDescending(e => e.Bin).ToList() : itemNeedFormat.OrderBy(e => e.Bin).ToList();
            _formated.AddRange(itemNeedFormat);
        }

        private static void SortForMinRoute()
        {
            var minRoute = GetMinRoute();
            var itemNeedFormat = GetItemNeedFormat(minRoute.Index);
            itemNeedFormat = _layout.ClockWise ? itemNeedFormat.OrderBy(e => e.Bin).ToList() : itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
            _formated.AddRange(itemNeedFormat);
        }

        private static void SortForBottomRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForBottomRoute(i, !_layout.ClockWise);
            _formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForBottomRoute(i, _layout.ClockWise);
            _formated.AddRange(itemNeedFormat);
        }

        private static void SortForTopRoute(int i)
        {
            var itemNeedFormat = GetItemNeedFormatForTopRoute(i, _layout.ClockWise);
            _formated.AddRange(itemNeedFormat);

            itemNeedFormat = GetItemNeedFormatForTopRoute(i, !_layout.ClockWise);
            _formated.AddRange(itemNeedFormat);
        }

        private static Item GetStartingItem(int routeIndex, bool? increase = null)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Index == routeIndex);
            if (route == null)
                throw new Exception("Please config starting route");
            if (_layout.StartFromBottomOfRoute)
            {
                return GetStartingItemForBottomOfRoute(routeIndex, increase);
            }
            return GetStartingItemForTopOfRoute(routeIndex, increase);
        }

        private static Item GetStartingItemForTopOfRoute(int routeIndex, bool? increase)
        {
            if (_layout.ClockWise)
            {
                return GetStartingItemForTopOfRouteWithClockWise(routeIndex, increase);
            }
            return GetStartingItemForTopOfRouteWithCoClockWise(routeIndex, increase);
        }

        private static Item GetStartingItemForTopOfRouteWithCoClockWise(int routeIndex, bool? increase)
        {
            var items = GetItemNeedFormatForTopRoute(routeIndex, false);
            if (items.Count == 0 || items.All(e => e.Bin > _layout.HaftBin))
            {
                items = GetItemNeedFormatForTopRoute(routeIndex, false);
                if (items.Count != 0)
                    return items.First();
                items = GetItemNeedFormatForTopRoute(routeIndex, true);
                if (items.Count != 0)
                    return items.First();
                if (increase == null)
                {
                    return GetStartingItemForTopOfRoute(--routeIndex, false);
                }
                if (IsMinRoute(routeIndex))
                    return GetItemOfMinRoute().First();
                return GetStartingItemForTopOfRoute(--routeIndex, false);
            }
            return items.First();
        }

        private static Item GetStartingItemForTopOfRouteWithClockWise(int routeIndex, bool? increase)
        {
            // for left
            var items = GetItemNeedFormatForTopRoute(routeIndex, true);
            if (items.Count != 0)
                return items.First();
            // for right
            items = GetItemNeedFormatForTopRoute(routeIndex, false);
            if (items.Count != 0)
                return items.First();
            if (increase == null)
                return GetStartingItemForTopOfRouteWithClockWise(++routeIndex, true);
            if (IsMaxRoute(routeIndex))
            {
                return GetItemOfMaxRoute().First();
            }
            return GetStartingItemForTopOfRouteWithClockWise(++routeIndex, true);
        }

        private static List<Item> GetItemOfMinRoute()
        {
            var itemNeedFormat = GetItemNeedFormat(GetMinRoute().Index);
            return _layout.ClockWise ? itemNeedFormat.OrderBy(e => e.Bin).ToList() : itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
        }

        private static bool IsMinRoute(int routeIndex)
        {
            var minRoute = GetMinRoute();
            return minRoute.Index == routeIndex;
        }

        private static List<Item> GetItemOfMaxRoute()
        {
            var itemNeedFormat = GetItemNeedFormat(GetMaxRoute().Index);
            return _layout.ClockWise ? itemNeedFormat.OrderByDescending(e => e.Bin).ToList() : itemNeedFormat.OrderBy(e => e.Bin).ToList();
        }

        private static bool IsMaxRoute(int routeIndex)
        {
            var maxRoute = GetMaxRoute();
            return maxRoute.Index == routeIndex;
        }

        private static Item GetStartingItemForBottomOfRoute(int routeIndex, bool? increase)
        {
            if (_layout.ClockWise)
            {
                return GetStartingItemForBottomOfRouteWithClockwise(routeIndex, increase);
            }
            return GetStartingItemForBottomOfRouteWithCoClockWise(routeIndex, increase);
        }

        private static Item GetStartingItemForBottomOfRouteWithCoClockWise(int routeIndex, bool? increase)
        {
            var items = GetItemNeedFormatForBottomRoute(routeIndex, true);
            if (items.Count != 0)
                return items.First();
            items = GetItemNeedFormatForBottomRoute(routeIndex, false);
            if (items.Count != 0)
                return items.First();
            if (increase == null)
                return GetStartingItemForBottomOfRouteWithCoClockWise(++routeIndex, true);
            if (increase.Value)
            {
                if (routeIndex == _layout.Routes.Max(e => e.Index))
                    return GetStartingItemForBottomOfRouteWithCoClockWise(--routeIndex, false);
                return GetStartingItemForBottomOfRouteWithCoClockWise(++routeIndex, true);
            }
            if (routeIndex == _layout.Routes.Min(e => e.Index))
                return GetItemNeedFormatForBottomRoute(routeIndex, true).FirstOrDefault() ?? GetItemNeedFormatForBottomRoute(routeIndex, false).FirstOrDefault();
            return GetStartingItemForBottomOfRouteWithCoClockWise(--routeIndex, false);
        }

        private static Item GetStartingItemForBottomOfRouteWithClockwise(int routeIndex, bool? increase)
        {
            var items = GetItemNeedFormatForBottomRoute(routeIndex, false);
            if (items.Count != 0)
                return items.First();
            items = GetItemNeedFormatForBottomRoute(routeIndex, true);
            if (items.Count != 0)
                return items.First();
            if (increase == null)
                return GetStartingItemForBottomOfRouteWithClockwise(--routeIndex, false);
            if (increase.Value)
            {
                if (routeIndex == _layout.Routes.Max(e => e.Index))
                    return GetStartingItemForBottomOfRouteWithClockwise(--routeIndex, false);
                return GetStartingItemForBottomOfRouteWithClockwise(++routeIndex, true);
            }
            if (routeIndex == _layout.Routes.Min(e => e.Index))
                return GetItemNeedFormatForBottomRoute(routeIndex, false).FirstOrDefault() ?? GetItemNeedFormatForBottomRoute(routeIndex, true).FirstOrDefault();
            return GetStartingItemForBottomOfRouteWithClockwise(--routeIndex, false);
        }

        private static List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin > _layout.HaftBin).ToList();
            if (_layout.ClockWise)
            {
                return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
            }
            return _layoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
        }
        private static List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin <= _layout.HaftBin).ToList();
            if (_layout.ClockWise)
            {
                return _layoutProvider.ClockWiseOrderItem(itemNeedFormat, leftRoute);
            }
            return _layoutProvider.CounterClockWiseOrderItem(itemNeedFormat, leftRoute);
        }


        private static List<Item> GetItemNeedFormat(int routeIndex, bool leftRoute)
        {
            var route = _layoutProvider.GetRoute(routeIndex);
            return _data.Where(e => (leftRoute && route.Aisles.Any(f => f.LeftSide && f.Aisle == e.Aisle)) || (!leftRoute && route.Aisles.Any(f => !f.LeftSide && f.Aisle == e.Aisle))).ToList();
        }


        private static List<Item> GetItemNeedFormat(int routeIndex)
        {
            var route = _layoutProvider.GetRoute(routeIndex);
            return _data.Where(e => route.Aisles.Any(f => f.Aisle == e.Aisle)).ToList();
        }

        private static int GetMinAisle()
        {
            return _data.Min(e => e.Aisle);
        }
        private static int GetMaxAisle()
        {
            return _data.Max(e => e.Aisle);
        }

        private static void InitData()
        {
            _data = new List<Item>()
            {
                new Item{Aisle = 14,Bin = 7},
                new Item{Aisle = 14,Bin = 3},
                new Item{Aisle = 11,Bin = 5},
                new Item{Aisle = 10,Bin = 16},
                new Item{Aisle = 10,Bin = 3},
                new Item{Aisle = 9,Bin = 6},
                new Item{Aisle = 7,Bin = 14},
                new Item{Aisle = 6,Bin = 17},
                new Item{Aisle = 3,Bin = 7},
                new Item{Aisle = 2,Bin = 15},
                new Item{Aisle = 1,Bin = 17},
                new Item{Aisle = 1,Bin = 7},
                new Item{Aisle = 1,Bin = 2},
                new Item{Aisle = 2,Bin = 4},
                new Item{Aisle = 3,Bin = 2},
                new Item{Aisle = 2,Bin = 14},
                new Item{Aisle = 2,Bin = 16},
                new Item{Aisle = 3,Bin = 14},
                new Item{Aisle = 3,Bin = 16}
            };
        }

        private static void InitLayout()
        {
            _layout = new LayoutConfig()
            {
                Bin = 20,
                ClockWise = false,
                StartFromBottomOfRoute = false,
                StartRoute = 3,
                // normal layout
                Routes = new List<Route>()
                {
                    new Route(1, new RouteAisleItem(1)),
                    new Route(2, new RouteAisleItem(2, true),new RouteAisleItem(3)),
                    new Route(3, new RouteAisleItem(4, true),new RouteAisleItem(5)),
                    new Route(4, new RouteAisleItem(6, true),new RouteAisleItem(7)),
                    new Route(5, new RouteAisleItem(8, true),new RouteAisleItem(9)),
                    new Route(6, new RouteAisleItem(10, true),new RouteAisleItem(11)),
                    new Route(7, new RouteAisleItem(12, true),new RouteAisleItem(13)),
                    new Route(8, new RouteAisleItem(14, true),new RouteAisleItem(15)),
                    new Route(9, new RouteAisleItem(16, true))
                }
                //Routes = new List<Route>()
                //{
                //    new Route(1, new RouteAisleItem(1)),
                //    new Route(2, new RouteAisleItem(2)),
                //    new Route(3,new RouteAisleItem(3)),
                //    new Route(4, new RouteAisleItem(4)),
                //    new Route(5, new RouteAisleItem(5)),
                //    new Route(6, new RouteAisleItem(6)),
                //    new Route(7, new RouteAisleItem(7, true),new RouteAisleItem(8)),
                //    new Route(8, new RouteAisleItem(9, true),new RouteAisleItem(10)),
                //    new Route(9, new RouteAisleItem(11, true),new RouteAisleItem(12)),
                //    new Route(10, new RouteAisleItem(13, true),new RouteAisleItem(14)),
                //    new Route(11, new RouteAisleItem(15, true), new RouteAisleItem(16))
                //}
            };
        }
    }
}
