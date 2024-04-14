using System;
using System.IO;
using System.Windows;
using LearningWPF.ContactsApp.Entities;
using SQLite;

namespace LearningWPF.ContactsApp {
    /// <summary>
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window {
        public NewContactWindow() {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) {
            // save contact

            var contact = new Contact {
                Name = nameTextBox.Text,
                Email = emailTextBox.Text,
                Phone = phoneTextBox.Text,
            };

            using var connection = new SQLiteConnection(App.DatabasePath);
            connection.CreateTable<Contact>();
            connection.Insert(contact);

            Close();
        }
    }
}
