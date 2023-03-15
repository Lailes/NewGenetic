namespace Algorithm.Utils;

public static class RandomExtensions
{
	public static double NextDouble(this Random random, double min, double max) => min + random.NextDouble() * (max - min);

	public static double NextDouble(this Random random, RangeBound rangeBound) => random.NextDouble(rangeBound.From, rangeBound.To);

	public static IEnumerable<T> RandomItems<T>(this IList<T> source, Random random, int count) =>
		Enumerable.Range(0, count)
		          .Select(_ => source[random.Next(0, source.Count)]);
}