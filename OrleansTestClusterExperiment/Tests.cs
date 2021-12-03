using NUnit.Framework;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

namespace OrleansTestClusterExperiment
{
    public class Tests
    {
        private TestCluster _cluster;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var builder = new TestClusterBuilder();
            _cluster = builder
                .AddSiloBuilderConfigurator<TestClusterSiloBuilderConfigurator>()
                .Build();
            _cluster.Deploy();
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

            await _cluster.Client.GetGrain<ICardGrain>(id).SetTitle(expected);
            // GetTitle returns null when it should return "my title"
            var actual = await _cluster.Client.GetGrain<ICardGrain>(id).GetTitle();

            Assert.AreEqual(expected, actual);
        }
    }

    public class TestClusterSiloBuilderConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder siloBuilder)
        {
            siloBuilder
                .UseLocalhostClustering()
                .AddMemoryGrainStorageAsDefault();
        }
    }
}