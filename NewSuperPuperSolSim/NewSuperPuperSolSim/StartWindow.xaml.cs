using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.IO;

namespace NewSuperPuperSolSim
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        FileInfo[] fileInfos;
        public StartWindow()
        {
            InitializeComponent();
            DirectoryInfo directoryInfo = new DirectoryInfo("./Saves/");
            fileInfos = directoryInfo.GetFiles();
            string[] fileName = new string[fileInfos.Length];
            for (int i = 0; i < fileInfos.Length; i++)
                fileName[i] = fileInfos[i].Name;
            ComboBox.ItemsSource = fileName;
        }

        private void ButtonStartNew_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.worldName = WorldNameFileTextBox.Text;
            mainWindow.solObjects = new List<SolObject>();
            mainWindow.Show();
        }

        private void ButtonStartOld_Click(object sender, RoutedEventArgs e)
        {
            List<SolObject> solObjects = new List<SolObject>();
            using (StreamReader streamReader = new StreamReader(fileInfos[ComboBox.SelectedIndex].FullName))
                while (!streamReader.EndOfStream)
                {
                    solObjects.Add(new SolObject()
                    {
                        Name = streamReader.ReadLine(),
                        vX = Double.Parse(streamReader.ReadLine()),
                        vY = Double.Parse(streamReader.ReadLine()),
                        m = Double.Parse(streamReader.ReadLine()),
                        x = Double.Parse(streamReader.ReadLine()),
                        y = Double.Parse(streamReader.ReadLine()),
                        UI = new Image()
                        {
                            Margin = new Thickness(0, 0,0,0),
                            Source = new BitmapImage(new Uri(streamReader.ReadLine())),
                            Width = Double.Parse(streamReader.ReadLine()),
                            Height = Width,
                            Visibility = Visibility.Visible,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        }
                    });
                    
                }
            MainWindow mainWindow = new MainWindow();
            mainWindow.worldName = WorldNameFileTextBox.Text;
            mainWindow.SetNewObjects(solObjects);
            mainWindow.Show();

        }
    }
}
/*
treamWriter.WriteLine(solObjects[i].Name);
streamWriter.WriteLine(solObjects[i].vX);
streamWriter.WriteLine(solObjects[i].vY);
streamWriter.WriteLine(solObjects[i].m);
streamWriter.WriteLine(solObjects[i].x);
streamWriter.WriteLine(solObjects[i].y);
streamWriter.WriteLine(((Image)solObjects[i].UI).Source);
*/