using Orleans;
using Orleans.Runtime;

namespace OrleansTestClusterExperiment
{
    public interface ICardGrain : IGrainWithGuidKey
    {
        public Task SetTitle(string title);
        public Task<string> GetTitle();
    }

    public class CardGrain : Grain, ICardGrain
    {
        private readonly IPersistentState<CardState> _card;

        public CardGrain([PersistentState("card")] IPersistentState<CardState> card)
        {
            _card = card;
        }

        public async Task SetTitle(string title)
        {
            _card.State.Title = title;
            await _card.WriteStateAsync();
        }

        public Task<string> GetTitle() => Task.FromResult(_card.State.Title);
    }

    [Serializable]
    public class CardState
    {
        public string Title { get; set; } = string.Empty;
    }
}
