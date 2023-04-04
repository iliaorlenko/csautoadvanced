using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConfiguration.Models;
using TestConfiguration.Interfaces;
using TestConfiguration.Repositories;
using TestConfiguration.Extensions;

namespace TestConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Get XML
            IRepository repo = new XmlRepository();
            Config config = repo.GetConfig();

            //Print invalid browsers
            Console.WriteLine("Printed list of incorrect browsers:");
            Console.WriteLine(string.Join("\n", config.GetIncorrectBrowsers()));

            //Convert XML into different JSONs per Browser
            repo.WriteConfig(config);

            //Get JSON and convert into different XMLs per Browser
            repo = new JsonRepository();
            config = repo.GetConfig();
            repo.WriteConfig(config);

            //Print whole config
            Console.WriteLine("Printed test configuration:");
            Console.WriteLine(config.AsString());

            Console.ReadKey();
        }
    }
}
