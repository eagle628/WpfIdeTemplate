using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;

namespace SampleCompany.SampleProduct.EngineEntity
{
    public sealed class ApplicationInstanceManager : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ApplicationInstanceManager> _logger;
        private readonly ConcurrentDictionary<Guid, ApplicationInstance> _instances;
        public ApplicationInstanceManager(IServiceProvider serviceProvider ,ILogger<ApplicationInstanceManager> logger)
        {
            _instances = new ConcurrentDictionary<Guid, ApplicationInstance>();
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public Task<ApplicationInstanceInfo> CreateApplicationInstance(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Creating application instance");
            var guid = Guid.NewGuid();
            _instances.TryAdd(guid, new ApplicationInstance(_serviceProvider.GetRequiredService<ILogger<ApplicationInstance>>()));
            _logger.LogInformation("Created application instance");
            return Task.FromResult(new ApplicationInstanceInfo() { Id = guid.ToString() });
        }
        public Task<Empty> DestroyAppliationInstance(ApplicationInstanceInfo request, ServerCallContext context)
        {
            _logger.LogInformation("Destroing application instance");
            _instances.TryRemove(Guid.Parse(request.Id), out _);
            _logger.LogInformation("Destoried application instance");
            return Task.FromResult(new Empty());
        }
        public Task<ApplicationInstanceIds> GetApplicationInstanceIds(Empty request, ServerCallContext context)
        {
            var keys = _instances.Keys.Select(x => x.ToString());
            var ids = new RepeatedField<string>();
            ids.AddRange(keys);
            var reply = new ApplicationInstanceIds();
            reply.Ids.AddRange(keys);
            return Task.FromResult(reply);
        }

        public void Dispose()
        {
            foreach (var disposable in _instances.Values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
            _instances.Clear();
        }
    }
}
