namespace Algorithm.Utils;

public static class RandomExtensions
{
	public static double NextDouble(this ValueRange range) => range.From + Random.Shared.NextDouble() * (range.To - range.From);

	public static double NextDouble(double min, double max) => min + Random.Shared.NextDouble() * (max - min);

	public static IEnumerable<T> RandomItems<T>(this IList<T> source, int count) =>
		Enumerable.Range(0, count)
		          .Select(_ => source[Random.Shared.Next(0, source.Count)]);

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) =>
		source.OrderBy(_ => Random.Shared.NextDouble());
}