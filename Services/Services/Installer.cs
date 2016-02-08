namespace Services
{
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.ServiceProcess;

    [RunInstaller(true)]
    public class DdssInstaller : Installer
    {
        public DdssInstaller()
        {
            this.Installers.Add(
                new ServiceInstaller
                {
                    StartType = ServiceStartMode.Manual, 
                    ServiceName = DigitalDocumentSystemService.SerivceName
                });

            this.Installers.Add(new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem });
        }
    }
}