using System.Collections.ObjectModel;
using LearningWPF.MVVMWeatherApp.ViewModels.Commands;
using MVVMWeatherApp.Models;
using MVVMWeatherApp.ViewModels.Helpers;

namespace LearningWPF.MVVMWeatherApp.ViewModels {
    public class WeatherViewModel : ViewModelBase {

        public CitiesSearchCommand CitiesSearchCommand { get; }

        private string? _queryTerm;
        public string QueryTerm {
            get => _queryTerm ?? string.Empty;
            set {
                _queryTerm = value;
                OnPropertyChanged(nameof(QueryTerm));
            }
        }

        private City? _selectedCity;
        public City? SelectedCity {
            get => _selectedCity;
            set {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
                _ = GetCurrentConditionsAsync();
            }
        }

        private CurrentConditions? _currentConditions;
        public CurrentConditions? CurrentConditions {
            get => _currentConditions;
            set {
                _currentConditions = value;
                OnPropertyChanged(nameof(CurrentConditions));
            }
        }

        public ObservableCollection<City> Cities { get; } = [];

        public WeatherViewModel() {
            CitiesSearchCommand = new CitiesSearchCommand(this);
        }

        public async Task ExecuteCitiesQuery() {
            Cities.Clear();

            var cities = await AccuWeatherHelper.GetCitiesAsync(QueryTerm);
            foreach (var city in cities) {
                Cities.Add(city);
            }
        }

        public async Task GetCurrentConditionsAsync() {
            //QueryTerm = string.Empty;
            //Cities.Clear();

            var currentConditions = await AccuWeatherHelper.GetCurrentConditionsAsync(SelectedCity.Key);
            if (currentConditions is not null) {
                CurrentConditions = currentConditions;
            }
        }
    }
}
