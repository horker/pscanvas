using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    [Cmdlet("New", "CanvasTabbedWindow")]
    public class NewCanvasTabbedWindow : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public Hashtable WindowProperties { get; set; }

        [Parameter(Position = 1, Mandatory = false)]
        public Hashtable TabProperties { get; set; }

        protected override void EndProcessing()
        {
            var windowProps = Helpers.ToDictionary(WindowProperties);
            var tabProps = Helpers.ToDictionary(TabProperties);

            var w = new TabbedWindow(windowProps, tabProps);

            PaneContainerManager.Instance.Add(w);

            WriteObject(w);
        }
    }

    [Cmdlet("Close", "CanvasWindow")]
    public class CloseCanvasWindow : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public IPaneContainer Window { get; set; }

        protected override void EndProcessing()
        {
            Window.Close();
        }
    }

    [Cmdlet("Invoke", "CanvasWindowAction")]
    public class InvokeCanvasWindowAction : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Window Window { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public ScriptBlock Action { get; set; }

        protected override void EndProcessing()
        {
            System.Collections.ObjectModel.Collection<PSObject> results = null;
            Window.Dispatcher.Invoke(() => {
                results = InvokeCommand.InvokeScript(false, Action, new object[] { Window });
            });

            foreach (var r in results)
                WriteObject(r);
        }
    }

    [Cmdlet("Test", "CanvasWindowClosed")]
    public class TestCanvasWindowClosed : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public IPaneContainer Window { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(Window.IsClosed());
        }
    }

    [Cmdlet("Get", "CanvasWindowList")]
    public class GetCanvasWindowList : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var list = PaneContainerManager.Instance.GetLiveContainers();

            foreach (var w in list)
                WriteObject(w);
        }
    }
}