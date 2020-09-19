using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.CatelProject
{
    public class CatelFileParser
    {
        private string source = "Catel";
        private ILogParserSettings LogParserSettings { get; }

        public CatelFileParser(ILogParserSettings logParserSettings)
        {

            LogParserSettings = logParserSettings;
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
                return new List<AnalogyLogMessage> { empty };
            }

            //if (!LogParserSettings.CanOpenFile(fileName))
            //{
            //    AnalogyLogMessage empty = new AnalogyLogMessage(
            //        $"File {fileName} Is not supported or not configured correctly in the windows settings",
            //        AnalogyLogLevel.Critical, AnalogyLogClass.General, source, "None")
            //    {
            //        Source = source,
            //        Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
            //    };
            //    messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
            //    return new List<AnalogyLogMessage> { empty };
            //}

            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            try
            {
                using (var stream = File.OpenRead(fileName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string firstLine = string.Empty;
                        string otherdata = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            if (string.IsNullOrEmpty(firstLine))
                                firstLine = await reader.ReadLineAsync();
                            if (reader.EndOfStream)
                            {
                                var m = Parse(firstLine);
                                messages.Add(m);
                                messagesHandler.AppendMessage(m, Utils.GetFileNameAsDataSource(fileName));
                                continue;
                            }
                            var line = await reader.ReadLineAsync();
                            if (line.Contains(" => "))
                            {
                                string lineToProcess = string.IsNullOrEmpty(firstLine)
                                    ? otherdata
                                    : firstLine + Environment.NewLine + otherdata;
                                var m = Parse(lineToProcess);
                                messages.Add(m);
                                messagesHandler.AppendMessage(m, Utils.GetFileNameAsDataSource(fileName));
                                firstLine = line;
                                otherdata = string.Empty;
                            }
                            else
                            {
                                otherdata += line + Environment.NewLine;
                            }
                        }
                        if (!string.IsNullOrEmpty(firstLine))
                        {
                            var m = Parse(firstLine);
                            messages.Add(m);
                            messagesHandler.AppendMessage(m, Utils.GetFileNameAsDataSource(fileName));
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
                return new List<AnalogyLogMessage> { empty };
            }


        }
        private AnalogyLogMessage Parse(string line)
        {
            int first = line.IndexOf(" => ", StringComparison.InvariantCultureIgnoreCase);
            AnalogyLogMessage m = new AnalogyLogMessage();
            m.Source = "Catel Log";
            m.Module = "Catel Log";
            string datetime = line.Substring(0, first);
            if (DateTime.TryParse(datetime, out DateTime dateVal))
            {
                m.Date = dateVal;
            }
            string sub = line.Substring(first + 4);
            int firstSpace = sub.IndexOf(' ');
            string level = sub.Substring(0, firstSpace);
            if (level.StartsWith("[INFO]"))
            {
                m.Level = AnalogyLogLevel.Information;
            }
            else if (level.StartsWith("[ERROR]"))
            {
                m.Level = AnalogyLogLevel.Error;
            }
            else if (level.StartsWith("[WARNING]"))
            {
                m.Level = AnalogyLogLevel.Warning;
            }
            sub = sub.Substring(level.Length);
            int sourceIndex = sub.IndexOf(']')+2;
            m.Source = sub.Substring(2, sourceIndex);
           m.Text = sub.Substring(sourceIndex);
            return m;
        }
    }
}

