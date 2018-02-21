using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfAppBar;

namespace MyTools.Widgets
{
    class MenuTool : WidgetBase
    {
        private string title;
        private Grid layout;
        public  new ContextMenu ContextMenu { get; set; }


        public MenuTool(String title)
        {
            this.Name = "MenuTool";
            this.title = title;

            ContextMenu = new ContextMenu();
            layout = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            this.Child = layout;
        }

        protected override void Initialize()
        {
            var button = new Button()
            {
                Content = this.title,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Width = 45,
                Padding = new Thickness() { Left = 2, Top = 0, Right = 2, Bottom = 0 },
                Margin = new Thickness() { Left = 1, Top = 0, Right = 0, Bottom = 0 },
            };

            button.Click += Click_Event;

            layout.Children.Add(button);
        }

        private void Click_Event(object sender, RoutedEventArgs e)
        {
            ContextMenu.PlacementTarget = sender as Button;
            ContextMenu.Placement = PlacementMode.Top;
            ContextMenu.HorizontalOffset = 0;
            ContextMenu.VerticalOffset = MainToolbar.DockMode == AppBarDockMode.Bottom ? 5 : 0;
            ContextMenu.IsOpen = true;
        }
    }
}
