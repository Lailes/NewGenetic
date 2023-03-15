namespace Algorithm.Entities;

public class Chromosome
{
	public double X1 { get; set; }

	public double X2 { get; set; }

	private static double Distance(Chromosome chromosome1, Chromosome chromosome2) =>
		Math.Sqrt(Math.Pow(chromosome1.X1 - chromosome2.X1, 2) + Math.Pow(chromosome1.X2 - chromosome2.X2, 2));

	private static double Sh(double distance, double pickRadius, double alpha) =>
		distance < pickRadius ? 1 - Math.Pow(distance / pickRadius, alpha) : 0;

	public double CalculateFitnessFunction(IEnumerable<Chromosome> chromosomes,
	                                       Func<Chromosome, double> fitnessFunction,
	                                       double pick, double alpha) =>
		fitnessFunction(this) / chromosomes.Sum(_ => Sh(Distance(this, _), pick, alpha));
}