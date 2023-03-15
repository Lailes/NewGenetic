using Algorithm.Genetic;
using Algorithm.Utils;

namespace Algorithm.Entities;

public static class PopulationFunctions
{
	public static IList<Individual> RandomPopulation(int count, ValueRange range) =>
		Enumerable.Range(0, count)
		          .Select(_ => new Individual { X1 = range.NextDouble(), X2 = range.NextDouble()}).ToList();

	public static IList<Individual> ProcessNextPopulation(this IList<Individual> population,
	                                                      IRulesOfNature nature,
	                                                      Func<Individual, double> fitnessFunc,
	                                                      ValueRange range)
	{
		var initialCount = population.Count;

		var parents = nature.Selection(population, fitnessFunc);
		var childs = nature.Mutation(nature.Replication(parents), range);

		population = population.Concat(childs).ToList();

		population = nature.Reduction(population, fitnessFunc, initialCount);

		return population;
	}
}