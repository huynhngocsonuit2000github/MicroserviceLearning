# Install gRPC for existing project api
- Install package
	<ItemGroup>
		<PackageReference Include="Grpc.AspNetCore" Version="2.49.0" /> 
	</ItemGroup>

	<ItemGroup>
		<Protobuf Include="Protos\greeter.proto" GrpcServices="Server" />
	</ItemGroup>

- Create new file Greeter.proto at Proto
	syntax = "proto3";

		service Greeter {
		  rpc SayHello (HelloRequest) returns (HelloReply);
		}

		message HelloRequest {
		  string name = 1;
		}

		message HelloReply {
		  string message = 1;
		}

- Create file service
	using Grpc.Core;

	namespace Users.Api.Services
	{
		public class GreeterService : Greeter.GreeterBase
		{
			private readonly ILogger<GreeterService> _logger;
			public GreeterService(ILogger<GreeterService> logger)
			{
				_logger = logger;
			}

			public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
			{
				return Task.FromResult(new HelloReply
				{
					Message = "Hello " + request.Name
				});
			}
		}
	}

- Register				
    builder.Services.AddGrpc();
	app.MapGrpcService<GreeterService>(); 

- Appsetting need to use Http1AndHttp2 if we want to use eather API and grpc
	
 "Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http1AndHttp2"
    }
  }, 

https://learn.microsoft.com/en-us/aspnet/core/grpc/aspnetcore?view=aspnetcore-7.0&tabs=visual-studio

Config cerrtificate
	https://learn.microsoft.com/vi-vn/aspnet/core/security/docker-compose-https?view=aspnetcore-7.0

Get started with GRPC Client and server
https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-7.0&tabs=visual-studio

- Create ssl
 dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\dockerdemo.pfx -p ssl123
	dotnet dev-certs https --trust
	- 

dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p ssl123
dotnet dev-certs https --trust


- Copy file this certificate to the volumn for binding, and using it for authenticating


  users.api:
    image: tshop-users-img:latest
    container_name: usersapi
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - UserDatabaseSettings:ConnectionString=mongodb://usersdb:27017
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ASPNETCORE_Kestrel__Certificates__Default__Password=ssl123
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
    depends_on:
      - usersdb
    ports:
      - 8002:80
      - 8003:443
    volumes:
      - ./https:/https:ro