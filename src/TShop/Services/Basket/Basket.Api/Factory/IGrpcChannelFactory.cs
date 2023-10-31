using Grpc.Net.Client;

namespace Basket.Api.Factory
{
    public interface IGrpcChannelFactory
    {
        GrpcChannel GetGrpcChannel(GrpcChannelServer grpcChannelServer);
    }
}