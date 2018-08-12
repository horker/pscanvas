using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var window = new Horker.Canvas.TabbedCanvas();

            var pane = new Horker.Canvas.GridPane("grid", new string[] { "1*", "2*", "3*" }, new string[] { "1*", "1*" });
            window.AddPane(pane);
        }
    }
}
