using System;
using System.IO;
using System.Windows;

namespace LearningWPF.ContactsApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private static string databaseName = "Contacts.db";
        private static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string DatabasePath = Path.Combine(folderPath, databaseName);
    }
}
