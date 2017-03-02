using NUnit.Framework;
using System;
using System.Linq;
using dev_Squares.Backend;

namespace dev_Squares.Tests.WorkerTests
{
    class DatabaseTests
    {
        [Test]
        public void PointContainerSavingAndRetrieving()
        {
            var rep = new PointContainerRepository("test");
            rep.CleanAllDb();

            var cntnr = new PointContainer();
            cntnr.Add(Point.TryCreate(5, 0));
            cntnr.Add(Point.TryCreate(5, 1));
            cntnr.Add(Point.TryCreate(5, 2));
            cntnr.Add(Point.TryCreate(5, 3));
            cntnr.Add(Point.TryCreate(5, 4));
            cntnr.Add(Point.TryCreate(5, 5));
            cntnr.Add(Point.TryCreate(5, 6));
            cntnr.Add(Point.TryCreate(5, 7));
            cntnr.Add(Point.TryCreate(5, 8));
            cntnr.Add(Point.TryCreate(5, 9));

            Assert.That(rep.GetNames().Length, Is.EqualTo(0));
            rep.Save("testName", cntnr);
            Assert.That(rep.GetNames().Length, Is.EqualTo(1));
            Assert.That(rep.GetNames().Single(), Is.EqualTo("testName"));
            var loadedRep = rep.GetByName("testName").ToArray();
            Assert.That(loadedRep.Length, Is.EqualTo(10));

            Assert.That(loadedRep[0], Is.EqualTo(Point.TryCreate(5, 0)));
            Assert.That(loadedRep[1], Is.EqualTo(Point.TryCreate(5, 1)));
            Assert.That(loadedRep[2], Is.EqualTo(Point.TryCreate(5, 2)));
            Assert.That(loadedRep[3], Is.EqualTo(Point.TryCreate(5, 3)));
            Assert.That(loadedRep[4], Is.EqualTo(Point.TryCreate(5, 4)));
            Assert.That(loadedRep[5], Is.EqualTo(Point.TryCreate(5, 5)));
            Assert.That(loadedRep[6], Is.EqualTo(Point.TryCreate(5, 6)));
            Assert.That(loadedRep[7], Is.EqualTo(Point.TryCreate(5, 7)));
            Assert.That(loadedRep[8], Is.EqualTo(Point.TryCreate(5, 8)));
            Assert.That(loadedRep[9], Is.EqualTo(Point.TryCreate(5, 9)));
        }


        [Test]
        public void GetWithMissingNameShouldThrow()
        {
            var rep = new PointContainerRepository("test");
            rep.CleanAllDb();
        
            Assert.Throws<InvalidOperationException>(() => rep.GetByName("testName"));
        }

    }
}
