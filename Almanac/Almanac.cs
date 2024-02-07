namespace Almanac;

public static class AlmanacResolver
{
    /// <summary>
    /// Assumptions:
    /// - Input is stream to text file.
    /// - I intentionally don't convert input into tables with numbers, ranges etc to do it straight on the source in efficient way.
    /// - It is straight approach by mapping each input to the output through the mapping steps end check which is lowest.
    /// 
    /// The processes optimisations:
    /// - processing line by line only once iteration through the file
    /// - use only two arrays
    /// - take advantage of the fact that the input text has a certain constant structure
    /// - not to process the particular source if it is already mapped
    /// - do not process the rest of mapping if all sources are already mapped
    /// - convert to numbers only once or never (if all sources are mapped)
    /// </summary>
    public static long GetMinimumLoaction(StreamReader almanac)
    {
        // here I'm assuming that i can hold all seeds in memory as there wouldn't be many of them
        var seeds = almanac.ReadLine()
            .Split(' ')
            .Skip(1)
            .Select(seed => Convert.ToInt64(seed))
            .ToArray();

        almanac.ReadLine();
        almanac.ReadLine();

        return GetLocations(almanac, seeds)
            .Min();
    }

    private static long[] GetLocations(StreamReader almanac, long[] sources)
    {
        var mappedSourcesIndexes = new bool[sources.Length];
        var mappedSourcesCount = 0;

        while (true)
        {
            var mapLine = almanac.ReadLine();

            if (mapLine is null) // end of file
            {
                return sources; 
            }

            if (mapLine == string.Empty) // end of mapping
            {
                Array.Fill(mappedSourcesIndexes, false);
                mappedSourcesCount = 0;

                almanac.ReadLine();
                continue;
            }

            if (mappedSourcesCount == sources.Length)
            {
                continue; // no need to process more if all sources are mapped
            }

            var mapData = mapLine.Split(' ').Select(x => Convert.ToInt64(x)).ToArray();
            MatchMapLine(sources, mappedSourcesIndexes, ref mappedSourcesCount, mapData[1], mapData[0], mapData[2]);
        }
    }

    private static void MatchMapLine(long[] sources, bool[] mappedSourcesIndexes, ref int mappedSourcesCount, long mapSourceStart, long mapDestiantionStart, long rangeLength)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            if (mappedSourcesIndexes[i])
            {
                continue; // no need to check already mapped source
            }    

            var destination = GetMatchedDestinationOrNegative(sources[i], mapSourceStart, mapDestiantionStart, rangeLength);

            if (destination < 0)
            {
                continue;
            }

            sources[i] = destination; // override source with destination in place to avoid creating new array
            mappedSourcesIndexes[i] = true;
            ++mappedSourcesCount;
        }   
    }

    private static long GetMatchedDestinationOrNegative(long source, long mapSourceStart, long mapDestiantionStart, long rangeLength)
    {
        if (source < mapSourceStart)
        {
            return -1; // means that source is lower than range
        }

        var sourceStartRangeToSourceOffset = source - mapSourceStart;

        if (sourceStartRangeToSourceOffset < rangeLength)
        {
            return mapDestiantionStart + sourceStartRangeToSourceOffset;
        }

        return -1;  // means that source is higher than range
    }
}
