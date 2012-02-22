using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;

namespace ShoopDoup
{
    class ImageManager
    {
        public ImageManager()
        {
        }


    }

    class ImageTarget
    {
        private String _filename;
        private Grid grid;
        private UIElement image;

        public ImageTarget(String filename)
        {
            image = new System.Windows.Shapes.Rectangle();
            grid = new Grid();

            System.Windows.Shapes.Rectangle rect = ((System.Windows.Shapes.Rectangle)image);
            rect.StrokeThickness = 5;
            rect.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));

            grid.Children.Add(rect);
        }
        
        public void setPosition(int x, int y) {
            Canvas.SetLeft(grid, x);
            Canvas.SetTop(grid, y);
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

        }

        public Grid getGrid() 
        {
            return grid;
        }

    }
}
