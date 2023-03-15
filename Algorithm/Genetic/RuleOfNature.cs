using Algorithm.Entities;
using Algorithm.Utils;

namespace Algorithm.Genetic;

public class StandardRulesOfNature : IRulesOfNature
{
	private readonly Random _random = new();
	public Random Random => _random;

	public int TournamentSize { get; init; } = 4;

	public double Alpha { get; init; } = 1;

	public double PickRadius { get; init; } = 1;

	public double MutationProbability { get; init; } = 0.001;

	public double CrossingoverExcess { get; set; } = 0.2;

	public double MutationExcess { get; set; } = 0.3;

	public Population ProcessSurvivingChance(Population population, Func<Chromosome, double> fi)
	{
		var min = double.MaxValue;
		foreach (var individual in population.Individuals)
		{
			individual.SurviveChance = individual.Chromosome.CalculateFitnessFunction(population.Individuals.Select(_ => _.Chromosome), fi, PickRadius, Alpha);
			if (individual.SurviveChance < min)
				min = individual.SurviveChance;
		}

		min = Math.Abs(min);
		var sum = 0.0;
		foreach (var individual in population.Individuals)
		{
			individual.SurviveChance += min;
			sum += individual.SurviveChance;
		}

		foreach (var individual in population.Individuals)
			individual.SurviveChance /= sum;

		return population;
	}

	// Tournament selection
	public Population Selection(Population population)
	{
		var individuals = Enumerable.Range(0, population.Individuals.Count)
		                            .Select(_ => population.Individuals.RandomItems(Random, TournamentSize))
		                            .Select(_ => _.MaxBy(i => i.SurviveChance))
		                            .ToList();

		population.Individuals = individuals!;
		return population;
	}

	public Population Replication(Population population)
	{
		var genes1 = population.Individuals.Select(_ => _.Chromosome).OrderBy(_ => Random.NextDouble());
		var genes2 = population.Individuals.Select(_ => _.Chromosome).OrderBy(_ => Random.NextDouble());

		population.Individuals = genes1.Zip(genes2)
		                               .Select(Cross)
		                               .Select(_ => new Individual(_))
		                               .ToList();

		return population;
	}

	private Chromosome Cross((Chromosome, Chromosome) pair)
	{
		var (c1, c2) = pair;
		var minX1 = Math.Min(c1.X1, c2.X1);
		var maxX1 = Math.Max(c1.X1, c2.X1);
		var delX1 = maxX1 - minX1;

		var minX2 = Math.Min(c1.X2, c2.X2);
		var maxX2 = Math.Max(c1.X2, c2.X2);
		var delX2 = maxX2 - minX2;

		return new Chromosome
		{
			X1 = Random.NextDouble(minX1 + delX1 * CrossingoverExcess, maxX1 - delX1 * CrossingoverExcess),
			X2 = Random.NextDouble(minX2 + delX2 * CrossingoverExcess, maxX2 - delX2 * CrossingoverExcess)
		};
	}

	public Population Mutation(Population population)
	{
		if (_random.NextDouble() > MutationProbability)
			return population;


		foreach (var individual in population.Individuals)
		{
			var x1 = individual.Chromosome.X1;
			var x2 = individual.Chromosome.X2;

			individual.Chromosome.X1 = _random.NextDouble(x1 - x1 * MutationExcess, x1 + x1 * MutationExcess);
			individual.Chromosome.X2 = _random.NextDouble(x2 - x2 * MutationExcess, x2 + x2 * MutationExcess);
		}

		return population;
	}

	public Population Reduction(Population population, int targetCount)
	{
		if (population.Individuals.Count == targetCount)
			return population;

		population.Individuals = population.Individuals
		                            .OrderBy(_ => _random.NextDouble())
		                            .Take(targetCount)
		                            .ToList();
		return population;
	}
}