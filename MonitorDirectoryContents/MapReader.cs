using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace MonitorDirectoryContents
{
    public static class MapReader
    {
        public static string _path;
        public static IReader _reader;
        public static IReader Reader
        {
            get { return _reader ?? new FileSystemReader(_path); }
            set { _reader = value; }
        }

        public static FileInfoWrapper[] ReadMapAsFileInfo(string path)
        {
            _path = path;
            return Reader.ReadMapAsFileInfo();
        }
    }

    public interface IReader
    {
        FileInfoWrapper[] ReadMapAsFileInfo();
    }

    public class FileSystemReader : IReader
    {
        private readonly string _historyPath;
        public FileSystemReader(string historyPath)
        {
            _historyPath = historyPath;
            if (!File.Exists(_historyPath))
                throw new FileNotFoundException();
        }

        public FileInfoWrapper[] ReadMapAsFileInfo()
        {
            XmlSerializer reader = new XmlSerializer(typeof(FileInfoWrapper[]));
            StreamReader inputFile = new StreamReader(_historyPath);
            FileInfoWrapper[] map = (FileInfoWrapper[])reader.Deserialize(inputFile);
            inputFile.Close();
            return map;
        }
    }
}
