using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ViewModels;
using ViewModelUtils.Interfaces;

namespace ViewModelsTests
{
    [TestClass()]
    public class ValueStateViewModelTests
    {
        /*
          Green,  // na - 999
          Yellow, // 1000 - 2999
          Red     // 3000 - na
        */
        private readonly ValueStateViewModel _valueStateViewModel = new ValueStateViewModel("", new Mock<IHasValidState>().Object);

        [TestMethod()]
        public void UpdateStateValueTest_GreenLimit()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.GreenThreshold);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Green, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_Green()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.GreenThreshold - 1);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Green, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_NotGreen()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.GreenThreshold + 1);
            Assert.AreNotEqual(ValueStateViewModel.DegradationState.Green, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_Yellow()
        {
            _valueStateViewModel.UpdateStateValue(1001);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Yellow, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_YellowLimitHigh()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.RedThreshold - 1);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Yellow, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_YellowLimitLow()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.GreenThreshold + 1);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Yellow, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_NotYellowBelowLimit()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.GreenThreshold);
            Assert.AreNotEqual(ValueStateViewModel.DegradationState.Yellow, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_NotYellowAboveLimit()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.RedThreshold);
            Assert.AreNotEqual(ValueStateViewModel.DegradationState.Yellow, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_RedLimit()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.RedThreshold);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Red, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_Red()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.RedThreshold + 1);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Red, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_NotRed()
        {
            _valueStateViewModel.UpdateStateValue(ValueStateViewModel.RedThreshold - 1);
            Assert.AreNotEqual(ValueStateViewModel.DegradationState.Red, _valueStateViewModel.State);
        }

        [TestMethod()]
        public void UpdateStateValueTest_NAHigh()
        {
            _valueStateViewModel.UpdateStateValue(500000);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Red, _valueStateViewModel.State);
        }
        [TestMethod()]
        public void UpdateStateValueTest_NALow()
        {
            _valueStateViewModel.UpdateStateValue(-50000);
            Assert.AreEqual(ValueStateViewModel.DegradationState.Green, _valueStateViewModel.State);
        }

        [TestMethod]
        public void InvalidateTest()
        {
            _valueStateViewModel.Invalidate();
            Assert.AreEqual(ValueStateViewModel.DegradationState.NotValid, _valueStateViewModel.State);
            Assert.AreEqual(0,_valueStateViewModel.Value);
        }
    }
}