using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using EbayBulk_Generator.Models;
using EbayBulk_Generator.Services;
using EbayBulk_Generator.Helpers;

namespace EbayBulk_Generator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Services
        private readonly ITemplateService _templateService = new TemplateService();
        private readonly IListingService _listingService = new ListingService();
        private readonly ICsvExportService _csvExportService = new CsvExportService();

        private ParentListing _parent = new();
        public ParentListing Parent
        {
            get => _parent;
            set { _parent = value; OnPropertyChanged(); }
        }

        public ObservableCollection<VariationListing> Variations { get; set; } = new();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterVariations(); }
        }

        private ObservableCollection<VariationListing> _filteredVariations = new();
        public ObservableCollection<VariationListing> FilteredVariations
        {
            get => _filteredVariations;
            set { _filteredVariations = value; OnPropertyChanged(); }
        }


        // Fehlerpanel
        private string _validationMessage = string.Empty;
        public string ValidationMessage
        {
            get => _validationMessage;
            set { _validationMessage = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand LoadTemplateCommand { get; }
        public ICommand ImportVariationsCommand { get; }
        public ICommand ExportEbayCsvCommand { get; }

        public MainViewModel()
        {
            FilteredVariations = new ObservableCollection<VariationListing>(Variations);
            Variations.CollectionChanged += (s, e) => FilterVariations();

            LoadTemplateCommand = new RelayCommand(_ => LoadTemplate());
            ImportVariationsCommand = new RelayCommand(_ => ImportVariations());
            ExportEbayCsvCommand = new RelayCommand(_ => ExportEbayCsv());
        }
        private List<string> _templateHeaders = new();
        private void LoadTemplate()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { Filter = "CSV-Dateien (*.csv)|*.csv" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var (headers, _, _) = _templateService.LoadTemplate(dlg.FileName);
                    _templateHeaders = headers;
                    ValidationMessage = "Template geladen.";
                }
                catch (Exception ex)
                {
                    ValidationMessage = $"Fehler beim Laden des Templates: {ex.Message}";
                }
            }
        }

        private void ImportVariations()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog { Filter = "CSV-Dateien (*.csv)|*.csv" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var imported = _listingService.ImportVariations(dlg.FileName, Parent.Title);
                    Variations.Clear();
                    foreach (var v in imported) Variations.Add(v);
                    ValidationMessage = $"{imported.Count} Variationen importiert.";
                }
                catch (Exception ex)
                {
                    ValidationMessage = $"Fehler beim Import: {ex.Message}";
                }
            }
        }

        private void ExportEbayCsv()
        {
            if (_templateHeaders == null || _templateHeaders.Count == 0)
            {
                ValidationMessage = "Bitte zuerst ein Template laden.";
                return;
            }
            var errors = ValidateAll();
            if (errors.Count > 0)
            {
                var msg = string.Join("\n", errors);
                var result = System.Windows.MessageBox.Show($"Es gibt folgende Probleme:\n\n{msg}\n\nTrotzdem exportieren?", "Validierungsfehler", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);
                if (result != System.Windows.MessageBoxResult.Yes)
                {
                    ValidationMessage = "Export abgebrochen.";
                    return;
                }
            }
            var dlg = new Microsoft.Win32.SaveFileDialog { Filter = "CSV-Dateien (*.csv)|*.csv" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    _csvExportService.ExportEbayCsv(dlg.FileName, Parent, Variations, _templateHeaders);
                    ValidationMessage = "Export erfolgreich.";
                }
                catch (Exception ex)
                {
                    ValidationMessage = $"Fehler beim Export: {ex.Message}";
                }
            }
        }

        private List<string> ValidateAll()
        {
            var errors = new List<string>();
            // Parent Pflichtfelder
            if (string.IsNullOrWhiteSpace(Parent.Title)) errors.Add("Parent: *Title fehlt");
            if (string.IsNullOrWhiteSpace(Parent.Category)) errors.Add("Parent: *Category fehlt");
            // Variationen prüfen
            var skuSet = new HashSet<string>();
            foreach (var v in Variations)
            {
                if (string.IsNullOrWhiteSpace(v.CustomLabel)) errors.Add($"Variation: SKU fehlt (Titel: {v.Title})");
                if (!skuSet.Add(v.CustomLabel)) errors.Add($"Variation: Doppelte SKU: {v.CustomLabel}");
                if (v.StartPrice <= 0) errors.Add($"Variation: Preis fehlt/ungültig (SKU: {v.CustomLabel})");
            }
            if (Variations.Count == 0) errors.Add("Keine Variationen vorhanden");
            return errors;
        }

        private void FilterVariations()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                FilteredVariations = new ObservableCollection<VariationListing>(Variations);
            else
                FilteredVariations = new ObservableCollection<VariationListing>(
                    Variations.Where(v => v.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                          v.CustomLabel.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
