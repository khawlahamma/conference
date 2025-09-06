using System;
using System.Windows;
using System.Windows.Controls;
using ConferenceManager.WPF.Models;
using System.Linq;

namespace ConferenceManager.WPF.Views
{
    public partial class ConferenceDialog : Window
    {
        public Conference Conference { get; private set; }

        public ConferenceDialog(Conference? conference = null)
        {
            InitializeComponent();
            Conference = conference ?? new Conference
            {
                Date = DateTime.Now,
                Status = "Planifié"
            };

            TitleTextBox.Text = Conference.Title;
            DescriptionTextBox.Text = Conference.Description;
            LocationTextBox.Text = Conference.Location;
            DatePicker.SelectedDate = Conference.Date;
            StatusComboBox.Text = Conference.Status;

            // Initialize status combobox
            StatusComboBox.Items.Add(new ComboBoxItem { Content = "Planned" });
            StatusComboBox.Items.Add(new ComboBoxItem { Content = "In Progress" });
            StatusComboBox.Items.Add(new ComboBoxItem { Content = "Completed" });
            StatusComboBox.Items.Add(new ComboBoxItem { Content = "Cancelled" });

            if (conference != null)
            {
                Title = "Edit Conference";
                StatusComboBox.SelectedItem = StatusComboBox.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == conference.Status);
            }
            else
            {
                Title = "Add Conference";
                StatusComboBox.SelectedIndex = 0;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Le titre est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Conference.Title = TitleTextBox.Text;
            Conference.Description = DescriptionTextBox.Text;
            Conference.Location = LocationTextBox.Text;
            Conference.Date = DatePicker.SelectedDate ?? DateTime.Now;
            Conference.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 