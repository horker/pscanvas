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

        [Parameter(Position = 1, Mandatory = false)]
        public string Name;

        [Parameter(Position = 2, Mandatory = false)]
        public SwitchParameter New = false;

        [Parameter(Position = 3, Mandatory = false)]
        public CanvasStyle CanvasStyle = CanvasStyle.NotSpecified;

        [Parameter(Position = 5, Mandatory = false)]
        public SwitchParameter Topmost = false;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter Activate = false;

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter MoveToForeground = false;

        private IHandler _handler;

        protected override void BeginProcessing()
        {
            // Settings

            UserSettings.ResetCurrent();

            if (MyInvocation.BoundParameters.ContainsKey("New"))
                UserSettings.Current.OpenNewCanvas = New;
            if (MyInvocation.BoundParameters.ContainsKey("CanvasStyle"))
                UserSettings.Current.CanvasStyle = CanvasStyle;
            if (MyInvocation.BoundParameters.ContainsKey("Topmost"))
                UserSettings.Current.Topmost = Topmost;
            if (MyInvocation.BoundParameters.ContainsKey("Activate"))
                UserSettings.Current.Activate = Activate;
            if (MyInvocation.BoundParameters.ContainsKey("SetForeground"))
                UserSettings.Current.MoveToForeground = MoveToForeground;

            // Handler

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

            var canvas = UserSettings.Current.OpenCanvas();

            foreach (var pane in panes)
            {
                if (MyInvocation.BoundParameters.ContainsKey("Name"))
                    pane.Name = Name;
                canvas.AddPane(pane);
            }

            canvas.SetFocusAt(-1);

            if (UserSettings.Current.MoveToForeground)
                canvas.MoveToForeground(-1);

            if (UserSettings.Current.Activate)
                canvas.Activate(-1);
        }
    }
}
