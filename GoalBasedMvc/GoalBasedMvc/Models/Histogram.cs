using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface IHistogram
    {
        IList<HistogramDatum> GetHistogramData(HistogramContext context, int num = 100);
    }

    public class Histogram:IHistogram
    {
        public IList<HistogramDatum> GetHistogramData(HistogramContext context, int num = 100)
        {
            var histogramDataArray = new HistogramDatum[num];
            double lastCumulativeFrequency = 0;
            double dblNum = num;
            for (int cnt = 0; cnt < num; cnt++)
            {
                var histogramData = new HistogramDatum();
                var interval = (cnt / dblNum) * (context.GlobalXMax - context.GlobalXMin) + context.GlobalXMin;
                histogramData.Interval = interval;
                var isTrue = context.Simulations[0] < interval;
                double cumulativeCount = context.Simulations.Count(x => x < interval);
                double totalCount = context.Simulations.Count;
                var cumulativeFrequency = cumulativeCount / totalCount;
                var frequency = cumulativeFrequency - lastCumulativeFrequency;
                histogramData.Frequency = frequency;
                lastCumulativeFrequency = cumulativeFrequency;
                histogramDataArray[cnt] = histogramData;
            }
            return histogramDataArray;
        }
    }
}
