namespace SampleCompany.SampleProduct.ApplicationEngineService
{
    public class GrpcClientServiceConfiguration
    {
        public string ServerExeFilePath { get; }
        public string GrpcAddress { get; }

        public GrpcClientServiceConfiguration(string serverExeFilePath, string grpcAddress)
        {
            ServerExeFilePath = serverExeFilePath;
            GrpcAddress = grpcAddress;
        }
    }
}
