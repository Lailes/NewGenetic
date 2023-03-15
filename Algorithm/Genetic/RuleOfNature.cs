using System.Runtime.CompilerServices;
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
	public IList<Individual> Selection(IList<Individual> population, FitnessFunc fitnessFunc) =>
		Enumerable.Range(0, population.Count)
		          .Select(_ => population.RandomItems(Random, TournamentSize))
		          .Select(_ => _.MaxBy(i => CalculateFitnessFunction(i, population, fitnessFunc, PickRadius, Alpha)))
		          .ToList();

	public IList<Individual> Replication(IList<Individual> population) =>
		population.OrderBy(_ => Random.NextDouble())
		          .Zip(population)
		          .Select(_ => Crossover(_.First, _.Second))
		          .ToList();

	private Individual Crossover(Individual c1, Individual c2) =>
		BuildChromosome(Math.Min(c1.X1, c2.X1), Math.Max(c1.X1, c2.X1), Math.Min(c1.X2, c2.X2), Math.Max(c1.X2, c2.X2));

	private Individual BuildChromosome(double minX1, double maxX1, double minX2, double maxX2) =>
		new()
		{
			X1 = Random.NextDouble(minX1 + CrossingoverExcess * (maxX1 - minX1), maxX1 - CrossingoverExcess * (maxX1 - minX1)),
			X2 = Random.NextDouble(minX2 + CrossingoverExcess * (maxX2 - minX2), maxX2 - CrossingoverExcess * (maxX2 - minX2))
		};

	public IList<Individual> Mutation(IList<Individual> population) =>
		_random.NextDouble() > MutationProbability
			? population
			: population.Select(_ => new Individual
			{
				X1 = _random.NextDouble(_.X1 - _.X1 * MutationExcess, _.X1 + _.X1 * MutationExcess),
				X2 = _random.NextDouble(_.X2 - _.X2 * MutationExcess, _.X2 + _.X2 * MutationExcess)
			}).ToList();

	public IList<Individual> Reduction(IList<Individual> population, int targetCount) =>
		population.Count == targetCount
			? population
			: population.RandomItems(Random, targetCount)
			            .ToList();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static double Distance(Individual chromosome1, Individual chromosome2) =>
		Math.Sqrt(Math.Pow(chromosome1.X1 - chromosome2.X1, 2) + Math.Pow(chromosome1.X2 - chromosome2.X2, 2));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static double Sh(double distance, double pickRadius, double alpha) =>
		distance < pickRadius ? 1 - Math.Pow(distance / pickRadius, alpha) : 0;

	private double CalculateFitnessFunction(Individual individual,
	                                        IEnumerable<Individual> population,
	                                        FitnessFunc fitnessFunc,
	                                        double pick, double alpha) =>
		fitnessFunc(individual) / population.Sum(_ => Sh(Distance(individual, _), pick, alpha));
}