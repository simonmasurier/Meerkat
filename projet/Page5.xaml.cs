using System;
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
using System.Collections;
using System.ComponentModel;

namespace projet
{
    /// <summary>
    /// Logique d'interaction pour Page3.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        ArrayList list = new ArrayList();
        ArrayList erreurs = new ArrayList();
        ArrayList rows = new ArrayList();
        public Page5()
        {
            InitializeComponent();
            name.Text = App.Current.Properties["Name"].ToString();
            Progress.Visibility = Visibility.Hidden;
            statusProgress.Visibility = Visibility.Hidden;

        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Addbar.Text))
            {
                if (!ListView1.Items.Contains(Addbar.Text))
                {
                    ListView1.Items.Add(Addbar.Text);
                    list.Add(Addbar.Text);
                    Addbar.Clear();
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "Commande déjà saisie");
                    Addbar.Clear();
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Veuillez entrer un numéro de commande");
                Addbar.Clear();
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Page0 page0 = new Page0();
            this.NavigationService.Navigate(page0);
        }
        private void Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddClick(sender, new RoutedEventArgs());
            }
        }
        private void supp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                list.Remove(ListView1.Items[ListView1.Items.IndexOf(ListView1.SelectedItem)].ToString());
                ListView1.Items.RemoveAt(ListView1.Items.IndexOf(ListView1.SelectedItem));
            }
            catch
            {
                MessageBox.Show(Application.Current.MainWindow, "Veuillez sélectionner un élément à supprimer");
            }
        }
        private void Suivant_Click(object sender, RoutedEventArgs e)
        {
            if (ListView1.Items.Count > 0)
            {
                statusProgress.Visibility = Visibility.Visible;
                statusProgress.Foreground = Brushes.LimeGreen;
                statusProgress.Text = "Chargement";
                Progress.Visibility = Visibility.Visible;
                BackgroundWorker worker = new BackgroundWorker();
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Veuillez saisir au moins une commande avant de continuer");
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((string)e.UserState == "Ouverture du planning")
            {
                Progress.IsIndeterminate = true;
                statusProgress.Text = (string)e.UserState;
            }
            else
            {
                Progress.IsIndeterminate = false;
                Progress.Value = e.ProgressPercentage;
                statusProgress.Text = (string)e.UserState;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            erreurs.Clear();
            rows.Clear();
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(5, "Ouverture du planning");


            //Excel excel = new Excel(@"P:\Logistique et Planning cdes\PLANNING Cdes\TEST.xlsx", 1);
            Excel excel = new Excel(@"J:\Logistique et Planning cdes\PLANNING Cdes\planning Cdes.xlsx", 1);
            //Excel excel = new Excel(@"C:\Users\simon\Desktop\planning Cdes.xlsx", 1);

            int range = excel.GetRange();

            for (int j = 0; j < list.Count; j++)
            {
                int flag = 0;
                for (int i = 2; i <= range; i++)
                {
                    var value = ((double)i / range) * 100;
                    var pc = Convert.ToInt32(Math.Round(value, 0));
                    worker.ReportProgress(pc, String.Format("Recherche de la commande : " + (j + 1).ToString() + "/" + list.Count.ToString()));
                    if (list[j].ToString() == excel.ReadCell(i, 1).ToString())
                    {
                        flag = 1;
                        rows.Add(i);
                        break;
                    }
                }
                if (flag == 0)
                {
                    erreurs.Add(list[j]);
                }
            }
            worker.ReportProgress(100, String.Format("Recherche Terminée"));
            excel.CloseFile();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (erreurs.Count == 0)
            {
                App.Current.Properties["Array"] = list;
                App.Current.Properties["Rows"] = rows;
                Page6 page6 = new Page6();
                this.NavigationService.Navigate(page6);
            }
            else if (erreurs.Count == 1 && list.Count != 1)
            {
                Progress.Visibility = Visibility.Hidden;
                statusProgress.Visibility = Visibility.Hidden;
                string erreurMsg = "Cette commande est introuvable :\n\n";
                erreurMsg += erreurs[0].ToString() + "\n";
                erreurMsg += "\nVoulez-vous continuer sans cette commande ?";
                if (MessageBox.Show(Application.Current.MainWindow, erreurMsg, "Commande Introuvable", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    list.Remove(erreurs[0].ToString());
                    App.Current.Properties["Array"] = list;
                    App.Current.Properties["Rows"] = rows;
                    Page6 page6 = new Page6();
                    this.NavigationService.Navigate(page6);
                }

            }
            else if (erreurs.Count == 1 && list.Count == 1)
            {
                Progress.Visibility = Visibility.Hidden;
                statusProgress.Visibility = Visibility.Hidden;
                MessageBox.Show(Application.Current.MainWindow, "Commande Introuvable");
            }
            else if (erreurs.Count != 1 && erreurs.Count == list.Count)
            {
                Progress.Visibility = Visibility.Hidden;
                statusProgress.Visibility = Visibility.Hidden;
                MessageBox.Show(Application.Current.MainWindow, "Commandes Introuvables");
            }
            else if (erreurs.Count > 1)
            {
                Progress.Visibility = Visibility.Hidden;
                statusProgress.Visibility = Visibility.Hidden;
                string erreurMsg = "Ces commandes sont introuvables :\n\n";
                for (int i = 0; i < erreurs.Count; i++)
                {
                    erreurMsg += erreurs[i].ToString() + "\n";
                }
                erreurMsg += "\nVoulez-vous continuer sans ces commandes ?";
                if (MessageBox.Show(Application.Current.MainWindow, erreurMsg, "Commande Introuvable", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    foreach (object erreur in erreurs)
                    {
                        list.Remove(erreur.ToString());
                    }
                    App.Current.Properties["Array"] = list;
                    App.Current.Properties["Rows"] = rows;
                    Page6 page6 = new Page6();
                    this.NavigationService.Navigate(page6);
                }
            }
        }
        private void toutSupp_Click(object sender, RoutedEventArgs e)
        {
            ListView1.Items.Clear();
            list.Clear();
        }
    }
}
