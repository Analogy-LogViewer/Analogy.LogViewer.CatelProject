using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.CatelProject
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public string Title { get; } = "Some Title";
        public IEnumerable<IAnalogyDataProvider> Items { get; }

        public DataProvidersFactory()
        {
            //get some data provider
            List<IAnalogyDataProvider> dataProviders = new List<IAnalogyDataProvider>();
            dataProviders.Add(new OfflineDataProvider());
        
        }
    }
}
