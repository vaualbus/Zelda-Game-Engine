using ZeldaEngine.Tests.Abstracts;
using ZeldaEngine.Tests.Scripts;

namespace ZeldaEngine.Tests
{
    public class DataProvider : IDataProvider
    {
        private readonly int _maxData;

        public DataProvider(int maxData)
        {
            _maxData = maxData;
        }
    }
}