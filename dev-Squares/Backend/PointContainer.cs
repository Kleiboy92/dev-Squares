using System;
using System.Collections.Generic;
using System.Collections;

namespace dev_Squares.Backend
{

    public class PointContainer : IEnumerable<Point>
    {
        private readonly int maxSize;

        private HashSet<Point> container = new HashSet<Point>();

        public int Length { get; private set; } = 0;

        public PointContainer(int size = Restrictions.maxSize)
        {
            this.maxSize = size;
        }

        public void Add(Point point)
        {
            if (container.Count >= Restrictions.maxSize)
                throw new InvalidOperationException("container reached max capacity");

            if (container.Contains(point))
                throw new ArgumentException(string.Format("container already has this element x:{0} y {1}", point.X, point.Y));

            container.Add(point);
            this.Length++;
        }

        public void AddRange(IEnumerable<Point> points, Action<ParsingInfo> infoOutput)
        {
            var exit = false;
            var pointsAdded = 0;
            foreach (var point in points)
            {
                try
                {
                    Add(point);
                    pointsAdded++;
                }
                catch(InvalidOperationException e)
                {
                    infoOutput(new ParsingInfo(0, e.Message, ParserType.danger));
                    exit = true;
                }
                catch(ArgumentException e)
                {
                    infoOutput(new ParsingInfo(0, e.Message, ParserType.danger));
                    exit = false;
                }

                if (exit)
                    break;
            }
            infoOutput(new ParsingInfo(0, "points added: " + pointsAdded, ParserType.success));

        }

        public void Remove(Point point)
        {
            if (container.Contains(point))
            {
                container.Remove(point);
                Length--;
            }
        }

        public void Clear()
        {
            container.Clear();
            Length = 0;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}