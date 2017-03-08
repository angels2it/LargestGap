namespace LargestGap
{
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
}