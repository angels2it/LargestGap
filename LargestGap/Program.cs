using System;
using System.Collections.Generic;
using LargestGap.Interfaces;
using LargestGap.Sort;

namespace LargestGap
{
    class Program
    {
        private static LayoutConfig _layout;
        private static ILayoutProvider _layoutProvider;
        private static List<Item> _data;
        private static List<Item> _formated;
        private static ISorter _sort;
        static void Main(string[] args)
        {
            InitLayout();
            _layoutProvider = new LayoutProvider(_layout);
            _sort = new Sorter(_layout, _layoutProvider);
            InitData();
            _formated = _sort.Sort(_data);
            Print();
            Console.ReadLine();
        }

        private static void Print()
        {
            Console.WriteLine(_layout.ToString());
            foreach (var item in _formated)
            {
                Console.WriteLine(item.ToString());
            }
        }


        private static void InitData()
        {
            _data = new List<Item>()
            {
                new Item{Aisle = 14,Bin = 17},
                new Item{Aisle = 15,Bin = 17},
                new Item{Aisle = 14,Bin = 18},
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
                new Item{Aisle = 3,Bin = 16},
                new Item{Aisle = 4,Bin = 6}
            };
        }

        private static void InitLayout()
        {
            _layout = new LayoutConfig()
            {
                Bin = 20,
                ClockWise = true,
                StartFromBottomOfRoute = true,
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
                //    new Route(1, new RouteAisleItem(1, true), new RouteAisleItem(2)),
                //    new Route(2, new RouteAisleItem(3, true),new RouteAisleItem(4)),
                //    new Route(3, new RouteAisleItem(5, true),new RouteAisleItem(6)),
                //    new Route(4, new RouteAisleItem(7, true),new RouteAisleItem(8)),
                //    new Route(5, new RouteAisleItem(9, true),new RouteAisleItem(10)),
                //    new Route(6, new RouteAisleItem(11, true),new RouteAisleItem(12)),
                //    new Route(7, new RouteAisleItem(13, true),new RouteAisleItem(14)),
                //    new Route(8, new RouteAisleItem(15, true),new RouteAisleItem(16)),
                //    //new Route(9, new RouteAisleItem(16, true))
                //}
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
