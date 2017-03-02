using System.Linq;

namespace dev_Squares.Backend
{

    public class Result
    {
        public readonly Point SouthWest;
        public readonly Point NorthWest;
        public readonly Point SouthEast;
        public readonly Point NorthEast;

        public Result(Point southWest, Point northWest, Point southEast, Point northEast)
        {
            this.SouthWest = southWest;
            this.NorthWest = northWest;
            this.SouthEast = southEast;
            this.NorthEast = northEast;
        }

        public static Result Order(Point p1, Point p2, Point p3, Point p4)
        {
            var pnts = new Point[] { p1, p2, p3, p4 };

            var maxX = pnts.Max(x => x.X);
            var minX = pnts.Min(x => x.X);
            var maxY = pnts.Max(x => x.Y);
            var minY = pnts.Min(x => x.Y);

            var sw = pnts.Where(x => x.X == minX && x.Y == minY).Single();
            var se = pnts.Where(x => x.X == maxX && x.Y == minY).Single();
            var nw = pnts.Where(x => x.X == minX && x.Y == maxY).Single();
            var ne = pnts.Where(x => x.X == maxX && x.Y == maxY).Single();

            return new Result(sw, nw, se, ne);
        }

        public override string ToString()
        {
              return   
                string.Format("SW-{0}:{1}|", SouthWest.X, SouthWest.Y) +
                string.Format("SE-{0}:{1}|", SouthEast.X, SouthEast.Y) +
                string.Format("NW-{0}:{1}|", NorthWest.X, NorthWest.Y) +
                string.Format("NE-{0}:{1}", NorthEast.X, NorthEast.Y);
        }
    }
}