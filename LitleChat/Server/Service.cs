using System;

namespace Server
{
    public class Service : IContract
    {
        public string Send(string input, bool flag)
        {
            if (flag == false)
            {
                Console.WriteLine("User name is : {0}", input);
                return "Hello, " + input + "!!!";
            }
            else
            {
                Console.WriteLine("User send: {1}", input);
                return " send: " + input;
            }
        }
    }
}