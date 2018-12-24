using AoC_2018.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem10 : BaseProblem, IProblem
    {
        public void Solve_1()
        {
            HashSet<Star> stars = FilterInput(ParseInput());

            // 342 stars, from [~-50_000,~-50_000] to [~+50_000, ~+50_000] initially

            PrintStarsPosition(stars);

            bool end = false;
            do
            {
                SimulateOneSecond(ref stars);

                end = PrintStarsPosition(stars);
            } while (!end);

            Console.Write($"\nDay 10, part 1 (they are 'Z', not '2', in case you're wondering");
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Star> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                string firstPart = parsedLine.NextElement<string>();
                if (firstPart.EndsWith('<'))
                {
                    firstPart += parsedLine.NextElement<string>();
                }
                string x = firstPart.Substring(firstPart.IndexOf("position=<") + "position=<".Length).TrimStart('<').TrimEnd(',');

                string secondPart = parsedLine.NextElement<string>();
                string y = secondPart.Trim().Trim('>');

                string thirdPart = parsedLine.NextElement<string>();
                if (thirdPart.EndsWith('<'))
                {
                    thirdPart += parsedLine.NextElement<string>();
                }
                string speedX = thirdPart.Substring(firstPart.IndexOf("velocity=<") + "velocity=<".Length).TrimStart('<').TrimEnd(',');

                string fourthPart = parsedLine.NextElement<string>();
                string speedY = fourthPart.Trim().Trim('>');

                if (!parsedLine.Empty)
                {
                    throw new Exception("Exception parsing lines, some elements left");
                }

                yield return new Star(int.Parse(x), int.Parse(y), new Point(int.Parse(speedX), int.Parse(speedY)));
            }
        }

        private HashSet<Star> FilterInput(IEnumerable<Star> stars) => stars.ToHashSet();

        private void SimulateOneSecond(ref HashSet<Star> stars)
        {
            foreach (Star star in stars)
            {
                star.X += star.Speed.X;
                star.Y += star.Speed.Y;
            }
        }

        private bool PrintStarsPosition(HashSet<Star> stars)
        {
            Point downLeft = new Point(stars.Min(star => star.X), stars.Min(star => star.Y));
            Point upRight = new Point(stars.Max(star => star.X), stars.Max(star => star.Y));

            if (Math.Abs(downLeft.X - upRight.X) < 150
                && Math.Abs(downLeft.Y - upRight.Y) < 150)
            {
                Console.Clear();

                var xRange = Enumerable.Range(downLeft.X, upRight.X - downLeft.X + 1);
                var yRange = Enumerable.Range(downLeft.Y, upRight.Y - downLeft.Y + 1);

                var points = Point.GenerateAlternativePointRange(xRange, yRange);
                int y0 = points.First().Y;

                foreach (Point point in points)
                {
                    if (point.Y != y0)
                    {
                        y0 = point.Y;
                        Console.WriteLine();
                    }

                    if (stars.Any(star => star.X == point.X && star.Y == point.Y))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }

                Console.WriteLine();
                string end = Console.ReadLine();

                return end.ToUpperInvariant() == "Y";
            }

            return false;
        }

        private IEnumerable<Point> GeneratePointRange(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int x in xRange)
            {
                foreach (int y in yRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private class Star : Point
        {
            public Point Speed { get; set; }

            public Star(int X, int Y, Point speed) : base(X, Y)
            {
                Speed = speed;
            }

            #region Equals override
            public override int GetHashCode()
            {
                return X ^ Y ^ Speed.X ^ Speed.Y;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                if (!(obj is Star))
                {
                    return false;
                }

                return Equals((Star)obj);
            }

            public bool Equals(Star other)
            {
                if (other == null)
                {
                    return false;
                }

                return X == other.X && Y == other.Y && Speed.X == other.Speed.X && Speed.Y == other.Speed.Y;
            }

            public static bool operator ==(Star star1, Star star2)
            {
                if (ReferenceEquals(star1, null))
                {
                    return ReferenceEquals(star2, null);
                }

                return star1.Equals(star2);
            }

            public static bool operator !=(Star star1, Star star2)
            {
                if (ReferenceEquals(star1, null))
                {
                    return !ReferenceEquals(star2, null);
                }

                return !star1.Equals(star2);
            }
            #endregion
        }
    }
}
