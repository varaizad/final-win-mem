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
  }
}