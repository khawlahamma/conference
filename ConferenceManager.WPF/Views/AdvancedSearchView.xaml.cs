using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using ConferenceManager.WPF.Data;
using ConferenceManager.WPF.Services;
using ConferenceManager.WPF.ViewModels;

namespace ConferenceManager.WPF.Views
{
    public partial class AdvancedSearchView : UserControl
    {
        public AdvancedSearchView()
        {
            InitializeComponent();
        }
    }

    public class SearchResult
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
    }
} 