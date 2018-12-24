using AoC_2018.Helpers;
using AoC_2018.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem11 : BaseProblem, IProblem
    {
        private ulong _gridSerialNumber;

        public void Solve_1()
        {
            CalculatePowerLeveTest();

            _gridSerialNumber = ParseInput();

            IEnumerable<Point> grid = GenerateGrid();

            Dictionary<Point, int> pointPowerLevel = new Dictionary<Point, int>();
            Dictionary<Point, int> pointSquareTotalPower = new Dictionary<Point, int>();

            foreach (Point point in grid)
            {
                IEnumerable<Point> points = Point.GeneratePointRangeIteratingOverXFirst(
                    RangeHelpers.GenerateRange(point.X, point.X + 2),
                    RangeHelpers.GenerateRange(point.Y, point.Y + 2));

                int totalPower = 0;
                foreach (Point p in points)
                {
                    if (pointPowerLevel.TryGetValue(p, out int pPower))
                    {
                        totalPower += pPower;
                    }
                    else
                    {
                        pPower = CalculatePowerLevel(p);
                        pointPowerLevel.Add(p, pPower);
                        totalPower += pPower;
                    }
                }
                pointSquareTotalPower.Add(point, totalPower);
            }

            var pairWithMaxPower = pointSquareTotalPower.OrderByDescending(pair => pair.Value).First();

            Console.Write($"\nDay 11, part 1: {pairWithMaxPower.Key}");
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
        }

        private ulong ParseInput()
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return ulong.Parse(reader.ReadToEnd());
            }
        }

        private IEnumerable<Point> GenerateGrid()
        {
            var rangeXAndY = RangeHelpers.GenerateRange(minValue: 1, maxValue: 300);
            return Point.GeneratePointRangeIteratingOverXFirst(rangeXAndY, rangeXAndY);
        }

        private int CalculatePowerLevel(Point point, ulong gridSerialNumber = 0)
        {
            gridSerialNumber = gridSerialNumber == 0 ? _gridSerialNumber : gridSerialNumber;

            ulong rackId = (ulong)(10 + point.X);

            ulong powerLevel = (rackId * (ulong)point.Y + gridSerialNumber) * rackId;

            int realpowerLevel = powerLevel > 99
                ? int.Parse(powerLevel.ToString().Reverse().ElementAt(2).ToString())
                : 0;

            return realpowerLevel - 5;
        }

        private void CalculatePowerLeveTest()
        {
            if (4 != CalculatePowerLevel(new Point(3, 5), 8)
                || -5 != CalculatePowerLevel(new Point(122, 79), 57)
                || 0 != CalculatePowerLevel(new Point(217, 196), 39)
                || 4 != CalculatePowerLevel(new Point(101, 153), 71))
            {
                throw new Exception("Error in CalculatePowerLevel method");
            }
        }
    }
}
