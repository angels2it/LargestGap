using System.Collections.Generic;

namespace LargestGap
{
    public interface ISorting
    {
        List<Item> Sort(List<Item> data);
        void SortWithNormalDepot();
        void SortForCustomDepot();
        void SortForMaxRoute();
        void SortForMinRoute();
        void SortForBottomRoute(int i);
        void SortForTopRoute(int i);
    }
}
