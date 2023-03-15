using Algorithm.Genetic;
using Algorithm.Utils;

namespace Algorithm.Entities;

public class Population
{
	public IList<Individual> Individuals { get; set; }

	public IList<IndividualPosition> IndividualPositions => Individuals.Select(_ => new IndividualPosition(_, _fitnessFunction(_.Chromosome)))
	                                                                   .ToList();

	private readonly Func<Chromosome, double> _fitnessFunction;

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
		_fitnessFunction = fitnessFunction;
	}

	public Population ProcessNextPopulation(IRulesOfNature nature)
	{
		var x = this;
		var initialCount = x.Individuals.Count;

		// Algorithm nature core. RULES OF NATURE!
		x = nature.ProcessSurvivingChance(x, _fitnessFunction);
		x = nature.Selection(x);
		x = nature.Replication(x);
		x = nature.Mutation(x);
		x = nature.Reduction(x, initialCount);

		return x;
	}
}