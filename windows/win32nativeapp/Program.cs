using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace win32nativeapp
{
    class Program
    {
        static HttpListener _httpListener = new HttpListener();
        static AutoResetEvent appServiceExit;
        static AppServiceConnection connection = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            appServiceExit = new AutoResetEvent(false);
            Thread appServiceThread = new Thread(new ThreadStart(ThreadProc));
            appServiceThread.Start();
            ExecuteHttpServer();
            appServiceExit.WaitOne();

        }

        public static void ExecuteHttpServer()
        {
            Console.WriteLine("Starting server...");
            _httpListener.Prefixes.Add("http://localhost:5090/"); // add prefix "http://localhost:5000/"
            _httpListener.Start(); // start server (Run application as Administrator!)
            Console.WriteLine("Server started.");
            /*while (true)
            {
                HttpListenerContext context = _httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                Console.WriteLine(request);
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }*/
            //Thread _responseThread = new Thread(ResponseThread);
            //_responseThread.Start(); // start the response thread
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

            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                Console.WriteLine(request);
                string text = "dd";
                using (var reader = new StreamReader(request.InputStream,
                                     request.ContentEncoding))
                {
                    text = text + reader.ReadToEnd();
                }
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
                // return to uwp
                ValueSet valueSet = new ValueSet
    {
        { "RegistryKeyValue", text }
    };
                await args.Request.SendResponseAsync(valueSet);
                return;
            }
            /*Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            string input = "d";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
            }*/
            /*ValueSet valueSet = new ValueSet
    {
        { "RegistryKeyValue", input }
    };*/

            /*await args.Request.SendResponseAsync(valueSet);*/
        }
        private static string OpenStandardStreamIn()
        {

            //// We need to read first 4 bytes for length information
            Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            string input = "";
            for (int i = 0; i < length; i++)
            {
                input += (char)stdin.ReadByte();
            }

            return input;
        }
    }
}
