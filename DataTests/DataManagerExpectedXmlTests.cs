using System.Linq;
using System.Xml.Linq;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTests
{
    [TestClass()]
    public class DataManagerExpectedXmlTests
    {
        private const string ExpectedTyreXml =
            @"<Tyre>
            <Name>SuperSoft - Front Tyre 1</Name>
            <Family>F1</Family>
            <Type>SuperSoft</Type>
            <Placement>FL</Placement>
            <DegradationCoefficient>10</DegradationCoefficient>
            </Tyre>";

        private static XElement _validTyreXElement;
        private static XElement[] _validChildElements;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            _validTyreXElement = XElement.Parse(ExpectedTyreXml);
            _validChildElements = _validTyreXElement.Elements().ToArray();
        }

        [TestMethod()]
        public void GetElementByNameTest_ValidElementsValidName()
        {
            var element = DataManager.GetElementByName(_validChildElements, "Name");
            Assert.AreEqual("SuperSoft - Front Tyre 1", element);
        }

        [TestMethod()]
        public void GetElementByNameTest_ValidElementsInvalidName()
        {
            var element = DataManager.GetElementByName(_validChildElements, "SomeElementName");
            Assert.AreEqual(null, element);
        }
    }
}