using TestConfiguration.Models;

namespace TestConfiguration.Interfaces
{
    internal interface IRepositoryReader
    {
        Config GetConfig();
    }
}
