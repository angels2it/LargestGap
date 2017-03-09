using System.Collections.Generic;

namespace LargestGap.Interfaces
{
    public interface ISort
    {
        List<Item> Sorting(List<Item> data);
        void SortWithNormalDepot();
        void SortForCustomDepot();
    }
}
