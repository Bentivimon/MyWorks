﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";

            Uri adress = new Uri("http://localhost:4000/IContract");
            BasicHttpBinding binding = new BasicHttpBinding();
            Type contract = typeof(IContract);
            ServiceHost host = new ServiceHost(typeof(Service));

            host.AddServiceEndpoint(contract, binding, adress);

            host.Open();

            Console.WriteLine("Server is alive!!!");
            Console.ReadLine();

            host.Close();
        }
    }
}
