using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModels;

namespace ViewModelsTests
{
    [TestClass()]
    public class TrackDetailsViewModelTests
    {
        [TestMethod()]
        public void ConvertStringSamplesToDoubleTest_ValidSamples()
        {
            var doubleSamples = new double[] {4, 57, 79, 85, 18, 67, 84, 8, 1, 66};
            var stringSamples = "4, 57, 79, 85, 18, 67, 84, 8, 1, 66".Split(',');
            var samplesAsDoubleResult = TrackDetailsViewModel.ConvertStringSamplesToDouble(stringSamples);
            Assert.IsTrue(samplesAsDoubleResult.SequenceEqual(doubleSamples));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void ConvertStringSamplesToDoubleTest_InvalidSamples()
        {
            var stringSamples = "4m, 57, 79, 85, 18, 67, 84, 8, 1, 66".Split(',');
            TrackDetailsViewModel.ConvertStringSamplesToDouble(stringSamples);
        }
    }
}