using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGridProcessor<T>
    {
        GridProcessorType ProcessorType { get; set; }

        IQueryable<T> Process(IQueryable<T> items);
    }
}
