using Utils;

namespace Genetic;

public interface IRulesOfNature<TIndividual>
{
	public IList<TIndividual> CreatePopulation(int count, ValueRange range);

	public IList<TIndividual> Selection(IList<TIndividual> population);

	public IList<TIndividual> Replication(IList<TIndividual> population);

	public IList<TIndividual> Mutation(IList<TIndividual> population, ValueRange range);

	public IList<TIndividual> Reduction(IList<TIndividual> population, int targetCount);
}