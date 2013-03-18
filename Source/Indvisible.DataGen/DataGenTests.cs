namespace Indvisible.DataGen
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using FizzWare.NBuilder;

    using Indvisible.DataGen.Models;

    using NUnit.Framework;

    [TestFixture]
    public class DataGenTests
    {
        [Test]
        public void Creation()
        {
            const int SIZE = 10;
            var list = Builder<SimpleModelWithName>.CreateListOfSize(SIZE, new RealisticPropertyNamer()).Build();
            Assert.IsNotNull(list);
            Assert.AreEqual(SIZE, list.Count);
        }

        [Test]
        public void CreateWithRealisticNames()
        {
            const int SIZE = 10;
            var models = Builder<SimpleModelWithName>.CreateListOfSize(SIZE, new RealisticPropertyNamer()).Build();
            foreach (var model in models)
            {
                Assert.IsNotNull(model);

                Assert.IsNotNull(model.CompanyName);
                Assert.IsFalse(model.CompanyName.Contains("CompanyName"));

                Assert.IsNotNull(model.Id);

                Assert.IsNotNull(model.Name);
                Assert.IsFalse(model.Name.Contains("Name"));

                Assert.IsNotNull(model.LastName);
                Assert.IsFalse(model.LastName.Contains("LastName"));
            }
        }

        [Test]
        public void CreateNestedEntities()
        {
            var model = Builder<SimpleModelWithNested>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();
            Assert.IsNotNull(model.SimpleModel);
        }

        [Test]
        public void CreateNestedEntitiesWithNotNullProperties()
        {
            var model = Builder<SimpleModelWithNested>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();
            Assert.IsNotNull(model.SimpleModel);
            Assert.IsNotNull(model.SimpleModel.Id);
            Assert.IsNotNullOrEmpty(model.SimpleModel.LastName);
        }

        [Test]
        public void ObjectCastTest()
        {
            int variable = 5;
            dynamic obj = variable;
            int a = obj;
            Assert.AreEqual(variable, a);
        }
    }
}