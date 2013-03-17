using FizzWare.NBuilder;
using FizzWare.NBuilder.Implementation;
using Indvisible.DataGen.Models;
using NUnit.Framework;

namespace Indvisible.DataGen
{
    [TestFixture]
    public class DataGenTests
    {
        [Test]
        public void TestNames()
        {
            const int size = 10;
            var list = Builder<SimpleModelWithName>.CreateListOfSize(size, new FakerPropertyNamer()).Build();
            Assert.IsNotNull(list);
            Assert.AreEqual(size, list.Count);
        }
    }
}