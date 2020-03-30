using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.CatelProject
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("D2B2AFEB-E103-406D-94F4-059AE9510F68");
        public Guid FactoryId { get; } = Id;
        public string Title { get; } = "Catel Log Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Log Parser for Catel Log files";
    }
}