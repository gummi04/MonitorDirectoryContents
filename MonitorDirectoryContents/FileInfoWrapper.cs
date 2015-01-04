using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MonitorDirectoryContents
{
    [Serializable()]
    public class FileInfoWrapper
    {
        private string _name;
        private string _fullName;
        private string _lastWriteTime;
        private string _length;
        private string _modified;
        private string _hash;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }
        public string LastWriteTime
        {
            get { return _lastWriteTime; }
            set { _lastWriteTime = value; }
        }
        public string Length
        {
            get { return _length; }
            set { _length = value; }
        }
        public string Modified
        {
            get { return _modified; }
            set { _modified = value; }
        }
        public string Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        public FileInfoWrapper(FileInfo fileInfo, string hash)
        {
            _name = fileInfo.Name;
            _fullName = fileInfo.FullName;
            _lastWriteTime = fileInfo.LastWriteTime.ToString();
            _length = fileInfo.Length.ToString();
            _modified = String.Empty;
            _hash = hash;
        }

        public FileInfoWrapper()
        {  }
    }
}
