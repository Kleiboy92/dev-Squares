using System;
using System.Collections.Generic;
using System.Linq;

namespace dev_Squares.Backend
{
    public static class Solver
    {
        public static  IEnumerable<Result> Solve(IEnumerable<Point> container)
        {
            var allPoints = container.ToList();
            //because square needs at least 4 points
            while (allPoints.Count > 4)
            {
                var startingPoint = allPoints[0];
                allPoints.Remove(startingPoint);

                var pointsInSameX = allPoints.Where(x => x.X == startingPoint.X);
                var pointsInSameY = allPoints.Where(x => x.Y == startingPoint.Y);

                if (!pointsInSameX.Any() || !pointsInSameY.Any())
                    continue;

                foreach (var sameXPoint in pointsInSameX)
                {
                    var sideLength = Math.Abs(startingPoint.Y - sameXPoint.Y);

                    foreach (var sameYPoint in pointsInSameY)
                    {
                        var otherSideLength = Math.Abs(startingPoint.X - sameYPoint.X);

                        if (sideLength == otherSideLength)
                        {
                            var lastNeededPoint = allPoints.Where(x => x.Y == sameXPoint.Y && x.X == sameYPoint.X);

                            if (lastNeededPoint.Count() == 1)
                                yield return Result.Order(startingPoint, sameXPoint, sameYPoint, lastNeededPoint.Single());
                        }
                    }
                }
            }
        }
    }
}