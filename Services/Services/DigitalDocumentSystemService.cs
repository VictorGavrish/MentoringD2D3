namespace Services
{
    using System;
    using System.IO;
    using System.ServiceProcess;
    using System.Threading;
    
    using Google.Apis.Drive.v3;

    public class DigitalDocumentSystemService : ServiceBase
    {
        public const string SerivceName = "DigitalDocumentSystemService";

        private readonly FileSystemWatcher fileWatcher;

        private readonly AutoResetEvent inputChangedEvent = new AutoResetEvent(false);

        private readonly DriveService driveService;

        private readonly DirectoryInfo inputDirectoryInfo;

        private readonly DirectoryInfo outputDirectoryInfo;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly CancellationToken cancellationToken = new CancellationToken();

        private readonly Thread worker;

        public DigitalDocumentSystemService(DirectoryInfo inputDirectoryInfo, DirectoryInfo outputDirectoryInfo, DriveService driveService)
        {
            this.ServiceName = SerivceName;
            this.CanStop = true;

            this.inputDirectoryInfo = inputDirectoryInfo;
            this.outputDirectoryInfo = outputDirectoryInfo;
            this.driveService = driveService;

            this.fileWatcher = new FileSystemWatcher(inputDirectoryInfo.FullName) { EnableRaisingEvents = false };
            this.fileWatcher.Changed += (s, args) => this.inputChangedEvent.Set();
            this.fileWatcher.Created += (s, args) => this.inputChangedEvent.Set();
            this.fileWatcher.Renamed += (s, args) => this.inputChangedEvent.Set();

            this.worker = new Thread(this.WorkerThread);
        }

        protected void WorkerThread()
        {
            this.cancellationToken.ThrowIfCancellationRequested();
            try
            {
                while (true)
                {
                    foreach (var fileInfo in this.inputDirectoryInfo.EnumerateFiles())
                    {
                        fileInfo.MoveTo(this.outputDirectoryInfo.FullName);
                        using (var file = fileInfo.OpenRead())
                        {
                            var request = this.driveService.Files.Create(
                                new Google.Apis.Drive.v3.Data.File(),
                                file,
                                string.Empty);
                            request.Upload();
                        }

                        this.EventLog.WriteEntry($"File {fileInfo.Name} uploaded");
                    }
                    this.inputChangedEvent.WaitOne();
                }
            }
            catch (OperationCanceledException) { }
        }

        protected override void OnStart(string[] args)
        {
            this.fileWatcher.EnableRaisingEvents = true;

            this.worker.Start();
        }

        protected override void OnStop()
        {
        }
    }
}