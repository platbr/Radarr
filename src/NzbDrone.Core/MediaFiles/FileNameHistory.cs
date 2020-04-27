using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NzbDrone.Core.MediaFiles
{
    public class FileNameHistory
    {
        private List<Entry> _entries = new List<Entry>();
        private readonly string _filePath;
        public FileNameHistory(string filePath)
        {
            _filePath = filePath;
        }
        public void Append(string newName, string originalName)
        {
            foreach (var entry in _entries.Where(entry => entry.NewName == newName).ToList())
            {
                _entries.Remove(entry);
            }

            _entries.Add(new Entry(newName, FindOldestFileName(originalName)));
        }
        public void Load()
        {
            try
            {
                using (var file =
                    new StreamReader(_filePath))
                {
                    string line;
                    while((line = file.ReadLine()) != null)
                    {
                        const string pattern = @"(.+)=""(.+)""";
                        var matches = Regex.Matches(line, pattern);
                        if (matches.Count > 0 && matches[0].Groups.Count > 1)
                        {
                            _entries.Add(new Entry(matches[0].Groups[1].Value, matches[0].Groups[2].Value));
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                // ignored
            }
        }

        public void Save()
        {
            using (var file =
                new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                foreach (var entry in _entries)
                {
                    file.WriteLine("{0}=\"{1}\"", entry.NewName, entry.OriginalName);
                }
            }
        }

        private string FindOldestFileName(string fileName)
        {
            foreach (var entry in _entries.Where(entry => entry.NewName == fileName))
            {
                return entry.OriginalName;
            }

            return fileName;
        }

        private class Entry
        {
            public string NewName { get; }
            public string OriginalName { get; }
            public Entry(string newName, string originalName)
            {
                NewName = newName;
                OriginalName = originalName;
            }
        }
    }
}
