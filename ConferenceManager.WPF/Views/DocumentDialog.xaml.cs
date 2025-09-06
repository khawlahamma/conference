using System;
using System.Collections.Generic;
using System.Windows;
using ConferenceManager.WPF.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using ConferenceManager.WPF.Data;
using Microsoft.Win32;
using ConferenceManager.WPF.Services;
using System.Linq;
using System.Threading.Tasks;
using ConferenceManager.WPF.ViewModels;

namespace ConferenceManager.WPF.Views
{
    public partial class DocumentDialog : Window
    {
        private readonly IDocumentService _documentService;
        private readonly DocumentDialogViewModel _viewModel;

        public Document Document => _viewModel.Document;

        public List<string> DocumentTypes { get; } = new List<string>
        {
            "Presentation",
            "Document",
            "Spreadsheet",
            "PDF",
            "Other"
        };

        public DocumentDialog(IDocumentService documentService, IConferenceService conferenceService, Document? document = null)
        {
            InitializeComponent();
            _documentService = documentService;
            _viewModel = new DocumentDialogViewModel(document, conferenceService);
            DataContext = _viewModel;
            _viewModel.RequestClose += (sender, result) => DialogResult = result;
            Loaded += DocumentDialog_Loaded;
            LoadDocumentData();
        }

        private async void DocumentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadConferencesAsync();
        }

        private void LoadDocumentData()
        {
            TitleTextBox.Text = Document.Title;
            DescriptionTextBox.Text = Document.Description;
            DatePicker.SelectedDate = Document.Date;
            StatusComboBox.Text = Document.Status;
            IsPublicCheckBox.IsChecked = Document.IsPublic;

            if (Document.Conference != null)
            {
                ConferenceComboBox.SelectedItem = Document.Conference;
            }

            if (Document.Type != null)
            {
                DocumentTypeComboBox.Text = Document.Type;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveCommand.Execute(null);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 