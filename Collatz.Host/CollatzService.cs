using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CollatzGrpc;
using Google.Protobuf;
using Grpc.Core;

namespace Collatz.Service
{
	public sealed class CollatzService 
		: CollatzGrpc.Collatz.CollatzBase
	{
		public override async Task CalculateStream(CollatzRequest request, 
			IServerStreamWriter<CollatzResponse> responseStream, ServerCallContext context)
		{
			var random = new Random();
			var bytes = request.Number.ToArray();
			var start = new BigInteger(request.Number.ToArray());

			if (start <= BigInteger.One)
			{
				throw new ArgumentException(
					"Must provide a starting value greater than one.", nameof(start));
			}

			var sequence = new List<byte[]>
			{
				start.ToByteArray()
			};

			while (start > 1)
			{
				await Task.Delay(random.Next(250, 1000));
				start = start % 2 == 0 ?
					start / 2 : ((3 * start) + 1) / 2;
				sequence.Add(start.ToByteArray());

				if(sequence.Count >= 3)
				{
					var result = new CollatzResponse()
					{
						Number = ByteString.CopyFrom(start.ToByteArray()),
					};

					result.Sequence.AddRange(sequence.Select(_ => ByteString.CopyFrom(_)));

					await responseStream.WriteAsync(result);
					sequence.Clear();
				}
			}
		}

		public override Task<CollatzResponse> Calculate(
			CollatzRequest request, ServerCallContext context)
		{
			var bytes = request.Number.ToArray();
			var start = new BigInteger(request.Number.ToArray());

			if (start <= BigInteger.One)
			{
				throw new ArgumentException(
					"Must provide a starting value greater than one.", nameof(start));
			}

			var sequence = new List<byte[]>
			{
				start.ToByteArray()
			};

			while (start > 1)
			{
				start = start % 2 == 0 ?
				start / 2 : ((3 * start) + 1) / 2;
				sequence.Add(start.ToByteArray());
			}

			var result = new CollatzResponse()
			{
				Number = ByteString.CopyFrom(start.ToByteArray()),
			};

			result.Sequence.AddRange(sequence.Select(_ => ByteString.CopyFrom(_)));

			return Task.FromResult(result);
		}
	}
}