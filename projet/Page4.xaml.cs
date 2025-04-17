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
    /// Logique d'interaction pour Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        List<string> saved = new List<string>();
        public Page4()
        {
            InitializeComponent();
            Progress.Visibility = Visibility.Hidden;
            Saving.Visibility = Visibility.Hidden;
            name.Text = App.Current.Properties["Name"].ToString();
            List<string> commandes = new List<string>();
            List<int> rows = new List<int>();
            List<int> pièces = new List<int>();
            rows = App.Current.Properties["Rows"] as List<int>;
            commandes = App.Current.Properties["Array"] as List<string>;

            //Excel excel = new Excel(@"P:\Logistique et Planning cdes\PLANNING Cdes\TEST.xlsx", 1);
            //Excel excel = new Excel(@"J:\Logistique et Planning cdes\PLANNING Cdes\planning Cdes.xlsx", 1);
            Excel excel = new Excel(@"C:\Users\Simon\Documents\Meerkat\test.xlsx", 1);

            int num_commande_column = 1;
            int client_column = 3;
            int designation_column = 5;
            int reference_column = 6;
            int qte_column = 7;
            try
            {
                num_commande_column = excel.GetColumnNumber("N° Cde");
                client_column = excel.GetColumnNumber("CLIENT");
                designation_column = excel.GetColumnNumber("DESIGNATION");
                reference_column = excel.GetColumnNumber("REFERENCE");
                qte_column = excel.GetColumnNumber("QTE");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }

            List<int> rowIndex = new List<int>();
            List<int> toutesCommandes = new List<int>();
            int count = -1;
            for (int i = 0; i < commandes.Count; i++)
            {
                count++;
                int row = int.Parse(rows[i].ToString());
                toutesCommandes.Add(row);
                while (excel.ReadCell(row + 1, num_commande_column) == excel.ReadCell(row, num_commande_column))
                {
                    row++;
                    count++;
                    toutesCommandes.Add(row);
                }
                if (i != commandes.Count - 1)
                {
                    rowIndex.Add(count);
                }
            }
            for (int i = 1; i < toutesCommandes.Count; i++)
            {
                AddRow();
            }
            for (int i = 0; i < rowIndex.Count; i++)
            {
                AddBorder(int.Parse(rowIndex[i].ToString()));
            }
            for (int i=0;i<toutesCommandes.Count;i++)
            {
                int row = int.Parse(toutesCommandes[i].ToString());
                TextBlock commandeText = TextGrid(excel.ReadCell(row, num_commande_column));
                Viewbox view = new Viewbox
                {
                    MaxWidth = 120,
                    StretchDirection = StretchDirection.DownOnly
                };
                view.Child = commandeText;
                AddViewBox(i, 0, view);

                TextBlock clientText = TextGrid(excel.ReadCell(row, client_column)); 
                Viewbox view2 = new Viewbox
                {
                    MaxWidth = 230,
                    StretchDirection = StretchDirection.DownOnly
                };
                view2.Child = clientText;
                AddViewBox(i, 1, view2);

                TextBlock designText = TextGrid(excel.ReadCell(row, designation_column)); 
                Viewbox view3 = new Viewbox
                {
                    MaxWidth = 230,
                    StretchDirection = StretchDirection.DownOnly
                };
                view3.Child = designText;
                AddViewBox(i, 2, view3);

                TextBlock planText = TextGrid(excel.ReadCell(row, reference_column)); 
                Viewbox view4 = new Viewbox
                {
                    MaxWidth = 230,
                    StretchDirection = StretchDirection.DownOnly
                };
                view4.Child = planText;
                AddViewBox(i, 3, view4);

                TextBlock quantitéText = TextGrid(excel.ReadCell(row, qte_column));
                Viewbox view5 = new Viewbox
                {
                    MaxWidth = 120,
                    StretchDirection = StretchDirection.DownOnly
                };
                view5.Child = quantitéText;
                AddViewBox(i, 4, view5);

                string name = "btn" + row.ToString();
                AddBtn(i, 5,excel.IsSent(row),name);      
            }
            excel.CloseFile();
        }
        public void AddViewBox(int i, int j, Viewbox box)
        {
            Grid.SetColumn(box, j);
            Grid.SetRow(box, i);
            grid.Children.Add(box);
        }
        public TextBlock TextGrid(string msg)
        {
            FontFamily fontFamily = new FontFamily("Berlin Sans FB Demi");
            TextBlock text = new TextBlock { Text = msg, FontFamily = fontFamily, FontSize = 24, Foreground = Brushes.White, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            return text;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Page0 page0 = new Page0();
            this.NavigationService.Navigate(page0);
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Page3 page3 = new Page3();
            this.NavigationService.Navigate(page3);
        }
        public void AddTextGrid(int i, int j, string msg)
        {
            FontFamily fontFamily = new FontFamily("Berlin Sans FB Demi");
            TextBlock text = new TextBlock { Text = msg, FontFamily = fontFamily, FontSize = 24, Foreground = Brushes.White, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            Grid.SetColumn(text, j);
            Grid.SetRow(text, i);
            grid.Children.Add(text);
        }
        public void AddRow()
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            grid.RowDefinitions.Add(rowDefinition);
        }
        public void AddBtn(int i, int j,bool sent, string name)
        {
            Brush background;
            Brush foreground;
            FontFamily font = new FontFamily("Berlin Sans FB Demi");
            string content;
            if (sent)
            {
                background = Brushes.Yellow;
                foreground = Brushes.Black;
                content = "Envoyé";   
            }
            else
            {
                background = Brushes.Red;
                foreground = Brushes.White;
                content = "Non";
            }
            
            Thickness thick = new Thickness(0);
            Button btn = new Button
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 100,
                Height = 30,
                Name = name,
                FontFamily = font,
                FontSize = 24,
                Foreground = foreground,
                Content = content,
                Background = background ,
                BorderThickness = thick,
                Resources =
                {
                    {
                        typeof(Border), new Style
                        {
                            TargetType = typeof(Border),
                            Setters =
                            {
                                new Setter { Property = Border.CornerRadiusProperty, Value = new CornerRadius(5) }
                            }
                        }
                    }
                }
            };
            btn.Click += Btn_Click;
            Grid.SetColumn(btn, j);
            Grid.SetRow(btn, i);
            grid.Children.Add(btn);
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Brush background = btn.Background;
            string commande = btn.Name.Substring(3);
            if (background == Brushes.LimeGreen)
            {
                btn.Background = Brushes.Red;
                btn.Content = "Non";
                saved.Remove(commande);
            }
            else if (background == Brushes.Red)
            {
                btn.Background = Brushes.LimeGreen;
                btn.Content = "Oui";
                saved.Add(commande);
            }   
            else
            {
                
            }
        }
        private void valider_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Application.Current.MainWindow, "Êtes-vous sur de vouloir enregistrer ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Saving.Visibility = Visibility.Visible;
                Progress.Visibility = Visibility.Visible;
                Progress.IsIndeterminate = true;
                BackgroundWorker worker2 = new BackgroundWorker();
                worker2.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker2.WorkerReportsProgress = true;
                worker2.DoWork += worker_DoWork;
                worker2.ProgressChanged += worker_ProgressChanged;
                worker2.RunWorkerAsync();
            }   
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 0)
            {
                Progress.Visibility = Visibility.Visible;
                if ((string)e.UserState == "Ouverture du fichier")
                {
                    Progress.IsIndeterminate = true;
                    Saving.Text = (string)e.UserState;
                }
                else
                {
                    Progress.IsIndeterminate = false;
                    Progress.Value = e.ProgressPercentage;
                    Saving.Text = (string)e.UserState;
                }
            }
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            //Excel excel = new Excel(@"P:\Logistique et Planning cdes\PLANNING Cdes\TEST.xlsx", 1);
            //Excel excel = new Excel(@"J:\Logistique et Planning cdes\PLANNING Cdes\planning Cdes.xlsx", 1);
            Excel excel = new Excel(@"C:\Users\Simon\Documents\Meerkat\test.xlsx", 1);

            int depart_column = 16;
            
            try
            {
                depart_column = excel.GetColumnNumber("DEPART");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }
            int i = 0;
            foreach (object item in saved)
            {
                var value = ((double)i / saved.Count) * 100;
                var pc = Convert.ToInt32(Math.Round(value, 0));
                worker.ReportProgress(pc, String.Format("Sauvegarde"));
                excel.WriteDate(int.Parse(item.ToString()), depart_column, DateTime.Now.Date); 
                i++;

            }          
            worker.ReportProgress(100, String.Format("Terminé"));
            excel.CloseSave();

        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(Application.Current.MainWindow, "Les modifications ont été sauvegardées");
            Page3 page3 = new Page3();
            this.NavigationService.Navigate(page3);
        }
        public void AddBorder(int row)
        {
            Thickness thick = new Thickness(0, 0, 0, 1);
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 23, 30, 46));
            Border border = new Border
            {
                Background = Brushes.Transparent,
                BorderBrush = Brushes.LightGray,
                BorderThickness = thick
            };
            Grid.SetRow(border, row);
            Grid.SetColumnSpan(border, 7);
            grid.Children.Add(border);
        }
    }
}
