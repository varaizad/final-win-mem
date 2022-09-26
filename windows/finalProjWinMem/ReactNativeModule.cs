using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace finalProjWinMem
{
[ReactModule]
public class ReactNativeAppServiceModule
{
        static AppServiceConnection connection = null;

        private ReactContext _reactContext;
        static AutoResetEvent appServiceExit;

        [ReactInitializer]
    public void Initialize(ReactContext reactContext)
    {
        _reactContext = reactContext;
            // appServiceExit = new AutoResetEvent(false);
        }

    [ReactMethod("launchFullTrustProcess")]
    public async Task LaunchFullTrustProcessAsync()
    {
        await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
    }
    [ReactMethod("getRegistryKey")]
    public async Task<string> GetRegistryKey(string key)
    {
        var ns = ReactPropertyBagHelper.GetNamespace("RegistryChannel");
        var name = ReactPropertyBagHelper.GetName(ns, "AppServiceConnection");

        var content = _reactContext.Handle.Properties.Get(name);

        var _connection = content as AppServiceConnection;

        ValueSet valueSet = new ValueSet
        {
            { "RegistryKeyName", key }
        };

        var result = await _connection.SendMessageAsync(valueSet);

        string message = result.Message["RegistryKeyValue"].ToString();
        return message;
    }
        static async void ThreadProc()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "RegistryService";
            connection.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            //we open the connection
            AppServiceConnectionStatus status = await connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                //if the connection fails, we terminate the Win32 process
                appServiceExit.Set();
            }
        }
        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            //when the connection with the App Service is closed, we terminate the Win32 process
            appServiceExit.Set();
        }
        private static async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            ValueSet valueSet = new ValueSet
    {
        { "RegistryKeyValue", "input" }
    };
            await args.Request.SendResponseAsync(valueSet);
        }
       }
}