using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Model
{
    /// <summary>
    /// Equals method and equality operators overriden
    /// https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017
    /// </summary>
    internal class Point : IEquatable<Point>
    {
        internal int X { get; set; }

        internal int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int ManhattanDistance(Point point)
        {
            return Math.Abs(point.X - X) + Math.Abs(point.Y - Y);
        }

        public Point CalculateClosestManhattanPoint(ICollection<Point> candidatePoints)
        {
            Dictionary<Point, int> pointDistanceDictionary = new Dictionary<Point, int>();

            foreach (Point point in candidatePoints)
            {
                pointDistanceDictionary.TryAdd(point, ManhattanDistance(point));
            }

            return pointDistanceDictionary.OrderBy(pair => pair.Value)
                .First().Key;
        }

        /// <summary>
        /// Returns null of there are multiple points at min Manhattan distance
        /// </summary>
        /// <param name="candidatePoints"></param>
        /// <returns></returns>
        public Point CalculateClosestManhattanPointNotTied(ICollection<Point> candidatePoints)
        {
            Dictionary<Point, int> pointDistanceDictionary = new Dictionary<Point, int>();

            foreach (Point point in candidatePoints)
            {
                pointDistanceDictionary.TryAdd(point, ManhattanDistance(point));
            }

            var orderedDictionary = pointDistanceDictionary.OrderBy(pair => pair.Value);

            return pointDistanceDictionary.Values.Count(distance => distance == orderedDictionary.First().Value) == 1
                ? orderedDictionary.First().Key
                : null;
        }

        static public IEnumerable<Point> GeneratePointRangeIteratingOverYFirst(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int x in xRange)
            {
                foreach (int y in yRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        static public IEnumerable<Point> GeneratePointRangeIteratingOverXFirst(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int y in yRange)
            {
                foreach (int x in xRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        #region Equals override
        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Point))
            {
                return false;
            }

            return Equals((Point)obj);
        }

        public bool Equals(Point other)
        {
            if (other == null)
            {
                return false;
            }

            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            if (ReferenceEquals(point1, null))
            {
                return ReferenceEquals(point2, null);
            }

            return point1.Equals(point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            if (ReferenceEquals(point1, null))
            {
                return !ReferenceEquals(point2, null);
            }

            return !point1.Equals(point2);
        }
        #endregion

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
