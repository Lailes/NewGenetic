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

	// Tournament selection
	public Population Selection(Population population) =>
		Enumerable.Range(0, population.Individuals.Count)
		          .Select(_ => population.Individuals.RandomItems(Random, TournamentSize))
		          .Select(_ => _.MaxBy(i => CalculateFitness(i, population)))
		          .ToList()
		          .ModifyPopulation(population);

	private double CalculateFitness(Individual individual, Population population) =>
		individual.Chromosome.CalculateFitnessFunction(population.Individuals.Select(_ => _.Chromosome).ToList(), population.FitnessFunction, PickRadius, Alpha);

	public Population Replication(Population population) =>
		population.Individuals
		          .Select(_ => _.Chromosome)
		          .OrderBy(_ => Random.NextDouble())
		          .Chunk(2)
		          .SelectMany(_ => CrossLiniar(_[0], _[1], population))
		          .Select(_ => new Individual(_))
		          .ToList()
		          .ModifyPopulation(population);

	private IEnumerable<Chromosome> CrossLiniar(Chromosome c1, Chromosome c2, Population population) =>
		new[]
		{
			new Chromosome {X1 = c1.X1 * 0.5 + c2.X1 * 0.5, X2 = c1.X2 * 0.5 + c2.X2 * 0.5},
			new Chromosome {X1 = c1.X1 * 1.5 - c2.X1 * 0.5, X2 = c1.X2 * 1.5 - c2.X2 * 0.5},
			new Chromosome {X1 =-c1.X1 * 0.5 + c2.X1 * 1.5, X2 =-c1.X2 * 0.5 + c2.X2 * 1.5}
		}.OrderByDescending(_ => _.CalculateFitnessFunction(population.Individuals.Select(i => i.Chromosome), population.FitnessFunction, PickRadius, Alpha))
		 .Take(2);

	public Population Mutation(Population population) =>
		_random.NextDouble() > MutationProbability
			? population
			: population.Individuals.Select(_ =>
			{
				_.Chromosome.X1 = _random.NextDouble(_.Chromosome.X1 - _.Chromosome.X1 * MutationExcess, _.Chromosome.X1 + _.Chromosome.X1 * MutationExcess);
				_.Chromosome.X2 = _random.NextDouble(_.Chromosome.X2 - _.Chromosome.X2 * MutationExcess, _.Chromosome.X2 + _.Chromosome.X2 * MutationExcess);
				return _;
			}).ToList().ModifyPopulation(population);

	public Population Reduction(Population population, int targetCount) =>
		population.Individuals.Count == targetCount
			? population
			: population.Individuals
			            .RandomItems(Random, targetCount)
			            .ToList()
			            .NewPopulation(population);
}