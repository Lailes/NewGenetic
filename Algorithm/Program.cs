using System.Diagnostics;
using Algorithm;
using Algorithm.Genetic;
using Algorithm.Logging;
using Algorithm.Utils;

double TargetFunction(double x1, double x2) => 200 - Math.Pow(x1 * x1 + x2 - 11, 2) - Math.Pow(x1 + x2 * x2 - 7, 2);

Console.WriteLine("Initilizing is started");

var stopWatch = Stopwatch.StartNew();
var algorithm = new GeneticAlgorithm
{
	PopulationSize = 100,
	RangeBound = new RangeBound(-5, 5),
	Logger = new FolderLogger(@"C:\Users\Amade\Desktop\Populations\Data"),
	FitnessFunc = _ => TargetFunction(_.X1, _.X2),
	RulesOfNature = new StandardRulesOfNature
	{
		MutationProbability = 0.001,
		Alpha = 2,
		PickRadius = 0.1,
		TournamentSize = 5,
		CrossingoverExcess = 0.1,
		MutationExcess = 0.2
	}
};

Console.WriteLine($"Algorithm is initilized, {stopWatch.ElapsedMilliseconds}ms\nAlgorithm is Started!");

await algorithm.FindSolution(100);

Console.WriteLine($"Algorithm is finished, {stopWatch.ElapsedMilliseconds}ms\nPress any key to exit");
