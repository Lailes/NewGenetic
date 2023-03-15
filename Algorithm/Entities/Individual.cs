namespace Algorithm.Entities;

public record Individual(Chromosome Chromosome)
{
    public double SurviveChance { get; set; }
}