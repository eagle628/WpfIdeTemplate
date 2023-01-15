using SampleCompany.SampleProduct.CommonLibrary.InMemoryLogger;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCompany.SampleProduct.InMemoryLogger
{
    public class InMemoryLogStore : IInMemoryLogStore
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Push(LogData logData)
        {
            lock (_syncObject)
            {
                if (_logData.Count == _capacity)
                {
                    _logData.Dequeue();
                }
                _logData.Enqueue(logData);
                //OnPropertyChanged(nameof(LogData));
            }
            OnPropertyChanged(nameof(LogData));
        }
        private readonly object _syncObject = new object();
        private readonly Queue<LogData> _logData;
        private readonly int _capacity;

        public IReadOnlyList<LogData> LogData
        {
            get => _logData.ToList();
        }

        public InMemoryLogStore(int capacity = 64)
        {
            _capacity = capacity;
            _logData = new Queue<LogData>(_capacity);

        }
    }
}
