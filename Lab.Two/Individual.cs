namespace Lab.Two;

public record Individual(List<double> Data)
{
	public int Rank { get; set; }

	public Guid ID { get; } = Guid.NewGuid();

	public override string ToString() => string.Join('\t', Data) + $"\t{Rank}";
}