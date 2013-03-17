using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Indvisible.RealisticData.Tests
{
    public class AddressTests
    {
        [Test]
        public void TestCity()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetCity(), @"[ a-z]+"));
        }

        [Test]
        public void TestCityPrefix()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetCityPrefix(), @"[ a-z]"));
        }

        [Test]
        public void TestCitySuffix()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetCitySuffix(), @"[ a-z]"));
        }

        [Test]
        public void TestSecondaryAddress()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetSecondaryAddress(), @"[ a-z]"));
        }

        [Test]
        public void TestStreetAddress()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetStreetAddress(), @"[ a-z]"));
        }

        [Test]
        public void TestStreetName()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetStreetName(), @"[ a-z]"));
        }

        [Test]
        public void TestStreetSuffix()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetStreetSuffix(), @"[ a-z]"));
        }

        [Test]
        public void TestUkCountry()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetUkCounty(), @"[ a-z]"));
        }

        [Test]
        public void TestUkCounty()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetUkCounty(), @"[ a-z]"));
        }

        [Test]
        public void TestUkPostcode()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetUkPostcode(), @"[ a-z]"));
        }

        [Test]
        public void TestUsState()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetUsState(), @"[ a-z]"));
        }

        [Test]
        public void TestUsStateAbbr()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetUsStateAbbr(), @"[A-Z]"));
        }

        [Test]
        public void TestUsZipCode()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetZipCode(), @"[0-9]"));
        }

        [Test]
        public void TestNeighborhood()
        {
            Assert.IsTrue(Regex.IsMatch(Address.GetNeighborhood(), @"[ a-z]+"));
        }
    }
}