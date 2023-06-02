using System.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Repository.Interfaces;
using Json.JsonConfigEntities;
using Repository.Models;

namespace Json
{
    public class JsonRepository : IRepository
    {
        public Config GetConfig()
        {
            var filePath = Directory.GetFiles(ConfigurationManager.AppSettings["inputPath"], "*.json")[0];
            var configFile = new FileInfo(filePath);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            using (var fileStream = File.Open(configFile.FullName, FileMode.Open))
            {
                var jsonConfig = JsonSerializer.Deserialize<JsonConfig>(fileStream, options);

                return new Config
                {
                    BrowserConfigs = jsonConfig.BrowserConfigs.Select(cfg => new BrowserConfig
                    {
                        BrowserName = cfg.BrowserName,
                        BrowserVersion = cfg.BrowserVersion,
                        Users = cfg.Users.Select(user => new User
                        {
                            Role = user.Role,
                            Login = user.Login,
                            Password = user.Password,
                            Tests = user.Tests.Select(test => new Test
                            {
                                Title = test.Title,
                                ExpectedResult = test.ExpectedResult,
                                Steps = test.Steps.Select(step => new TestStep
                                {
                                    StepNumber = step.StepNumber,
                                    StepText = step.StepText
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };
            }
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
    }
}