using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestConfigProcessor.TestConfigProcessors;

namespace TestConfigProcessor
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var xmlConfigProcessor = new XmlTestConfigProcessor();

            xmlConfigProcessor.PrintDeserializedConfigs();

            Console.WriteLine();
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine();

            xmlConfigProcessor.PrintIncorrectBrowsers();

            Console.WriteLine();
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine();

            xmlConfigProcessor.PrintConfigFromFile(xmlConfigProcessor.ConfigFiles[0].FullName);

            Console.WriteLine();
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("=================================================================================================");
            Console.WriteLine();

            xmlConfigProcessor.ConvertConfig(xmlConfigProcessor.TestConfigs[0], Enums.TestConfigType.Json);

            xmlConfigProcessor.ExportConfigurations(Enums.TestConfigType.Json);

            Console.ReadKey();
        }
    }
}
