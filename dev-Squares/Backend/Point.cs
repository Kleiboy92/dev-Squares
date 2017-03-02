using System;
using System.Text.RegularExpressions;

namespace dev_Squares.Backend
{

        public struct Point
        {
            public readonly int Y;
            public readonly int X;

            Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static Point TryCreate(int x, int y)
            {
                if (x > Restrictions.maxConstraint)
                    throw new ArgumentException("x cannot be bigger than " + Restrictions.maxConstraint);

                if (x < Restrictions.minConstraint)
                    throw new ArgumentException("x cannot be less than " + Restrictions.minConstraint);

                if (y > Restrictions.maxConstraint)
                    throw new ArgumentException("y cannot be bigger than " + Restrictions.maxConstraint);

                if (y < Restrictions.minConstraint)
                    throw new ArgumentException("y cannot be less than " + Restrictions.minConstraint);

                return new Point(x, y);
            }

            public static Point Parse(string line)
            {
                var pattern = "(-?[0-9]\\d*) (-?[0-9]\\d*)";
                var match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    var x = int.Parse(match.Groups[1].Value);
                    var y = int.Parse(match.Groups[2].Value);
                    return TryCreate(x, y);
                }
                else
                {
                    throw new ArgumentException("unable to parse value: " + line);
                }
            }
        }
}