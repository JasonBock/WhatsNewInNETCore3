using CollatzGrpc;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Collatz.Client
{
	class Program
	{
		static async Task Main()
		{
			await Console.Out.WriteLineAsync("Press enter to start...");
			await Console.In.ReadLineAsync();

			var channel = GrpcChannel.ForAddress("https://localhost:5501");
			var client = new CollatzGrpc.Collatz.CollatzClient(channel);

			await Calculate(client);
			await CalculateWithStreaming(client);

			await Console.Out.WriteLineAsync("Press enter to quit...");
			await Console.In.ReadLineAsync();
		}

		private static async Task CalculateWithStreaming(CollatzGrpc.Collatz.CollatzClient client)
		{
			var request = new CollatzRequest()
			{
				Number = ByteString.CopyFrom(new BigInteger(55).ToByteArray())
			};

			using var responseCall = client.CalculateStream(request);

			await foreach (var response in responseCall.ResponseStream.ReadAllAsync())
			{
				await Console.Out.WriteLineAsync(string.Join(", ",
					response.Sequence.Select(_ => new BigInteger(_.ToByteArray()).ToString())));
			}
		}

		private static async Task Calculate(CollatzGrpc.Collatz.CollatzClient client)
		{
			var reply = await client.CalculateAsync(
				new CollatzRequest() { Number = ByteString.CopyFrom(new BigInteger(55).ToByteArray()) });

			await Console.Out.WriteLineAsync(string.Join(", ",
				reply.Sequence.Select(_ => new BigInteger(_.ToByteArray()).ToString())));
		}
	}
}
