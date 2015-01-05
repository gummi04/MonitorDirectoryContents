using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MonitorDirectoryContents
{
    class Program
    {
        static void Main(string[] args)
        { 
            if (args.Length == 2)
            {
                if (!File.Exists(args[1].ToString()))
                {
                    Console.WriteLine("Map file does not exist, creating...");
                    MapWriter.WriteMap(MapStructure.MapAsFileInfo(args[0]), args[1]);
                    Console.WriteLine("Done!");
                }
                else
                {
                    Console.WriteLine("Mapping structure and comparing...");
                    FileMonitorReporter.FileInfoReporter(
                        CompareMaps.CompareAsFileInfo(
                            MapReader.ReadMapAsFileInfo(args[1]),
                            MapStructure.MapAsFileInfo(args[0])));
                    Console.WriteLine("Comparison done, writing map...");
                    MapWriter.WriteMap(MapStructure.MapAsFileInfo(args[0]), args[1]);
                    Console.WriteLine("Map written, press any key to close.");
                }
            }
            else 
            {
                Console.WriteLine("Incorrect number of parameters, please read the documentation carefully.");
            }
            
            Console.ReadKey();

            
        }
    }
}
