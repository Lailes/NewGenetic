using Algorithm.Entities;

namespace Algorithm.Utils;

public interface ILogger
{
	public IList<Individual> Log(IList<Individual> positions, string label);
}

public static class LoggerExtensions
{
	public static IList<Individual> Log(this IList<Individual> positions, ILogger logger, string label) =>
		logger.Log(positions, label);
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

	public IList<Individual> Log(IList<Individual> positions, string label)
	{
		var file = Path.Combine(_folderPath, $"{label}.tsv");

		if (File.Exists(file))
			File.Delete(file);

		var data = positions.Select(_ => $"{_.X1}\t{_.X2}\t{_.Y}");
		File.WriteAllLines(file, data);
		return positions;
	}
}