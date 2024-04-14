using System.Windows.Input;

namespace LearningWPF.MVVMWeatherApp.ViewModels.Commands {
    public class CitiesSearchCommand : ICommand {
        private readonly WeatherViewModel _viewModel;

        public event EventHandler? CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CitiesSearchCommand(WeatherViewModel viewModel) {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
            => !string.IsNullOrWhiteSpace(parameter as string);

        public async void Execute(object? parameter)
            => await _viewModel
                .ExecuteCitiesQuery();
    }
}
