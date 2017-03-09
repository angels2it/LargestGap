namespace LargestGap.Interfaces
{
    public interface ISortDirection
    {
        void SortWithNormalDepot();
        void SortForMaxRoute();
        void SortForMinRoute();
        void SortForBottomRoute(int i);
        void SortForTopRoute(int i);
    }
}