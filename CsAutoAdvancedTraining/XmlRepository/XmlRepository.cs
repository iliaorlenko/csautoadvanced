using System.IO;
using System.Xml.Serialization;
using Repository.Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Repository.DAL.XmlConfigEntities;

namespace Xml
{
    public class XmlRepository : IRepository
    {
        public Config GetConfig()
        {
            var filePath = Directory.GetFiles(ConfigurationManager.AppSettings["inputPath"], "*.xml")[0];
            var configFile = new FileInfo(filePath);
            var xmlSerializer = new XmlSerializer(typeof(XmlConfig));

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
