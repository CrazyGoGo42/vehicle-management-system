using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using VehicleManager.Core.Models;

namespace VehicleManager.Core.Services
{
    /// <summary>
    /// Service für das Exportieren von Fahrzeugdaten als professionelle PDF-Angebote
    /// Für Studenten: Das zeigt, wie man externe Bibliotheken (iTextSharp) in C# verwendet
    /// </summary>
    public class PdfExportService
    {
        #region Private Konstanten für das PDF-Layout
        // Für Studenten: Konstanten machen den Code wartbarer und lesbarer
        private static readonly BaseColor HEADER_COLOR = new BaseColor(41, 84, 144); // Dunkelblau
        private static readonly BaseColor ACCENT_COLOR = new BaseColor(51, 122, 183); // Hellblau
        private static readonly BaseColor TEXT_COLOR = BaseColor.BLACK;
        private static readonly BaseColor LIGHT_GRAY = new BaseColor(245, 245, 245);
        
        private const float MARGIN = 50f;
        private const int TITLE_FONT_SIZE = 24;
        private const int HEADER_FONT_SIZE = 16;
        private const int NORMAL_FONT_SIZE = 12;
        private const int SMALL_FONT_SIZE = 10;
        #endregion

        #region Öffentliche Methoden
        /// <summary>
        /// Generiert eine professionelle PDF-Fahrzeugofferte in deutscher Sprache
        /// Für Studenten: Das ist die Hauptmethode, die alle anderen privaten Methoden koordiniert
        /// </summary>
        /// <param name="vehicle">Das Fahrzeug, für das die Offerte erstellt werden soll</param>
        /// <param name="filePath">Der Pfad, wo die PDF-Datei gespeichert werden soll</param>
        /// <returns>True wenn erfolgreich, False bei Fehlern</returns>
        public bool GeneriereFahrzeugOfferte(Vehicle vehicle, string filePath)
        {
            try
            {
                // Validation für Studenten: Immer Eingaben validieren
                if (vehicle == null)
                {
                    Console.WriteLine("Fehler: Vehicle ist null");
                    return false;
                }
                
                if (string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("Fehler: Dateipfad ist leer");
                    return false;
                }

                Console.WriteLine($"PDF Export gestartet für: {vehicle.Marke} {vehicle.Modell}");
                Console.WriteLine($"Speichere nach: {filePath}");

                // Für Studenten: using-Statement sorgt für automatische Ressourcen-Freigabe
                // Wichtig: Document muss vor FileStream geschlossen werden!
                using (var document = new Document(PageSize.A4, MARGIN, MARGIN, MARGIN, MARGIN))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    using (var writer = PdfWriter.GetInstance(document, fileStream))
                    {
                        // PDF-Dokument öffnen
                        document.Open();
                        Console.WriteLine("PDF Dokument geöffnet");

                        // Verschiedene Bereiche des PDFs erstellen
                        Console.WriteLine("Füge Kopfbereich hinzu...");
                        FügeKopfbereichHinzu(document, vehicle);
                        Console.WriteLine("Füge Fahrzeugdetails hinzu...");
                        FügeFahrzeugdetailsHinzu(document, vehicle);
                        Console.WriteLine("Füge Preisinformationen hinzu...");
                        FügePreisinformationenHinzu(document, vehicle);
                        Console.WriteLine("Füge technische Daten hinzu...");
                        FügeTechnischeDatenHinzu(document, vehicle);
                        Console.WriteLine("Füge Ausstattung hinzu...");
                        FügeAusstattungHinzu(document, vehicle);
                        Console.WriteLine("Füge Fußbereich hinzu...");
                        FügeFußbereichHinzu(document);

                        // Explizit schließen für korrekte Reihenfolge
                        document.Close();
                        Console.WriteLine("PDF erfolgreich erstellt!");
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                // Für Studenten: Immer Fehler loggen für Debugging
                System.Diagnostics.Debug.WriteLine($"Fehler bei PDF-Generierung: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"PDF Export Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Generiert einen Dateinamen für die PDF-Offerte basierend auf Fahrzeugdaten
        /// </summary>
        /// <param name="vehicle">Das Fahrzeug</param>
        /// <returns>Formatierter Dateiname</returns>
        public string GeneriereOffertenDateiname(Vehicle vehicle)
        {
            // Für Studenten: String-Interpolation und Datum-Formatierung
            var heute = DateTime.Now.ToString("yyyy-MM-dd");
            var fahrzeugName = $"{vehicle.Marke}_{vehicle.Modell}_{vehicle.Baujahr}"
                .Replace(" ", "_")
                .Replace("-", "_");
            
            return $"Offerte_{fahrzeugName}_{heute}.pdf";
        }
        #endregion

        #region Private Hilfsmethoden für PDF-Bereiche
        /// <summary>
        /// Fügt den Kopfbereich mit Logo und Überschrift hinzu
        /// </summary>
        private void FügeKopfbereichHinzu(Document document, Vehicle vehicle)
        {
            try
            {
                // Hauptüberschrift
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, TITLE_FONT_SIZE, HEADER_COLOR);
                var title = new Paragraph("🚗 FAHRZEUG-OFFERTE", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                document.Add(title);

                // Untertitel mit Fahrzeugbezeichnung
                var subtitleFont = FontFactory.GetFont(FontFactory.HELVETICA, HEADER_FONT_SIZE, ACCENT_COLOR);
                var subtitle = new Paragraph($"{vehicle.Marke} {vehicle.Modell} ({vehicle.Baujahr})", subtitleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(subtitle);

                // Datum der Offerte
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, SMALL_FONT_SIZE, BaseColor.GRAY);
                var dateParagraph = new Paragraph($"Offerte erstellt am: {DateTime.Now:dd.MM.yyyy um HH:mm} Uhr", dateFont)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 20f
                };
                document.Add(dateParagraph);

                // Trennlinie
                var separator = new Chunk("_", FontFactory.GetFont(FontFactory.HELVETICA, 1, BaseColor.LIGHT_GRAY));
                for (int i = 0; i < 100; i++) document.Add(separator);
                document.Add(Chunk.NEWLINE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler im Kopfbereich: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt detaillierte Fahrzeugdaten hinzu
        /// </summary>
        private void FügeFahrzeugdetailsHinzu(Document document, Vehicle vehicle)
        {
            try
            {
                // Sektion-Überschrift
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, HEADER_FONT_SIZE, HEADER_COLOR);
                var header = new Paragraph("📋 FAHRZEUGDATEN", headerFont)
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 15f
                };
                document.Add(header);

                // Tabelle für Fahrzeugdaten erstellen
                var table = new PdfPTable(2) { WidthPercentage = 100f };
                table.SetWidths(new float[] { 40f, 60f });

                // Für Studenten: Hilfsmethode zum Hinzufügen von Zeilen
                FügeTableZeileHinzu(table, "🏭 Marke:", vehicle.Marke);
                FügeTableZeileHinzu(table, "🚙 Modell:", vehicle.Modell);
                FügeTableZeileHinzu(table, "📅 Baujahr:", vehicle.Baujahr.ToString());
                FügeTableZeileHinzu(table, "🎨 Farbe:", vehicle.Farbe);
                FügeTableZeileHinzu(table, "🚗 Fahrzeugtyp:", vehicle.Fahrzeugtyp);
                FügeTableZeileHinzu(table, "📅 Kaufdatum:", vehicle.Kaufdatum.ToString("dd.MM.yyyy"));

                document.Add(table);
                document.Add(Chunk.NEWLINE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler bei Fahrzeugdetails: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt Preisinformationen mit aktueller Wertberechnung hinzu
        /// </summary>
        private void FügePreisinformationenHinzu(Document document, Vehicle vehicle)
        {
            try
            {
                // Preise berechnen
                var aktuellerWert = vehicle.AktuellerWert;
                var wertverlust = vehicle.Kaufpreis - aktuellerWert;
                var wertverlustProzent = (wertverlust / vehicle.Kaufpreis) * 100;

                // Sektion-Überschrift
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, HEADER_FONT_SIZE, HEADER_COLOR);
                var header = new Paragraph("💰 PREISINFORMATIONEN", headerFont)
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 15f
                };
                document.Add(header);

                // Hintergrund-Box für Preise erstellen
                var priceTable = new PdfPTable(2) { WidthPercentage = 100f };
                priceTable.SetWidths(new float[] { 50f, 50f });

                // Linke Spalte - Kaufpreis und Wertverlust
                var leftCell = new PdfPCell();
                leftCell.Border = Rectangle.BOX;
                leftCell.BackgroundColor = LIGHT_GRAY;
                leftCell.Padding = 15f;

                var kaufpreisFont = FontFactory.GetFont(FontFactory.HELVETICA, NORMAL_FONT_SIZE, TEXT_COLOR);
                var verlustFont = FontFactory.GetFont(FontFactory.HELVETICA, NORMAL_FONT_SIZE, BaseColor.RED);
                
                leftCell.AddElement(new Paragraph($"Ursprünglicher Kaufpreis:\n{vehicle.Kaufpreis:C}", kaufpreisFont));
                leftCell.AddElement(new Paragraph($"\nWertverlust:\n{wertverlust:C} ({wertverlustProzent:F1}%)", verlustFont));

                // Rechte Spalte - Aktueller Wert (hervorgehoben)
                var rightCell = new PdfPCell();
                rightCell.Border = Rectangle.BOX;
                rightCell.BackgroundColor = new BaseColor(220, 255, 220); // Hellgrün
                rightCell.Padding = 15f;

                var wertFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, new BaseColor(0, 128, 0)); // Grün
                var aktuellerWertParagraph = new Paragraph($"AKTUELLER WERT:\n\n{aktuellerWert:C}", wertFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                rightCell.AddElement(aktuellerWertParagraph);

                priceTable.AddCell(leftCell);
                priceTable.AddCell(rightCell);
                document.Add(priceTable);
                
                // Hinweis zur Wertberechnung
                var hinweisFont = FontFactory.GetFont(FontFactory.HELVETICA, SMALL_FONT_SIZE, BaseColor.GRAY);
                var hinweis = new Paragraph($"\n* Wertberechnung basiert auf Fahrzeugalter und Kilometerstand. " +
                                          $"Aktuelle Laufleistung: {vehicle.Kilometerstand:N0} km", hinweisFont)
                {
                    Alignment = Element.ALIGN_LEFT
                };
                document.Add(hinweis);
                document.Add(Chunk.NEWLINE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler bei Preisinformationen: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt technische Daten hinzu
        /// </summary>
        private void FügeTechnischeDatenHinzu(Document document, Vehicle vehicle)
        {
            try
            {
                // Sektion-Überschrift
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, HEADER_FONT_SIZE, HEADER_COLOR);
                var header = new Paragraph("⚡ TECHNISCHE DATEN", headerFont)
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 15f
                };
                document.Add(header);

                // Tabelle für technische Daten
                var table = new PdfPTable(2) { WidthPercentage = 100f };
                table.SetWidths(new float[] { 40f, 60f });

                FügeTableZeileHinzu(table, "⚡ Leistung:", $"{vehicle.Leistung} PS");
                FügeTableZeileHinzu(table, "🛣️ Kilometerstand:", $"{vehicle.Kilometerstand:N0} km");
                FügeTableZeileHinzu(table, "⛽ Kraftstoff:", vehicle.Kraftstoff);
                FügeTableZeileHinzu(table, "⚙️ Getriebe:", vehicle.Getriebe);

                document.Add(table);
                document.Add(Chunk.NEWLINE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler bei technischen Daten: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt Ausstattungsdetails hinzu
        /// </summary>
        private void FügeAusstattungHinzu(Document document, Vehicle vehicle)
        {
            try
            {
                if (!string.IsNullOrEmpty(vehicle.Zusatzausstattung))
                {
                    // Sektion-Überschrift
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, HEADER_FONT_SIZE, HEADER_COLOR);
                    var header = new Paragraph("🛠️ AUSSTATTUNG", headerFont)
                    {
                        SpacingBefore = 10f,
                        SpacingAfter = 15f
                    };
                    document.Add(header);

                    // Ausstattung als formatierte Liste
                    var ausstattungFont = FontFactory.GetFont(FontFactory.HELVETICA, NORMAL_FONT_SIZE, TEXT_COLOR);
                    var ausstattungText = vehicle.Zusatzausstattung
                        .Replace(",", "\n• ")
                        .Insert(0, "• ");
                    
                    var ausstattung = new Paragraph(ausstattungText, ausstattungFont)
                    {
                        SpacingAfter = 20f
                    };
                    document.Add(ausstattung);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler bei Ausstattung: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt den Fußbereich mit Kontaktdaten hinzu
        /// </summary>
        private void FügeFußbereichHinzu(Document document)
        {
            try
            {
                // Trennlinie vor Fußbereich  
                var footerSeparator = new Chunk("_", FontFactory.GetFont(FontFactory.HELVETICA, 1, BaseColor.LIGHT_GRAY));
                for (int i = 0; i < 100; i++) document.Add(footerSeparator);
                document.Add(Chunk.NEWLINE);

                // Kontaktinformationen
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, SMALL_FONT_SIZE, BaseColor.GRAY);
                var footer = new Paragraph(
                    "🏢 Fahrzeugverwaltung GmbH\n" +
                    "📧 info@fahrzeugverwaltung.de | 📞 +49 (0)123 456-789\n" +
                    "🌐 www.fahrzeugverwaltung.de\n\n" +
                    "Diese Offerte wurde automatisch generiert mit unserem C# Fahrzeugverwaltungssystem.\n" +
                    $"Generiert am {DateTime.Now:dd.MM.yyyy} um {DateTime.Now:HH:mm} Uhr.",
                    footerFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20f
                };
                document.Add(footer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler im Fußbereich: {ex.Message}");
            }
        }

        /// <summary>
        /// Hilfsmethode zum Hinzufügen einer Zeile zur Tabelle
        /// Für Studenten: Das zeigt Code-Wiederverwendung durch Hilfsmethoden
        /// </summary>
        private void FügeTableZeileHinzu(PdfPTable table, string label, string value)
        {
            try
            {
                var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, NORMAL_FONT_SIZE, TEXT_COLOR);
                var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, NORMAL_FONT_SIZE, TEXT_COLOR);

                var labelCell = new PdfPCell(new Phrase(label, labelFont))
                {
                    Border = Rectangle.BOTTOM_BORDER,
                    BorderColor = BaseColor.LIGHT_GRAY,
                    Padding = 8f,
                    BackgroundColor = LIGHT_GRAY
                };

                var valueCell = new PdfPCell(new Phrase(value, valueFont))
                {
                    Border = Rectangle.BOTTOM_BORDER,
                    BorderColor = BaseColor.LIGHT_GRAY,
                    Padding = 8f
                };

                table.AddCell(labelCell);
                table.AddCell(valueCell);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Hinzufügen der Tabellenzeile: {ex.Message}");
            }
        }
        #endregion
    }
}