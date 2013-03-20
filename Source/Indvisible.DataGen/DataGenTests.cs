namespace Indvisible.DataGen
{
    using System;
    using System.Collections;

    using FizzWare.NBuilder;
    using FizzWare.NBuilder.Implementation;

    using Indvisible.DataGen.Models;

    using NUnit.Framework;

    [TestFixture]
    public class DataGenTests
    {
        [Test]
        public void Creation()
        {
            const int SIZE = 10;
            var list = Builder<ModelWithName>.CreateListOfSize(SIZE, new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(list);
            Assert.AreEqual(SIZE, list.Count);
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
            var model = Builder<ModelWithNested>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model.Model);
        }

        [Test]
        public void CreateNestedEntitiesWithNotNullProperties()
        {
            var model = Builder<ModelWithNested>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model.Model);
            Assert.IsNotNull(model.Model.Id);
            Assert.IsNotNullOrEmpty(model.Model.LastName);
        }

        [Test]
        public void CreateModelWithNestedElementsDeep2()
        {
            var model = Builder<ModelWithDeep2>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ModelWithNested);
            Assert.IsNotNull(model.ModelWithNested.Model);
        }

        [Test]
        public void CreateModelWithNestedElementsDeep3()
        {
            var model = Builder<ModelWithDeep3>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.ModelWithDeep2);
            Assert.IsNotNull(model.ModelWithDeep2.ModelWithNested);
            Assert.IsNotNull(model.ModelWithDeep2.ModelWithNested.Model);
        }

        [Test]
        public void TestHierarhicalModel()
        {
            var model = Builder<HierarhicalModel>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();
            Assert.DoesNotThrow(() => new StackOverflowException());
        }

        [Test]
        public void CreateWithDates()
        {
            var user = Builder<User>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();
            
            Assert.IsNotNull(user);
            Assert.IsNotNull(user.DateObirth);
        }

        [Test]
        public void CreateWithDatesWithFromDateLimit()
        {
            var dateTime = new DateTime(2010, 1, 1);
            BuilderSetup.DateFromRestriction = dateTime;
            for (int i = 0; i < 100; i++)
            {
                var user = Builder<User>.CreateNewWithPropertyNamer(new RealisticPropertyNamer()).Build();

                Assert.IsNotNull(user);
                Assert.IsNotNull(user.DateObirth);
                Assert.IsTrue(user.DateObirth > dateTime);
            }
        }
    }

    class Mail
    {
        public User Sender { get; set; }

        public User Receiver { get; set; }

        public string MailText { get; set; }
    }
}