using System.Diagnostics;
using Algorithm;
using Algorithm.Genetic;
using Algorithm.Utils;

var algorithm = new GeneticAlgorithm
{
	PopulationSize = 50,
	Range = new(-5, 5),
	Logger = new FolderLogger(@"C:\Users\Amade\Desktop\Populations\Data"),
	FitnessFunc = _ => 200- Math.Pow(_.X1 * _.X1 + _.X2 - 11, 2) - Math.Pow(_.X1 + _.X2 * _.X2 - 7, 2),
	RulesOfNature = new StandardRulesOfNature
	{
		MutationProbability = 0.0001,
		Alpha = 2,
		PickRadius = 25,
		TournamentSize = 10,
		CrossingoverExcess = 0.3,
		MutationExcess = 0.1
	}
};

Console.WriteLine("Algorithm is Started");

var watch = Stopwatch.StartNew();

await algorithm.FindSolution(200);

Console.WriteLine($"Algorithm is finished, {watch.ElapsedMilliseconds} ms");
