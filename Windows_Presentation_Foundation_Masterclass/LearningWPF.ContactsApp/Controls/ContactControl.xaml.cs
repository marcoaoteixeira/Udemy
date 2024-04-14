using System;
using System.Windows;
using System.Windows.Controls;
using LearningWPF.ContactsApp.Entities;

namespace LearningWPF.ContactsApp.Controls {
    /// <summary>
    /// Interaction logic for ContactControl.xaml
    /// </summary>
    public partial class ContactControl : UserControl {

        public Contact? Contact {
            get { return (Contact?)GetValue(ContactProperty); }
            set { SetValue(ContactProperty, value); }
        }

        public static readonly DependencyProperty ContactProperty =
            DependencyProperty.Register("Contact", typeof(Contact), typeof(ContactControl), new PropertyMetadata(null, SetText));

        private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var contractControl = (ContactControl)d;

            contractControl.nameTextBlock.Text = ((Contact)e.NewValue).Name;
            contractControl.emailTextBlock.Text = ((Contact)e.NewValue).Email;
            contractControl.phoneTextBlock.Text = ((Contact)e.NewValue).Phone;
        }

        public ContactControl() {
            InitializeComponent();
        }
    }
}
