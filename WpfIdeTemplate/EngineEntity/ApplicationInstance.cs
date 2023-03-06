using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SampleCompany.SampleProduct.EngineEntity
{
    public class ApplicationInstance
    {
        private readonly ILogger<ApplicationInstance> _logger;

        public ApplicationInstance(ILogger<ApplicationInstance> logger)
        {
            _logger = logger;
        }
    }
}
