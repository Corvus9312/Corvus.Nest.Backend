using System;

namespace Corvus.Nest.Backend.Middlewares.Tools
{
    public class LogIDs : ILogIDs
    {
        public Guid ID { get; private set; }

        public LogIDs()
        {
            ID = Guid.NewGuid();
        }
    }

    public interface ILogIDs
    {
        public Guid ID { get; }
    }
}
