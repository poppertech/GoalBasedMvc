using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface IHistogram
    {
        IList<HistogramDatum> GetHistogramData(HistogramContext context, int num = 30);
    }

    public class Histogram:IHistogram
    {
        public IList<HistogramDatum> GetHistogramData(HistogramContext context, int num = 30)
        {
            var histogramDataArray = new HistogramDatum[num];
            double lastCumulativeFrequency = 0;
            double dblNum = num;
            var sortedSimulations = new List<double>(context.Simulations);
            sortedSimulations.Sort();
            for (int cnt = 1; cnt <= num; cnt++)
            {
                var histogramData = new HistogramDatum();
                var interval = (cnt / dblNum) * (context.GlobalXMax - context.GlobalXMin) + context.GlobalXMin;
                histogramData.Interval = interval;
                var index = sortedSimulations.BinarySearch(interval);
                double cumulativeCount = index < 0? ~index: index + 1;
                double totalCount = context.Simulations.Count;
                var cumulativeFrequency = cumulativeCount / totalCount;
                var frequency = cumulativeFrequency - lastCumulativeFrequency;
                histogramData.Frequency = frequency;
                lastCumulativeFrequency = cumulativeFrequency;
                histogramDataArray[cnt-1] = histogramData;
            }
            return histogramDataArray;
        }
    }
}
