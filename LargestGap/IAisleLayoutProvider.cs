using System.Collections.Generic;
using System.Linq;

namespace LargestGap
{
    public interface IAisleLayoutProvider
    {
        double GetDistanceForTopRoute(Item item1, Item item2);
        double GetDistanceForBottomRoute(Item item1, Item item2);
        List<Item> GetSameRouteAisle(List<Item> items, int aisle);
        List<Item> GetNextAisle(List<Item> sortedList, int aisle);
    }

    public class AisleLayoutProvider : IAisleLayoutProvider
    {
        public double GetDistanceForTopRoute(Item lastItemOfFirstAisle, Item nextBin)
        {
            return Program1.Bin - lastItemOfFirstAisle.Bin + (Program1.Bin - nextBin.Bin) +
                   (nextBin.Aisle - lastItemOfFirstAisle.Aisle);
        }

        public double GetDistanceForBottomRoute(Item lastItemOfFirstAisle, Item nextBin)
        {
            return lastItemOfFirstAisle.Bin + nextBin.Bin + (nextBin.Aisle - lastItemOfFirstAisle.Aisle);
        }

        public List<Item> GetSameRouteAisle(List<Item> items, int aisle)
        {
            if (aisle % 2 != 0)
                return null;
            return items.Where(e => e.Aisle == aisle + 1).ToList();
        }
        public List<Item> GetNextAisle(List<Item> sortedList, int aisle)
        {
            var listAisles = sortedList.Where(e => e.Aisle > aisle).OrderBy(e => e.Aisle);
            return listAisles.Where(e => e.Aisle == (listAisles.FirstOrDefault()?.Aisle ?? aisle)).OrderBy(e => e.Bin).ToList();
        }
    }
}
