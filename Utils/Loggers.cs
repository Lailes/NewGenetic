namespace Utils;

public interface ILogger<TIndividual>
{
	public IList<TIndividual> Log(IList<TIndividual> positions, string label);
}

public static class LoggerExtensions
{
	public static IList<TIndividual> Log<TIndividual>(this IList<TIndividual> positions, ILogger<TIndividual> logger, string label) =>
		logger.Log(positions, label);
}

public class FolderLogger<TIndividual> : ILogger<TIndividual>
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

	public IList<TIndividual> Log(IList<TIndividual> positions, string label)
	{
		var file = Path.Combine(_folderPath, $"{label}.tsv");

		if (File.Exists(file))
			File.Delete(file);

		var data = positions.Select(_ => _.ToString());
		File.WriteAllLines(file, data);
		return positions;
	}
}