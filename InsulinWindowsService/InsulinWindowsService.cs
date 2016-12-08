using System;
using System.ServiceModel.Web;
using System.ServiceProcess;

namespace InsulinWindowsService
{

    // https://msdn.microsoft.com/en-us/library/ms733069.aspx#Y382

    public class InsulinWindowsService : ServiceBase
    {
        //public ServiceHost serviceHost = null;
        public WebServiceHost serviceHost = null;


        public InsulinWindowsService()
        {
            // Name the Windows Service
            ServiceName = "InsulinWindowsService";
        }

        public static void Main()
        {
            ServiceBase.Run(new InsulinWindowsService());
        }

        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new WebServiceHost(typeof(InsulinServiceLibrary.InsulinHttpService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
            PrintServiceInfo(serviceHost);
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

        static void PrintServiceInfo(WebServiceHost host)
        {
            Console.WriteLine("{0} is up and running with these endpoints: ", host.Description.Name);
            foreach (var se in host.Description.Endpoints)
            {
                Console.WriteLine(se.Address);
            }
        }
    }
}
