using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDirectoryContents
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                FileMonitorReporter.FileInfoReporter(
                    CompareMaps.CompareAsFileInfo(
                        MapReader.ReadMapAsFileInfo(args[1]), 
                        MapStructure.MapAsFileInfo(args[0]))); 

                MapWriter.WriteMap(MapStructure.MapAsFileInfo(args[0]), args[1]);
            }
            else 
            {
                Console.WriteLine("Incorrect number of parameters, please read the documentation carefully.");
            }
            
            Console.ReadKey();

            
        }
    }
}
