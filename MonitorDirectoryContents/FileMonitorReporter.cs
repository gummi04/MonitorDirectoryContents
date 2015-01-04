using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDirectoryContents
{
    public class FileMonitorReporter
    {
        public static IReporter _reporter;
        public static FileInfoWrapper[] _files;
        public static IReporter Reporter
        {
            get { return _reporter ?? new FileInfoReporter(_files); }
            set { _reporter = value; }
        }
        public static void FileInfoReporter(FileInfoWrapper[] files)
        {
            _files = files;
            Reporter.Report(_files);
        }
    }

    public interface IReporter
    {
        void Report(FileInfoWrapper[] files);
    }

    public class FileInfoReporter : IReporter
    {
        private readonly FileInfoWrapper[] _files;
        public FileInfoReporter(FileInfoWrapper[] files)
        {
            _files = files;
        }

        public void Report(FileInfoWrapper[] files)
        {
            if (files.Count() > 0)
            {
                foreach (var entry in files)
                    Console.WriteLine("reporting: Skrá: {0} - breyting: {1}", entry.Name, entry.Modified);
            }
            else
            {
                Console.WriteLine("Nothing new here....");
            }
        }
    }
}
