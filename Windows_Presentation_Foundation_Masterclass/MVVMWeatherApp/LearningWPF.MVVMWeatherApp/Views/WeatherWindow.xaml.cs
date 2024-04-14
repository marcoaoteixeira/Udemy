using System.Windows;
using LearningWPF.MVVMWeatherApp.ViewModels;

namespace LearningWPF.MVVMWeatherApp.Views {
    /// <summary>
    /// Interaction logic for WeatherWindow.xaml
    /// </summary>
    public partial class WeatherWindow : Window {
        public WeatherViewModel ViewModel { get; }

        public WeatherWindow() {
            ViewModel = new WeatherViewModel();

            DataContext = this;

            InitializeComponent();
        }
    }
}
