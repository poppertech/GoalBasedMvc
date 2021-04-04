using System;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface IStatistic
    {
        double Mean { get;}
        double Stdev { get;}
        double Skew { get;}
        double Kurt { get;}
        void Init(IList<double> inputReturns);
    }

    public class Statistic: IStatistic
    {
        public double Mean { get; private set; }
        public double Stdev { get; private set; }
        public double Skew { get; private set; }
        public double Kurt { get; private set; }

        public void Init(IList<double> inputReturns)
        {
            Mean = inputReturns.Average();
            var count = inputReturns.Count;
            var deMeanedReturns = inputReturns.Select(rett => rett - Mean).ToArray();

            Stdev = CalculateStdev(deMeanedReturns, count);
            Skew = CalculateSkew(deMeanedReturns, count);
            Kurt = CalculateKurt(deMeanedReturns, count);
        }


        private double CalculateStdev(IList<double> deMeanedReturns, double count)
        {       
                double sumSq = deMeanedReturns.Sum(rett => Math.Pow(rett, 2));
                return Math.Pow(sumSq / (count - 1), .5);    
        }

        private double CalculateSkew(IList<double> deMeanedReturns, double count)
        {
            double sumCube = deMeanedReturns.Sum(rett => Math.Pow(rett, 3));
            return (count / ((count - 1) * (count - 2))) * (sumCube / Math.Pow(Stdev, 3));
        }

        private double CalculateKurt(IList<double> deMeanedReturns, double count)
        {
            double sumPow4 = deMeanedReturns.Sum(rett => Math.Pow(rett, 4));
            double coef = (((count) * (count + 1)) / ((count - 1) * (count - 2) * (count - 3)));
            double adjFact = (-3 * ((Math.Pow(count - 1, 2)) / ((count - 2) * (count - 3))));
            return (coef * (sumPow4 / Math.Pow(Stdev, 4)) + adjFact);
        }
    }
}
