using System.Linq;
using System.Xml.Linq;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTests
{
    [TestClass()]
    public class DataManagerUnexpectedXmlTests
    {
        private const string UnexpectedTyreXml =
            @"<Tyre>
            <TyreName>SuperSoft - Front Tyre 1</TyreName>
            <TyreFamily>F1</TyreFamily>
            <TyreType>SuperSoft</TyreType>
            <TyrePlacement>FL</TyrePlacement>
            <TyreDegradationCoefficient>10</TyreDegradationCoefficient>
            </Tyre>";

        private static XElement _invalidTyreXElement;
        private static XElement[] _invalidChildElements;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            _invalidTyreXElement = XElement.Parse(UnexpectedTyreXml);
            _invalidChildElements = _invalidTyreXElement.Elements().ToArray();
        }

        [TestMethod()]
        public void GetElementByNameTest_InvalidElementsValidName()
        {
            var element = DataManager.GetElementByName(_invalidChildElements, "Name");
            Assert.AreEqual(null, element);
        }

        [TestMethod()]
        public void GetElementByNameTest_InvalidElementsInvalidName()
        {
            var element = DataManager.GetElementByName(_invalidChildElements, "SomeElementName");
            Assert.AreEqual(null, element);
        }
    }
}