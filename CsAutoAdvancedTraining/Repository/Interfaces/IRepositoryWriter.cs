using Repository.Models;

namespace Repository.Interfaces
{
    public interface IRepositoryWriter
    {
        void WriteConfig(Config config);
    }
}
