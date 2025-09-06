using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ConferenceManager.WPF.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Windows.Controls;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.ViewModels;
using ConferenceManager.WPF.Services;
using System.Threading.Tasks;

namespace ConferenceManager.WPF.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private async void ProfileView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConferenceManager.WPF.ViewModels.ProfileViewModel vm)
            {
                try
                {
                    await vm.LoadUserProfileAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
                Title = "Select Profile Picture"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (DataContext is ProfileViewModel vm)
                    {
                        vm.UpdateProfilePicture(openFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating profile picture: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is ProfileViewModel vm)
                {
                    await vm.SaveProfileAsync();
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileViewModel vm)
            {
                var changePasswordWindow = new ChangePasswordWindow(vm.UserService);
                changePasswordWindow.ShowDialog();
            }
        }
    }
} 