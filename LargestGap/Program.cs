using System;
using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
    class Program
    {
        private static LayoutConfig _layout;
        private static List<Item> _data;
        private static List<Item> _formated;
        static void Main(string[] args)
        {
            InitLayout();
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
            if (_layout.StartRoute <= 1)
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
            var minAisle = GetMinAisle();
            var minRoute = GetRouteOfAisle(minAisle);
            var maxAisleAisle = GetMaxAisle();
            var maxRoute = GetRouteOfAisle(maxAisleAisle);
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

        private static void SortClockWise()
        {
            // for starting point is route 1
            var minAisle = GetMinAisle();
            var minRoute = GetRouteOfAisle(minAisle);
            var maxAisleAisle = GetMaxAisle();
            var maxRoute = GetRouteOfAisle(maxAisleAisle);
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
            var maxAisleAisle = GetMaxAisle();
            var maxRoute = GetRouteOfAisle(maxAisleAisle);
            var itemNeedFormat = GetItemNeedFormat(maxRoute.Index);
            itemNeedFormat = _layout.ClockWise ? itemNeedFormat.OrderByDescending(e => e.Bin).ToList() : itemNeedFormat.OrderBy(e => e.Bin).ToList();
            _formated.AddRange(itemNeedFormat);
        }

        private static void SortForMinRoute()
        {
            var minAisle = GetMinAisle();
            var minRoute = GetRouteOfAisle(minAisle);
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
            List<Item> items;
            if (_layout.ClockWise)
            {
                // for left
                items = GetItemNeedFormatForTopRoute(routeIndex, true);
                if (items.Count != 0)
                    return items.First();
                // for right
                items = GetItemNeedFormatForTopRoute(routeIndex, false);
                if (items.Count != 0)
                    return items.First();
                if (increase == null)
                    return GetStartingItemForTopOfRoute(++routeIndex, true);
                if (IsMaxRouteOfData(routeIndex))
                {
                    return GetItemOfMaxRoute().First();
                }
                return GetStartingItemForTopOfRoute(++routeIndex, true);
            }
            items = GetItemNeedFormatForTopRoute(routeIndex, false);
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
                if (IsMinRouteOfData(routeIndex))
                    return GetItemOfMinRoute().First();
                return GetStartingItemForTopOfRoute(--routeIndex, false);
            }
            return items.First();
        }

        private static List<Item> GetItemOfMinRoute()
        {
            var itemNeedFormat = GetItemNeedFormat(GetMinRouteOfData().Index);
            return _layout.ClockWise ? itemNeedFormat.OrderBy(e => e.Bin).ToList() : itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
        }

        private static bool IsMinRouteOfData(int routeIndex)
        {
            var minRoute = GetMinRouteOfData();
            return minRoute.Index == routeIndex;
        }

        private static Route GetMinRouteOfData()
        {
            var minAisleAisle = GetMinAisle();
            return GetRouteOfAisle(minAisleAisle);
        }

        private static List<Item> GetItemOfMaxRoute()
        {
            var itemNeedFormat = GetItemNeedFormat(GetMaxRouteOfData().Index);
            return _layout.ClockWise ? itemNeedFormat.OrderByDescending(e => e.Bin).ToList() : itemNeedFormat.OrderBy(e => e.Bin).ToList();
        }

        private static bool IsMaxRouteOfData(int routeIndex)
        {
            var maxRoute = GetMaxRouteOfData();
            return maxRoute.Index == routeIndex;
        }

        private static Route GetMaxRouteOfData()
        {
            var maxAisleAisle = GetMaxAisle();
            return GetRouteOfAisle(maxAisleAisle);
        }

        private static Item GetStartingItemForBottomOfRoute(int routeIndex, bool? increase)
        {
            List<Item> items;
            if (_layout.ClockWise)
            {
                items = GetItemNeedFormatForBottomRoute(routeIndex, false);
                if (items.Count != 0)
                    return items.First();
                items = GetItemNeedFormatForBottomRoute(routeIndex, true);
                if (items.Count != 0)
                    return items.First();
                if (increase == null)
                    return GetStartingItemForBottomOfRoute(--routeIndex, false);
                if (increase.Value)
                {
                    if (routeIndex == _layout.Routes.Max(e => e.Index))
                        return GetStartingItemForBottomOfRoute(--routeIndex, false);
                    return GetStartingItemForBottomOfRoute(++routeIndex, true);
                }
                if (routeIndex == _layout.Routes.Min(e => e.Index))
                    return GetItemNeedFormatForBottomRoute(routeIndex, false).FirstOrDefault() ?? GetItemNeedFormatForBottomRoute(routeIndex, true).FirstOrDefault();
                return GetStartingItemForBottomOfRoute(--routeIndex, false);
            }
            items = GetItemNeedFormatForBottomRoute(routeIndex, true);
            if (items.Count != 0)
                return items.First();
            items = GetItemNeedFormatForBottomRoute(routeIndex, false);
            if (items.Count != 0)
                return items.First();
            if (increase == null)
                return GetStartingItemForBottomOfRoute(++routeIndex, true);
            if (increase.Value)
            {
                if (routeIndex == _layout.Routes.Max(e => e.Index))
                    return GetStartingItemForBottomOfRoute(--routeIndex, false);
                return GetStartingItemForBottomOfRoute(++routeIndex, true);
            }
            if (routeIndex == _layout.Routes.Min(e => e.Index))
                return GetItemNeedFormatForBottomRoute(routeIndex, true).FirstOrDefault() ?? GetItemNeedFormatForBottomRoute(routeIndex, false).FirstOrDefault();
            return GetStartingItemForBottomOfRoute(--routeIndex, false);
        }

        private static List<Item> GetItemNeedFormatForTopRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin > _layout.HaftBin).ToList();
            if (_layout.ClockWise)
            {
                if (leftRoute)
                    return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
                return itemNeedFormat.OrderBy(e => e.Bin).ToList();
            }
            if (leftRoute)
                return itemNeedFormat.OrderBy(e => e.Bin).ToList();
            return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
        }
        private static List<Item> GetItemNeedFormatForBottomRoute(int index, bool leftRoute)
        {
            var itemNeedFormat = GetItemNeedFormat(index, leftRoute).Where(e => e.Bin <= _layout.HaftBin).ToList();
            if (_layout.ClockWise)
            {
                if (leftRoute)
                    return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
                return itemNeedFormat.OrderBy(e => e.Bin).ToList();
            }
            if (leftRoute)
                return itemNeedFormat.OrderBy(e => e.Bin).ToList();
            return itemNeedFormat.OrderByDescending(e => e.Bin).ToList();
        }

        private static Route GetRouteOfAisle(int aisle)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Aisles.Any(f => f.Aisle == aisle));
            if (route == null)
                throw new Exception("No any route for this aisle");
            return route;
        }

        private static List<Item> GetItemNeedFormat(int routeIndex, bool leftRoute)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Index == routeIndex);
            if (route == null)
                throw new Exception("No any route for this aisle");
            return _data.Where(e => (leftRoute && route.Aisles.Any(f => f.LeftSide && f.Aisle == e.Aisle)) || (!leftRoute && route.Aisles.Any(f => !f.LeftSide && f.Aisle == e.Aisle))).ToList();
        }


        private static List<Item> GetItemNeedFormat(int routeIndex)
        {
            var route = _layout.Routes.FirstOrDefault(e => e.Index == routeIndex);
            if (route == null)
                throw new Exception("No any route for this aisle");
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

    public class Route
    {
        public Route(int index, params RouteAisleItem[] aisles)
        {
            Index = index;
            Aisles = aisles.ToList();
        }

        public List<RouteAisleItem> Aisles { get; set; }

        public int Index { get; set; }
    }

    public class RouteAisleItem
    {
        public RouteAisleItem(int aisle, bool leftSide = false)
        {
            Aisle = aisle;
            LeftSide = leftSide;
        }

        public int Aisle { get; set; }
        public bool LeftSide { get; set; }
    }
    public class LayoutConfig
    {
        public List<Route> Routes { get; set; }
        public int Bin { get; set; }
        public int HaftBin => Bin / 2;
        public int StartRoute { get; set; }
        public bool ClockWise { get; set; }
        public bool StartFromBottomOfRoute { get; set; }
    }
}
