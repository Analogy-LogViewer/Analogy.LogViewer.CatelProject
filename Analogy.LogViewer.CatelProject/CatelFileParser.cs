using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.CatelProject
{
    public class CatelFileParser
    {
        private string source = "Catel";
        private ILogParserSettings LogParserSettings { get; }
        private string[] splitters;

        public CatelFileParser(ILogParserSettings logParserSettings)
        {
            LogParserSettings = logParserSettings;
            splitters = new[] {logParserSettings.Splitter};
        }

        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, source, "None")
                {
                    Source = source,
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> {empty};
            }

            if (!LogParserSettings.CanOpenFile(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage(
                    $"File {fileName} Is not supported or not configured correctly in the windows settings",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, source, "None")
                {
                    Source = source,
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> {empty};
            }

            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            try
            {
                using (var stream = File.OpenRead(fileName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line.StartsWith("#Software:", StringComparison.CurrentCultureIgnoreCase) ||
                                line.StartsWith("#Version:", StringComparison.CurrentCultureIgnoreCase) ||
                                line.StartsWith("#Date:", StringComparison.CurrentCultureIgnoreCase))
                            {
                                //var headerMsg = HandleHeaderMessage(line, fileName);
                                //messagesHandler.AppendMessage(headerMsg, Utils.GetFileNameAsDataSource(fileName));
                                //messages.Add(headerMsg);
                                continue;
                            }

                            var items = line.Split(splitters, StringSplitOptions.None);
                            var entry = Parse(items);
                            entry.FileName = fileName;
                            messages.Add(entry);
                            messagesHandler.AppendMessage(entry, Utils.GetFileNameAsDataSource(fileName));

                        }
                    }
                }

                return messages;
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage(
                    $"Error occured processing file {fileName}. Reason: {e.Message}",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, source, "None")
                {
                    Source = source,
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> {empty};
            }


        }

        private AnalogyLogMessage Parse(string[] items)
        {
            AnalogyLogMessage m = new AnalogyLogMessage();
            for (var index = 0; index < items.Length; index++)
            {
                string value = items[index];
                //string field = columnIndexToName[index];
                //ActionMapping[field](value, m);
            }

            return m;
        }
    }
}

