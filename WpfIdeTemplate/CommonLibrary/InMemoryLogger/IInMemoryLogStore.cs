using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger
{
    public interface IInMemoryLogStore : INotifyPropertyChanged
    {
        public void Push(LogData logData);
        public IReadOnlyList<LogData> LogData { get; }
    }
}
