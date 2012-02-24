using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using MySql.Data.MySqlClient;

namespace ShoopDoup
{
    class ImageManager
    {
        static ImageManager sharedImageManager;
        public static ImageManager sharedInstance()
        {
            if (sharedImageManager == null)
            {
                sharedImageManager = new ImageManager();
            }
            return sharedImageManager;
        }

        List<ImageTarget> targets;
        MySqlConnection connection;

        public ImageManager()
        {
            targets = new List<ImageTarget>();
            loadImagesToImageTargets();
        }

        public int numImages()
        {
            return targets.Count;
        }

        private void loadImagesToImageTargets()
        {
            connection = new MySqlConnection();

            connection.ConnectionString = "server=50.22.41.96;" + "database=tschmidt_cs247;" + "uid=tschmidt_tom;" + "password=cs247forlife;";
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select * from movies";

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                targets.Add(new ImageTarget((string)reader.GetValue(1), (string)reader.GetValue(2)));
            }

            connection.Close();
        }

        public ImageTarget getTargetAtIndex(int index) {
            return targets.ElementAt(index);
        }
    }

    class ImageTarget
    {
        private String filename;
        private String name;
        private Grid grid;
        private System.Windows.Controls.Image image;
        private bool highlighted, selected;
        private bool permanentlySelected;
        System.Windows.Shapes.Rectangle highlight, select;
        int x, y;

        public ImageTarget(String name, String filename)
        {
            image = new System.Windows.Controls.Image();
            grid = new Grid();
            this.name = name;
            LoadImage(filename);
            image.Height = 100;
            image.Width = 100;
            grid.Children.Add(image);

            highlight = new System.Windows.Shapes.Rectangle();
            highlight.Width = 100;
            highlight.Height = 100;
            highlight.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(70, 130, 80));
            highlight.Opacity = 0.4;
            highlight.Visibility = System.Windows.Visibility.Hidden;
            grid.Children.Add(highlight);

            
            select = new System.Windows.Shapes.Rectangle();
            select.Width = 120;
            select.Height = 120;
            select.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
            Grid.SetZIndex(select, -1);
            select.Visibility = System.Windows.Visibility.Hidden;
            grid.Children.Add(select);
            
             
        }

        private void LoadImage(string sourceUri)
        {
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri(sourceUri, UriKind.RelativeOrAbsolute);
            bitImage.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bitImage;
        }
        
        public void setPosition(int x, int y) {
            this.x = x;
            this.y = y;
            Canvas.SetLeft(grid, x - 50);
            Canvas.SetTop(grid, y - 50);
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public void setPermanentlySelected()
        {
            permanentlySelected = true;
            setSelected(true);
        }

        public bool isPermanentlySelected()
        {
            return permanentlySelected;
        }

        public void setVisibility(bool isVisible)
        {
            if (isVisible)
            {
                grid.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                grid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void setHighlighted(bool isHighlighted)
        {
            if (isHighlighted)
            {
                highlight.Visibility = System.Windows.Visibility.Visible;
                highlighted = true;
            }
            else
            {
                highlight.Visibility = System.Windows.Visibility.Hidden;
                highlighted = false;
            }
        }

        public bool isHighlighted()
        {
            return highlight.IsVisible;
        }

        public void setSelected(bool isSelected)
        {
            if (isSelected)
            {
                select.Visibility = System.Windows.Visibility.Visible;
                selected = true;
            }
            else
            {
                if (!permanentlySelected)
                {
                    select.Visibility = System.Windows.Visibility.Hidden;
                    selected = false;
                }
            }
        }

        public bool isSelected()
        {
            return select.IsVisible;
        }

        public Grid getGrid() 
        {
            return grid;
        }

    }
}
