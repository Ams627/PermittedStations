using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PermittedStations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || args.Length > 2)
                {
                    throw new Exception("Please supply an origin and (optionally) a group destination");
                }

                var origin = args[0];
                var dest = args.Length > 1 ? args[1] : "";

                var filename = @"s:\\FareGroupPermittedStations_v1.0.xml";
                var doc = XDocument.Load(filename);
                var result = doc.Descendants("PermittedStations")
                    .Where(x=>x?.Attribute("FareLocationNlc").Value == origin && (string.IsNullOrEmpty(dest) || x?.Attribute("FareGroupNlc").Value == dest))
                    .Select(y => new { Route = y.Attribute("RouteCode").Value,
                                       EndDate = y.Attribute("EndDate").Value,
                                       StartDate = y.Attribute("StartDate").Value,
                                       Dest = y.Attribute("FareGroupNlc").Value,
                                       CrsList = y.Elements("Crs").Select(z=>z.Value).ToList() });
                foreach (var item in result)
                {
                    Console.WriteLine($"destination {item.Dest} route code {item.Route} EndDate {item.EndDate} StartDate {item.StartDate}");
                    foreach (var crs in item.CrsList)
                    {
                        Console.WriteLine($"    {crs}");
                    }
                }
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
