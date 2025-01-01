using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace projet
{ 
    class Excel
    {     
        string path = "";
        _Excel.Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        public Excel (string path, int sheet)
        {
            Object pwd = "bystronic";
            Object MissingValue = System.Reflection.Missing.Value;
            this.path= path;
            excel.Visible = false;
            excel.ScreenUpdating = false;          
            wb = excel.Workbooks.Open(path, MissingValue, MissingValue, MissingValue, pwd);
            wb.Windows[1].WindowState = XlWindowState.xlMinimized;
            ws = wb.Worksheets[sheet];
        }
        public string ReadCell(int i, int j)
        {
            if (ws.Cells[i, j].Value2 != null)
            {
                string cell = ws.Cells[i, j].Value2.ToString();
                return cell;
            }
            else
                return "?";
        }
        public void CloseFile()
        {          
            wb.Close(false);
            excel.Quit();
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(excel);
        }
        public void CloseSave()
        {          
            wb.Close(true);
            excel.Quit();
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(excel);
        }
        public int GetRange()
        {
            int row = ws.UsedRange.Rows.Count;
            return row;
        }
        public void FillBlue(int i, int j)
        {
            ws.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
        }
        public void FillGreen(int i, int j)
        {
            System.Drawing.Color green = new System.Drawing.Color();
            green = System.Drawing.Color.FromArgb(255, 0, 176, 80);
            ws.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(green);
            //ws.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LawnGreen);
        }
        public void FillRed(int i, int j)
        {
            ws.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        }
        public void FillWhite(int i, int j)
        {
            ws.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
        }
        public void CellWrite(int i,int j, string content)
        {
            if (ws.Cells[i, j].Value2!= null)
            {
                string cell = ws.Cells[i, j].Value2.ToString();
                ws.Cells[i, j].Value = cell +"\n"+content;
            }
            else
            {
                ws.Cells[i, j].Value = content;
            }
                
        }
        public void CellOverWrite(int i, int j, string content)
        {
            ws.Cells[i, j].Value = content;
        }
        public bool IsCellDated(int i, int j)
        {
            DateTime check = new DateTime();
            if (ws.Cells[i, j].Value2 != null)
            {
                try
                {
                    check = ws.Cells[i, j].Value;
                    return true;
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
                {
                    return false;
                }
            }      
            else
            {
                return false;
            }
        }
        public bool SoudurePrévue(int i, int j)
        {
            if (ws.Cells[i, j].Value2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void WriteDate(int i, int j, DateTime date)
        {
            ws.Cells[i, j].Value = date;
        }
        public void WriteTS(int i, int j, TimeSpan ts)
        {
            int daysHour = ts.Days * 24;
            int hours = ts.Hours + daysHour;
            string temps = hours.ToString()+ " h " + ts.ToString("%m") + " m";
            //string temps = ts.ToString("%d") +" j " + ts.ToString("%h") + " h "+ ts.ToString("%m") + " m";
            ws.Cells[i, j].Value = temps;
        }
        public DateTime ReadDate(int i,int j)
        {
            return ws.Cells[i, j].Value;    
        }
        public string ColorCode(int i, int j)
        {
            string couleur = ws.Cells[i, j].Interior.Color.ToString();
            return couleur;
        }
        public int GetColor(int i,int j)
        {
            var couleur = ws.Cells[i,j].Interior.Color; 
            if(couleur.ToString()== "5287936") //Vert
            {
                return 0; //terminé
            }
            else if (couleur.ToString() == "16711680") //Bleu
            {
                return 1; //en cours
            }
            else if (couleur.ToString() == "255") //Rouge
            {
                return 2; //a faire
            }
            else if (couleur.ToString() == "16777215") //Blanc ou Vide 
            {
                return -1;
            }
            else
            {
                return -1;
            }
        }
        public void InsertRow(int i)
        {
            ws.Rows[i].Insert();
        }
        public bool IsSent(int i)
        {
            if (ws.Cells[i,16].Value2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DeleteBlankRow(int range)
        {           
            int i = 2;
            int count = 2;      
            while (count != range)
            {
                if (ws.Cells[i, 1].Value2 != null)
                {
                    i++;
                    count++;
                }
                else
                {
                    ws.Rows[i].Delete();
                    count++;
                }
            }          
        }
    }
}