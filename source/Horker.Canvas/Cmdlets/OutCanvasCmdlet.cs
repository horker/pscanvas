using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    [Cmdlet("Out", "Canvas")]
    public class OutCanvas : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public object InputObject;

        [Parameter(Position = 0, Mandatory = false)]
        public string HandlerName;

        private IHandler _handler;

        protected override void BeginProcessing()
        {
            if (!string.IsNullOrEmpty(HandlerName))
                _handler = HandlerSelector.Instance.SelectByName(HandlerName);

            if (_handler != null)
            {
                var panes = _handler.BeginProcessing();
                ShowPanes(panes);
            }
        }

        protected override void ProcessRecord()
        {
            if (InputObject == null)
                return;

            if (InputObject is PSObject)
                InputObject = (InputObject as PSObject).BaseObject;

            IEnumerable<IPane> panes;

            if (_handler == null)
            {
                _handler = HandlerSelector.Instance.SelectByType(InputObject.GetType());
                panes = _handler.BeginProcessing();
                ShowPanes(panes);
            }

            panes = _handler.ProcessRecord(InputObject);
            ShowPanes(panes);
        }

        protected override void EndProcessing()
        {
            if (_handler == null)
                return;

            var panes = _handler.EndProcessing();
            ShowPanes(panes);
        }

        private void ShowPanes(IEnumerable<IPane> panes)
        {
            if (panes == null)
                return;

            var canvas = CanvasManager.Instance.GetActiveCanvas();

            foreach (var pane in panes)
                canvas.AddPane(pane);

            canvas.SetFocusAt(-1);
        }
    }
}
