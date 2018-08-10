using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Markup;
using System;
using System.Windows.Controls;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    public class WpfWindow
    {
        private static PropertyInfo IsDisposedMethod = typeof(System.Windows.Window).GetProperty("IsDisposed", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Window _rootWindow;
        private static PowerShell _powerShell;

        public static Window RootWindow { get => _rootWindow; }

        static public bool IsWindowClosed(System.Windows.Window w)
        {
            return (bool)IsDisposedMethod.GetValue(w);
        }

        static public void OpenInvisibleWindow(List<System.Windows.Window> result, AutoResetEvent e)
        {
            var window = new Window();

            window.AllowsTransparency = true;
            window.Background = Brushes.Transparent;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowInTaskbar = false;

            window.Loaded += (sender, args) => {
                Win32Api.MakeWindowInvisibleInTaskSwitcher((System.Windows.Window)sender);
            };

            result.Add(window);

            e.Set();

            window.ShowDialog();
        }

        public static void OpenRootWindow()
        {
            if (_rootWindow != null)
            {
                if (!IsWindowClosed(_rootWindow))
                {
                    return;
                }
            }

            var runspace = RunspaceFactory.CreateRunspace();
            runspace.ApartmentState = ApartmentState.STA;
            runspace.ThreadOptions = PSThreadOptions.UseNewThread;
            runspace.Open();

            _powerShell = PowerShell.Create();
            _powerShell.Runspace = runspace;

            var result = new List<Window>();
            var e = new AutoResetEvent(false);

            _powerShell.AddScript(@"
                param($result, $event)
                [Horker.Canvas.WpfWindow]::OpenInvisibleWindow($result, $event)");

            _powerShell.AddParameter("result", result);
            _powerShell.AddParameter("event", e);

            _powerShell.BeginInvoke();

            e.WaitOne();

            _rootWindow = result[0];
        }

        static public Window CreateWindow(string xamlString = null, Action<Window> action = null)
        {
            Window window = null;

            OpenRootWindow();

            _rootWindow.Dispatcher.Invoke(() =>
            {
                if (xamlString != null)
                    window = (Window)XamlReader.Parse(xamlString);
                else
                    window = new Window();

                action.Invoke(window);

                window.Show();
            });

            return window;
        }

        static public Window CreateGridWindow(int rows, int columns)
        {
            Window window = CreateWindow(null, w =>
            {
                var grid = new Grid();

                for (var i = 0; i < rows; ++i)
                {
                    var def = new RowDefinition();
                    def.Height = (GridLength)(new GridLengthConverter()).ConvertFrom(1.0);
                    grid.RowDefinitions.Add(def);
                }

                for (var i = 0; i < columns; ++i)
                {
                    var def = new ColumnDefinition();
                    def.Width = (GridLength)(new GridLengthConverter()).ConvertFrom(1.0);
                    grid.ColumnDefinitions.Add(def);
                }

                w.Content = grid;

            });

            return window;
        }
    }
}