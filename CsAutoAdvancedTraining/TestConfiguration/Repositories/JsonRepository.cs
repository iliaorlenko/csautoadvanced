using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestConfiguration.Models;
using TestConfiguration.Interfaces;
using TestConfiguration.DAL;
using System;
using TestConfiguration.DAL.JsonConfigEntities;
using System.Collections.Generic;
using System.Linq;

namespace TestConfiguration.Repositories
{
    internal class JsonRepository : IRepository
    {
        public Config GetConfig()
        {
            var filePath = Directory.GetFiles(ConfigurationManager.AppSettings["inputPath"], "*.json")[0];
            var configFile = new FileInfo(filePath);

            using (var fileStream = File.Open(configFile.FullName, FileMode.Open))
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter());
                var jsonConfig = JsonSerializer.Deserialize<JsonConfig>(fileStream, options);
                jsonConfig.ConfigName = configFile.Name;
                return Convert(jsonConfig);
            }
            throw new NotImplementedException();
        }

        public void WriteConfig(Config config)
        {
            var path = ConfigurationManager.AppSettings["outputPath"] + $"\\JSON\\";
            Directory.CreateDirectory(path);

            foreach (var browser in config.BrowserConfigs)
            {
                browser.Users.ForEach(user => user.Password = string.IsNullOrEmpty(user.Password) ? "password" : user.Password);

                using (var sw = new StreamWriter(path + $"{browser.BrowserName}.json", false))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() }
                    };

                    sw.Write(JsonSerializer.Serialize(browser, options));
                }    
            }
        }

        private Config Convert(JsonConfig jsonConfig)
        {
            var config = new Config
            {
                ConfigName = jsonConfig.ConfigName,
                BrowserConfigs = new List<BrowserConfig>()
            };

            config.BrowserConfigs.AddRange(jsonConfig.BrowserConfigs.Select(cfg => new BrowserConfig
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