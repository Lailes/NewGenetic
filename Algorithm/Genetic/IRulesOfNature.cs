using Algorithm.Entities;

namespace Algorithm.Genetic;

public interface IRulesOfNature
{
	public Random Random { get; }

	public IList<Individual> Selection(IList<Individual> population, FitnessFunc fitnessFunc);

	public IList<Individual> Replication(IList<Individual> population);

	public IList<Individual> Mutation(IList<Individual> population);

	public IList<Individual> Reduction(IList<Individual> population, int targetCount);
}

public delegate double FitnessFunc(Individual individual);