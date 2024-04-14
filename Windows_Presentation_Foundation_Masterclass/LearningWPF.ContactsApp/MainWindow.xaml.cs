using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LearningWPF.ContactsApp.Entities;
using SQLite;

namespace LearningWPF.ContactsApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private List<Contact> _contacts = new();
        public MainWindow() {
            InitializeComponent();
            ReadDatabase();
        }

        private void newContact_Click(object sender, RoutedEventArgs e) {
            var newContactWindow = new NewContactWindow();

            newContactWindow.ShowDialog();
            ReadDatabase();
        }

        private void ReadDatabase() {
            using var conn = new SQLiteConnection(App.DatabasePath);
            conn.CreateTable<Contact>();
            _contacts = conn.Table<Contact>().ToList();
            contactsListView.ItemsSource = _contacts;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            var filteredList = _contacts.Where(_ => _.Name.Contains(((TextBox)sender).Text)).ToList();
            contactsListView.ItemsSource = filteredList;
        }

        private void contactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var contact = ((ListView)sender).SelectedItem as Contact;

            if (contact != null) {
                new ContactDetailsWindow(contact).ShowDialog();
            }
            ReadDatabase();
        }
    }
}
