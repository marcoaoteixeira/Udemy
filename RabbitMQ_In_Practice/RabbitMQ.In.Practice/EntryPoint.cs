using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.In.Practice.Infrastructure;

namespace RabbitMQ.In.Practice {
    public static class EntryPoint {
        public static void Main(params string[] args) {
            using var provider = CreateServiceProvider();
            var exercises = provider
                .GetRequiredService<IEnumerable<Exercise>>()
                .OrderBy(exercise => exercise.Code)
                .ToArray();
            
            while (true) {
                Console.Clear();

                ShowIntroduction();
                ShowExerciseOptions(exercises);

                var option = GetUserInput();
                if (option == 999) { break; }

                var exercise = exercises.FirstOrDefault(item => item.Code == option);

                if (exercise is not null) {
                    Console.Clear();
                    exercise.Run(Console.In, Console.Out);
                }
            }
        }

        private static void ShowIntroduction() {
            Console.WriteLine("*** RabbitMQ in Practice - Exercises ***");
            Console.WriteLine();
        }

        private static void ShowExerciseOptions(Exercise[] exercises) {
            Console.WriteLine("Choose the exercise that you want to run");
            Console.WriteLine();

            foreach (var exercise in exercises) {
                Console.WriteLine($"    [{exercise.Code.ToString().PadLeft(3, '0')}] {exercise.Description}");
            }
            // Exit option
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    [999] Exit");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }

        private static int GetUserInput() {
            Console.Write("Run exercise: ");
            var input = Console.ReadLine();

            return int.TryParse(input, out var result) ? result : 0;
        }

        private static ServiceProvider CreateServiceProvider() {
            var serviceCollection = new ServiceCollection();

            // Add configuration
            serviceCollection
                .AddSingleton<IConfiguration>(_ => {
                    var builder = new ConfigurationBuilder();
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
                    return builder.Build();
                });

            // Add channel factory
            serviceCollection
                .AddSingleton(provider => {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    return new ChannelFactory(configuration);
                });

            // Add exercises
            var exercises = typeof(EntryPoint)
                .Assembly
                .GetExportedTypes()
                .Where(type =>
                    !type.IsAbstract &&
                    typeof(Exercise).IsAssignableFrom(type)
                ).ToArray();

            foreach (var exercise in exercises) {
                serviceCollection.AddSingleton(typeof(Exercise), exercise);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}