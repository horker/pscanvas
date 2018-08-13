using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horker.Canvas
{
    public enum CanvasStyle
    {
        NotSpecified,
        Single,
        Multiple,
        Tabbed
    }

    public class UserSettings
    {
        public static UserSettings Default = null;
        public static UserSettings Current = null;

        public CanvasStyle CanvasStyle = CanvasStyle.Tabbed;
        public CanvasStyle NewCanvasStyle = CanvasStyle.NotSpecified;

        public double? CanvasWidth = null;
        public double? CanvasHeight = null;

        internal static void ResetCurrent()
        {
            Current = (UserSettings)Default.MemberwiseClone();
        }

        internal ICanvas OpenCanvas()
        {
            var active = CanvasManager.Instance.GetActiveCanvas();
            if (NewCanvasStyle == CanvasStyle.NotSpecified && active != null)
                return active;

            CanvasStyle style = CanvasStyle;
            if (NewCanvasStyle != CanvasStyle.NotSpecified)
                style = NewCanvasStyle;

            var windowProps = new Dictionary<string, object>();

            if (CanvasWidth.HasValue)
                windowProps["Width"] = CanvasWidth.Value;

            if (CanvasHeight.HasValue)
                windowProps["Height"] = CanvasHeight.Value;

            switch (style)
            {
                case CanvasStyle.Single:
                    active = new SingleCanvas(windowProps);
                    break;

                case CanvasStyle.Multiple:
                    active = new MultipleCanvas(windowProps);
                    break;

                case CanvasStyle.Tabbed:
                    active = new TabbedCanvas(windowProps);
                    break;

                default:
                    throw new ApplicationException("Invalid CanvasStyle");
            }

            CanvasManager.Instance.Add(active);

            return active;
        }
    }
}
