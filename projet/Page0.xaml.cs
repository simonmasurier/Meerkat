using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Collections;

namespace projet
{
    /// <summary>
    /// Logique d'interaction pour Page0.xaml
    /// </summary>
    public partial class Page0 : Page
    {
        string password = "";
        string name = "";
        int flag = 0;
        public Page0()
        {
            InitializeComponent();
            Progress.Visibility = Visibility.Hidden;              
        }
        public void Button1_Click(object sender, RoutedEventArgs e)
        {
            flag = 0;
            if (!String.IsNullOrWhiteSpace(password = passwordBox.Password))
            {
                password = passwordBox.Password.ToString();
                statusPasswordText.Foreground = Brushes.LimeGreen;
                statusPasswordText.Text = "Chargement";
                Progress.Visibility = Visibility.Visible;

                BackgroundWorker worker = new BackgroundWorker();
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync(password);
            }
            else
            {
                statusPasswordText.Foreground = Brushes.Red;
                statusPasswordText.Text = "Veuillez entrer un identifiant";
                passwordBox.Clear();
            }
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = e.ProgressPercentage;
            statusPasswordText.Text = (string)e.UserState;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var worker = sender as BackgroundWorker;
                string password = (string)e.Argument;
                worker.ReportProgress(5, "Ouverture du fichier");

                string path1 = @"P:\Logistique et Planning cdes\PLANNING Cdes\identifiants.xlsx";
                string path2 = @"J:\Logistique et Planning cdes\PLANNING Cdes\identifiants.xlsx";
                //string path2 = @"C:\Users\Simon\Documents\Meerkat\id.xlsx";

                string fileToOpen = null;

                // Vérifier si l'un des deux fichiers existe
                if (File.Exists(path1))
                {
                    fileToOpen = path1;
                }
                else if (File.Exists(path2))
                {
                    fileToOpen = path2;
                }

                if (fileToOpen != null)
                {
                    Excel excel = new Excel(fileToOpen, 1);
                    int range = excel.GetRange();
                    int progressInterval = 20;

                    for (int i = 2; i <= range; i++)
                    {
                        if (i % progressInterval == 0)
                        {
                            var value = ((double)i / range) * 100;
                            var pc = Convert.ToInt32(Math.Round(value, 0));
                            worker.ReportProgress(pc, "Chargement");
                        }
                        if (excel.ReadCell(i, 2) == password && password != "")
                        {
                            flag = 1;
                            name = excel.ReadCell(i, 1).ToString();
                            App.Current.Properties["Name"] = excel.ReadCell(i, 1).ToString();
                            App.Current.Properties["Password"] = password;
                            break;
                        }
                    }
                    worker.ReportProgress(100, "Terminé");
                    excel.CloseFile();
                }
                else
                {
                    MessageBox.Show("Aucun fichier n'a été trouvé aux chemins spécifiés.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress.Value = 0;
            statusPasswordText.Text = "";
            if (flag == 1)
            {
                if(password == "admin")
                {
                    Page3 page3 = new Page3();
                    this.NavigationService.Navigate(page3);
                }
                else
                {
                    Page5 page5 = new Page5();
                    this.NavigationService.Navigate(page5);
                }
                
            }
            else
            {
                Progress.Visibility = Visibility.Hidden;
                statusPasswordText.Foreground = Brushes.Red;
                statusPasswordText.Text = "Identifiant incorrect";
                passwordBox.Clear();
            }
        }
        private void Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button1_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
