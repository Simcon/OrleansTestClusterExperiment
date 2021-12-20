using NUnit.Framework;
using Orleans;

namespace OrleansTestClusterExperiment
{
    internal interface IBasicGrain : IGrainWithGuidKey
    {
        public Task SetValueAsync(int value);
        public Task<int> GetValueAsync();
    }

    internal class BasicGrain : Grain, IBasicGrain
    {
        private int value;

        public Task<int> GetValueAsync() => Task.FromResult(value);

        public Task SetValueAsync(int value)
        {
            this.value = value;
            return Task.CompletedTask;
        }
    }

    internal class IsolatedTests
    {
        [Test]
        public async Task Gets_And_Sets_Value()
        {
            // create a new grain
            var grain = new BasicGrain();

            // assert the default value is zero
            Assert.AreEqual(0, await grain.GetValueAsync());

            // set a new value
            await grain.SetValueAsync(123);

            // assert the new value is as set
            Assert.AreEqual(123, await grain.GetValueAsync());
        }
    }
}
