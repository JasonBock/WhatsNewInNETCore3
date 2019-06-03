using BenchmarkDotNet.Attributes;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace WhatsNewInNETCore3
{
	[MemoryDiagnoser]
	public class AddingValues
	{
		private readonly int[] values1 = new int[] { 22, 33, 44, 55, 11, 33, 22, 99 };
		private readonly int[] values2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
		private readonly Vector256<int> v1 = Vector256.Create(22, 33, 44, 55, 11, 33, 22, 99);
		private readonly Vector256<int> v2 = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);
		private readonly Vector<int> vector1;
		private readonly Vector<int> vector2;

		public AddingValues()
		{
			this.vector1 = new Vector<int>(this.values1);
			this.vector2 = new Vector<int>(this.values2);
		}

		[Benchmark]
		public int[] AddWithArrays()
		{
			var result = new int[8];

			for(var i = 0; i < result.Length; i++)
			{
				result[i] = this.values1[i] + this.values2[i];
			}

			return result;
		}

		[Benchmark]
		public Vector<int> AddWithVectors() =>
			Vector.Add(this.vector1, this.vector2);

		[Benchmark]
		public Vector256<int> AddWithIntrinsics() =>
			Avx2.Add(this.v1, this.v2);
	}
}
