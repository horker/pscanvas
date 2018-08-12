using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    [Cmdlet("New", "TabbedCanvas")]
    public class NewTabbedCanvas : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public Hashtable WindowProperties { get; set; }

        [Parameter(Position = 1, Mandatory = false)]
        public Hashtable TabControlProperties { get; set; }

        protected override void EndProcessing()
        {
            var windowProps = Helpers.ToDictionary(WindowProperties);
            var tabProps = Helpers.ToDictionary(TabControlProperties);

            var w = new TabbedCanvas(windowProps, tabProps);

            CanvasManager.Instance.Add(w);

            WriteObject(w);
        }
    }

    [Cmdlet("Close", "Canvas")]
    public class CloseCanvas : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ICanvas Window { get; set; }

        protected override void EndProcessing()
        {
            Window.Close();
        }
    }

    [Cmdlet("Invoke", "CanvasAction")]
    public class InvokeCanvasAction : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ScriptBlock Action { get; set; }

        protected override void EndProcessing()
        {
            System.Collections.ObjectModel.Collection<PSObject> results = null;
            Helpers.InvokeInWindowLoop(() => {
                results = InvokeCommand.InvokeScript(false, Action, null);
            });

            foreach (var r in results)
                WriteObject(r);
        }
    }

    [Cmdlet("Test", "CanvasClosed")]
    public class TestCanvasClosed : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ICanvas Window { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(Window.IsClosed());
        }
    }

    [Cmdlet("Get", "CanvasList")]
    public class GetCanvasList : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var list = CanvasManager.Instance.GetLiveCanvases();

            foreach (var w in list)
                WriteObject(w);
        }
    }
}