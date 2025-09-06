using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;
using System.Collections.ObjectModel;
using Models = ConferenceManager.WPF.Models;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;

namespace ConferenceManager.WPF.Views
{
    public partial class ExportView : UserControl
    {
        private readonly IConferenceService _conferenceService;
        private readonly IDocumentService _documentService;
        private readonly ISpeakerService _speakerService;
        private readonly IExportService _exportService;
        private ObservableCollection<Models.Conference> _conferences;
        private ObservableCollection<Models.Document> _documents;

        public ExportView(IConferenceService conferenceService, IDocumentService documentService, ISpeakerService speakerService, IExportService exportService)
        {
            InitializeComponent();
            _conferenceService = conferenceService;
            _documentService = documentService;
            _speakerService = speakerService;
            _exportService = exportService;
            _conferences = new ObservableCollection<Models.Conference>();
            _documents = new ObservableCollection<Models.Document>();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var conferences = await _conferenceService.GetConferencesAsync();
                var documents = await _documentService.GetDocumentsAsync();

                _conferences.Clear();
                _documents.Clear();

                foreach (var conference in conferences)
                {
                    _conferences.Add(conference);
                }

                foreach (var document in documents)
                {
                    _documents.Add(document);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExportConferences_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Fichiers JSON (*.json)|*.json",
                    DefaultExt = ".json",
                    FileName = "conferences.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    var conferences = await _conferenceService.GetConferencesAsync();
                    var json = JsonConvert.SerializeObject(conferences, Formatting.Indented);
                    await File.WriteAllTextAsync(dialog.FileName, json);
                    MessageBox.Show("Export des conférences réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export des conférences : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExportDocuments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Fichiers JSON (*.json)|*.json",
                    DefaultExt = ".json",
                    FileName = "documents.json"
                };

                if (dialog.ShowDialog() == true)
                {
                    var documents = await _documentService.GetDocumentsAsync();
                    var json = JsonConvert.SerializeObject(documents, Formatting.Indented);
                    await File.WriteAllTextAsync(dialog.FileName, json);
                    MessageBox.Show("Export des documents réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export des documents : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var workbook = new XLWorkbook();
                await ExportConferencesToExcel(workbook);
                await ExportSpeakersToExcel(workbook);
                await ExportDocumentsToExcel(workbook);

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    DefaultExt = "xlsx",
                    FileName = "ConferenceManager_Export.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Export Excel terminé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export Excel : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    DefaultExt = "pdf",
                    FileName = "ConferenceManager_Export.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using var writer = new PdfWriter(saveFileDialog.FileName);
                    using var pdf = new PdfDocument(writer);
                    using var document = new Document(pdf);

                    await ExportConferencesToPdf(document);
                    await ExportSpeakersToPdf(document);
                    await ExportDocumentsToPdf(document);

                    document.Close();
                    MessageBox.Show("Export PDF terminé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export PDF : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExportConferencesToExcel(XLWorkbook workbook)
        {
            var conferences = await _conferenceService.GetConferencesAsync();
            var worksheet = workbook.Worksheets.Add("Conférences");

            // En-têtes
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Titre";
            worksheet.Cell(1, 3).Value = "Date";
            worksheet.Cell(1, 4).Value = "Lieu";
            worksheet.Cell(1, 5).Value = "Description";
            worksheet.Cell(1, 6).Value = "Statut";

            // Données
            int row = 2;
            foreach (var conference in conferences)
            {
                worksheet.Cell(row, 1).Value = conference.Id;
                worksheet.Cell(row, 2).Value = conference.Title;
                worksheet.Cell(row, 3).Value = conference.Date;
                worksheet.Cell(row, 4).Value = conference.Location;
                worksheet.Cell(row, 5).Value = conference.Description;
                worksheet.Cell(row, 6).Value = conference.Status;
                row++;
            }
        }

        private async Task ExportSpeakersToExcel(XLWorkbook workbook)
        {
            var speakers = await _speakerService.GetAllSpeakersAsync();
            var worksheet = workbook.Worksheets.Add("Conférenciers");

            // En-têtes
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nom";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Téléphone";
            worksheet.Cell(1, 5).Value = "Biographie";
            worksheet.Cell(1, 6).Value = "Statut";

            // Données
            int row = 2;
            foreach (var speaker in speakers)
            {
                worksheet.Cell(row, 1).Value = speaker.Id;
                worksheet.Cell(row, 2).Value = speaker.Name;
                worksheet.Cell(row, 3).Value = speaker.Email;
                worksheet.Cell(row, 4).Value = speaker.Phone;
                worksheet.Cell(row, 5).Value = speaker.Biography;
                worksheet.Cell(row, 6).Value = speaker.Status;
                row++;
            }
        }

        private async Task ExportDocumentsToExcel(XLWorkbook workbook)
        {
            var documents = await _documentService.GetDocumentsAsync();
            var worksheet = workbook.Worksheets.Add("Documents");

            // En-têtes
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Titre";
            worksheet.Cell(1, 3).Value = "Type";
            worksheet.Cell(1, 4).Value = "Date";
            worksheet.Cell(1, 5).Value = "Statut";

            // Données
            int row = 2;
            foreach (var document in documents)
            {
                worksheet.Cell(row, 1).Value = document.Id;
                worksheet.Cell(row, 2).Value = document.Title;
                worksheet.Cell(row, 3).Value = document.Type;
                worksheet.Cell(row, 4).Value = document.Date;
                worksheet.Cell(row, 5).Value = document.Status;
                row++;
            }
        }

        private async Task ExportConferencesToPdf(Document document)
        {
            var conferences = await _conferenceService.GetConferencesAsync();

            document.Add(new Paragraph("Conférences").SetFontSize(20));

            var table = new Table(6);
            table.AddHeaderCell("ID");
            table.AddHeaderCell("Titre");
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Lieu");
            table.AddHeaderCell("Description");
            table.AddHeaderCell("Statut");

            foreach (var conference in conferences)
            {
                table.AddCell(conference.Id.ToString());
                table.AddCell(conference.Title);
                table.AddCell(conference.Date.ToString());
                table.AddCell(conference.Location);
                table.AddCell(conference.Description);
                table.AddCell(conference.Status);
            }

            document.Add(table);
        }

        private async Task ExportSpeakersToPdf(Document document)
        {
            var speakers = await _speakerService.GetAllSpeakersAsync();

            document.Add(new Paragraph("Conférenciers").SetFontSize(20));

            var table = new Table(6);
            table.AddHeaderCell("ID");
            table.AddHeaderCell("Nom");
            table.AddHeaderCell("Email");
            table.AddHeaderCell("Téléphone");
            table.AddHeaderCell("Biographie");
            table.AddHeaderCell("Statut");

            foreach (var speaker in speakers)
            {
                table.AddCell(speaker.Id.ToString());
                table.AddCell(speaker.Name);
                table.AddCell(speaker.Email);
                table.AddCell(speaker.Phone);
                table.AddCell(speaker.Biography);
                table.AddCell(speaker.Status);
            }

            document.Add(table);
        }

        private async Task ExportDocumentsToPdf(Document document)
        {
            var documents = await _documentService.GetDocumentsAsync();

            document.Add(new Paragraph("Documents").SetFontSize(20));

            var table = new Table(5);
            table.AddHeaderCell("ID");
            table.AddHeaderCell("Titre");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Statut");

            foreach (var doc in documents)
            {
                table.AddCell(doc.Id.ToString());
                table.AddCell(doc.Title);
                table.AddCell(doc.Type);
                table.AddCell(doc.Date.ToString());
                table.AddCell(doc.Status);
            }

            document.Add(table);
        }
    }
} 