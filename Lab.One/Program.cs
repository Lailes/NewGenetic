using System.Diagnostics;
using Algorithm;
using Algorithm.Entities;
using Algorithm.Genetic;
using Utils;

var algorithm = new GeneticAlgorithm
{
	PopulationSize = 100,
	Range = new(-5, 5),
	Logger = new FolderLogger<Individual>(@"C:\Users\Amade\Desktop\Populations\Data"),
	FitnessFunc = (x1, x2) => 200- Math.Pow(x1 * x1 + x2 - 11, 2) - Math.Pow(x1 + x2 * x2 - 7, 2),
	RulesOfNature = new StandardRulesOfNature
	{
		MutationIncident = 0.0001,
		Alpha = 2,
		PickRadius = 2,
		TournamentSize = 10,
		CrossoverExcess = 0.3,
		MutationExcess = 0.1
	}
};

Console.WriteLine("Algorithm is Started");

var watch = Stopwatch.StartNew();

var result = algorithm.FindSolution(200);

result.Select((pair, i) => $"[{i}] X1={pair.X1}\tX2={pair.X2}\t{pair.Y}")
      .ToList()
      .ForEach(Console.WriteLine);

Console.WriteLine($"Algorithm is finished, {watch.ElapsedMilliseconds} ms");


