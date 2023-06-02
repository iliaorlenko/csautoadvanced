using Repository.Enums;
using Repository.Interfaces;
using Repository.Models;
using SqlRepository_CodeFirst;
using SqlRepository_DbFirst.Models;

namespace SqlRepository_DbFirst
{
    public class SqlRepository : IRepository
    {
        public Config GetConfig()
        {
            using (var context = new TestConfigurationContext())
            {
                return new Config
                {
                    BrowserConfigs = context.BrowserConfigs.Select(cfg => new Repository.Models.BrowserConfig
                    {
                        BrowserName = Enum.Parse<Browser>(cfg.BrowserName),
                        BrowserVersion = cfg.BrowserVersion,
                        Users = cfg.Users.Select(user => new Repository.Models.User
                        {
                            Role = Enum.Parse<UserRole>(user.Role),
                            Login = user.Login,
                            Password = user.Password,
                            Tests = user.Tests.Select(test => new Repository.Models.Test
                            {
                                Title = test.Title,
                                ExpectedResult = test.ExpectedResult,
                                Steps = test.TestSteps.Select(step => new Repository.Models.TestStep
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
            using (var context = new SqlContext())
            {
                context.AddRange(config.BrowserConfigs.Select(cfg => new Models.BrowserConfig
                {
                    BrowserName = cfg.BrowserName.ToString(),
                    BrowserVersion = cfg.BrowserVersion,
                    Users = cfg.Users.Select(user => new Models.User
                    {
                        Role = user.Role.ToString(),
                        Login = user.Login,
                        Password = user.Password,
                        Tests = user.Tests.Select(test => new Models.Test
                        {
                            Title = test.Title,
                            ExpectedResult = test.ExpectedResult,
                            TestSteps = test.Steps.Select(step => new Models.TestStep
                            {
                                StepNumber = step.StepNumber,
                                StepText = step.StepText,
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }));

                context.SaveChanges();
            }
        }
    }
}
