using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.IO;

namespace NewSuperPuperSolSim
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timeKeeper = new DispatcherTimer();
        DispatcherTimer changePosPlanetKeeper = new DispatcherTimer();
        double T;
        double G = 6.67430 * Math.Pow(10, -11);
        double speedOfScreenMoove = 10;
        public List<SolObject> solObjects = new List<SolObject>();
        bool isOnAir = true;
        double smethnie = 4000000;
        public Image newPlanetImage = null;
        Image selectedPlanet;
        double zatichka;
        public string worldName = "world";

        public MainWindow()
        {
            InitializeComponent();

            Fild.Height = 8000000;
            Fild.Width = 8000000;
            Fild.Margin = new Thickness((-1) * smethnie, (-1) * smethnie, 0, 0);
            T = 20; //sec

            

            /* {

            new SolObject(){
                Name = "Son",
                vX = 0,
                vY = -0.03,
                m = 6000000000, //kg
                x = A2.Margin.Left + A2.Width / 2,
                y = A2.Margin.Top + A2.Width / 2,
                UI = A2,
                IsStattic = false,
            },
            new SolObject(){
                Name = "Eth",
                vX = 0, // V* T
                vY = 0.03,
                m = 6000000000,
                x = A1.Margin.Left + A1.Width / 2,
                y = A1.Margin.Top + A1.Width / 2,
                UI = A1,
                IsStattic = false,
                aX = 0,
                aY = 0,
            }
            };
            */
            timeKeeper.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timeKeeper.Tick += Timer_Tick;
            timeKeeper.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (!isOnAir)
                return;

            for (int i = 0; i < solObjects.Count; i++)
                for (int j = 0; j < solObjects.Count; j++)
                {
                    if ((i == j) || solObjects[i].IsStattic)
                        continue;

                    double x1 = solObjects[i].x;
                    double y1 = solObjects[i].y;
                    double x2 = solObjects[j].x;
                    double y2 = solObjects[j].y;
                    double m2 = solObjects[j].m;

                    double R = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                    if (R == 0)
                        continue;

                    solObjects[i].aX += G * m2 * (x2 - x1) / Math.Pow(R, 3);   //// гдето тут 
                    solObjects[i].aY += G * m2 * (y2 - y1) / Math.Pow(R, 3);
                }

            for (int i = 0; i < solObjects.Count; i++)
            {
                if (solObjects[i].IsStattic)
                    continue;
                solObjects[i].vX += solObjects[i].aX * T;
                solObjects[i].vY += solObjects[i].aY * T;

                solObjects[i].x += solObjects[i].vX * T;
                solObjects[i].y += solObjects[i].vY * T;

                solObjects[i].aX = 0;
                solObjects[i].aY = 0;
            }
            for (int i = 0; i < solObjects.Count; i++)
                ((Image)solObjects[i].UI).Margin = new Thickness(solObjects[i].x - ((Image)solObjects[i].UI).Width / 2 + smethnie, solObjects[i].y - ((Image)solObjects[i].UI).Width / 2 + smethnie, 0, 0);
        }

        public void SetNewObjects(List<SolObject> newSolObjects)
        {
            solObjects = newSolObjects;
            Fild.Children.Clear();

            ListOfPlanet.Items.Clear();
            /*string[] newPlanetList = new string[solObjects.Count];
            for (int i = 0; i < solObjects.Count; i++)
            {
                newPlanetList[i] = solObjects[i].Name + $"({i})";
                Fild.Children.Add((Image)solObjects[i].UI);
            }
            ListOfPlanet.ItemsSource = newPlanetList;
            */
            for (int i = 0; i < solObjects.Count; i++)
            {
                ListOfPlanet.Items.Add(solObjects[i].Name + $"({i})");
                Fild.Children.Add((Image)solObjects[i].UI);
            }
        }   

        private void Button_Click_Add_Planet(object sender, RoutedEventArgs e)
        {
            if (!(Double.TryParse(TextRad.Text, out zatichka) &&
                Double.TryParse(TextBoxVx.Text, out zatichka) &&
                Double.TryParse(TextBoxVy.Text, out zatichka) &&
                Double.TryParse(TextBoxM.Text, out zatichka) &&
                Double.TryParse(TextBoxX.Text, out zatichka) &&
                Double.TryParse(TextBoxY.Text, out zatichka)))
                return;

            double Rad = Double.Parse(TextRad.Text);
            if (Rad < 0)
            {
                Rad *= (-1);
                MessageBox.Show("no it`s " + Rad);
            }
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            Image newPlanet = new Image()
            {
                Width = Rad,
                Height = Rad,
                Margin = new Thickness(Double.Parse(TextBoxX.Text), Double.Parse(TextBoxY.Text), 0, 0),
                Visibility = Visibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Source = newPlanetImage.Source,

                //Name = solObjects.Count + "",
            };
            newPlanet.MouseRightButtonDown += new MouseButtonEventHandler(Planet_MouseRightButtonDown);
            Fild.Children.Add(newPlanet);
            solObjects.Add(new SolObject()
            {
                Name = TextBoxName.Text,
                vX = Double.Parse(TextBoxVx.Text),
                vY = Double.Parse(TextBoxVy.Text),
                m = Double.Parse(TextBoxM.Text), //kg
                x = Double.Parse(TextBoxX.Text),
                y = Double.Parse(TextBoxY.Text),
                UI = newPlanet,
                IsStattic = false,
            });

            ListOfPlanet.Items.Add(solObjects[solObjects.Count - 1].Name + $"({solObjects.Count - 1})");
        }

        private void Button_Click_Delete_Planet(object sender, RoutedEventArgs e)
        {
            try
            {
                int nobr = SelectedItem();
                Fild.Children.Remove((Image)(solObjects[nobr].UI));
                solObjects.RemoveAt(nobr);
                ListOfPlanet.Items.Remove(ListOfPlanet.SelectedItem);
                SelectedItem();

            }
            catch (Exception eror)
            {
                MessageBox.Show("you already deleted it");
            }
}

        private void Button_Click_Stop_Start(object sender, RoutedEventArgs e)
        {
            isOnAir = !isOnAir;
        }

        private void ListOfPlanet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ButtonOfAddPlanet.Visibility = Visibility.Collapsed;
            ButtonOfChangePlanet.Visibility = Visibility.Visible;

            int nombr = SelectedItem();
            TextBoxM.Text = solObjects[nombr].m + "";
            TextBoxName.Text = solObjects[nombr].Name + "";
            TextBoxVx.Text = solObjects[nombr].vX + "";
            TextBoxVy.Text = solObjects[nombr].vY + "";
            TextBoxX.Text = solObjects[nombr].x + "";
            TextBoxY.Text = solObjects[nombr].y + "";
        }

        private void Button_Click_Change_Planet(object sender, RoutedEventArgs e)
        {
            ButtonOfAddPlanet.Visibility = Visibility.Visible;
            ButtonOfChangePlanet.Visibility = Visibility.Collapsed;

            int nombr = SelectedItem();
            solObjects[nombr] = new SolObject()
            {
                Name = TextBoxName.Text,
                vX = Double.Parse(TextBoxVx.Text),
                vY = Double.Parse(TextBoxVy.Text),
                m = Double.Parse(TextBoxM.Text), //kg
                x = Double.Parse(TextBoxX.Text),
                y = Double.Parse(TextBoxY.Text),
                UI = solObjects[nombr].UI,
                IsStattic = false,
            };
            ((Image)solObjects[nombr].UI).Source = PlanetImage.Source;
        }

        private int SelectedItem()
        {
            try
            {
                string selected = ListOfPlanet.SelectedItem.ToString();
                string nombrString = "";
                int nombr;

                for (int i = selected.Length - 2; i >= 0; i--)
                {
                    if (selected[i] == '(')
                        break;
                    nombrString += selected[i];
                }
                char[] a = nombrString.ToCharArray();
                Array.Reverse(a);
                nombrString = new string(a);
                nombr = Int32.Parse(nombrString);

                return nombr;
            }
            catch(Exception eror) 
            {
                MessageBox.Show("fig tebe");
                return 0;
            }
        }

        private void Button_Click_Up(object sender, RoutedEventArgs e)
        {
            Thickness pos = Fild.Margin;
            pos.Top += speedOfScreenMoove;
            Fild.Margin = pos;
        }
        private void Button_Click_Down(object sender, RoutedEventArgs e)
        {
            Thickness pos = Fild.Margin;
            pos.Top -= speedOfScreenMoove;
            Fild.Margin = pos;
        }
        private void Button_Click_Left(object sender, RoutedEventArgs e)
        {
            Thickness pos = Fild.Margin;
            pos.Left += speedOfScreenMoove;
            Fild.Margin = pos;
        }
        private void Button_Click_Right(object sender, RoutedEventArgs e)
        {
            Thickness pos = Fild.Margin;
            pos.Left -= speedOfScreenMoove;
            Fild.Margin = pos;
        }

        private void PlanetImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowViorIamgePlnt viorIamgePlnt = new WindowViorIamgePlnt(this);
            viorIamgePlnt.Owner = this;
            viorIamgePlnt.Show();
        }

        public void UbdatePlanetImage()
        {
            PlanetImage.Source = newPlanetImage.Source;
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(e.Key == Key.Enter) || !(Double.TryParse(TextBoxWrap.Text, out zatichka)))
                return;
            T = Double.Parse(TextBoxWrap.Text);
        }

        private void Planet_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (changePosPlanetKeeper.IsEnabled)
            {
                changePosPlanetKeeper.Stop();
                isOnAir = true;
                for (int i = 0; i < solObjects.Count; i++)
                    if (solObjects[i].UI == selectedPlanet)
                    {
                        solObjects[i].x = selectedPlanet.Margin.Left - smethnie + selectedPlanet.Height / 2;
                        solObjects[i].y = selectedPlanet.Margin.Top - smethnie + selectedPlanet.Height / 2;
                        break;
                    }
                return;
            }

            isOnAir = false;
            selectedPlanet = (Image)sender;
            changePosPlanetKeeper.Interval = new TimeSpan(0, 0, 0, 0, 16);
            changePosPlanetKeeper.Tick += ChangePosPlanetTick;
            changePosPlanetKeeper.Start();

        }

        private void ChangePosPlanetTick(object sender, object e) 
        {
            
            // CorPos.Content = "x:" + Mouse.GetPosition(null).X + "| y:" + Mouse.GetPosition(null).Y;
            
            double newPosX = Mouse.GetPosition(null).X + Math.Abs(Fild.Margin.Left) - 30 - selectedPlanet.Height / 2; // 30 ширина кнопки вверх влево
            double newPosY = Mouse.GetPosition(null).Y + Math.Abs(Fild.Margin.Top) - 30 - selectedPlanet.Width / 2;
            selectedPlanet.Margin = new Thickness( newPosX, newPosY , 0, 0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Dont worry we save all");

            using (StreamWriter streamWriter = new StreamWriter($"./Saves/{worldName}.txt", false)) 
            {
                for(int i = 0; i < solObjects.Count; i++) 
                {
                    streamWriter.WriteLine(solObjects[i].Name);
                    streamWriter.WriteLine(solObjects[i].vX);
                    streamWriter.WriteLine(solObjects[i].vY);
                    streamWriter.WriteLine(solObjects[i].m);
                    streamWriter.WriteLine(solObjects[i].x);
                    streamWriter.WriteLine(solObjects[i].y);
                    streamWriter.WriteLine(((Image)solObjects[i].UI).Source);
                    streamWriter.WriteLine(((Image)solObjects[i].UI).Width);
                }
            }
        }
    }
}

/*
public double vX, vY, m, x, y, aX, aY;
public Object UI;
public string Name;
public bool IsStattic;
*/