using TestConfiguration.Models;

namespace TestConfiguration.Interfaces
{
    internal interface IRepositoryWriter
    {
        void WriteConfig(Config config);
    }
}
