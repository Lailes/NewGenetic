using Algorithm.Genetic;
using Algorithm.Utils;

namespace Algorithm.Entities;

public static class PopulationFunctions
{
	public static IList<Individual> RandomPopulation(Random random, int count, RangeBound rangeBound) =>
		Enumerable.Range(0, count).Select(_ => new Individual { X1 = random.NextDouble(rangeBound), X2 = random.NextDouble(rangeBound)}).ToList();

	public static IList<Individual> ProcessNextPopulation(this IList<Individual> population, IRulesOfNature nature, FitnessFunc fitnessFunc)
	{
		var initialCount = population.Count;

		// Algorithm nature core. RULES OF NATURE!
		population = nature.Selection(population, fitnessFunc);
		population = nature.Replication(population);
		population = nature.Mutation(population);
		population = nature.Reduction(population, initialCount);

		return population;
	}
}