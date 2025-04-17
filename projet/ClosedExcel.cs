using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace projet
{
    class ClosedExcel
    {
        string path = "";
        XLWorkbook wb;
        IXLWorksheet ws;

        public ClosedExcel(string path, int sheet)
        {
            this.path = path;

            try
            {
                wb = new XLWorkbook(path);  // Ouvrir le fichier Excel
                ws = wb.Worksheet(sheet);  // Accéder à la feuille de travail spécifiée
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Excel file: {ex.Message}");
                Dispose();
            }
        }

        public string ReadCell(int i, int j)
        {
            var cell = ws.Cell(i, j);
            if (!cell.IsEmpty())
            {
                return cell.GetValue<string>();
            }
            return "?";
        }

        public void CloseFile()
        {
            try
            {
                wb.Dispose();  // Libérer le workbook
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CloseSave()
        {
            try
            {
                wb.Save();  // Sauvegarder le fichier
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                wb.Dispose();  // Libérer le workbook
            }
        }

        public int GetRange()
        {
            return ws.LastRowUsed().RowNumber();
        }

        public void FillBlue(int i, int j)
        {
            ws.Cell(i, j).Style.Fill.BackgroundColor = XLColor.Blue;
        }

        public void FillGreen(int i, int j)
        {
            ws.Cell(i, j).Style.Fill.BackgroundColor = XLColor.Green;
        }

        public void FillRed(int i, int j)
        {
            ws.Cell(i, j).Style.Fill.BackgroundColor = XLColor.Red;
        }

        public void FillWhite(int i, int j)
        {
            ws.Cell(i, j).Style.Fill.BackgroundColor = XLColor.White;
        }

        public void CellWrite(int i, int j, string content)
        {
            var cell = ws.Cell(i, j);
            if (!cell.IsEmpty())
            {
                cell.Value = cell.GetValue<string>() + "\n" + content;
            }
            else
            {
                cell.Value = content;
            }
        }

        public void CellOverWrite(int i, int j, string content)
        {
            ws.Cell(i, j).Value = content;
        }

        public bool IsCellDated(int i, int j)
        {
            var cell = ws.Cell(i, j);
            if (!cell.IsEmpty())
            {
                try
                {
                    DateTime check = cell.GetValue<DateTime>();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool SoudurePrévue(int i, int j)
        {
            return !ws.Cell(i, j).IsEmpty();
        }

        public void WriteDate(int i, int j, DateTime date)
        {
            ws.Cell(i, j).Value = date;
        }

        public void WriteTS(int i, int j, TimeSpan ts)
        {
            int hours = ts.Days * 24 + ts.Hours;
            string temps = hours.ToString() + " h " + ts.Minutes.ToString() + " m";
            ws.Cell(i, j).Value = temps;
        }

        public DateTime ReadDate(int i, int j)
        {
            return ws.Cell(i, j).GetValue<DateTime>();
        }

        public string ColorCode(int i, int j)
        {
            return ws.Cell(i, j).Style.Fill.BackgroundColor.ToString();
        }

        public int GetColor(int i, int j)
        {

            var colorA2 = ws.Cell(2, 1).Style.Fill.BackgroundColor;  // A2
            var colorB2 = ws.Cell(2, 2).Style.Fill.BackgroundColor;  // A2
            var colorC2 = ws.Cell(2, 3).Style.Fill.BackgroundColor;  // A2

            MessageBox.Show($"Color of A2: {colorA2}");
            MessageBox.Show($"Color of B2: {colorB2}");
            MessageBox.Show($"Color of C2: {colorC2}");
            var color = ws.Cell(i, j).Style.Fill.BackgroundColor;
            if (color.Equals(XLColor.Green)) return 0;  // terminé
            else if (color.Equals(XLColor.Blue)) return 1;  // en cours
            else if (color.Equals(XLColor.Red)) return 2;  // à faire
            return -1;
        }

        public void InsertRow(int i)
        {
            ws.Row(i).InsertRowsBelow(1);  // Insérer une ligne à la position i
        }

        public bool IsSent(int i)
        {
            return !ws.Cell(i, 16).IsEmpty();
        }

        public void DeleteBlankRow(int range)
        {
            int i = 2;
            int count = 2;
            while (count != range)
            {
                if (!ws.Cell(i, 1).IsEmpty())
                {
                    i++;
                    count++;
                }
                else
                {
                    ws.Row(i).Delete();
                    count++;
                }
            }
        }

        public void Dispose()
        {
            try
            {
                wb.Dispose();  // Fermer et libérer le workbook
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la libération des ressources : " + ex.Message);
            }
        }
    }
}
