using System;
using System.Threading;

namespace SouthwestCheckin
{
    class Program
    {
        static String ConfirmationNumber;
        static String FirstName;
        static String LastName;
        static Boolean checkedIn = false;
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("First Name?");
                FirstName = Console.ReadLine();
                Console.WriteLine("Last Name?");
                LastName = Console.ReadLine();
                Console.WriteLine("Confirmation Number?");
                ConfirmationNumber = Console.ReadLine();
            }
            else
            {
                FirstName = args[0];
                LastName = args[1];
                ConfirmationNumber = args[2];
            }
            Console.WriteLine($"Starting checkin for {FirstName} {LastName} {ConfirmationNumber}");
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(startProcess);
                t.Start();
                Thread.Sleep(100);
            }
            while (!checkedIn) { }
            Console.WriteLine("Press any key to exit!");
            Console.Read();
        }
        static void startProcess()
        {
            var c = new Confirmation { ConfirmationNumber = ConfirmationNumber, FirstName = FirstName, LastName = LastName };
            while (!c.CheckIn())
            {
                Console.Write(".");
            }
            Console.WriteLine("");
            Console.WriteLine(c.BoardingGroupPosition);
            checkedIn = true;
        }
    }
}
