namespace Algorithm.Utils;

public record struct ValueRange(double From, double To);

public static class ValueRangeExtensions
{
	public static double LimitByRange(this double value, ValueRange range) => Math.Min(range.To, Math.Max(value, range.From));
}