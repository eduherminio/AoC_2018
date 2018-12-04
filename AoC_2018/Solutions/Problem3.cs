using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2018.Solutions
{
    class Problem3 : IProblem
    {
        /// <summary>
        /// Equals method and equality operators overriden
        /// https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1815-override-equals-and-operator-equals-on-value-types?view=vs-2017
        /// </summary>
        class Point : IEquatable<Point>
        {
            int X { get; set; }

            int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override int GetHashCode()
            {
                return X ^ Y;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Point))
                    return false;

                return Equals((Point)obj);
            }

            public bool Equals(Point other)
            {
                if (X != other.X)
                    return false;

                return Y == other.Y;
            }

            public static bool operator ==(Point point1, Point point2)
            {
                return point1.Equals(point2);
            }

            public static bool operator !=(Point point1, Point point2)
            {
                return !point1.Equals(point2);
            }

        }

        class Rectangle
        {
            public int Id { get; set; }
            public HashSet<Point> PointSet { get; set; } = new HashSet<Point>();

            public Rectangle(int x, int y, int x0, int y0)
            {
                for (int xIndex = x0 + 1; xIndex <= x0 + x; ++xIndex)
                {
                    for (int yIndex = y0; yIndex < y0 + y; ++yIndex)
                    {
                        PointSet.Add(new Point(xIndex, yIndex));
                    }
                }
                if (x * y != PointSet.Count)
                {
                    throw new Exception("Error in rectangle constructor");
                }
            }
        }

        public string FilePath => Path.Combine("Inputs", "3.in");

        public void Solve_1()
        {
            List<Rectangle> rectangles = ParseInput().ToList();

            HashSet<Point> evaluatedPoints = new HashSet<Point>();
            HashSet<Point> repeatedPoints = new HashSet<Point>();

            foreach (Rectangle rectangle in rectangles)
            {
                foreach (Point point in rectangle.PointSet)
                {
                    if (!evaluatedPoints.Add(point))
                    {
                        repeatedPoints.Add(point);
                    }
                }
            }

            Console.WriteLine($"Day 3, part 1: {repeatedPoints.Count}");
        }

        public void Solve_2()
        {
            List<Rectangle> rectangles = ParseInput().ToList();

            HashSet<Point> evaluatedPoints = new HashSet<Point>();
            HashSet<Point> repeatedPoints = new HashSet<Point>();
            foreach (Rectangle rectangle in rectangles)
            {
                foreach (Point point in rectangle.PointSet)
                {
                    if (!evaluatedPoints.Add(point))
                    {
                        repeatedPoints.Add(point);
                    }
                }
            }

            foreach (Rectangle rectangle in rectangles)
            {
                bool isIntact = true;
                foreach (Point point in rectangle.PointSet)
                {
                    if (repeatedPoints.TryGetValue(point, out Point _))
                    {
                        isIntact = false;
                        break;
                    }
                }

                if (isIntact)
                {
                    Console.WriteLine($"Day 3, part 2: {rectangle.Id}");
                }
            }
        }

        private IEnumerable<Rectangle> ParseInput()
        {
            ICollection<Rectangle> rectangles = new List<Rectangle>();

            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();
                string idString = parsedLine.NextElement<string>();
                if (!idString.StartsWith("#"))
                {
                    throw new Exception($"{idString} is not #n");
                }

                int.TryParse(idString.Trim('#'), out int id);

                string at = parsedLine.NextElement<string>();
                if (at != "@")
                {
                    throw new Exception($"{at} is not @");
                }

                string x0y0 = parsedLine.NextElement<string>();
                int.TryParse(x0y0.Substring(0, x0y0.IndexOf(',')), out int x0);
                int.TryParse(x0y0.Substring(x0y0.IndexOf(',') + 1).Trim(':'), out int y0);

                string xy = parsedLine.NextElement<string>();

                if (!parsedLine.Empty)
                {
                    throw new Exception($"Error parsing line, missing at least {parsedLine.PeekNextElement<string>()}");
                }

                int.TryParse(xy.Substring(0, xy.IndexOf('x')), out int x);
                int.TryParse(xy.Substring(xy.IndexOf('x') + 1), out int y);

                rectangles.Add(new Rectangle(x: x, y: y, x0: x0, y0: y0) { Id = id });
            }
            return rectangles;
        }
    }
}
