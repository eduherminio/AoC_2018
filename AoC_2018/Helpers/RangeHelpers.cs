using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Helpers
{
    public static class RangeHelpers
    {
        public static IEnumerable<int> GenerateRange(int minValue, int maxValue)
        {
            return Enumerable.Range(minValue, maxValue - minValue + 1);
        }
    }
}
