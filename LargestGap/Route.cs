using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
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
}