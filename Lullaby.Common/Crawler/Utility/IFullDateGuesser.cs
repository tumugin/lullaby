﻿namespace Lullaby.Common.Crawler.Utility;

public interface IFullDateGuesser
{
    /// <summary>
    /// Guess full date by uncompleted date.
    /// </summary>
    /// <param name="uncompletedDate">Uncompleted date in mm/dd format.</param>
    /// <param name="currentTime">Current time with timezone to guess the uncompleted dates.</param>
    /// <returns>Will return completed date in complete as-is order of <see param="uncompletedDate"/></returns>
    IReadOnlyList<DateTimeOffset> GuessFullDateByUncompletedDate(IReadOnlyList<string> uncompletedDate, DateTimeOffset currentTime);
}
