using Algorithm.Entities;

namespace Algorithm.Logging;

public interface ILogger
{
	public Task LogAsync(IEnumerable<(Individual Individual, double Y)> positions, string label);
}

public class FolderLogger : ILogger
{
	private readonly string _folderPath;

	public FolderLogger(string folderPath)
	{
		if (Directory.Exists(folderPath))
			Directory.EnumerateFiles(folderPath).ToList().ForEach(File.Delete);
		else
			Directory.CreateDirectory(folderPath);

		_folderPath = folderPath;
	}

	public async Task LogAsync(IEnumerable<(Individual Individual, double Y)> positions, string label)
	{
		var file = Path.Combine(_folderPath, $"{label}.tsv");

		if (File.Exists(file))
			File.Delete(file);

		var data = positions.Select(_ => $"{_.Individual.X1}\t{_.Individual.X2}\t{_.Y}");
		await File.WriteAllLinesAsync(file, data);
	}
}

public class ConsoleLogger : ILogger
{
	public Task LogAsync(IEnumerable<(Individual Individual, double Y)> positions, string label)
	{
		var topLabel = $"{new string('=', 10)} {label} {new string('=', 10)}";
		Console.WriteLine(label);

		foreach (var position in positions)
			Console.WriteLine($"[X1={position.Individual.X1}, X1={position.Individual.X2}] => Y = {position.Y}");

		Console.WriteLine(new string('=', topLabel.Length));
		return Task.CompletedTask;
	}
}