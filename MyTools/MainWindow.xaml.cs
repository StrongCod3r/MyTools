using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls.Primitives;
using System.Xml;
using System.IO;
using WpfAppBar;
using MyTools.Widgets;

namespace MyTools
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<WidgetBase> listWidgets;
        private double sizeTotal = 0;
        private bool wide = true;

        public MainWindow()
        {
            listWidgets = new List<WidgetBase>();

            InitializeComponent();

            this.cbEdge.ItemsSource = new[]
            {
                            //AppBarDockMode.Left,
                            //AppBarDockMode.Right,
                            AppBarDockMode.Top,
                            AppBarDockMode.Bottom
            };
            //this.cbMonitor.ItemsSource = MonitorInfo.GetAllMonitors();
            DockMode = AppBarDockMode.Bottom;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var c1 = new MenuTool(" FILE ");
            Config.LoadConfig("Data/tree.xml", c1.ContextMenu);
            this.AddWidget(c1.BaseInitialize(this));
        }


        public void AddWidget(WidgetBase widget)
        {
            sizeTotal += widget.Width;
            sizeTotal += widget.Margin.Left;
            sizeTotal += widget.Margin.Right;

            MainLayout.Children.Add(widget);
            listWidgets.Add(widget);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void WindowRefreshSize()
        {
            Top = 0;
            Height = 7;
            double MonitorWidth = SystemParameters.WorkArea.Width;
            double MonitorHeight = SystemParameters.WorkArea.Height;

            if (!wide)
            {
                Width = sizeTotal;
                Left = (MonitorWidth / 2) - Width / 2;
            }
            else
            {
                Width = MonitorWidth;
                Left = 0;
            }
        }


        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void rzThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            double delta;
            switch (DockMode)
            {
                case AppBarDockMode.Left:
                    delta = e.HorizontalChange;
                    break;
                case AppBarDockMode.Right:
                    delta = e.HorizontalChange * -1;
                    break;
                case AppBarDockMode.Top:
                    delta = e.VerticalChange;
                    break;
                case AppBarDockMode.Bottom:
                    delta = e.VerticalChange * -1;
                    break;
                default: throw new NotSupportedException();
            }

            this.DockedWidthOrHeight += (int)(delta / VisualTreeHelper.GetDpi(this).PixelsPerDip);
        }

        private void AppBarWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}