using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModelUtils.Enums;

namespace ViewModelUtilsTests
{
    [TestClass]
    public class EnumExtensionsTest
    {
        public struct TestStruct { }
        private const string PlacementAsString = "FL";
        private const string FamilyAsString = "F1";

        #region TyrePlacement

        [TestMethod()]
        public void ToEnumPlacement_ValidPlacement()
        {
            var placement = PlacementAsString.ToEnum<TyrePlacement>();
            Assert.AreEqual(TyrePlacement.FL, placement);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void ToEnumPlacement_InvalidPlacement()
        {
            "FF".ToEnum<TyrePlacement>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEnumPlacement_NotEnumType()
        {
            PlacementAsString.ToEnum<TestStruct>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEnumPlacement_InvalidString()
        {
            "".ToEnum<TyrePlacement>();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void ToEnumPlacement_WrongEnum()
        {
            PlacementAsString.ToEnum<TyreFamily>();
        }

        #endregion

        #region TyreFamily

        [TestMethod()]
        public void ToEnumFamily_ValidFamily()
        {
            var family = FamilyAsString.ToEnum<TyreFamily>();
            Assert.AreEqual(TyreFamily.F1, family);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void ToEnumFamily_InvalidFamily()
        {
            "F3".ToEnum<TyreFamily>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEnumFamily_NotEnumType()
        {
            FamilyAsString.ToEnum<TestStruct>();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ToEnumFamily_InvalidString()
        {
            "".ToEnum<TyreFamily>();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidCastException))]
        public void ToEnumFamily_WrongEnum()
        {
            FamilyAsString.ToEnum<TyrePlacement>();
        }

        #endregion

        [TestMethod()]
        public void GetAxleByTyrePlacementTest_ValidPlacement()
        {
            var axle = TyrePlacement.FL.GetAxleByTyrePlacement();
            Assert.AreEqual(Axle.Front, axle);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetAxleByTyrePlacementTest_PlacementNotOnAxle()
        {
            TyrePlacement.NotSet.GetAxleByTyrePlacement();
        }
    }
}
