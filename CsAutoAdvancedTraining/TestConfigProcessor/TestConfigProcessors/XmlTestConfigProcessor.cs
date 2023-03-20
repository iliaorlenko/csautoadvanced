using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestConfigProcessor.Configuration;
using TestConfigProcessor.Enums;
using TestConfigProcessor.Interfaces;

namespace TestConfigProcessor.TestConfigProcessors
{
    internal class XmlTestConfigProcessor : TestConfigProcessor, ITestConfigProcessor
    {

        public XmlTestConfigProcessor()
        {
            ConfigFiles = GetTestConfigFiles(TestConfigType.Xml);
            TestConfigs = DeserializeConfigs();
        }

        /// <summary>
        /// Deserializes XML config file
        /// </summary>
        /// <returns></returns>
        public override IList<Config> DeserializeConfigs()
        {
            var xmlSerializer = new XmlSerializer(typeof(Config));
            var deserializedConfigs = new List<Config>();

            foreach (var configFile in ConfigFiles)
            {
                using (var fileStream = File.Open(configFile.FullName, FileMode.Open))
                {
                    var config = (Config)xmlSerializer.Deserialize(fileStream);
                    config.Type = TestConfigType.Xml;
                    config.Name = Path.GetFileNameWithoutExtension(configFile.FullName);
                    deserializedConfigs.Add(config);
                }
            }

            return deserializedConfigs;
        }

        /// <summary>
        /// Converts XML config to specified output format
        /// </summary>
        /// <param name="outputType">Format to convert XML to</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public override string ConvertConfig(Config config, TestConfigType outputType)
        {
            switch (outputType)
            {
                case TestConfigType.Json:
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Converters ={ new JsonStringEnumConverter() }
                    };

                    return JsonSerializer.Serialize(TestConfigs[0], options);
                case TestConfigType.Xml:
                    throw new NotSupportedException("Cannot convert xml to xml.");
                default: 
                    throw new NotSupportedException($"Output type '{outputType}' is not supported.");
            }
        }

        /// <summary>
        /// Prints configuration file content on the console
        /// </summary>
        /// <param name="filePath">Path to the XML config file</param>
        public override void PrintConfigFromFile(string filePath)
        {
            XDocument.Load(filePath).Root
                .Descendants("browser")
                .ToList()
                .ForEach(browser =>
                    {
                        Console.WriteLine($"Browser: {browser?.Attribute("name")?.Value}, Version: {browser?.Attribute("version")?.Value}");

                        browser?.Descendants("user")?.ToList()?.ForEach(user =>
                        {
                            Console.WriteLine($"\tUser Role: {user?.Attribute("role")?.Value}");
                            Console.WriteLine($"\tLogin: {user?.Element("login")?.Value}");
                            Console.WriteLine($"\tPassword: {user?.Element("password")?.Value}");
                            Console.WriteLine($"\tTests:");

                            user?.Descendants("tests")?.Descendants("test")?.ToList()?.ForEach(test =>
                            {
                                Console.WriteLine($"\t\tTest {test?.Attribute("id")?.Value}: {test?.Element("title")?.Value}");
                                Console.WriteLine("\t\tTest steps:");

                                test?.Descendants("steps")?.Descendants("step")?.ToList()?.ForEach(step =>
                                {
                                    Console.WriteLine($"\t\t\t{step?.Attribute("number")?.Value}. {step?.Value}");
                                });

                                Console.WriteLine($"\t\tExpected result: {test?.Element("expected")?.Value}");
                                Console.WriteLine();
                            });
                        });

                        Console.WriteLine("=================================================================================================");
                        Console.WriteLine();
                    });
        }

        /// <summary>
        /// Prints deserialized config object
        /// </summary>
        public override void PrintDeserializedConfigs()
        {
            base.PrintDeserializedConfigs();
        }

        /// <summary>
        /// Prints browsers containing incorrect configurations
        /// </summary>
        public override void PrintIncorrectBrowsers()
        {
            base.PrintIncorrectBrowsers();
        }

        /// <summary>
        /// Subdivides input XML config files by Browser and UserRole, converts them exports as files of specified format
        /// </summary>
        /// <param name="outputType"></param>
        public override void ExportConfigurations(TestConfigType outputType)
        {
            base.ExportConfigurations(outputType);
        }
    }
}
