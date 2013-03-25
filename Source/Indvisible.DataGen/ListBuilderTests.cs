namespace Indvisible.DataGen
{
    using FizzWare.NBuilder;

    using Indvisible.DataGen.Models;

    using NUnit.Framework;

    [TestFixture]
    public class ListBuilderTests
    {

        [Test]
        public void CreateListWithRealisticDtaa()
        {
            var list = Builder<ModelWithName>.CreateListOfSize(10, new RealisticPropertyNamer()).Build();

            foreach (var model in list)
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
        public void CreateListWithNestedEntities()
        {
            var list = Builder<ModelWithNested>.CreateListOfSize(10, new RealisticPropertyNamer()).Build();
            foreach (var item in list)
            {
                Assert.IsNotNull(item.Model);
            }
        }

        [Test]
        public void CreateListWithNestedIList()
        {
            var list = Builder<ModelWithNestedIList>.CreateListOfSize(10, new RealisticPropertyNamer()).Build();
            foreach (var item in list)
            {
                Assert.IsNotNull(item.ModelsWithName);
            }
        }

        [Test]
        public void CreateListWithNestedCollection()
        {
            var list = Builder<ModelWithNestedCollection>.CreateListOfSize(10, new RealisticPropertyNamer()).Build();
            foreach (var item in list)
            {
                Assert.IsNotNull(item.ModelsWithName);
            }
        }

        [Test]
        public void CreateListWithNestedEnumerable()
        {
            var list = Builder<ModelWithNestedEnumerable>.CreateListOfSize(10, new RealisticPropertyNamer()).Build();
            foreach (var item in list)
            {
                Assert.IsNotNull(item.ModelsWithName);
            }
        }
    }
}