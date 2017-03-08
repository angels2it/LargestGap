using System.Collections.Generic;

namespace LargestGap
{
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