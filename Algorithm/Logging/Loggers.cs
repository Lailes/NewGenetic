namespace Algorithm.Logging;

public interface ILogger
{
	public Task LogAsync(LoggingInfo info, string? title = null);
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

	public async Task LogAsync(LoggingInfo info, string? title = null)
	{
		var fileName = $"{title ?? $"population-{info.Step}"}.tsv";
		var file = Path.Combine(_folderPath, fileName);

		if (File.Exists(file))
			File.Delete(file);

		var data = info.Positions.Select(_ => $"{_.Individual.Chromosome.X1}\t{_.Individual.Chromosome.X2}\t{_.Y}");
		await File.WriteAllLinesAsync(file, data);
	}
}

public class ConsoleLogger : ILogger
{
	public Task LogAsync(LoggingInfo info, string? title)
	{
		var label = $"{new string('=', 10)} {title ?? info.Step.ToString()} {new string('=', 10)}";
		Console.WriteLine(label);

		foreach (var position in info.Positions)
			Console.WriteLine($"[X1={position.Individual.Chromosome.X1}, X1={position.Individual.Chromosome.X2}] => Y = {position.Y}");

		Console.WriteLine(new string('=', label.Length));
		return Task.CompletedTask;
	}
}