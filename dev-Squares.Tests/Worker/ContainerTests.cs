using dev_Squares.Backend;
using NUnit.Framework;
using System;
using System.Linq;

namespace dev_Squares.Tests.WorkerTests
{

    public class ContainerTests
    {
        [Test]
        public void CantAddAfterMaxCapacity()
        {
            var pointContainer = new PointContainer(0);
            Assert.Throws<InvalidOperationException>(() => pointContainer.Add(Point.TryCreate(0, 0)));
        }

        [Test]
        public void CantAddDuplicate()
        {
            var point = Point.TryCreate(0, 0);
            var pointContainer = new PointContainer(5);
            pointContainer.Add(point);
            Assert.Throws<ArgumentException>(() => pointContainer.Add(point));
            Assert.That(pointContainer.Count(), Is.EqualTo(1)); 
        }

        [Test]
        public void RemoveWorks()
        {
            var point = Point.TryCreate(0, 0);
            var pointContainer = new PointContainer(5);
            pointContainer.Add(point);
            pointContainer.Remove(point);
            Assert.That(pointContainer.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AddWorks()
        {
            var point = Point.TryCreate(5, 55);
            var pointContainer = new PointContainer(5);
            Assert.That(pointContainer.Count(), Is.EqualTo(0));
            pointContainer.Add(point);
            Assert.That(pointContainer.Count(), Is.EqualTo(1));
            Assert.That(pointContainer.Single().X, Is.EqualTo(5));
            Assert.That(pointContainer.Single().Y, Is.EqualTo(55));
        }
    }
}