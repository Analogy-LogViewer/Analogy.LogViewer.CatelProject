using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.CatelProject
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("D2B2AFEB-E103-406D-94F4-059AE9510F68");
        public Guid FactoryId { get; set; } = Id;
        public Image LargeImage { get; set; } = null;
        public Image SmallImage { get; set; } = null;
        public string Title { get; set; } = "Catel Log Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Log Parser for Catel Log files";
    }
}