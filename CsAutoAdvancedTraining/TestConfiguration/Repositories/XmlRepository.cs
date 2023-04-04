using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using TestConfiguration.Models;
using TestConfiguration.DAL;
using TestConfiguration.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TestConfiguration.Repositories
{
    internal class XmlRepository : IRepository
    {
        public Config GetConfig()
        {
            var xmlSerializer = new XmlSerializer(typeof(XmlConfig));
            var configFile = new FileInfo(Directory.GetFiles(Path.GetDirectoryName(ConfigurationManager.AppSettings["inputPath"]), "*.xml")[0]);

            using (var fileStream = File.Open(configFile.FullName, FileMode.Open))
            {
                XmlConfig xmlConfig = (XmlConfig)xmlSerializer.Deserialize(fileStream);
                xmlConfig.ConfigName = configFile.Name;
                return Convert(xmlConfig);
            }
        }

        public void WriteConfig(Config config)
        {
            var path = ConfigurationManager.AppSettings["outputPath"] + $"\\XML\\";
            Directory.CreateDirectory(path);

            foreach (var browser in config.BrowserConfigs)
            {
                browser.Users.ForEach(user => user.Password = string.IsNullOrEmpty(user.Password) ? "password" : user.Password);

                using (var sw = new StreamWriter(path + $"{browser.BrowserName}.xml", false))
                {
                    var serializer = new XmlSerializer (typeof(BrowserConfig));
                    serializer.Serialize(sw, browser);
                }
            }
        }

        private Config Convert(XmlConfig xmlConfig)
        {
            var config = new Config
            {
                ConfigName = xmlConfig.ConfigName,
                BrowserConfigs = new List<BrowserConfig>()
            };

            config.BrowserConfigs.AddRange(xmlConfig.BrowserConfigs.Select(cfg => new BrowserConfig
            {
                BrowserName = cfg.BrowserName,
                BrowserVersion = cfg.BrowserVersion,
                Users = cfg.Users.Select(user => new User
                {
                    Login = user.Login,
                    Password = user.Password,
                    Role = user.Role,
                    Tests = user.Tests.Select(test => new Test
                    {
                        Id = test.Id,
                        Title = test.Title,
                        ExpectedResult = test.ExpectedResult,
                        Steps = test.Steps.Select(step => new TestStep
                        {
                            StepNumber = step.StepNumber,
                            StepText = step.StepText
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList());

            return config;
        }
    }
}
