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
        private double _count;
        private IList<double> _deMeanedReturns;
        private const int NUMBER_DAYS_IN_YEAR = 250;
        private const int ERROR_CODE = 9999;

        public double Mean { get; private set; }
        public double Stdev { get; private set; }
        public double Skew { get; private set; }
        public double Kurt { get; private set; }

        public void Init(IList<double> inputReturns)
        {
            Mean = inputReturns.Average();
            _count = inputReturns.Count();
            _deMeanedReturns = inputReturns.Select(rett => rett - Mean).ToArray();

            Stdev = CalculateStdev();
            Skew = CalculateSkew();
            Kurt = CalculateKurt();
        }


        private double CalculateStdev()
        {       
                double sumSq = _deMeanedReturns.Select(rett => Math.Pow(rett, 2)).Sum();
                return Math.Pow(sumSq / (_count - 1), .5);    
        }

        private double CalculateSkew()
        {
            double sumCube = _deMeanedReturns.Select(rett => Math.Pow(rett, 3)).Sum();
            return (_count / ((_count - 1) * (_count - 2))) * (sumCube / Math.Pow(Stdev, 3));
        }

        private double CalculateKurt()
        {
            double sumPow4 = _deMeanedReturns.Select(rett => Math.Pow(rett, 4)).Sum();
            double coef = (((_count) * (_count + 1)) / ((_count - 1) * (_count - 2) * (_count - 3)));
            double adjFact = (-3 * ((Math.Pow(_count - 1, 2)) / ((_count - 2) * (_count - 3))));
            return (coef * (sumPow4 / Math.Pow(Stdev, 4)) + adjFact);
        }
    }
}
