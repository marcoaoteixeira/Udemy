using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LearningWPF.ContactsApp.Entities;
using SQLite;

namespace LearningWPF.ContactsApp {
    /// <summary>
    /// Interaction logic for ContactDetailsWindow.xaml
    /// </summary>
    public partial class ContactDetailsWindow : Window {
        private readonly Contact _contact;

        public ContactDetailsWindow(Contact contact) {
            InitializeComponent();

            _contact = contact;

            nameTextBox.Text = contact.Name;
            emailTextBox.Text = contact.Email;
            phoneTextBox.Text = contact.Phone;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e) {
            _contact.Name = nameTextBox.Text;
            _contact.Email = emailTextBox.Text;
            _contact.Phone = phoneTextBox.Text;

            using var conn = new SQLiteConnection(App.DatabasePath);
            conn.CreateTable<Contact>();
            conn.Update(_contact);

            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e) {
            using var conn = new SQLiteConnection(App.DatabasePath);
            conn.CreateTable<Contact>();
            conn.Delete(_contact);

            Close();
        }
    }
}
