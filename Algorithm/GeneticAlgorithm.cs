using Algorithm.Entities;
using Algorithm.Genetic;
using Algorithm.Utils;

namespace Algorithm;

public class GeneticAlgorithm
{
	public int PopulationSize { get; init; } = 100;

	public required ValueRange Range { get; init; }

	public required IRulesOfNature RulesOfNature { get; init; }

	public required ILogger Logger { get; init; }

	public required Func<Individual, double> FitnessFunc { get; init; }

	public async Task<IList<Individual>> FindSolution(int maxIterationSize = 1000)
	{
		var population = PopulationFunctions.RandomPopulation(PopulationSize, Range);

		await Log(population, "Initial Population");
		await Task.WhenAll(Enumerable.Range(0, maxIterationSize).Select(i =>
		{
			population = population.ProcessNextPopulation(RulesOfNature, FitnessFunc, Range);
			return Log(population, $"Population-{i}");
		}).ToArray());
		await Log(population, "Final Population");
		return population;
	}

	private Task Log(IEnumerable<Individual> population, string title) => Logger.LogAsync(population.Select(_ => (_, FitnessFunc(_))), title);
}