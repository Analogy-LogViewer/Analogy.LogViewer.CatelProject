using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.DataProviders.Extensions;
using Analogy.Interfaces;
using Newtonsoft.Json;

namespace Analogy.LogViewer.CatelProject
{
   public class OfflineDataProvider:IAnalogyOfflineDataProvider
    {
  
        public Guid ID { get; }=new Guid("A984AE66-20D1-47A0-8AAE-575D115943E1");
        public string OptionalTitle { get; } = "CatelProject Offline log";
        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "Catel log files|*.log";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.log" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;

        private ILogParserSettings LogParserSettings { get; set; }
        private CatelFileParser CatelFileParser { get; set; }
        private string CatelFileSetting { get; } = "CatelSSettings.json";

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            if (File.Exists(CatelFileSetting))
            {
                try
                {
                    LogParserSettings = JsonConvert.DeserializeObject<LogParserSettings>(CatelFileSetting);
                }
                catch (Exception ex)
                {
                    logger.LogException(ex,"Catel","Error loading file "+CatelFileSetting);
                    LogParserSettings = new LogParserSettings();
                    LogParserSettings.Splitter = " ";
                    LogParserSettings.SupportedFilesExtensions = new List<string> { "*.log" };
                }
            }
            else
            {
                LogParserSettings = new LogParserSettings();
                LogParserSettings.Splitter = " ";
                LogParserSettings.SupportedFilesExtensions = new List<string> { "*.log" };

            }
            CatelFileParser = new CatelFileParser(LogParserSettings);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
                return await CatelFileParser.Process(fileName, token, messagesHandler);
            return new List<AnalogyLogMessage>(0);
        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
            => GetSupportedFilesInternal(dirInfo, recursiveLoad);

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool CanOpenFile(string fileName) => LogParserSettings.CanOpenFile(fileName);

        public bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);
        
        public void MessageOpened(AnalogyLogMessage message)
        {
            throw new NotImplementedException();
        }

        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.log")
                .Concat(dirInfo.GetFiles("*.log"))
                .ToList();
            if (!recursive)
                return files;
            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }
    }

}
