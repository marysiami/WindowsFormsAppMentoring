using System;

namespace BusinessLogic
{
    public static class Greeting
    {
        public static string Welcome(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var date = DateTime.Now;
                return $"{date.ToShortTimeString()} {date.ToShortDateString()}\n\nHello, {name}!";
            }

            return "There is no name.";
        }

        public static void Welcome()
        {
            Console.WriteLine("Your name is...");
            var name = Console.ReadLine();
            var date = DateTime.Now;
            Console.WriteLine($"{date.ToShortTimeString()} {date.ToShortDateString()} Hello, {name}!");
        }
    }
}
