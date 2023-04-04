using System;
using TestConfiguration.Models;
using TestConfiguration.Interfaces;

namespace TestConfiguration.Repositories
{
    internal abstract class Repository : IRepository
    {
        public abstract Config GetConfig();

        public abstract void WriteConfig(Config config);
    }
}
