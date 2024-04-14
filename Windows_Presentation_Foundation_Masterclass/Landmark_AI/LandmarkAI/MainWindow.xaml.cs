using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace LandmarkAI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog {
                Filter = "Image files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg|All files(*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true) {
                var fileName = openFileDialog.FileName;

                selectedImage.Source = new BitmapImage(new Uri(fileName));

                MakePredictionAsync(fileName);
            }
        }

        private async Task MakePredictionAsync(string filePath) {
            var url = "https://uksouth.api.cognitive.microsoft.com/customvision/v3.0/Prediction/e294a3ec-11b2-4962-8f9a-3ca5aea4c452/classify/iterations/Landmarks_v1/image";
            var predictionKey = "97ad32ad449d43b89ccf8bf44e95a6a5";
            var contentType = "application/octet-stream";
            var file = File.ReadAllBytes(filePath);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
            
            using var requestContent = new ByteArrayContent(file);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            
            var response = await httpClient.PostAsync(url, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var landmarkPrediction = JsonSerializer.Deserialize<LandmarkPrediction>(responseContent);
            predictionsListView.ItemsSource = landmarkPrediction.Predictions;
        }
    }
}