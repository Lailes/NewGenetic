using Lab.Two;
using Utils;

namespace Genetic;

public class StandardRulesOfNature: IRulesOfNature<Individual>
{
	public int TournamentSize { get; init; } = 3;

	public double MutationIncident { get; init; } = 0.001;

	public double CrossoverExcess { get; init; } = 0.2;

	public double MutationExcess { get; init; } = 0.3;

	public FitnessCalculator FitnessCalculator { get; set; }

	public IList<Individual> CreatePopulation(int count, ValueRange range) =>
		FitnessCalculator.CalculateFitnessValue(PopulationFunctions.RandomPopulation(count, 2, range));

	public IList<Individual> Selection(IList<Individual> population) =>
	 	IndividualsSelection(FitnessCalculator.CalculateFitnessValue(population));

	private IList<Individual> IndividualsSelection(IList<Individual> population) =>
		Enumerable.Range(0, population.Count)
		          .Select(_ => population.RandomItems(TournamentSize))
		          .Select(_ => _.MaxBy(i => 1 / (1 + i.Rank)))
		          .ToList()!;

	public IList<Individual> Replication(IList<Individual> population) =>
		population.Shuffle()
		          .Select(p1 => (Parent1: p1, Parent2: population.Shuffle().FirstOrDefault(p2 => p1 != p2)))
		          .Select(_ => Crossover(_.Parent1, _.Parent2))
		          .ToList();

	private Individual Crossover(in Individual c1, in Individual c2) =>
		new(c1.Data
		      .Zip(c2.Data)
		      .Select(_ => RandomExtensions.NextDouble(_.First + CrossoverExcess * (_.Second - _.First), _.Second - CrossoverExcess * (_.Second - _.First)))
		      .ToList());

	public IList<Individual> Mutation(IList<Individual> population, ValueRange range) =>
		population.Select(individual => Mutate(individual, range)).ToList();

	private Individual Mutate(in Individual individual, ValueRange range) =>
		Random.Shared.NextDouble() > MutationIncident
			? individual with
			{
				Data = individual.Data.Select(_ => RandomExtensions.NextDouble(_ - _ * MutationExcess, _ + _ * MutationExcess).LimitByRange(range)).ToList()
			}
			: individual;

	public IList<Individual> Reduction(IList<Individual> population, int targetCount) =>
		population.Count == targetCount
			? population
			: FitnessCalculator.CalculateFitnessValue(population)
			                   .OrderByDescending(_ => 1 / (1 + _.Rank))
			                   .Take(targetCount)
			                   .ToList();
}