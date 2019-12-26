using Orleans;
using System.Threading.Tasks;

namespace TestGrain
{
    public interface IAdderGrain : IGrainWithIntegerKey
    {
        Task<int> Add(int value1, int value2);
    }
}
