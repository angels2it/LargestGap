using System;
using System.Collections.Generic;
using System.Linq;
using LargestGap.Interfaces;

namespace LargestGap
{
    class Program1
    {
        static List<Item> _aisles;
        static int Aisle = 16;
        public static int Bin = 20;
        private static int haft = Bin / 2;
        static readonly int _aisleStep = 2;
        static List<Item> _lineFormated;
        private static IAisleLayoutProvider _aisleLayout;
        static void Main1(string[] args)
        {
            _aisleLayout = new AisleLayoutProvider();
            InitLineFormat();
            //InitData1();
            //InitData2();
            //InitData3();
            //InitData4();
            //InitData6();
            //InitData7();
            //InitData5();
            //InitData8();
            //StartSort();
            TestRandomData();
            Console.ReadLine();
        }

        private static void TestRandomData()
        {
            for (int i = 0; i < 10000000; i++)
            {
                InitRandomData();
                StartSort();
                // check failed case
                for (int j = 1; j < _aisles.Count - 1; j++)
                {
                    // mid condition
                    if (_aisles[j - 1].Bin > 10 && _aisles[j].Bin <= 10 && _aisles.Skip(j).Any(e => e.Bin > 10))
                    {
                        Console.WriteLine("Failed");
                        break;
                    }
                    // mid condition
                    if (_aisles[j].Bin > 10 && _aisles[j + 1].Bin <= 10 && _aisles.Skip(j + 1).Any(e => e.Bin > 10))
                    {
                        Console.WriteLine("Failed");
                        break;
                    }
                    if (_aisles[j - 1].Bin <= 10 && _aisles[j].Bin > 10 && _aisles[j + 1].Bin <= 10 && _aisles.Skip(j + 1).Any(e => e.Bin > 10))
                    {
                        Console.WriteLine("Failed");
                        break;
                    }

                    // check bottom route and aisle same route
                    var firstAisle = _aisles[j - 1];
                    var nextAisle = _aisles[j];

                    // [2-3] - [3-1]
                    if (firstAisle.Aisle % 2 == 0 && nextAisle.Aisle - firstAisle.Aisle == 1 &&
                        firstAisle.Bin > nextAisle.Bin &&
                        _aisles.Where(e => e.Aisle <= firstAisle.Aisle).All(e => e.Bin <= 10))
                    {
                        Console.WriteLine("Failed");
                        break;
                    }

                    firstAisle = _aisles[j];
                    nextAisle = _aisles[j + 1];
                    if (firstAisle.Aisle % 2 == 0 && nextAisle.Aisle - firstAisle.Aisle == 1 &&
                        firstAisle.Bin > nextAisle.Bin &&
                        _aisles.Where(e => e.Aisle <= firstAisle.Aisle).All(e => e.Bin <= 10))
                    {
                        Console.WriteLine("Failed");
                        break;
                    }
                }
                Console.WriteLine("++++++++++++++++++++++++++");
                _aisles.Clear();
            }
        }

        private static void InitData8()
        {
            _aisles = new List<Item>()
            {
                new Item() {Aisle = 16, Bin = 2},
                new Item() {Aisle = 16, Bin = 9},
                new Item() {Aisle = 1, Bin = 9},
                new Item() {Aisle = 16, Bin = 20},
                //new Item() {Aisle = 2, Bin = 4},
                //new Item() {Aisle = 5, Bin = 3},
                //new Item() {Aisle = 6, Bin = 10},
                //new Item() {Aisle = 9, Bin = 1},
                //new Item() {Aisle = 13, Bin = 7},
                //new Item() {Aisle = 13, Bin = 8},
                //new Item() {Aisle = 15, Bin = 12},
                //new Item() {Aisle = 15, Bin =8},
                //new Item() {Aisle =15, Bin = 15},

                //new Item() {Aisle = 2, Bin = 1},
                //new Item() {Aisle = 4, Bin = 8},
                //new Item() {Aisle = 5, Bin = 2},
                //new Item() {Aisle = 5, Bin = 9},
                //new Item() {Aisle = 7, Bin = 17},
                //new Item() {Aisle = 6, Bin = 19},
                //new Item() {Aisle = 16, Bin = 12},
                //new Item() {Aisle = 16, Bin =13},
                //new Item() {Aisle =12, Bin = 10},
                //new Item() {Aisle = 9, Bin =8},
            };
        }
        static Random r = new Random();
        private static void InitRandomData()
        {
            if (_aisles == null)
                _aisles = new List<Item>();
            if (_aisles.Count == 10)
                return;
            int aisle, bin;

            do
            {
                aisle = r.Next(1, Aisle + 1);
                bin = r.Next(1, Bin + 1);
            } while (_aisles.Any(e => e.Aisle == aisle && e.Bin == bin));
            _aisles.Add(new Item()
            {
                Aisle = aisle,
                Bin = bin
            });
            InitRandomData();
        }

        private static Func<Item, int, int, bool> orderFunc =
            (e, aisle, sameRouteAisle) => e.Aisle == aisle || (e.Aisle == sameRouteAisle);
        private static void StartSort()
        {
            var minAisle = _aisles.Min(e => e.Aisle);
            SpecificFormat(_aisles, minAisle);
            foreach (var item in _aisles)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static void SpecificFormat(List<Item> sortedList, int checkAisle)
        {
            var nextAisle = _aisleLayout.GetNextAisle(sortedList, checkAisle);
            if (nextAisle == null || nextAisle.Count == 0)
            {
                if (sortedList.All(e => e.Aisle == checkAisle))
                {
                    var itemNeedFormat = _aisles
                                .OrderBy(e => e.Bin)
                                .ToList();
                    ReformatItems(_aisles, itemNeedFormat, checkAisle);
                }
                else
                    NormalLargestGapSort(_aisles, checkAisle - 1);
                return;
            }
            var firstAisleItems = sortedList.Where(e => e.Aisle == checkAisle).OrderBy(e => e.Bin);
            List<Item> sameRouteAisle;
            if (firstAisleItems.Any(e => e.Bin > haft))
            {
                sameRouteAisle = _aisleLayout.GetSameRouteAisle(_aisles, checkAisle);
                List<Item> itemNeedFormat;
                if (sameRouteAisle == null || sameRouteAisle.Count == 0)
                {
                    itemNeedFormat = GetItemNeedFormatByAisle(checkAisle);
                    ReformatItems(_aisles, itemNeedFormat, checkAisle);
                    NormalLargestGapSort(_aisles, checkAisle);
                    return;
                }
                itemNeedFormat = GetItemNeedFormatByAisle(checkAisle);
                ReformatItems(_aisles, itemNeedFormat, sameRouteAisle.First().Aisle);
                NormalLargestGapSort(_aisles, sameRouteAisle.First().Aisle);
                return;
            }
            // need reformat
            Item nextBinFromTop;
            if (IsBottomRoute(sortedList, checkAisle, out nextBinFromTop))
            {
                StartSortForBottomRoute(sortedList, checkAisle, nextBinFromTop);
                return;
            }
            sameRouteAisle = _aisleLayout.GetSameRouteAisle(sortedList, checkAisle);
            if (sameRouteAisle != null && sameRouteAisle.Any())
            {
                var itemNeedFormat = GetItemNeedFormatByAisle(checkAisle);
                ReformatItems(sortedList, itemNeedFormat, checkAisle);
                if (itemNeedFormat.All(e => e.Bin <= haft))
                {
                    SpecificFormatForNextAisle(sortedList, nextAisle.First().Aisle);
                }
                else
                {
                    CheckNormalLargestGap(sortedList, itemNeedFormat, nextAisle.First().Aisle);
                }
            }
            else
            {
                var itemNeedFormat =
                        sortedList.Where(e => e.Aisle == checkAisle)
                            .OrderBy(e => e.Bin)
                            .ToList();
                ReformatItems(sortedList, itemNeedFormat, checkAisle);
                if (itemNeedFormat.Max(e => e.Bin) <= haft)
                    SpecificFormat(sortedList, nextAisle.First().Aisle);
                else
                    NormalLargestGapSort(sortedList, checkAisle);
            }
        }

        private static List<Item> GetItemNeedFormatByAisle(int checkAisle)
        {
            var sameRouteAisle = _aisleLayout.GetSameRouteAisle(_aisles, checkAisle);
            // has same route aisle
            if (sameRouteAisle != null && sameRouteAisle.Any())
                return _aisles
                    .Where(e => e.Aisle >= checkAisle && e.Aisle <= sameRouteAisle.First().Aisle)
                    .OrderByDescending(e => orderFunc(e, checkAisle, sameRouteAisle.First().Aisle))
                    .ThenBy(e => e.Bin)
                    .ToList();
            // nope
            return _aisles
                .Where(e => e.Aisle == checkAisle)
                .OrderBy(e => e.Bin)
                .ToList();
        }

        private static bool IsBottomRoute(List<Item> sortedList, int checkAisle, out Item nextBinFromTop)
        {
            var nextAisle = _aisleLayout.GetNextAisle(sortedList, checkAisle);
            if (nextAisle == null)
            {
                nextBinFromTop = null;
                return false;
            }
            var firstAisleItems = sortedList.Where(e => e.Aisle == checkAisle).OrderBy(e => e.Bin);
            var lastItemOfFirstAisle = firstAisleItems.Last();
            // from item to top
            nextBinFromTop = nextAisle.Last();
            var nextOfNextAisle = _aisleLayout.GetNextAisle(sortedList, nextBinFromTop.Aisle);
            var nextBinOfNextAisleFromTop = nextOfNextAisle?.LastOrDefault();
            // Bin with min distance to get route from top line
            var nextBin = nextBinFromTop.Bin > nextBinOfNextAisleFromTop?.Bin
                ? nextBinFromTop
                : nextBinOfNextAisleFromTop ?? nextBinFromTop;
            var distance1 = _aisleLayout.GetDistanceForTopRoute(lastItemOfFirstAisle, nextBin);
            // from item to bottom and next item
            var nextBinFromBottom = nextAisle.First();
            var nextBinOfNextAisleFromBottom = nextOfNextAisle?.LastOrDefault();
            // Bin with min distance for get route from bottom line
            nextBin = nextBinFromBottom.Bin > nextBinOfNextAisleFromBottom?.Bin
                ? nextBinOfNextAisleFromBottom
                : nextBinFromBottom;
            var distance2 = _aisleLayout.GetDistanceForBottomRoute(lastItemOfFirstAisle, nextBin);
            return distance1 > distance2;
        }

        private static void StartSortForBottomRoute(List<Item> sortedList, int checkAisle, Item nextBinFromTop)
        {
            var sameRouteAisle = _aisleLayout.GetSameRouteAisle(sortedList, nextBinFromTop.Aisle);
            var effectAsile = sameRouteAisle != null && sameRouteAisle.Any()
                ? sameRouteAisle.First().Aisle
                : nextBinFromTop.Aisle;
            var itemNeedFormat =
                    sortedList.Where(e => e.Aisle >= checkAisle && e.Aisle <= effectAsile)
                        .OrderByDescending(e => e.Aisle == checkAisle || (checkAisle % 2 == 0 && e.Aisle - checkAisle == 1))
                        .ThenBy(e => e.Bin)
                        .ToList();
            ReformatItems(sortedList, itemNeedFormat, checkAisle);
            if (itemNeedFormat.Max(e => e.Bin) <= haft)
            {
                SpecificFormatForNextAisle(sortedList, effectAsile);
            }
            else
            {
                CheckNormalLargestGap(sortedList, itemNeedFormat, effectAsile);
            }
        }

        private static void SpecificFormatForNextAisle(List<Item> sortedList, int effectAsile)
        {
            var nextAisleNeedFormat = _aisleLayout.GetNextAisle(sortedList, effectAsile)?.FirstOrDefault();
            if (nextAisleNeedFormat != null)
                SpecificFormat(sortedList, nextAisleNeedFormat.Aisle);
        }

        private static void CheckNormalLargestGap(List<Item> sortedList, List<Item> itemNeedFormat, int effectAsile)
        {
            if (effectAsile % 2 != 0 || itemNeedFormat.Any(e => e.Aisle == effectAsile && e.Bin > haft))
                NormalLargestGapSort(sortedList, effectAsile);
            else
            {
                var nextAisleNeedFormat = _aisleLayout.GetNextAisle(sortedList, effectAsile)?.FirstOrDefault();
                //check if need format for next Aisle
                if (nextAisleNeedFormat != null)
                    NormalLargestGapSort(sortedList, nextAisleNeedFormat.Aisle);
            }
        }

        private static void NormalLargestGapSort(List<Item> sortedList, int checkAisle)
        {
            var unSortItems = sortedList.Where(e => e.Aisle > checkAisle).ToList();
            unSortItems =
            unSortItems.OrderBy(e => _lineFormated.FindIndex(f => f.Aisle == e.Aisle && f.Bin == e.Bin))
                .ToList();
            ReformatItems(sortedList, unSortItems, checkAisle);
        }

        private static void ReformatItems(List<Item> sortedList, List<Item> itemNeedFormat, int minAisle)
        {
            sortedList.RemoveAll(itemNeedFormat.Contains);
            var addIndex = sortedList.FindIndex(e => e.Aisle > minAisle);
            if (addIndex == -1)
            {
                addIndex = sortedList.FindLastIndex(e => e.Aisle <= minAisle);
                if (addIndex <= sortedList.Count - 1)
                    addIndex++;
            }
            sortedList.InsertRange(addIndex, itemNeedFormat);
        }


        private static void InitLineFormat()
        {
            _lineFormated = new List<Item>();
            if (Aisle < 1)
            {
                Console.WriteLine("Aisle greater than 1");
                return;
            }
            if (Aisle % 2 != 0)
            {
                Console.WriteLine("Aisle must be even number");
                return;
            }
            int halfBin = Bin / 2;
            if (Aisle == 1)
            {
                _lineFormated.AddRange(GetLineFormated(1, 1, 20, true));
                PrintAisleFormated();
                return;
            }
            if (Aisle == 2)
            {
                _lineFormated.AddRange(GetLineFormated(1, 1, 20, true));
                _lineFormated.AddRange(GetLineFormated(2, 20, 1, false));
                PrintAisleFormated();
                return;
            }
            for (int i = 1; i <= (Aisle - 1) * 2; i++)
            {
                var currentAisle = i <= Aisle ? i : Aisle * 2 - i;
                bool isForward = i < Aisle;
                // is plus Bin step
                bool startCount = (i - 1) % _aisleStep == 0;
                int start;
                int end;
                // first line we will read all Bin from 1 - total Bin
                if (i == 1)
                {
                    start = 1;
                    end = 20;
                }
                // last line we will read all Bin from total Bin - 1
                else if (i == Aisle)
                {
                    start = 20;
                    end = 1;
                }
                else
                {
                    if (startCount)
                    {
                        start = isForward ? halfBin + 1 : 1;
                        end = isForward ? Bin : halfBin;
                    }
                    else
                    {
                        start = isForward ? Bin : halfBin;
                        end = isForward ? halfBin + 1 : 1;
                    }
                }
                _lineFormated.AddRange(GetLineFormated(currentAisle, start, end, startCount));
            }
            PrintAisleFormated();
        }

        private static void PrintAisleFormated()
        {
            return;
            foreach (var item in _lineFormated)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static List<Item> GetLineFormated(int currentAisle, int start, int end, bool startCount)
        {
            List<Item> items = new List<Item>();
            if (startCount)
                for (int j = start; j <= end; j++)
                {
                    items.Add(new Item()
                    {
                        Aisle = currentAisle,
                        Bin = j
                    });
                }
            else
                for (int j = start; j >= end; j--)
                {
                    items.Add(new Item()
                    {
                        Aisle = currentAisle,
                        Bin = j
                    });
                }
            return items;
        }

        private static void InitData1()
        {
            _aisles = new List<Item>()
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
                new Item{Aisle = 1,Bin = 2}
            };
        }
        private static void InitData2()
        {
            _aisles = new List<Item>()
            {
                new Item{Aisle = 1,Bin = 2},
                //new Item{Aisle = 1,Bin = 13},
                new Item{Aisle = 2,Bin = 4},
                new Item{Aisle =3,Bin = 2},
                new Item{Aisle = 2,Bin = 3},
                new Item{Aisle = 5,Bin = 3},
                //new Item{Aisle = 4,Bin = 3},
                //new Item{Aisle = 14,Bin = 14},
                //new Item{Aisle = 12,Bin = 17},
                new Item{Aisle = 13,Bin = 5},
                new Item{Aisle = 13,Bin = 7},
                new Item{Aisle = 12,Bin = 1},
                //new Item{Aisle = 12,Bin = 15},
                new Item{Aisle = 6,Bin = 3},
                new Item{Aisle = 8,Bin = 7},
                new Item{Aisle = 7,Bin = 2},
                new Item{Aisle = 14,Bin = 2},
                new Item{Aisle = 14,Bin = 10},
                new Item{Aisle = 16,Bin = 20},
                new Item{Aisle = 16,Bin = 17},
                new Item{Aisle = 15,Bin = 17},
                new Item{Aisle = 15,Bin = 5},
//                new Item{Aisle = 9,Bin = 15},
                //new Item{Aisle =2,Bin = 15},
                new Item{Aisle = 12,Bin = 9},
                new Item{Aisle = 7,Bin = 11},
            };
        }
        private static void InitData3()
        {
            _aisles = new List<Item>()
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
                new Item{Aisle = 3,Bin = 17},
                new Item{Aisle = 2,Bin = 5},
                new Item{Aisle = 2,Bin = 2},
                new Item{Aisle = 1,Bin = 7},
                new Item{Aisle = 1,Bin = 2},
                new Item{Aisle = 3,Bin = 3}
            };
        }
        private static void InitData4()
        {
            _aisles = new List<Item>()
            {
                new Item{Aisle = 14,Bin = 7},
                new Item{Aisle = 14,Bin = 3},
                new Item{Aisle = 11,Bin = 5},
                new Item{Aisle = 10,Bin = 16},
                new Item{Aisle = 11,Bin = 17},
                new Item{Aisle = 10,Bin = 3},
                new Item{Aisle = 9,Bin = 6},
                new Item{Aisle = 9,Bin = 16},
                new Item{Aisle = 7,Bin = 14},
                new Item{Aisle = 6,Bin = 17},

                new Item{Aisle = 3,Bin = 7},
                new Item{Aisle = 3,Bin = 17},
                new Item{Aisle = 2,Bin = 5},
                new Item{Aisle = 2,Bin = 2},
                new Item{Aisle = 1,Bin = 7},
                new Item{Aisle = 1,Bin = 2},
                new Item{Aisle = 4,Bin = 7},
                new Item{Aisle = 4,Bin = 17},
            };
        }
        private static void InitData6()
        {
            _aisles = new List<Item>()
            {
                new Item{Aisle = 14,Bin = 7},
                new Item{Aisle = 14,Bin = 3},
                new Item{Aisle = 11,Bin = 5},
                new Item{Aisle = 10,Bin = 16},
                new Item{Aisle = 11,Bin = 17},
                new Item{Aisle = 10,Bin = 3},
                new Item{Aisle = 9,Bin = 6},
                new Item{Aisle = 9,Bin = 16},
                new Item{Aisle = 7,Bin = 14},
                new Item{Aisle = 6,Bin = 17},

                new Item{Aisle = 3,Bin = 7},
                new Item{Aisle = 2,Bin = 5},
                new Item{Aisle = 2,Bin = 2},
                new Item{Aisle = 1,Bin = 7},
                new Item{Aisle = 1,Bin = 2},
                new Item{Aisle = 4,Bin = 7},
                new Item{Aisle = 4,Bin = 4},
                new Item{Aisle = 4,Bin = 1},
            };
        }
        private static void InitData7()
        {
            _aisles = new List<Item>()
            {
                new Item{Aisle = 14,Bin = 7},
                new Item{Aisle = 14,Bin = 3},
                new Item{Aisle = 11,Bin = 5},
                new Item{Aisle = 10,Bin = 16},
                new Item{Aisle = 11,Bin = 17},
                new Item{Aisle = 10,Bin = 3},
                new Item{Aisle = 9,Bin = 6},
                new Item{Aisle = 9,Bin = 16},
                new Item{Aisle = 7,Bin = 8},
                new Item{Aisle = 6,Bin = 17},

                new Item{Aisle = 3,Bin = 7},
                new Item{Aisle = 2,Bin = 5},
                new Item{Aisle = 2,Bin = 2},
                new Item{Aisle = 1,Bin = 7},
                new Item{Aisle = 1,Bin = 2},
                new Item{Aisle = 4,Bin = 7},
                new Item{Aisle = 4,Bin = 4},
                new Item{Aisle = 4,Bin = 1},
            };
        }
        private static void InitData5()
        {
            _aisles = new List<Item>()
            {
                new Item{Aisle = 2,Bin = 4},
                new Item{Aisle =3,Bin = 2},
                new Item{Aisle = 2,Bin = 3},
                new Item{Aisle = 5,Bin = 3},
                //new Item{Aisle = 4,Bin = 3},
                new Item{Aisle = 16,Bin = 6},
                new Item{Aisle = 14,Bin = 14},
                new Item{Aisle = 12,Bin = 17},
                new Item{Aisle = 13,Bin = 5},
                new Item{Aisle = 12,Bin = 15},
                new Item{Aisle = 6,Bin = 3},
                new Item{Aisle = 8,Bin = 1},
                new Item{Aisle = 7,Bin = 2},
                new Item{Aisle = 9,Bin = 2},
                new Item{Aisle = 8,Bin = 20},
                new Item{Aisle = 2,Bin = 19},
                new Item{Aisle = 1,Bin = 9},
                new Item{Aisle = 5,Bin = 11},
                new Item{Aisle = 5,Bin = 10},
            };
        }
    }

    public class Item
    {
        public int Aisle { get; set; }
        public int Bin { get; set; }
        public override string ToString()
        {
            return $"{Aisle} - {Bin}";
        }
    }
}

