using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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
            Reporter.ReportToHtml(_files);
        }
    }

    public interface IReporter
    {
        void ReportToConsole(FileInfoWrapper[] files);
        void ReportToHtml(FileInfoWrapper[] files);
    }

    public class FileInfoReporter : IReporter
    {
        private readonly FileInfoWrapper[] _files;
        public FileInfoReporter(FileInfoWrapper[] files)
        {
            _files = files;
        }

        public void ReportToConsole(FileInfoWrapper[] files)
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

        public void ReportToHtml(FileInfoWrapper[] files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(OpenHTMLReport());
            if (files.Count() > 0)
            {
                foreach (var entry in files)
                    sb.Append(AddRowHTLMReport(entry));
            }
            else
            {
                sb.Append(AddMessageToHTMLReport("Nothing new to report."));
            }
            sb.Append(CloseHTMLReport());

            System.IO.File.WriteAllText(@"C:\map.html", sb.ToString());
            System.Diagnostics.Process.Start(@"c:\map.html");
        }

        #region HTML helper stuff
        private string OpenHTMLReport()
        {
            return "<html><body><h1>Report</h1><table border=\"1\"><tr><th>File Name</th><th>Change</th><th>Full File Name</th></tr>";
        }
        private string CloseHTMLReport()
        {
            return "</table></body></html>";
        }
        private string AddRowHTLMReport(FileInfoWrapper file)
        {
            return String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", WebUtility.HtmlEncode(file.Name), WebUtility.HtmlEncode(file.Modified), WebUtility.HtmlEncode(file.FullName));
        }
        private string AddMessageToHTMLReport(string message)
        {
            return String.Format("<tr><td colspan=\"3\"><font color=\"navy\">{0}</font></td></tr>", WebUtility.HtmlEncode(message));
        }


        #endregion
    }
}
