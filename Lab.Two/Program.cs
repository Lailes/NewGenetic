// See https://aka.ms/new-console-template for more information

using Genetic;
using Lab.Two;
using Utils;

Console.WriteLine("Стартуем!");

var algorithm = new GeneticAlgorithm
{
	Logger = new FolderLogger<Individual>(@"C:\Users\Amade\Desktop\Populations\Data"),
	PopulationSize = 200,
	Range = new (-4, 4),
	RulesOfNature = new StandardRulesOfNature
	{
		CrossoverExcess = 0.3,
		MutationExcess = 0.2,
		MutationIncident = 0.001,
		FitnessCalculator = new FitnessCalculator
		{
			FitnessFunctions = new List<Func<IList<double>, double>>
			{
				x => Math.Pow(x[0] - 2, 2) / 2 + Math.Pow(x[1] + 1, 2) / 13 + 3,
				x => Math.Pow(x[0] + x[1] - 3, 2) / 36 + Math.Pow(-x[0] + x[1] + 2, 2) / 8 - 17,
				x => Math.Pow(3 * x[0] - 2 * x[1] + 4, 2) / 18 + Math.Pow(x[0] - x[1] + 1, 2) / 27 + 15
			}
		}
	}
};

var solution = algorithm.FindSolution();

Console.WriteLine("JOB DONE");

solution.ToList().ForEach(Console.WriteLine);