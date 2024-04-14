using MVVMWeatherApp.ViewModels.Helpers;

namespace LearningWPF.MVVMWeatherApp.UnitTest.ViewModels.Helpers {
    public class AccuWeatherHelperTests {
        [Test]
        public async Task GetCitiesAsync_Should_Returns_Cities() {
            // arrange

            // act
            var result = await AccuWeatherHelper.GetCitiesAsync("Lisbon");

            // assert
            Assert.That(result.Length, Is.GreaterThan(1));
        }
    }
}
