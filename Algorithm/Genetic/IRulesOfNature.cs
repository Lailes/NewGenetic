﻿using Algorithm.Entities;

namespace Algorithm.Genetic;

public interface IRulesOfNature
{
	public Random Random { get; }

	public Population ProcessSurvivingChance(Population population, Func<Chromosome, double> fi);

	public Population Selection(Population population);

	public Population Replication(Population population);

	public Population Mutation(Population population);

	public Population Reduction(Population population, int targetCount);
}