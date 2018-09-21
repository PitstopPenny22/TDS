using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModels;

namespace ViewModelsTests
{
    [TestClass()]
    public class TyreDetailsViewModelTests
    {
            /*
DataManager.Current.Tyres[0] will be:

<Tyre>
<Name>SuperSoft - Front Tyre 1</Name>
<Family>F1</Family>
<Type>SuperSoft</Type>
<Placement>FL</Placement>
<DegradationCoefficient>10</DegradationCoefficient>
</Tyre>
            */

        [TestMethod()]
        public async Task IsSameFamilyAsTest_SameFamily()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var tyreDetailsViewModel = new TyreDetailsViewModel(dataManager.Tyres[0]);
            var newTyreDetails = new TyreDetailsViewModel(dataManager.Tyres.FirstOrDefault(t => t.Family.Trim() == tyreDetailsViewModel.Family.ToString()));
            Assert.IsTrue(tyreDetailsViewModel.IsSameFamilyAs(newTyreDetails));
        }
        [TestMethod()]
        public async Task IsSameFamilyAsTest_DifferentFamily()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var tyreDetailsViewModel = new TyreDetailsViewModel(dataManager.Tyres[0]);
            var newTyreDetails = new TyreDetailsViewModel(dataManager.Tyres.FirstOrDefault(t => t.Family.Trim() != tyreDetailsViewModel.Family.ToString()));
            Assert.IsFalse(tyreDetailsViewModel.IsSameFamilyAs(newTyreDetails));
        }

        [TestMethod()]
        public void CalculateTyreCoefficientTest()
        {
            const int degradationCoefficient = 10;
            const int percentage = 80;
            var tyreCoefficient = TyreDetailsViewModel.CalculateTyreCoefficient(percentage, degradationCoefficient);
            Assert.AreEqual(8, tyreCoefficient);
        }

        [TestMethod]
        public async Task GetPercentageValueTest()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var tyreDetailsViewModel = new TyreDetailsViewModel(dataManager.Tyres[0]);
            var value = tyreDetailsViewModel.GetPercentageValue();
            Assert.AreEqual(80, value);
        }
    }
}