using System;
using System.Collections.Generic;

namespace WarmDrinkStation
{
    public interface IWarmDrink
    {
        void Consume();
    }
    internal class Tea : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Tea is served.");
            Console.ReadLine();
        }
    }

    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot Coffee is served.");
            Console.ReadLine();
        }
    }

    internal class Chocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot Chocolate is served.");
            Console.ReadLine();
        }
    }

    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
            Console.ReadLine();
        }
    }
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }
    internal class TeaFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup\n");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            return new Tea();
        }
    }

    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pours {total} ml Hot Coffee in your cup\n");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            return new Coffee();
        }
    }

    internal class HotChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pours {total} ml Hot Chocolate in your cup\n");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            return new Chocolate();
        }
    }

    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pours {total} ml Cappuccino in your cup\n");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            return new Chocolate();
        }
    }
    public class WarmDrinkMachine
    {
        public enum AvailableDrink // violates open-closed
        {
            Coffee, Tea, Chocolate
        }
        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
          new Dictionary<AvailableDrink, IWarmDrinkFactory>();

        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
          new List<Tuple<string, IWarmDrinkFactory>>();

        public WarmDrinkMachine()
        {
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));

                }


            }
        }
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");

            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");

            }
            Console.WriteLine("\nSelect a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();


        }
    }
}