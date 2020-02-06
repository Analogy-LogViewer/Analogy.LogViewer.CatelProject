using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.CatelProject
{
   public class OfflineDataProvider:IAnalogyOfflineDataProvider
    {
  
        public Guid ID { get; }
        public string OptionalTitle { get; }
        public bool CanSaveToLogFile { get; }
        public string FileOpenDialogFilters { get; }
        public string FileSaveDialogFilters { get; }
        public IEnumerable<string> SupportFormats { get; }
        public string InitialFolderFullPath { get; }
        public Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool CanOpenFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool CanOpenAllFiles(IEnumerable<string> fileNames)
        {
            throw new NotImplementedException();
        }

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            throw new NotImplementedException();
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            throw new NotImplementedException();
        }

    }
}
