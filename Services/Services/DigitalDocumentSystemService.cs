namespace Services
{
    using System.IO;
    using System.ServiceProcess;
    using System.Threading;

    using Google.Apis.Drive.v3;

    public class DigitalDocumentSystemService : ServiceBase
    {
        private readonly FileSystemWatcher fileWatcher;

        private readonly AutoResetEvent inputChangedEvent = new AutoResetEvent(false);

        private DirectoryInfo inputDirectoryInfo;

        private DirectoryInfo outputDirectoryInfo;

        private ManualResetEvent stopEvent = new ManualResetEvent(false);

        private Thread worker;

        public DigitalDocumentSystemService(DirectoryInfo inputDirectoryInfo, DirectoryInfo outputDirectoryInfo)
        {
            this.ServiceName = "DigitalDocumentSystemService";
            this.CanStop = true;

            this.inputDirectoryInfo = inputDirectoryInfo;
            this.outputDirectoryInfo = outputDirectoryInfo;

            this.fileWatcher = new FileSystemWatcher(inputDirectoryInfo.FullName) { EnableRaisingEvents = false };

            this.fileWatcher.Changed += (s, args) => this.inputChangedEvent.Set();
            this.fileWatcher.Created += (s, args) => this.inputChangedEvent.Set();
            this.fileWatcher.Renamed += (s, args) => this.inputChangedEvent.Set();
        }

        protected void DoWork()
        {
            var driveService = new DriveService();
        }

        protected override void OnStart(string[] args)
        {
            this.fileWatcher.EnableRaisingEvents = true;
        }

        protected override void OnStop()
        {
        }
    }
}