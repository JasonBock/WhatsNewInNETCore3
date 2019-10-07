using CollatzGrpc;
using Google.Protobuf;
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
			var reply = await client.CalculateAsync(
				new CollatzRequest() { Number = ByteString.CopyFrom(new BigInteger(5).ToByteArray()) });

			await Console.Out.WriteLineAsync(string.Join(", ",
				reply.Sequence.Select(_ => new BigInteger(_.ToByteArray()).ToString())));

			await Console.Out.WriteLineAsync("Press enter to quit...");
			await Console.In.ReadLineAsync();
		}
	}
}
