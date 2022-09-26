using System;
using System.Windows;
using Windows.System;
using Windows.UI.Input.Preview.Injection;
using Microsoft.ReactNative.Managed;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Diagnostics;

namespace finalProjWinMem
{
  [ReactModule]
  public class FancyMath
  {
    public FancyMath() {

    }

        [ReactConstant]
    public double E = Math.E;

    [ReactConstant("Pi")]
    public double PI = Math.PI;

        [ReactMethod("add")]
        public async void Add(double a, double b)
        {
            double result = a + b;
            // SendKeys.Send("^V");
            IList<AppDiagnosticInfo> infos = await AppDiagnosticInfo.RequestInfoForAppAsync();
            IList<AppResourceGroupInfo> resourceInfos = infos[0].GetResourceGroups();
            await resourceInfos[0].StartSuspendAsync();
            //await Task.Delay(3000);
            //Thread.Sleep(1000);
            InputInjector inputInjector = InputInjector.TryCreate();
             var shift = new InjectedInputKeyboardInfo();
             shift.VirtualKey = (ushort)(VirtualKey.Control);
             shift.KeyOptions = InjectedInputKeyOptions.None;
             var shift1 = new InjectedInputKeyboardInfo();
             shift1.VirtualKey = (ushort)(VirtualKey.Shift);
             shift1.KeyOptions = InjectedInputKeyOptions.None;
            var pasteV = new InjectedInputKeyboardInfo();
            pasteV.VirtualKey = (ushort)(VirtualKey.V);
            pasteV.KeyOptions = InjectedInputKeyOptions.None;
            inputInjector.InjectKeyboardInput(new[] { shift, pasteV });
            //IEnumerable<AppListEntry> appListEntries = await Package.Current.GetAppListEntriesAsync();
            //await appListEntries.First().LaunchAsync();
            //Windows.System.

            //Forms.SendKeys.SendWait("k");
            // Environment.Exit(0);
        }

    //     [ReactMethod("runServer")]
    //     public void runServer()
    //     {
    //         ExecuteHttpServer();
    //         //Thread appServiceThread = new Thread(new ThreadStart(ExecuteHttpServer));
    //         //appServiceThread.Start();
    //     }
    //     public void ExecuteHttpServer()
    //     {
    //       if (!HttpListener.IsSupported)
    // {
    //     return;
    // }
    //       HttpListener _httpListener = new HttpListener();
    //         _httpListener.Prefixes.Add("http://localhost:9090/"); // add prefix "http://localhost:5000/"
    //         _httpListener.Start(); // start server (Run application as Administrator!)
    //         // while (true)
    //         // {
    //             HttpListenerContext context = _httpListener.GetContext();
    //             HttpListenerRequest request = context.Request;
    //             HttpListenerResponse response = context.Response;
    //             // Construct a response.
    //             string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
    //             byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
    //             // Get a response stream and write the response to it.
    //             response.ContentLength64 = buffer.Length;
    //             System.IO.Stream output = response.OutputStream;
    //             output.Write(buffer, 0, buffer.Length);
    //             // You must close the output stream.
    //             output.Close();
    //         // }
    //         //Thread _responseThread = new Thread(ResponseThread);
    //         //_responseThread.Start(); // start the response thread
    //     }
    }
}