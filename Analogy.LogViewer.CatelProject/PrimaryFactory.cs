using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.CatelProject
{
    public class PrimaryFactory : IAnalogyFactory
    {
        public Guid FactoryID { get; } = new Guid("D2B2AFEB-E103-406D-94F4-059AE9510F68");
        public string Title { get; } = "Catel Log Parser";
        public IAnalogyDataProvidersFactory DataProviders { get; }
        public IAnalogyCustomActionsFactory Actions { get; } = new EmptyActionsFactory(); // if no custom action needed
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Log Parser for Catel Log files";

        public PrimaryFactory()
        {
            DataProviders = new DataProvidersFactory();
        }
    }
}