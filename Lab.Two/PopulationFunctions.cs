using Genetic;
using Utils;

namespace Lab.Two;

public static class PopulationFunctions
{
	public static IList<Individual> RandomPopulation(int count, int functionRank, ValueRange range) =>
		Enumerable.Range(0, count)
		          .Select(_ => new Individual(Enumerable.Repeat(0, functionRank).Select(_ => range.NextDouble()).ToList())).ToList();

	public static IList<Individual> ProcessNextPopulation(this IList<Individual> population,
	                                                      IRulesOfNature<Individual> nature,
	                                                      ValueRange range)
	{
		var initialCount = population.Count;

		var parents = nature.Selection(population);
		var childs = nature.Mutation(nature.Replication(parents), range);

		population = population.Concat(childs).ToList();

		population = nature.Reduction(population, initialCount);

		return population;
	}
}