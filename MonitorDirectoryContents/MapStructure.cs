using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace MonitorDirectoryContents
{
    public static class MapStructure
    {
        public static IMapper _mapper;
        public static string _path;

        public static IMapper Mapper
        {
            get { return _mapper ?? new FileSystemMapper(new DirectoryInfo(_path)); }
            set { _mapper = value; }
        }

        public static FileInfoWrapper[] MapAsFileInfo(string path)
        {
            _path = path;
            return Mapper.MapAsFileInfo();
        }
    }


    public interface IMapper
    {
        FileInfoWrapper[] MapAsFileInfo();
    }

    public class FileSystemMapper : IMapper
    {
        private readonly DirectoryInfo _dir;
        public FileSystemMapper(DirectoryInfo dir)
        {
            _dir = dir;
            if(!Directory.Exists(_dir.ToString()))
                throw new FileNotFoundException("Directory not found.");
        }

        public FileInfoWrapper[] MapAsFileInfo()
        {
            FileInfo[] directory = _dir.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfoWrapper> wrappedDirectory = new List<FileInfoWrapper>();
            foreach (FileInfo file in directory)
                wrappedDirectory.Add(new FileInfoWrapper(file, getSHA256(file)));
            return wrappedDirectory.ToArray();
        }

        private string getSHA256(FileInfo file)
        {
            SHA256 hashFunction = SHA256Managed.Create();
            byte[] hashValue;
            FileStream fileStream = file.Open(FileMode.Open);
            fileStream.Position = 0;
            hashValue = hashFunction.ComputeHash(fileStream);
            fileStream.Close();
            string hash = String.Empty;
            foreach (byte bit in hashValue)
                hash += bit.ToString("x2");
            return hash;
        }
    }
}
