using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace MonitorDirectoryContents
{
    public static class MapWriter
    {
        public static string _path;
        public static IWriter _writer;
        public static IWriter Writer
        {
            get { return _writer ?? new FileSystemWriter(_path); }
            set { _writer = value; }
        }

        public static void WriteMap(FileInfoWrapper[] map, string path)
        {
            _path = path;
            Writer.WriteMap(map);
        }
    }

    public interface IWriter
    {
        void WriteMap(FileInfoWrapper[] map);
    }

    public class FileSystemWriter : IWriter
    {
        private readonly string _historyPath;
        public FileSystemWriter(string historyPath)
        {
            _historyPath = historyPath;
            if (!File.Exists(historyPath))
                File.Create(historyPath).Dispose();
            else File.WriteAllText(historyPath, string.Empty);
        }

        public void WriteMap(FileInfoWrapper[] map)
        {
            XmlSerializer writer = new XmlSerializer(typeof(FileInfoWrapper[]));
            StreamWriter outputFile = new StreamWriter(_historyPath);
            writer.Serialize(outputFile, map);
            outputFile.Close();
        }
    }
}
