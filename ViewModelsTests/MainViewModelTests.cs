using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModels;

namespace ViewModelsTests
{
    [TestClass()]
    public class MainViewModelTests
    {
        [TestMethod()]
        public void MainViewModelTest()
        {
            var newMainViewModel = new MainViewModel();
            Assert.AreNotEqual(null, newMainViewModel);
            Assert.AreNotEqual(null, newMainViewModel.CurrentSelectionViewModel);
        }
    }
}