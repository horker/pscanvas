using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Windows;

#pragma warning disable CS1591

namespace Horker.Canvas
{
    public class HandlerInfo
    {
        public Type Type;
        public string FileExtension;
        public IHandler Handler;
    };

    [Cmdlet("Get", "CanvasHandler")]
    public class GetCanvasHandler : PSCmdlet
    {
        protected override void EndProcessing()
        {
            foreach (var entry in HandlerSelector.Instance.TypeHandlerMap)
            {
                var info = new HandlerInfo()
                {
                    Type = entry.Key,
                    Handler = entry.Value
                };

                WriteObject(info);
            }

            foreach (var entry in HandlerSelector.Instance.FileExtensionHandlerMap)
            {
                var info = new HandlerInfo()
                {
                    FileExtension = entry.Key,
                    Handler = entry.Value
                };

                WriteObject(info);
            }
        }
    }
}
