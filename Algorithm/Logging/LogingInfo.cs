using Algorithm.Utils;

namespace Algorithm.Logging;

public record LoggingInfo(IEnumerable<IndividualPosition> Positions, int? Step = null);