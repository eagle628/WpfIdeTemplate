using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SampleCompany.SampleProduct.ApplicationEngine.Proto;
using SampleCompany.SampleProduct.EngineEntity;

namespace SampleCompany.SampleProduct.ApplicationEngine.Services
{
    public class ApplicationInstanceManagementService : ApplicationInstanceManagement.ApplicationInstanceManagementBase
    {
        private readonly ILogger<ApplicationInstanceManagementService> _logger;
        private readonly ApplicationInstanceManager _instanceManager;

        public ApplicationInstanceManagementService(ILogger<ApplicationInstanceManagementService> logger, ApplicationInstanceManager instanceManager)
        {
            _logger = logger;
            _instanceManager = instanceManager;
        }
        public override Task<ApplicationInstanceInfo> CreateApplicationInstance(Empty request, ServerCallContext context)
        {
            return _instanceManager.CreateApplicationInstance(request, context);
        }
        public override Task<Empty> DestroyApplicationInstance(ApplicationInstanceInfo request, ServerCallContext context)
        {
            return _instanceManager.DestroyAppliationInstance(request, context);
        }
        public override Task<ApplicationInstanceIds> GetApplicationInstanceIds(Empty request, ServerCallContext context)
        {
            return _instanceManager.GetApplicationInstanceIds(request, context);
        }
    }
}
