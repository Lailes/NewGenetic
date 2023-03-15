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

	public ILogger Logger { get; set; }

	public FitnessFunc FitnessFunc { get; set; }

	public async Task<IList<Individual>> FindSolution(int maxIterationSize = 1000)
	{
		var population = PopulationFunctions.RandomPopulation(RulesOfNature.Random, PopulationSize, RangeBound);

		await Logger.LogAsync(population.Select(_ => (_, FitnessFunc(_))), "Initial Population");

		for (var i = 0; i < maxIterationSize; i++)
		{
			population = population.ProcessNextPopulation(RulesOfNature, FitnessFunc);
			await Logger.LogAsync(population.Select(_ => (_, FitnessFunc(_))), $"Population-{i}");
		}

		await Logger.LogAsync(population.Select(_ => (_, FitnessFunc(_))), "Final Population");
		return population;
	}
}