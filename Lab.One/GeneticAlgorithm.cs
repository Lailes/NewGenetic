using Algorithm.Entities;
using Algorithm.Genetic;
using Utils;

namespace Algorithm;

public class GeneticAlgorithm
{
	public int PopulationSize { get; init; } = 100;

	public required ValueRange Range { get; init; }

	public required IRulesOfNature RulesOfNature { get; init; }

	public required ILogger<Individual> Logger { get; init; }

	public required Func<double, double, double> FitnessFunc { get; init; }

	public IEnumerable<Individual> FindSolution(int maxIterationSize = 1000) =>
		Enumerable.Range(0, maxIterationSize)
		          .Aggregate(PopulationFunctions.RandomPopulation(PopulationSize, Range, FitnessFunc), (p, i) => p.Log(Logger, $"Population-{i}").ProcessNextPopulation(RulesOfNature, Range))
		          .DistinctBy(individual => ((int) Math.Round(individual.X1), (int) Math.Round(individual.X2)));
}