using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LargestGap.Test
{
    [TestFixture]
    public class Class1
    {
        private static LayoutConfig _layout;
        private LayoutProvider _layoutProvider;

        public Class1()
        {
            InitLayout();
            _layoutProvider = new LayoutProvider(_layout);
        }
        private static void InitLayout()
        {
            _layout = new LayoutConfig()
            {
                Bin = 20,
                ClockWise = true,
                StartFromBottomOfRoute = true,
                StartRoute = 4,
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
        [Test]
        public void TestSort()
        {
            // arrange
            var s = new Sorting(_layout, _layoutProvider);
            // act
            
            // assert
        }
    }
}
