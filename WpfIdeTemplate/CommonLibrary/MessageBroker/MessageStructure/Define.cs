using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCompany.SampleProduct.CommonLibrary.MessageBroker.MessageStructure
{
    public readonly struct SampleMessage
    {
        public readonly string Message { get; }

        public SampleMessage(string message)
        {
            Message = message;
        }
    }
}
