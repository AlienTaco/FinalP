
using System;

namespace Server
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("I am server!");

            Console.WriteLine("Enter server ip");
            var serverip = Console.ReadLine().Trim();


            new Server(serverip, int.Parse("11111"));
            Console.WriteLine("Server is started");
            Console.ReadKey();
        }
    }
}

