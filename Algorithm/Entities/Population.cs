using Algorithm.Genetic;
using Algorithm.Utils;

namespace Algorithm.Entities;

public class Population
{
	public IList<Individual> Individuals { get; set; }

	public IList<IndividualPosition> IndividualPositions => Individuals.Select(_ => new IndividualPosition(_, FitnessFunction(_.Chromosome)))
	                                                                   .ToList();

	public Func<Chromosome, double> FitnessFunction { get; }

	public static Population RandomPopulation(Random random,
	                                          int count,
	                                          RangeBound rangeBound,
	                                          Func<Chromosome, double> fitnessFunction)
	{
		var individuals = Enumerable.Range(0, count)
		                            .Select(_ => new Chromosome
		                            {
			                            X1 = random.NextDouble(rangeBound),
			                            X2 = random.NextDouble(rangeBound)
		                            })
		                            .Select(_ => new Individual(_))
		                            .ToList();

		return new Population(individuals, fitnessFunction);
	}

	public Population(IList<Individual> individuals, Func<Chromosome, double> fitnessFunction)
	{
		Individuals = individuals;
		FitnessFunction = fitnessFunction;
	}

	public Population ProcessNextPopulation(IRulesOfNature nature)
	{
		var x = this;
		var initialCount = x.Individuals.Count;

		// Algorithm nature core. RULES OF NATURE!
		x = nature.Selection(x);
		x = nature.Replication(x);
		x = nature.Mutation(x);
		x = nature.Reduction(x, initialCount);

		return x;
	}
}

public static class PopulationExtensions{
	public static Population ModifyPopulation(this IList<Individual> individuals, Population population)
	{
		population.Individuals = individuals;
		return population;
	}

	public static Population NewPopulation(this IList<Individual> individuals, Population population) =>
		new(individuals, population.FitnessFunction);
}