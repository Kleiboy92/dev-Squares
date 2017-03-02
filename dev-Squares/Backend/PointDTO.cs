using System;

namespace dev_Squares.Backend
{

    public class PointDTO
    {
        public PointDTO()
        {

        }

        public PointDTO(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as PointDTO;

            if (item == null)
                return false;

            if (item.X != this.X)
                return false;

            if (item.Y != this.Y)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}