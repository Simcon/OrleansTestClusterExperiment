using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

namespace OrleansTestClusterExperiment
{
    public class Tests
    {
        private TestCluster _cluster;
        private IClusterClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var builder = new TestClusterBuilder();
            _cluster = builder
                .AddSiloBuilderConfigurator<TestClusterSiloBuilderConfigurator>()
                .Build();
            _cluster.Deploy();
            _client = _cluster.Client;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _cluster.StopAllSilos();
        }

        [Test]
        public async Task Test()
        {
            var id = Guid.NewGuid();
            var expected = "my title";

            await _client.GetGrain<ICardGrain>(id).SetTitle(expected);
            var actual = await _client.GetGrain<ICardGrain>(id).GetTitle();

            Assert.AreEqual(expected, actual);
        }
    }

    public class TestClusterSiloBuilderConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder
                .ConfigureServices(services =>
                    services.AddTransient<ICardGrain, CardGrain>())
                //.UseLocalhostClustering() // for dev only
                .AddMemoryGrainStorageAsDefault();
        }
    }
}