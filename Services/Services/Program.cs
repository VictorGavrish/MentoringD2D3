using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var inputDir = new DirectoryInfo(@"E:\Input");
            var outputDir = new DirectoryInfo(@"E:\Output");

            UserCredential credential;
            string[] scopes = { DriveService.Scope.Drive };
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service =
                new DriveService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = DigitalDocumentSystemService.SerivceName
                    });

            var servicesToRun = new ServiceBase[]
            {
                new DigitalDocumentSystemService(inputDir, outputDir, service)
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
