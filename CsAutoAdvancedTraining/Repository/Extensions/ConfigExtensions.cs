using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Repository.Models;

namespace Repository.Extensions
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
        public static List<string> ValidatePasswords(this Config config)
        {
            var incorrectPasswords = new List<string>();
            var pattern = @"^(?=.*[A-Z])(?=.*\d.*\d)[A-Za-z\d+_]+$";

            config.BrowserConfigs.ForEach(brw => brw.Users.ForEach(usr =>
            {
                if (!string.IsNullOrEmpty(usr.Password))
                {
                    if (!Regex.IsMatch(usr.Password, pattern))
                    {
                        incorrectPasswords.Add(usr.Password);
                    }
                }
            }));

            return incorrectPasswords;

        }

        public static Config ValidateTestTitles(this Config config)
        {
            var pattern = @"^(\w+)(?i:test)(_|-)\d+\.\d+$";
            var tests = config.BrowserConfigs.SelectMany(brw => brw.Users.SelectMany(usr => usr.Tests));

            foreach (var test in tests)
            {
                if (Regex.IsMatch(test.Title, pattern))
                {
                    // Patterns for test ID numbers
                    var firstNumPattern = @"_(\d+)\.";
                    var secondNumPattern = @"\.\d+$";

                    test.Title = Regex.Replace(test.Title, "-", "_");

                    // Get actual test ID numbers
                    var firstNum = Regex.Match(test.Title, firstNumPattern).Groups[0].Value.TrimEnd('.').TrimStart('_');
                    var secondNum = Regex.Match(test.Title, secondNumPattern).Groups[0].Value.TrimStart('.');

                    if (firstNum.Length < 4)
                    {
                        test.Title = Regex.Replace(test.Title, firstNumPattern, $"_{firstNum.PadLeft(4, '0')}.");
                    }

                    if (firstNum.Length < 3)
                    {
                        test.Title = Regex.Replace(test.Title, secondNumPattern, $".{secondNum.PadLeft(3, '0')}");
                    }
                }
            }

            return config;
        }
    }
}