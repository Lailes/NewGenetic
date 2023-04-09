namespace Lab.Two;

public class FitnessCalculator
{
    public required IList<Func<IList<double>, double>> FitnessFunctions { get; init; }
	
	public IList<Individual> CalculateFitnessValue(IList<Individual> individuals)
	{
		var currentRank = 1;
		var list = new List<Individual>();
		var startCount = individuals.Count;
		while (individuals.Any())
		{
			var notDominated = GetNotDominated(individuals);
			individuals = individuals.ExceptBy(notDominated.Select(_ => _.ID), _ => _.ID).ToList();

			foreach (var individual in notDominated)
				individual.Rank = currentRank;

			currentRank++;
			list.AddRange(notDominated);
		}

		if (list.Count != startCount)
			throw new Exception("Calculate Fitness Error");

		return list;
	}

	private IList<Individual> GetNotDominated(IList<Individual> individuals)
	{
		var dominatedIds = new List<Guid>();

		for (var i = 0; i < individuals.Count; i++)
		{
			var currentResults = Calc(individuals[i].Data);
			for (var j = 0; j < individuals.Count; j++)
			{
				if (i == j) continue;

				var results = Calc(individuals[j].Data);

				var pairs = currentResults.Zip(results);

				if (pairs.All(_ => _.First >= _.Second))
					dominatedIds.Add(individuals[i].ID);
			}
		}
		
		return individuals.Where(_ => !dominatedIds.Contains(_.ID)).ToList();
	}

	private List<double> Calc(List<double> data) => FitnessFunctions.Select(_ => _(data)).ToList();
}