namespace Indvisible.DataGen
{
    using System;

    using FizzWare.NBuilder;

    using Indvisible.DataGen.Models;

    using NUnit.Framework;

    [TestFixture]
    public class SingleBuilderTests
    {
        [SetUp]
        public void Setup()
        {
            BuilderSetup.DateFromRestriction = null;
            BuilderSetup.DateToRestriction = null;
        }

        [Test]
        public void CreateWithRealisticNames()
        {
            const int SIZE = 10;
            var models = Builder<ModelWithName>.CreateListOfSize(SIZE, new RealisticPropertyNamer()).Build();

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
            var model = Builder<ModelWithNested>.CreateNew(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model.Model);
        }

        [Test]
        public void CreateNestedEntitiesWithNotNullProperties()
        {
            var model = Builder<ModelWithNested>.CreateNew(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model.Model);
            Assert.IsNotNull(model.Model.Id);
            Assert.IsNotNullOrEmpty(model.Model.LastName);
        }

        [Test]
        public void CreateModelWithNestedElementsDeep2()
        {
            var model = Builder<ModelWithDeep2>.CreateNew(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ModelWithNested);
            Assert.IsNotNull(model.ModelWithNested.Model);
        }

        [Test]
        public void CreateModelWithNestedElementsDeep3()
        {
            var model = Builder<ModelWithDeep3>.CreateNew(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ModelWithDeep2);
            Assert.IsNotNull(model.ModelWithDeep2.ModelWithNested);
            Assert.IsNotNull(model.ModelWithDeep2.ModelWithNested.Model);
        }

        //[Test]
        public void TestHierarhicalModel()
        {
            var model = Builder<HierarhicalModel>.CreateNew(new RealisticPropertyNamer()).Build();
            Assert.DoesNotThrow(() => new StackOverflowException());
        }

        [Test]
        public void CreateWithDates()
        {
            var user = Builder<Customer>.CreateNew(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.DateBirth);
        }

        [Test]
        public void CreateWithDatesWithDateFromRestriction()
        {
            var dateTime = new DateTime(2010, 1, 1);
            BuilderSetup.DateFromRestriction = dateTime;

            for (var i = 0; i < 100; i++)
            {
                var user = Builder<Customer>.CreateNew(new RealisticPropertyNamer()).Build();

                Assert.IsNotNull(user);
                Assert.IsNotNull(user.DateBirth);
                Assert.IsTrue(user.DateBirth > dateTime);
            }
        }

        [Test]
        public void CreateWithDatesWithDateToRestriction()
        {
            var dateTime = new DateTime(2010, 1, 1);
            BuilderSetup.DateToRestriction = dateTime;

            for (var i = 0; i < 100; i++)
            {
                var user = Builder<Customer>.CreateNew(new RealisticPropertyNamer()).Build();

                Assert.IsNotNull(user);
                Assert.IsNotNull(user.DateBirth);
                Assert.IsTrue(user.DateBirth < dateTime);
            }
        }

        [Test]
        public void CreateWithDatesRestrictions()
        {
            var dateFromRestriction = new DateTime(2000, 1, 1);
            BuilderSetup.DateFromRestriction = dateFromRestriction;

            var dateToRestriction = new DateTime(2010, 1, 1);
            BuilderSetup.DateToRestriction = dateToRestriction;

            Assert.True(dateFromRestriction < dateToRestriction, "Incorrect dates");

            for (var i = 0; i < 100; i++)
            {
                var user = Builder<Customer>.CreateNew(new RealisticPropertyNamer()).Build();

                Assert.IsNotNull(user);
                Assert.IsNotNull(user.DateBirth);
                Assert.IsTrue(user.DateBirth < dateToRestriction);
            }
        }
    }
}