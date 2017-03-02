using dev_Squares.Backend;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dev_Squares.Tests.WorkerTests
{
    public class ParserTests
    {
        [Test]
        public void DefaultCase()
        {
            var import = @"0 0
            0 10
            10 0
            10 -10
            5 5";

            var errors = new List<ParsingInfo>();
            var points = PointParser.ParseFile(import, (x) => errors.Add(x)).ToArray();
            Assert.That(points.Length, Is.EqualTo(5));
            Assert.That(errors.Count, Is.EqualTo(0));

            Assert.That(points[0].X, Is.EqualTo(0));
            Assert.That(points[0].Y, Is.EqualTo(0));
            Assert.That(points[1].X, Is.EqualTo(0));
            Assert.That(points[1].Y, Is.EqualTo(10));
            Assert.That(points[2].X, Is.EqualTo(10));
            Assert.That(points[2].Y, Is.EqualTo(0));
            Assert.That(points[3].X, Is.EqualTo(10));
            Assert.That(points[3].Y, Is.EqualTo(-10));
            Assert.That(points[4].X, Is.EqualTo(5));
            Assert.That(points[4].Y, Is.EqualTo(5));
        }

        [Test]
        public void UnparsableRow()
        {
            var import = @"0 0
            0 10
            !@#$
            10 10
            5 5";

            var errors = new List<ParsingInfo>();
            var points = PointParser.ParseFile(import, (x) => errors.Add(x)).ToArray();

            Assert.That(points.Length, Is.EqualTo(4));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.Single().Row, Is.EqualTo(2));

            Console.WriteLine(errors.Single().Message);
        }

        [Test]
        public void OverEdges()
        {
            var import = @"0 0
            0 10
            -5001 0
            10 5001
            5 5";

            var errors = new List<ParsingInfo>();
            var points = PointParser.ParseFile(import, (x) => errors.Add(x)).ToArray();

            Assert.That(points.Length, Is.EqualTo(3));
            Assert.That(errors.Count, Is.EqualTo(2));
            Assert.That(errors.First().Row, Is.EqualTo(2));
            Assert.That(errors.Last().Row, Is.EqualTo(3));

            Console.WriteLine(errors.First().Message);
        }
    }
}