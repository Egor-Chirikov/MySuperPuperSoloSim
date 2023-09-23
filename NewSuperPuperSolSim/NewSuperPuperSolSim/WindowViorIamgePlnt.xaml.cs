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

namespace NewSuperPuperSolSim
{
    /// <summary>
    /// Логика взаимодействия для WindowViorIamgePlnt.xaml
    /// </summary>
    public partial class WindowViorIamgePlnt : Window
    {
        MainWindow owner;
        public WindowViorIamgePlnt(MainWindow e)
        {
            InitializeComponent();
            DirectoryInfo directoryInfo = new DirectoryInfo("./Images/");
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            Image[] images = new Image[fileInfos.Length];
            for (int i = 0; i < images.Length; i++)
                images[i] = new Image()
                {
                    Name = "i" + i,
                    Width = 200,
                    Height = 200,
                    Source = new BitmapImage(new Uri(fileInfos[i].FullName)),
                };

            ListBox.ItemsSource = images;
            
            owner = e;
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            owner.newPlanetImage = (Image)ListBox.SelectedItem;
            owner.UbdatePlanetImage();
        }
    }
}
