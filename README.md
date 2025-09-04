<<<<<<< HEAD
# ebayBulk2
=======
# EbayBulk Generator

## Übersicht

Eine WPF-Desktop-App (.NET 8, C#, MVVM) zum komfortablen Erfassen und Exportieren von eBay-Listings (Parent + Variationen) als eBay-kompatible CSV.

### Features
- eBay-Template-CSV laden (Header-Erkennung, Spaltenreihenfolge)
- Parent-Listing-Formular
- Variationen-Import (CSV) & Generierung
- DataGrid mit Inline-Edit, Filter, Suche
- Export: eBay-kompatible CSV (UTF-8-SIG, Semikolon)
- Validierung (Pflichtfelder, doppelte SKUs, Preise)
- Helper: SKU-Normalizer, Titel-Kürzer, Header-Finder
- Ribbon-Menü, Statusbar, Dialoge

### Build & Run
1. .NET 8 SDK installieren
2. NuGet-Pakete wiederherstellen (CsvHelper, CommunityToolkit.Mvvm)
3. Projekt in Visual Studio 2022+ oder VS Code öffnen
4. Starten (F5)

### Benötigte NuGets
- CsvHelper
- CommunityToolkit.Mvvm

### Beispiel-Variationen-CSV

```
Titel;SKU;Preis;Attribute
Blubberschlauch Set – Classic Azurblau;CLASSIC-AZURBLAU;18.75;Modell=Classic|Farbe=Azurblau|Tasche=ohne Tasche
Set Color Premium Orange Marine;COLOR-PREM-ORANGE-MARINE;29.99;Modell=Color|Farbe=Orange|Tasche=Marine
```

---

Weitere Infos siehe Quellcode und Kommentare.
>>>>>>> a8a8f1f (Initiale Version EbayBulk Generator)
