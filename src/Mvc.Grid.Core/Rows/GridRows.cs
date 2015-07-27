using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class GridRows<T> : IGridRowsOf<T>
    {
        public IEnumerable<IGridRow<T>> CurrentRows { get; set; }
        public Func<T, String> CssClasses { get; set; }
        public IGrid<T> Grid { get; set; }

        public GridRows(IGrid<T> grid)
        {
            Grid = grid;
        }

        public virtual IEnumerator<IGridRow<T>> GetEnumerator()
        {
            if (CurrentRows == null)
            {
                IQueryable<T> items = Grid.Source;
                foreach (IGridProcessor<T> processor in Grid.Processors.Where(proc => proc.ProcessorType == GridProcessorType.Pre))
                    items = processor.Process(items);

                foreach (IGridProcessor<T> processor in Grid.Processors.Where(proc => proc.ProcessorType == GridProcessorType.Post))
                    items = processor.Process(items);

                CurrentRows = items
                    .ToList()
                    .Select(model => new GridRow<T>(model)
                    {
                        CssClasses = (CssClasses != null)
                            ? CssClasses(model)
                            : null
                    });
            }

            return CurrentRows.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
