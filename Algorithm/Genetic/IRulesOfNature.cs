using Algorithm.Entities;
using Algorithm.Utils;

namespace Algorithm.Genetic;

public interface IRulesOfNature
{
	public IList<Individual> Selection(IList<Individual> population, Func<Individual, double> fitnessFunc);

	public IList<Individual> Replication(IList<Individual> population);

	public IList<Individual> Mutation(IList<Individual> population, ValueRange range);

	public IList<Individual> Reduction(IList<Individual> population, Func<Individual, double> fitnessFunction, int targetCount);
}