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
using System.Threading;

namespace projet
{
    /// <summary>
    /// Logique d'interaction pour Page4.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        ArrayList content = new ArrayList();
        ArrayList column = new ArrayList();
        ArrayList row = new ArrayList();
        ArrayList color = new ArrayList();
        ArrayList toutesCommandes = new ArrayList();

        ArrayList decoupe_list = new ArrayList();
        ArrayList pliage_list = new ArrayList();
        ArrayList soudure_list = new ArrayList();
        ArrayList traitement_list = new ArrayList();

        ArrayList etat1 = new ArrayList()
            {
                "Terminé",
                "En Cours",
                "À Faire"
            };
        ArrayList etat2 = new ArrayList()
            {
                "Terminé",
                "En Cours"
            };
        ArrayList etat3 = new ArrayList()
            {
                "En Cours",
                "À Faire"
            };
        ArrayList soudureList = new ArrayList();
        public Page6()
        {
            InitializeComponent();
            Progress.Visibility = Visibility.Hidden;
            Saving.Text = "";
            name.Text = App.Current.Properties["Name"].ToString();
            ArrayList commandes = new ArrayList();
            ArrayList rows = new ArrayList();
            ArrayList pièces = new ArrayList();
            rows = App.Current.Properties["Rows"] as ArrayList;
            commandes = App.Current.Properties["Array"] as ArrayList;

            //Excel excel = new Excel(@"P:\Logistique et Planning cdes\PLANNING Cdes\TEST.xlsx", 1);
            Excel excel = new Excel(@"J:\Logistique et Planning cdes\PLANNING Cdes\planning Cdes.xlsx", 1);
            //Excel excel = new Excel(@"C:\Users\simon\Desktop\planning Cdes.xlsx", 1);
            ArrayList rowIndex= new ArrayList();
            int count = -1;
            for (int i = 0; i < commandes.Count; i++)
            {
                count++;
                int row = int.Parse(rows[i].ToString());
                toutesCommandes.Add(row);
                while (excel.ReadCell(row + 1, 1) == excel.ReadCell(row, 1))
                {
                    row++;
                    count++;
                    toutesCommandes.Add(row);
                }
                if(i!=commandes.Count-1)
                {
                    rowIndex.Add(count);
                }              
            }
            for (int i = 1; i < toutesCommandes.Count; i++)
            {
                AddRow();
            }
            for (int i = 0; i < toutesCommandes.Count; i++)
            {
                //SETUP DES COMBOBOX
                int row0 = int.Parse(toutesCommandes[i].ToString());
                string decoupe = "Decoupe" + i.ToString();
                string pliage = "Pliage" + i.ToString();
                string soudure = "Soudure" + i.ToString();
                string traitement = "Traitement" + i.ToString();

                TextBlock commandeText = TextGrid(excel.ReadCell(row0, 1));
                Viewbox view = new Viewbox
                {
                    MaxWidth = 100,
                    StretchDirection = StretchDirection.DownOnly
                };
                view.Child = commandeText;
                AddViewBox(i, 0, view);

                TextBlock clientText = TextGrid(excel.ReadCell(row0, 3));//changé
                Viewbox view2 = new Viewbox
                {
                    Width = 130,
                    StretchDirection = StretchDirection.DownOnly
                };
                view2.Child = clientText;
                AddViewBox(i, 1, view2);

                TextBlock designText = TextGrid(excel.ReadCell(row0, 5));//changé
                Viewbox view3 = new Viewbox
                {
                    Width = 180,
                    StretchDirection = StretchDirection.DownOnly
                };
                view3.Child = designText;
                AddViewBox(i, 2, view3);

                TextBlock planText = TextGrid(excel.ReadCell(row0, 6));//changé
                Viewbox view4 = new Viewbox
                {
                    Width = 180,
                    StretchDirection = StretchDirection.DownOnly
                };
                view4.Child = planText;
                AddViewBox(i, 3, view4);

                TextBlock quantitéText = TextGrid(excel.ReadCell(row0, 7));//changé
                Viewbox view5 = new Viewbox
                {
                    Width = 80,
                    StretchDirection = StretchDirection.DownOnly
                };
                view5.Child = quantitéText;
                AddViewBox(i, 4, view5);

                //AddTextGrid(i, 0, excel.ReadCell(row0, 1)); //Commande
                //AddTextGrid(i, 1, excel.ReadCell(row0, 4)); //Designation
                //AddTextGrid(i, 2, excel.ReadCell(row0, 5)); //Plan
                //DECOUPE
                if (excel.GetColor(row0, 12) == 0)//Terminé //changé
                {
                    decoupe_list.Add("Terminé");
                    AddBtn(i, 5, 0, decoupe);
                }
                else if (excel.GetColor(row0, 12) == 1)//En Cours
                {
                    decoupe_list.Add("En Cours");
                    AddBtn(i, 5, 1, decoupe);
                }
                else if (excel.GetColor(row0, 12) == 2)//A Faire
                {
                    decoupe_list.Add("À Faire");
                    AddBtn(i, 5, 2, decoupe);
                }
                else
                {
                    excel.FillRed(row0, 12);
                    decoupe_list.Add("À Faire");
                    AddBtn(i, 5, 2, decoupe);
                }
                //PLIAGE
                if (excel.GetColor(row0, 13) == 0)//Terminé //changé
                {
                    pliage_list.Add("Terminé");
                    AddBtn(i, 6, 0, pliage);
                }
                else if (excel.GetColor(row0, 13) == 1)//En Cours
                {
                    pliage_list.Add("En Cours");
                    AddBtn(i, 6, 1, pliage);
                }
                else if (excel.GetColor(row0, 13) == 2)//A Faire
                {
                    pliage_list.Add("À Faire");
                    AddBtn(i, 6, 2, pliage);
                }
                else
                {
                    excel.FillRed(row0, 13);
                    pliage_list.Add("À Faire");
                    AddBtn(i, 6, 2, pliage);                                      
                }
                //SOUDURE
                if (excel.SoudurePrévue(row0, 10))//changé
                {
                    soudureList.Add(1);
                    if (excel.GetColor(row0, 14) == 0)//Terminé //changé
                    {
                        soudure_list.Add("Terminé");
                        AddBtn(i, 7, 0, soudure);
                    }
                    else if (excel.GetColor(row0, 14) == 1)//En Cours //changé
                    {
                        soudure_list.Add("En Cours");
                        AddBtn(i, 7, 1, soudure);
                    }
                    else if (excel.GetColor(row0, 14) == 2)//A Faire
                    {
                        soudure_list.Add("À Faire");
                        AddBtn(i, 7, 2, soudure);
                    }
                    else
                    {
                        excel.FillRed(row0, 14);
                        soudure_list.Add("À Faire");
                        AddBtn(i, 7, 2, soudure);
                    }
                }
                else
                {
                    soudureList.Add(0);
                    soudure_list.Add("");
                    AddText(i, 7, "PAS DE SOUDURE");
                }
                //TRAITEMENT
                if (excel.GetColor(row0, 15) == 0)//Terminé //changé
                {
                    traitement_list.Add("Terminé");
                    AddBtn(i, 8, 0, traitement);
                }
                else if (excel.GetColor(row0, 15) == 1)//En Cours
                {
                    traitement_list.Add("En Cours");
                    AddBtn(i, 8, 1, traitement);
                }
                else if (excel.GetColor(row0, 15) == 2)//A Faire
                {
                    traitement_list.Add("À Faire");
                    AddBtn(i, 8, 2, traitement);
                }
                else
                {
                    excel.FillRed(row0, 15);
                    traitement_list.Add("À Faire");
                    AddBtn(i, 8, 2, traitement);
                }
            }
            excel.CloseSave();
        }
        public void AddBtn(int i, int j, int color, string name)
        {
            Brush background;
            Brush foreground;
            FontFamily font = new FontFamily("Berlin Sans FB Demi");
            string content;
            if (color == 0) //Terminé
            {
                background = Brushes.LimeGreen;
                foreground = Brushes.White;
                content = "Terminé";
            }
            else if(color == 1)// En cours
            {
                background = Brushes.Blue;
                foreground = Brushes.White;
                content = "En Cours";
            }
            else if(color == 2) //A faire
            {
                background = Brushes.Red;
                foreground = Brushes.White;
                content = "À Faire";
            }
            else //A faire
            {
                background = Brushes.Red;
                foreground = Brushes.White;
                content = "À Faire";
            }

            Thickness thick = new Thickness(0);
            Button btn = new Button
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 100,
                Height = 40,
                Name = name,
                FontFamily = font,
                FontSize = 20,
                Foreground = foreground,
                Content = content,
                Background = background,
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
            string name = btn.Name;
            string commande = btn.Name.Substring(3);
            bool flag = false;
            if (background != Brushes.LimeGreen)
            {
                for (int i = 0; i < toutesCommandes.Count; i++) //Pour chaque ligne
                {
                    for (int j = 0; j < 4; j++) //Pour chaque colonne
                    {
                        int place = 9 * i + 5 + j;
                        object btn2 = grid.Children[place];
                        if (btn2 is Button)
                        {
                            string name2 = (btn2 as Button).Name;
                            if (name == name2)
                            {
                                flag = true;
                                if (background == Brushes.Red)
                                {
                                    if (j == 0)
                                    {
                                        btn.Background = Brushes.Blue;
                                        btn.Content = "En Cours";
                                    }
                                    else if (j == 1 || j == 2)
                                    {
                                        int place0 = 9 * i + 4 + j;
                                        object avant = grid.Children[place0];
                                        Brush check = (avant as Button).Background;
                                        if (check == Brushes.Blue)
                                        {
                                            btn.Background = Brushes.Blue;
                                            btn.Content = "En Cours";
                                        }
                                        else if (check == Brushes.LimeGreen)
                                        {
                                            btn.Background = Brushes.Blue;
                                            btn.Content = "En Cours";
                                        }
                                    }
                                    else if (j == 3)
                                    {
                                        int place0 = 9 * i + 4 + j;
                                        object avant = grid.Children[place0];
                                        if (avant is Button)
                                        {
                                            Brush check = (avant as Button).Background;
                                            if (check == Brushes.Blue)
                                            {
                                                btn.Background = Brushes.Blue;
                                                btn.Content = "En Cours";
                                            }
                                            else if (check == Brushes.LimeGreen)
                                            {
                                                btn.Background = Brushes.Blue;
                                                btn.Content = "En Cours";
                                            }
                                        }
                                        else
                                        {
                                            int place02 = 9 * i + 3 + j;
                                            object avant2 = grid.Children[place02];
                                            Brush check = (avant2 as Button).Background;
                                            if (check == Brushes.Blue)
                                            {
                                                btn.Background = Brushes.Blue;
                                                btn.Content = "En Cours";
                                            }
                                            else if (check == Brushes.LimeGreen)
                                            {
                                                btn.Background = Brushes.Blue;
                                                btn.Content = "En Cours";
                                            }
                                        }
                                    }
                                }
                                else if (background == Brushes.Blue)
                                {
                                    if (j == 0)
                                    {
                                        btn.Background = Brushes.LimeGreen;
                                        btn.Content = "Terminé";
                                    }
                                    else if (j == 1 || j == 2)
                                    {
                                        int place0 = 9 * i + 4 + j;
                                        object avant = grid.Children[place0];
                                        Brush check = (avant as Button).Background;
                                        if (check == Brushes.LimeGreen)
                                        {
                                            btn.Background = Brushes.LimeGreen;
                                            btn.Content = "Terminé";
                                        }
                                    }
                                    else if (j == 3)
                                    {
                                        int place0 = 9 * i + 4 + j;
                                        object avant = grid.Children[place0];
                                        if (avant is Button)
                                        {
                                            Brush check = (avant as Button).Background;
                                            if (check == Brushes.LimeGreen)
                                            {
                                                btn.Background = Brushes.LimeGreen;
                                                btn.Content = "Terminé";
                                            }
                                        }
                                        else
                                        {
                                            int place02 = 9 * i + 3 + j;
                                            object avant2 = grid.Children[place02];
                                            Brush check = (avant2 as Button).Background;
                                            if (check == Brushes.LimeGreen)
                                            {
                                                btn.Background = Brushes.LimeGreen;
                                                btn.Content = "Terminé";
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }                       
                    }
                    if (flag == true)
                    {
                        break;
                    }
                }
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Page0 page0 = new Page0();
            this.NavigationService.Navigate(page0);
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Page5 page5 = new Page5();
            this.NavigationService.Navigate(page5);
        }
        public TextBlock TextGrid(string msg)
        {
            FontFamily fontFamily = new FontFamily("Berlin Sans FB Demi");
            TextBlock text = new TextBlock {Text = msg, FontFamily = fontFamily, FontSize = 20, Foreground = Brushes.White, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            return text;
        }
        public void AddRow()
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            grid.RowDefinitions.Add(rowDefinition);
        }
        public void AddViewBox(int i, int j, Viewbox box)
        {
            Grid.SetColumn(box, j);
            Grid.SetRow(box, i);
            grid.Children.Add(box);
        }
        public void AddBlankRow()
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(2);
            grid.RowDefinitions.Add(rowDefinition);
        }
        private void valider_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Application.Current.MainWindow, "Êtes-vous sur de vouloir enregistrer ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BackgroundWorker worker2 = new BackgroundWorker();
                worker2.RunWorkerCompleted += worker_RunWorkerCompleted2;
                worker2.WorkerReportsProgress = true;
                worker2.DoWork += worker_DoWork2;
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
                else if ((string)e.UserState == "Chargement des modifications")
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
        private void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(10, String.Format("Chargement des modifications"));
            Thread.Sleep(500);
            worker.ReportProgress(100, String.Format("Chargement des modifications"));
        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            Saving.Text = "Veuillez patienter";
            Sauvegarde();
        }
        private void Sauvegarde()
        {
            content.Clear();
            column.Clear();
            row.Clear();
            color.Clear();
            string name = App.Current.Properties["Name"].ToString();
            string date = DateTime.Now.ToString("dd/MM/yyyy H:mm");

            for (int i = 0; i < toutesCommandes.Count; i++)
            {
                int row2 = int.Parse(toutesCommandes[i].ToString());
                string decoupe_flag = decoupe_list[i].ToString();
                string pliage_flag = pliage_list[i].ToString();
                string soudure_flag = "";
                if (int.Parse(soudureList[i].ToString()) == 1)
                {
                    soudure_flag = soudure_list[i].ToString();
                }
                string traitement_flag = traitement_list[i].ToString();
                if (GetState(5 + 9 * i) != decoupe_flag)
                {
                    row.Add(row2);
                    column.Add(22);//changé
                    if (GetState(5 + 9 * i) == "À Faire")
                    {
                        //excel.FillRed(row2, 11);
                        color.Add("Red");
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                    else if (GetState(5 + 9 * i) == "En Cours")
                    {
                        //excel.FillBlue(row2, 11);
                        color.Add("Blue");
                        content.Add(name + " :" + "\nEn Cours - " + date);
                    }
                    else if (GetState(5 + 9 * i) == "Terminé")
                    {
                        //excel.FillGreen(row2, 11);
                        color.Add("Green");
                        content.Add(name + " :" + "\nTerminé - " + date);
                    }
                    else
                    {
                        //excel.FillRed(row2, 11);
                        color.Add("Red");
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                }
                if (GetState(6 + 9 * i) != pliage_flag)
                {
                    row.Add(row2);
                    column.Add(23);//changé
                    if (GetState(6 + 9 * i) == "À Faire")
                    {
                        //excel.FillRed(row2, 12);
                        color.Add("Red");
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                    else if (GetState(6 + 9 * i) == "En Cours")
                    {
                        //excel.FillBlue(row2, 12);
                        color.Add("Blue");
                        content.Add(name + " :" + "\nEn Cours - " + date);
                    }
                    else if (GetState(6 + 9 * i) == "Terminé")
                    {
                        //excel.FillGreen(row2, 12);
                        color.Add("Green");
                        content.Add(name + " :" + "\nTerminé - " + date);
                    }
                    else
                    {
                        ;
                        //excel.FillRed(row2, 12);
                        color.Add("Red");
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                }
                if (int.Parse(soudureList[i].ToString()) == 1)
                {
                    if (GetState(7 + 9 * i) != soudure_flag)
                    {
                        row.Add(row2);
                        column.Add(24);//changé
                        if (GetState(7 + 9 * i) == "À Faire")
                        {
                            //excel.FillRed(row2, 13);
                            color.Add("Red");
                            content.Add(name + " :" + "\nÀ faire - " + date);
                        }
                        else if (GetState(7 + 9 * i) == "En Cours")
                        {
                            //excel.FillBlue(row2, 13);
                            color.Add("Blue");
                            content.Add(name + " :" + "\nEn Cours - " + date);
                        }
                        else if (GetState(7 + 9 * i) == "Terminé")
                        {
                            //excel.FillGreen(row2, 13);
                            color.Add("Green");
                            content.Add(name + " :" + "\nTerminé - " + date);
                        }
                        else
                        {
                            color.Add("Red");
                            //excel.FillRed(row2, 13);
                            content.Add(name + " :" + "\nÀ faire - " + date);
                        }
                    }
                }
                else
                {
                    row.Add(row2);
                    column.Add(24);//changé
                    color.Add("White");
                    content.Add("");
                }
                if (GetState(8 + 9 * i) != traitement_flag)
                {
                    row.Add(row2);
                    column.Add(25);//changé
                    if (GetState(8 + 9 * i) == "À Faire")
                    {
                        color.Add("Red");
                        //excel.FillRed(row2, 14);
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                    else if (GetState(8 + 9 * i) == "En Cours")
                    {
                        color.Add("Blue");
                        //excel.FillBlue(row2, 14);
                        content.Add(name + " :" + "\nEn Cours - " + date);
                    }
                    else if (GetState(8 + 9 * i) == "Terminé")
                    {
                        color.Add("Green");
                        //excel.FillGreen(row2, 14);
                        content.Add(name + " :" + "\nTerminé - " + date);
                    }
                    else
                    {
                        color.Add("Red");
                        //excel.FillRed(row2, 14);
                        content.Add(name + " :" + "\nÀ faire - " + date);
                    }
                }
            }
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();

        }
        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime ajd = DateTime.Now;
            var worker = sender as BackgroundWorker;
            worker.ReportProgress(5, String.Format("Ouverture du fichier"));
            //Excel excel = new Excel(@"P:\Logistique et Planning cdes\PLANNING Cdes\TEST.xlsx", 1);
            Excel excel = new Excel(@"J:\Logistique et Planning cdes\PLANNING Cdes\planning Cdes.xlsx", 1);
            //Excel excel = new Excel(@"C:\Users\simon\Desktop\planning Cdes.xlsx", 1);
            if (row.Count > 0)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var value = ((double)i / row.Count) * 100;
                    var pc = Convert.ToInt32(Math.Round(value, 0));
                    worker.ReportProgress(pc, String.Format("Sauvegarde"));
                    if (color[i] == "Red")
                    {
                        excel.FillRed(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 10);
                    }
                    else if (color[i] == "Blue")
                    {
                        excel.FillBlue(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 10);
                        excel.WriteDate(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 4, ajd);
                    }
                    else if (color[i] == "Green")
                    {
                        excel.FillGreen(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 10);
                        if (excel.IsCellDated(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 4))
                        {
                            DateTime start = excel.ReadDate(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 4);
                            TimeSpan time = WorkTime(start, ajd);
                            excel.WriteTS(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 4, time);
                        }
                        else
                        {
                            excel.CellOverWrite(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 4, "fini : " + ajd.ToString() + "\n(début inconnu)");
                        }

                    }
                    else if (color[i] == "White")
                    {
                        excel.FillWhite(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()) - 10);
                    }
                    excel.CellWrite(int.Parse(row[i].ToString()), int.Parse(column[i].ToString()), content[i].ToString());
                }
            }           
            worker.ReportProgress(100, String.Format("Sauvegarde"));
            excel.CloseSave();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress.Value = 100;
            Saving.Text = "Sauvegarde Terminée";
            MessageBox.Show(Application.Current.MainWindow, "Les modifications ont été sauvegardées");
            Page5 page5 = new Page5();
            this.NavigationService.Navigate(page5);
        }
        public TimeSpan WorkTime(DateTime startDate, DateTime endDate)
        {
            DateTime start = startDate.Date;
            DateTime end = endDate.Date;
            TimeSpan total = new TimeSpan();
            TimeSpan startTime = new TimeSpan();
            TimeSpan endTime = new TimeSpan();
            TimeSpan matin = new TimeSpan(0, 8, 0, 0);
            TimeSpan pauseMidi = new TimeSpan(0, 1, 30, 0);
            TimeSpan apresmidi = new TimeSpan(0, 3, 00, 0);
            TimeSpan seizeT = new TimeSpan(0, 16, 30, 0);
            TimeSpan midi = new TimeSpan(0, 12, 0, 0);
            TimeSpan treizeT = new TimeSpan(0, 13, 30, 0);
            TimeSpan soir = new TimeSpan(0, 9, 30, 0);
            TimeSpan deuxJours = new TimeSpan(2, 0, 0, 0);
            TimeSpan journee = new TimeSpan(0, 7, 0, 0);
            TimeSpan hed = new TimeSpan(0, 8, 30, 0);

            int year = DateTime.Now.Year;
            int count = 0;
            DateTime date1 = new DateTime(year, 01, 01);
            DateTime date2 = new DateTime(year, 05, 01);
            DateTime date3 = new DateTime(year, 05, 08);
            DateTime date4 = new DateTime(year, 07, 14);
            DateTime date5 = new DateTime(year, 09, 15);
            DateTime date6 = new DateTime(year, 11, 01);
            DateTime date7 = new DateTime(year, 11, 11);
            DateTime date8 = new DateTime(year, 12, 25);
            List<DateTime> excludeDates = new List<DateTime>()
            {
                date1,date2,date3,date4,date5,date6,date7,date8
            };
            //////////////////////////////////////////////////
            ///
            TimeSpan diff = endDate - startDate;
            if (startDate.Date == endDate.Date)
            {
                if (startDate.TimeOfDay < midi && endDate.TimeOfDay <= midi)
                {
                    total = diff;
                }
                else if (startDate.TimeOfDay >= treizeT && endDate.TimeOfDay > treizeT)
                {
                    total = diff;
                }
                else if (startDate.TimeOfDay <= midi && endDate.TimeOfDay >= treizeT)
                {
                    total = diff - pauseMidi;
                }
                else
                {
                    total = diff;
                }
            }
            else if (endDate.Date - startDate.Date >= deuxJours)
            {
                for (DateTime index = start.AddDays(1); index < end; index = index.AddDays(1))
                {
                    if (index.DayOfWeek != DayOfWeek.Sunday && index.DayOfWeek != DayOfWeek.Saturday)
                    {
                        bool excluded = false;
                        for (int i = 0; i < excludeDates.Count; i++)
                        {
                            if (index.Date.CompareTo(excludeDates[i].Date) == 0)
                            {
                                excluded = true;
                                break;
                            }
                        }

                        if (!excluded)
                        {
                            total += journee;
                        }
                    }
                }
                if (startDate.TimeOfDay <= midi)
                {
                    startTime = midi - startDate.TimeOfDay + apresmidi;
                }
                else
                {
                    startTime = seizeT - startDate.TimeOfDay;
                }
                if (endDate.TimeOfDay >= treizeT)
                {
                    endTime = endDate.TimeOfDay - soir;
                }
                else
                {
                    endTime = endDate.TimeOfDay - matin;
                }
                total += startTime + endTime;
            }
            else
            {
                if (startDate.TimeOfDay <= midi)
                {
                    startTime = midi - startDate.TimeOfDay + apresmidi;
                }
                else
                {
                    startTime = seizeT - startDate.TimeOfDay;
                }
                if (endDate.TimeOfDay >= treizeT)
                {
                    endTime = endDate.TimeOfDay - soir;
                }
                else
                {
                    endTime = endDate.TimeOfDay - matin;
                }
                total = startTime + endTime;
            }
            return total;
        }
        public string GetState(int i)
        {
            Button btn = grid.Children[i] as Button;          
            if (btn.Background == Brushes.Blue)
            {
                return "En Cours";
            }
            if (btn.Background == Brushes.LimeGreen)
            {
                return "Terminé";
            }
            else
            {
                return "À Faire";
            }
        }      
        public void AddText(int i, int j, string text)
        {
            FontFamily fontFamily = new FontFamily("Berlin Sans FB Demi");
            //FontWeight fontWeight = FontWeights.Bold;
            TextBlock textBlock = new TextBlock { Text = text, FontSize = 16, FontFamily = fontFamily, Foreground = Brushes.Red, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(textBlock, i);
            Grid.SetColumn(textBlock, j);
            grid.Children.Add(textBlock);
        }
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Page6 page6 = new Page6();
            this.NavigationService.Navigate(page6);
        }       
    }
}
