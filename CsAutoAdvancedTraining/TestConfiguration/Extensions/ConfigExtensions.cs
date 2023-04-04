using System.Collections.Generic;
using System.Linq;
using TestConfiguration.Models;

namespace TestConfiguration.Extensions
{
    public static class ConfigExtensions
    {
        public static string AsString(this Config config)
        {
            string cfgString = string.Empty;

            config.BrowserConfigs.ForEach(brw =>
            {
                cfgString += $"Browser: {brw.BrowserName}, Version: {brw.BrowserVersion}";
                cfgString += "\nUsers:";

                brw.Users.ForEach(user =>
                {
                    cfgString += $"\n\tUser Role: {user.Role}";
                    cfgString += $"\n\tLogin: {user.Login}";
                    cfgString += $"\n\tPassword: {user.Password}";
                    cfgString += "\n\tTests:";

                    user.Tests.ForEach(test =>
                    {
                        cfgString += $"\n\t\tTest {test.Id}: {test.Title}";

                        test.Steps.ForEach(step =>
                        {
                            cfgString += $"\n\t\t\t{step.StepNumber}. {step.StepText}";
                        });

                        cfgString += $"\n\t\tExpected Result: {test.ExpectedResult}\n";
                    });
                });
                cfgString += "\n";
            });

            return cfgString;
        }

        public static List<string> GetIncorrectBrowsers(this Config config)
        {
            return (from browser in config.BrowserConfigs
                    where browser.Users.Any(user => string.IsNullOrEmpty(user.Login)
                    || string.IsNullOrEmpty(user.Password)
                    || user.Tests.Any(test => string.IsNullOrEmpty(test.Title)))
                    select $"Browser: {browser.BrowserName}, Version: {browser.BrowserVersion}").ToList();
        }
    }
}
