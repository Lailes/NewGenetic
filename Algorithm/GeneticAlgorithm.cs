using Algorithm.Entities;
using Algorithm.Genetic;
using Algorithm.Logging;
using Algorithm.Utils;

namespace Algorithm;

public class GeneticAlgorithm
{
	public int PopulationSize { get; set; } = 100;

	public required RangeBound RangeBound { get; set; }

	public required IRulesOfNature RulesOfNature { get; set; }

	public required Func<Chromosome, double> FitnessFunction { get; set; }

	public ILogger? Logger { get; set; }

	public int[]? LogSteps { get; set; }

	public async Task<Population> FindSolution(int maxIterationSize = 1000)
	{
		var population = Population.RandomPopulation(RulesOfNature.Random, PopulationSize, RangeBound, FitnessFunction);

		Logger?.LogAsync(new LoggingInfo(population.IndividualPositions), "Initial Population");

		for (var i = 0; i < maxIterationSize; i++)
		{
			population = population.ProcessNextPopulation(RulesOfNature);
			if (Logger == null || (LogSteps != null && LogSteps.Contains(i))) continue;

			await Logger.LogAsync(new LoggingInfo(population.IndividualPositions, i));
		}

		Logger?.LogAsync(new LoggingInfo(population.IndividualPositions), "Final Population");

		return population;
	}
}