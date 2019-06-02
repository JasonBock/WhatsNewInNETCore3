using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace WhatsNewInNETCore3
{
	public static class Program
	{
		static async Task Main()
		{
			var original = 0b_0110_0010_1101_1100u;
			Console.Out.WriteLine(original.ToString());
			var rotate = BitOperations.RotateLeft(original, 2);
			Console.Out.WriteLine(rotate.ToString());

			Console.Out.WriteLine();

			var range = new Range(3, 9);
			Console.Out.WriteLine($"{range.Start}, {range.End}");

			Console.Out.WriteLine();

			var jason = new Person("Jason", "Bock");
			var jane = new Person("Jane", "Doe");
			var jason2 = new Person("Jason", "Bock");

			Console.Out.WriteLine($"{nameof(jason)} - hash code = {jason.GetHashCode()}");
			Console.Out.WriteLine($"{nameof(jane)} - hash code = {jane.GetHashCode()}");
			Console.Out.WriteLine($"{nameof(jason2)} - hash code = {jason2.GetHashCode()}");

			Console.Out.WriteLine();

			await using (var service = new Service()) { }

			Console.Out.WriteLine();
			var data = new byte[] { 0x52, 0x6F, 0x89, 0xAD };
			Console.Out.WriteLine(Convert.ToBase64String(data.AsSpan()));

			var v3 = Vector256.Create(22, 33, 44, 55, 11, 33, 22, 99);
			var v7 = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);

			Console.Out.WriteLine();
			Console.Out.WriteLine(Avx2.IsSupported);
			var vResult = Avx2.Add(v3, v7);
			Console.Out.WriteLine(vResult);
		}
	}

	public class Person
	{
		public Person(string firstName, string lastName) =>
			(this.FirstName, this.LastName) = (firstName, lastName);

		public string FirstName { get; }
		public string LastName { get; }

		public override int GetHashCode() => 
			HashCode.Combine(this.FirstName, this.LastName);
	}

	public sealed class Service
		: IAsyncDisposable
	{
		public async ValueTask DisposeAsync() => 
			await Console.Out.WriteLineAsync("I am disposed");
	}
}