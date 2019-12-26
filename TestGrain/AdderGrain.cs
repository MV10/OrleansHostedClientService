using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace TestGrain
{
    [StatelessWorker]
    public class AdderGrain : Grain, IAdderGrain
    {
        public Task<int> Add(int value1, int value2)
            => Task.FromResult(value1 + value2);
    }
}

