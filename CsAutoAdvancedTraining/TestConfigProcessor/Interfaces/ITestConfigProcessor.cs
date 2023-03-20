using System.Collections.Generic;
using TestConfigProcessor.Configuration;
using TestConfigProcessor.Enums;

namespace TestConfigProcessor.Interfaces
{
    public interface ITestConfigProcessor
    {
        IList<Config> DeserializeConfigs();

        string ConvertConfig(Config config, TestConfigType outputType);

        void PrintConfigFromFile(string filePath);

        void PrintDeserializedConfigs();

        void PrintIncorrectBrowsers();

        void ExportConfigurations(TestConfigType outputType);
    }
}
