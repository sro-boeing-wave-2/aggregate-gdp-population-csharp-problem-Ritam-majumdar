using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AggregateGDPPopulation;
using System.IO;
using System.Threading.Tasks;
using Xunit;


namespace AggregateGDPPopulation.Tests
{
    public class AggregateGDPPopulationTesting
    {
        [Fact]
        public  async void Test1()
        {
            Task process1 = Async.CalculateGDPPopulation();
            await process1;
            JObject Actual = JObject.Parse(File.ReadAllText("../../../../AggregateGDPPopulation/data/outputfilenew.json"));
            JObject Expected = JObject.Parse(File.ReadAllText("../../../expected-output.json"));
            Assert.Equal(Expected, Actual);
            Assert.Equal(Expected, Actual);
        }
    }
}
