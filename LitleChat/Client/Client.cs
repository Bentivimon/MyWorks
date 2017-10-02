using System;
using System.ServiceModel;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            bool flag = false;
            Console.Title = "Client";

            Uri adress = new Uri("http://localhost:4000/IContract");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(adress);
            ChannelFactory<IContract> factory = new ChannelFactory<IContract>(binding, endpoint);
            IContract channel = factory.CreateChannel();
            Console.Write("Input your name: ");
            var message = Console.ReadLine();
            while(message != "Exit")
            {
                var response = channel.Send(message,flag);
                flag = true;
                Console.WriteLine(response + Environment.NewLine);
                Console.WriteLine("Input message:");
                message = Console.ReadLine();
            }
        }
    }
}
