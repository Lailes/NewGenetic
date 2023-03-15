namespace Algorithm.Entities;

public record struct Individual(double X1, double X2);

public static class IndividualExtensions
{
	public static double Distance(this in Individual chromosome1, in Individual chromosome2) =>
		Math.Sqrt(Math.Pow(chromosome1.X1 - chromosome2.X1, 2) + Math.Pow(chromosome1.X2 - chromosome2.X2, 2));

	private static double Sh(double distance, double pickRadius, double alpha) =>
		distance < pickRadius ? 1 - Math.Pow(distance / pickRadius, alpha) : 0;

	public static double CalculateFitnessFunction(this Individual individual,
	                                              IEnumerable<Individual> population,
	                                              Func<Individual, double> fitnessFunc,
	                                              double pick, double alpha) =>
		fitnessFunc(individual) / population.Sum(_ => Sh(Distance(individual, _), pick, alpha));
}