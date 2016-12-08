using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace InsulinServiceConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // if we use WebServiceHost, we can clean up our app.config
            WebServiceHost host = new WebServiceHost(typeof(InsulinServiceLibrary.InsulinHttpService));

            //ServiceHost host = new ServiceHost(typeof(InsulinServiceLibrary.InsulinHttpService));
        
            try
            {
                host.Open();
                PrintServiceInfo(host);
                Console.ReadLine();
                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                host.Abort();
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
