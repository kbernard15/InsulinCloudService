using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace InsulinWindowsService
{
    // Provide the ProjectInstaller class which allows 
    // the service to be installed by the Installutil.exe tool

    // run visual studio command prompt as admin.
    //
    // 1. installutil bin\Debug\InsulinWindowsService.exe
    // 2. net start InsulinWindowsService
    //
    // To uninstall:
    // 3. installutil /u bin\Debug\InsulinWindowsService.exe
    //

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "InsulinWindowsService";
            service.Description = "Roche Diabetes Care Insulin Microservice";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}