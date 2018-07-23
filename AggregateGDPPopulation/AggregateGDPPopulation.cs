using System;

using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AggregateGDPPopulation
{
    public static class Async
    {
            public static async Task CalculateGDPPopulation()
            {
                string dataFilePath = @"D:\C# assignments\aggregate-gdp-population-csharp-problem-Ritam-majumdar\AggregateGDPPopulation\data\datafile.csv";
                string mapperFilePath = @"D:\C# assignments\aggregate-gdp-population-csharp-problem-Ritam-majumdar\AggregateGDPPopulation\data\country-continent.json";
                string writeFilePath = @"D:\C# assignments\aggregate-gdp-population-csharp-problem-Ritam-majumdar\AggregateGDPPopulation\data\outputfilenew.json";
                JObject CountryVsContinent = JObject.Parse(await File.ReadAllTextAsync(mapperFilePath));
                List<string> rows = new List<string>();
                string line;
                string line1;
                //string[] data = File.ReadAllLines(dataFilePath);
                string[] data = await File.ReadAllLinesAsync(dataFilePath);
                foreach (string str in data)
                {
                    line = str.Replace("[\"]+", "");
                    rows.Add(line);
                }
                string[] headers;
                headers = rows[0].Split(',');
                int CountryIndex = Array.IndexOf(headers, "\"Country Name\"");
                int PopulationIndex = Array.IndexOf(headers, "\"Population (Millions) - 2012\"");
                int GDPIndex = Array.IndexOf(headers, "\"GDP Billions (US Dollar) - 2012\"");
                rows.RemoveAt(0);
                Dictionary<string, Dictionary<string, double>> resultset = new Dictionary<string, Dictionary<string, double>>();
                string[] rowdata;
                foreach (string row in rows)
                {
                    line1 = row.Replace("\"", "");
                    rowdata = line1.Split(',');
                    try
                    {
                        string continent = CountryVsContinent[rowdata[CountryIndex]].ToString();

                        if (resultset.ContainsKey(continent))
                        {
                            double GDPToAdd = Convert.ToDouble(rowdata[GDPIndex]);
                            double PopulationToAdd = Convert.ToDouble(rowdata[PopulationIndex]);
                            resultset[continent]["GDP_2012"] = resultset[continent]["GDP_2012"] + GDPToAdd;
                            resultset[continent]["POPULATION_2012"] = resultset[continent]["POPULATION_2012"] + PopulationToAdd;
                        }
                        else
                        {
                            Dictionary<string, double> GDPVsPopulation = new Dictionary<string, double>();
                            GDPVsPopulation.Add("GDP_2012", Convert.ToDouble(rowdata[GDPIndex]));
                            GDPVsPopulation.Add("POPULATION_2012", Convert.ToDouble(rowdata[PopulationIndex]));
                            resultset.Add(continent, GDPVsPopulation);
                        }
                    }
                    catch { };

                }
                string Json = JsonConvert.SerializeObject(resultset, Formatting.Indented);
                 await File.WriteAllTextAsync(writeFilePath, Json);
            }
    }
}
