using System;
using System.Configuration;
using System.Reflection;

namespace TestConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var xmlRepositoryAssembly = Assembly.LoadFrom(ConfigurationManager.AppSettings["xmlRepositoryAssemblyPath"]);
            var xmlRepositoryType = xmlRepositoryAssembly.GetType(ConfigurationManager.AppSettings["xmlRepositoryClassName"]);
            var xmlConfigInstance = xmlRepositoryAssembly.CreateInstance(xmlRepositoryType.FullName);

            var jsonRepositoryAssembly = Assembly.LoadFrom(ConfigurationManager.AppSettings["jsonRepositoryAssemblyPath"]);
            var jsonRepositoryType = jsonRepositoryAssembly.GetType(ConfigurationManager.AppSettings["jsonRepositoryClassName"]);
            var jsonConfigInstance = jsonRepositoryAssembly.CreateInstance(jsonRepositoryType.FullName);

            // Get deserialized config from XML file
            var config = xmlRepositoryType.GetMethod(ConfigurationManager.AppSettings["getConfigMethodName"]).Invoke(xmlConfigInstance, null);

            // Write config to JSON files
            jsonRepositoryType.GetMethod(ConfigurationManager.AppSettings["writeConfigMethodName"]).Invoke(jsonConfigInstance, new[] { config });

            Console.ReadKey();
        }
    }
}
