using Genetic;
using Utils;

namespace Lab.Two;

public class GeneticAlgorithm
{
	public required int PopulationSize { get; init; }

	public required ValueRange Range { get; init; }

	public required IRulesOfNature<Individual> RulesOfNature { get; init; }

	public required ILogger<Individual> Logger { get; init; }

	public IEnumerable<Individual> FindSolution(int maxIterationSize = 100, bool forceStop = true)
	{
		var population = RulesOfNature.CreatePopulation(PopulationSize, Range);

		int index;
		for (index = 0; index < maxIterationSize; index++)
		{
			population.Log(Logger, $"Population-{index}");
			population = population.ProcessNextPopulation(RulesOfNature, Range);

			if (forceStop && population.All(_ => _.Rank == 1))
				break;
		}

		population.Log(Logger, $"Population-{++index}");

		return population;
	}
}