using Repository.Enums;
using Repository.Interfaces;
using Repository.Models;
using SqlRepository_CodeFirst.SqlConfigEntities;

namespace SqlRepository_CodeFirst
{
    public class SqlRepository : IRepository
    {
        public Config GetConfig()
        {
            using (var context = new SqlContext())
            {
                return new Config
                {
                    BrowserConfigs = context.BrowserConfigs.Select(cfg => new BrowserConfig
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
            using (var context = new SqlContext())
            {
                context.AddRange(config.BrowserConfigs.Select(cfg => new SqlBrowserConfig
                {
                    BrowserName = cfg.BrowserName,
                    BrowserVersion = cfg.BrowserVersion,
                    Users = cfg.Users.Select(user => new SqlUser
                    {
                        Role = user.Role,
                        Login = user.Login,
                        Password = user.Password,
                        Tests = user.Tests.Select(test => new SqlTest
                        {
                            Title = test.Title,
                            ExpectedResult = test.ExpectedResult,
                            Steps = test.Steps.Select(step => new SqlTestStep
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
