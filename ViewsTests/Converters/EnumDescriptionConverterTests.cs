using System;
using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModelUtils.Enums;
using Views.Converters;

namespace ViewsTests.Converters
{
    [TestClass()]
    public class EnumDescriptionConverterTests
    {
        [TestMethod()]
        public void ConvertTest_ValidValueAsEnumWithDescription()
        {
            var description = EnumDescriptionConverter.Instance.Convert(TyrePlacement.FL, typeof(string), null, CultureInfo.CurrentUICulture);
            Assert.AreEqual("Front Left", description);
        }

        /// <summary>
        /// Having a parameter shouldn't make a difference.
        /// </summary>
        [TestMethod()]
        public void ConvertTest_ValidValueAsEnumWithDescriptionWithAnyParameter()
        {
            var description = EnumDescriptionConverter.Instance.Convert(TyrePlacement.FL, typeof(string), typeof(TyreFamily), CultureInfo.CurrentUICulture);
            Assert.AreEqual("Front Left", description);
        }

        [TestMethod()]
        public void ConvertTest_ValidValueAsEnumNoDescriptionWithParameter()
        {
            const TyreFamily value = TyreFamily.F1;
            var description = EnumDescriptionConverter.Instance.Convert(value, typeof(string), typeof(TyreFamily), CultureInfo.CurrentUICulture);
            Assert.AreEqual(value.ToString(), description);
        }

        [TestMethod()]
        public void ConvertTest_ValidValueAsEnumNoDescriptionNoParameter()
        {
            const TyreFamily value = TyreFamily.F1;
            var description = EnumDescriptionConverter.Instance.Convert(value, typeof(string), null, CultureInfo.CurrentUICulture);
            Assert.AreEqual(value.ToString(), description);
        }
      
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertTest_NullValueWithParameter()
        {
            EnumDescriptionConverter.Instance.Convert(null, typeof(string), TyrePlacement.FL, CultureInfo.CurrentUICulture);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConvertTest_NoValueNoParameter()
        {
            EnumDescriptionConverter.Instance.Convert(null, typeof(string), null, CultureInfo.CurrentUICulture);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTest_ValueNotEnumWithParameter()
        {
            EnumDescriptionConverter.Instance.Convert("Not an enum", typeof(string), typeof(TyrePlacement), CultureInfo.CurrentUICulture);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTest_ValueNotEnumNoParameter()
        {
            EnumDescriptionConverter.Instance.Convert("Not an enum", typeof(string), null, CultureInfo.CurrentUICulture);
        }

        [TestMethod()]
        public void ConvertBackTest()
        {
            var description = EnumDescriptionConverter.Instance.ConvertBack("Description", null, null, CultureInfo.CurrentUICulture);
            Assert.AreEqual(DependencyProperty.UnsetValue, description);
        }
    }
}