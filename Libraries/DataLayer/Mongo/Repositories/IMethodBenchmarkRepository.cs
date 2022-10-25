using Common;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IMethodBenchmarkRepository
    {
        Task InsertBenchmark(BenchmarkMethodLogger method);
    }
}
