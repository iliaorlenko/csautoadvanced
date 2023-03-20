using System;
using System.Collections.Generic;
using TestConfigProcessor.Configuration;
using TestConfigProcessor.Enums;
using TestConfigProcessor.Interfaces;

namespace TestConfigProcessor.TestConfigProcessors
{
    // Stub for future implementation
    public class JsonTestConfigProcessor : TestConfigProcessor, ITestConfigProcessor
    {
        public override string ConvertConfig(Config config, TestConfigType outputType)
        {
            throw new NotImplementedException();
        }

        public override IList<Config> DeserializeConfigs()
        {
            throw new NotImplementedException();
        }

        public override void PrintConfigFromFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
