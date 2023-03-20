using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using TestConfigProcessor.Configuration;
using TestConfigProcessor.Enums;
using TestConfigProcessor.Interfaces;

namespace TestConfigProcessor.TestConfigProcessors
{
    public abstract class TestConfigProcessor : ITestConfigProcessor
    {
        public IList<Config> TestConfigs;
        public IList<FileInfo> ConfigFiles;

        public abstract IList<Config> DeserializeConfigs();

        public abstract string ConvertConfig(Config config, TestConfigType outputType);

        public abstract void PrintConfigFromFile(string filePath);

        public virtual void ExportConfigurations(TestConfigType outputType)
        {
            var outputPath = ConfigurationManager.AppSettings["outputPath"] + $"\\{outputType}";

            // Iterate input config files
            TestConfigs.ToList().ForEach(testConfig =>
            {
                //Iterate current config file browsers
                testConfig.Browsers.ToList().ForEach(browser =>
                {
                    var browserRoleTestsSets = new Dictionary<UserRole, List<Test>>();

                    Directory.CreateDirectory(outputPath + $"\\{browser.Name}");

                    // Add Role/Tests combinations to the dictionary
                    browser.Users.ToList().ForEach(user =>
                    {
                        if (!browserRoleTestsSets.ContainsKey(user.Role))
                        {
                            browserRoleTestsSets.Add(user.Role, user.Tests);
                        }
                        else
                        {
                            // This handles duplicated role nodes in the single config file
                            browserRoleTestsSets[user.Role].AddRange(user.Tests);
                        }
                    });

                    // Convert config files to specified format
                    foreach (var testSet in browserRoleTestsSets)
                    {
                        using (var sw = new StreamWriter($"{outputPath + $"\\{browser.Name}"}\\{testConfig.Name}_{testSet.Key}.{outputType.ToString().ToLower()}", false))
                        {
                            switch (outputType)
                            {
                                case TestConfigType.Xml:
                                    using (var xmlWriter = XmlWriter.Create(sw))
                                    {
                                        var xs = new XmlSerializer(typeof(List<Test>));
                                        xs.Serialize(sw, testSet.Value);
                                    }
                                    break;
                                case TestConfigType.Json:
                                    JsonSerializerOptions options = new JsonSerializerOptions
                                    {
                                        Converters = { new JsonStringEnumConverter() }
                                    };
                                    sw.Write(JsonSerializer.Serialize(testSet.Value, options));
                                    break;
                                default:
                                    throw new NotSupportedException($"Output format '{outputType}' is not supported.");
                            }
                        }
                    }
                });
            });
        }

        /// <summary>
        /// Prints configuration from deserialized object
        /// </summary>
        public virtual void PrintDeserializedConfigs()
        {
            TestConfigs.ToList().ForEach(config => config.Browsers.ToList().ForEach(browser =>
            {
                Console.WriteLine($"Browser: {browser.Name}, Version: {browser.BrowserVersion}");

                browser.Users.ToList().ForEach(user =>
                {
                    Console.WriteLine($"\tUser Role: {user.Role}");
                    Console.WriteLine($"\tLogin: {user.Login}");
                    Console.WriteLine($"\tPassword: {user.Password}");
                    Console.WriteLine($"\tTests:");

                    user.Tests.ToList().ForEach(test =>
                    {
                        Console.WriteLine($"\t\tTest {test.Id}: {test.Title}");
                        Console.WriteLine("\t\tTest steps:");

                        test.Steps.ToList().ForEach(step =>
                        {
                            Console.WriteLine($"\t\t\t{step.StepNumber}. {step.StepText}");
                        });

                        Console.WriteLine($"\t\tExpected result: {test.ExpectedResult}");
                        Console.WriteLine();
                    });
                });

                Console.WriteLine("=================================================================================================");
                Console.WriteLine();
            }));
        }

        /// <summary>
        /// Prints browsers containing incorrect configurations
        /// </summary>
        public virtual void PrintIncorrectBrowsers()
        {
            Console.WriteLine("Browsers with invalid configurations: ");
            Console.WriteLine();

            foreach (var config in TestConfigs)
            {
                Console.WriteLine($"Config name: {config.Name}.{config.Type.ToString().ToLower()}");

                var incorrectBrowsers = from browser in config.Browsers
                                        where browser.Users.Any(user => string.IsNullOrEmpty(user.Login)
                                        || string.IsNullOrEmpty(user.Password)
                                        || user.Tests.Any(test => string.IsNullOrEmpty(test.Title)))
                                        select "\tBrowser: " + browser.Name + ", Version: " + browser.BrowserVersion;

                Console.WriteLine($"{string.Join("\n", incorrectBrowsers)}");
            }
        }

        /// <summary>
        /// Gets config files of specified format
        /// </summary>
        /// <param name="type">Config format</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public IList<FileInfo> GetTestConfigFiles(TestConfigType type)
        {
            var inputFilesPaths = Directory.GetFiles(ConfigurationManager.AppSettings["inputPath"]);
            var filesToReturn = new List<FileInfo>();

            if (inputFilesPaths.Count() > 0)
            {
                foreach (var inputFilePath in inputFilesPaths)
                {
                    var file = new FileInfo(inputFilePath);

                    if (file.Extension.Replace(".", "").ToLower() == type.ToString().ToLower())
                    {
                        filesToReturn.Add(file);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"There are now files in {inputFilesPaths}");
            }

            return filesToReturn;
        }
    }
}
