using Analogy.Interfaces;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.CatelProject
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Add SourceLink", AnalogChangeLogType.Improvement, "Lior Banai", new DateTime(2020, 03, 14));
            yield return new AnalogyChangeLog("Initial commit", AnalogChangeLogType.None, "Lior Banai", new DateTime(2020, 02, 07));
        }
    }
}
