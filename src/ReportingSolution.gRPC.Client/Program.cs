using Grpc.Net.Client;

namespace ReportingSolution.gRPC.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var gRpcChannel = GrpcChannel.ForAddress("https://localhost:7234");
        }
    }
}
