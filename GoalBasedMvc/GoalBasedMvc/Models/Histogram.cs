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
            for (int cnt = 1; cnt <= num; cnt++)
            {
                var histogramData = new HistogramDatum();
                var interval = (cnt / dblNum) * (context.GlobalXMax - context.GlobalXMin) + context.GlobalXMin;
                histogramData.Interval = interval;
                double cumulativeCount = context.Simulations.Count(x => x <= interval);
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
