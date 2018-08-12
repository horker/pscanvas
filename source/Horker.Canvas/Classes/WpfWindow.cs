using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Reflection;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Markup;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    public class WpfWindow
    {
        private static PropertyInfo IsDisposedMethod = typeof(Window).GetProperty("IsDisposed", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Window _rootWindow;
        private static PowerShell _powerShell;

        public static Window RootWindow { get => _rootWindow; }

        static public bool IsWindowClosed(Window w)
        {
            return (bool)IsDisposedMethod.GetValue(w);
        }

        static public void OpenInvisibleWindow(List<Window> result, AutoResetEvent e)
        {
            var window = new Window();

            window.AllowsTransparency = true;
            window.Background = Brushes.Transparent;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowInTaskbar = false;

            window.Loaded += (sender, args) => {
                Win32Api.MakeWindowInvisibleInTaskSwitcher((Window)sender);
            };

            result.Add(window);

            e.Set();

            window.ShowDialog();
        }

        public static void OpenRootWindow()
        {
            if (_rootWindow != null && !IsWindowClosed(_rootWindow))
                return;

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

        static public Window CreateWindow(string xamlString = null, IDictionary<string, object> props = null, Action<Window> action = null)
        {
            Window window = null;

            OpenRootWindow();

            _rootWindow.Dispatcher.Invoke(() =>
            {
                if (xamlString != null)
                    window = (Window)XamlReader.Parse(xamlString);
                else
                    window = new Window();

                Helpers.SetProperties(window, props);

                action.Invoke(window);

                window.Show();
            });

            return window;
        }
    }
}