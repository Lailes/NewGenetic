﻿using Algorithm.Entities;
using Algorithm.Utils;

namespace Algorithm.Genetic;

public class StandardRulesOfNature : IRulesOfNature
{
	public int TournamentSize { get; init; } = 4;

	public double Alpha { get; init; } = 1;

	public double PickRadius { get; init; } = 1;

	public double MutationProbability { get; init; } = 0.001;

	public double CrossingoverExcess { get; init; } = 0.2;

	public double MutationExcess { get; init; } = 0.3;

	public IList<Individual> Selection(IList<Individual> population, Func<Individual, double> fitnessFunc) =>
		Enumerable.Range(0, population.Count)
		          .Select(_ => population.RandomItems(TournamentSize))
		          .Select(_ => _.MaxBy(i => i.CalculateFitnessFunction(population, fitnessFunc, PickRadius, Alpha)))
		          .ToList();

	public IList<Individual> Replication(IList<Individual> population) =>
		population.Shuffle()
		          .Select(p1 => (Parent1: p1, Parent2: population.FirstOrDefault(p2 => p1 != p2 && p1.Distance(p2) <= PickRadius * 1.5)))
		          .Select(_ => Crossover(_.Parent1, _.Parent2))
		          .ToList();

	private Individual Crossover(in Individual c1, in Individual c2) =>
		BuildChromosome(Math.Min(c1.X1, c2.X1), Math.Max(c1.X1, c2.X1), Math.Min(c1.X2, c2.X2), Math.Max(c1.X2, c2.X2));

	private Individual BuildChromosome(double minX1, double maxX1, double minX2, double maxX2) =>
		new()
		{
			X1 = RandomExtensions.NextDouble(minX1 + CrossingoverExcess * (maxX1 - minX1), maxX1 - CrossingoverExcess * (maxX1 - minX1)),
			X2 = RandomExtensions.NextDouble(minX2 + CrossingoverExcess * (maxX2 - minX2), maxX2 - CrossingoverExcess * (maxX2 - minX2))
		};

	public IList<Individual> Mutation(IList<Individual> population, ValueRange range) =>
		population.Select(individual => Mutate(individual, range)).ToList();

	private Individual Mutate(in Individual individual, in ValueRange range) =>
		Random.Shared.NextDouble() > MutationProbability
			? new Individual
			{
				X1 = RandomExtensions.NextDouble(individual.X1 - individual.X1 * MutationExcess, individual.X1 + individual.X1 * MutationExcess).LimitByRange(range),
				X2 = RandomExtensions.NextDouble(individual.X2 - individual.X2 * MutationExcess, individual.X2 + individual.X2 * MutationExcess).LimitByRange(range)
			}
			: individual;

	public IList<Individual> Reduction(IList<Individual> population, Func<Individual, double> fitnessFunction, int targetCount) =>
		population.Count == targetCount
			? population
			: population.OrderByDescending(_ => _.CalculateFitnessFunction(population, fitnessFunction, PickRadius, Alpha))
			            .Take(targetCount)
			            .ToList();
}