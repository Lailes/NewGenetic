using Algorithm.Genetic;
using Utils;

namespace Algorithm.Entities;

public static class PopulationFunctions
{
	public static IList<Individual> RandomPopulation(int count, ValueRange range, Func<double, double, double> fitnessFunc) =>
		Enumerable.Range(0, count)
		          .Select(_ => new Individual
		          {
			          X1 = range.NextDouble(),
			          X2 = range.NextDouble(),
			          FitnessFunc = fitnessFunc
		          }).ToList();

	public static IList<Individual> ProcessNextPopulation(this IList<Individual> population,
	                                                      IRulesOfNature nature,
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