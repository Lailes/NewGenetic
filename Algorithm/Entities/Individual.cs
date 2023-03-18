namespace Algorithm.Entities;

public readonly record struct Individual(double X1, double X2, Func<double, double, double> FitnessFunc)
{
	public double Y => FitnessFunc(X1, X2);
}

public static class IndividualExtensions
{
	public static double Distance(this in Individual chromosome1, in Individual chromosome2) =>
		Math.Sqrt(Math.Pow(chromosome1.X1 - chromosome2.X1, 2) + Math.Pow(chromosome1.X2 - chromosome2.X2, 2));

	private static double Sh(double distance, double pickRadius, double alpha) =>
		distance < pickRadius ? 1 - Math.Pow(distance / pickRadius, alpha) : 0;

	public static double CalculateFitnessFunction(this Individual individual,
	                                              IEnumerable<Individual> population,
	                                              double pick, double alpha) =>
		individual.Y / population.Sum(_ => Sh(Distance(individual, _), pick, alpha));
}