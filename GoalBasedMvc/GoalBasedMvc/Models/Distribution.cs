using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class Distribution
    {
        private const double LEFT_TAIL = 10;
        private const double RIGHT_TAIL = 10;
        private const double TOTAL_PROBABILITY = 100;
        private const double NORMAL_PROBABILITY = TOTAL_PROBABILITY - LEFT_TAIL - RIGHT_TAIL;

        private readonly double _slope1, _slope2, _slope3, _slope4;
        private readonly double _intercept1, _intercept2, _intercept3, _intercept4;

        private readonly double _leftNormalProbability, _rightNormalProbability;
        private readonly double _moment3;

        public Distribution(int id, double minimum, double worst, double likely, double best, double maximum)
        {
            Id = id;
            Minimum = minimum;
            Worst = worst;
            Likely = likely;
            Best = best;
            Maximum = maximum;

            HeightWorst = GetHeightWorst();
            HeightBest = GetHeightBest();
            HeightLikely = GetHeightLikely();

            _leftNormalProbability = GetLeftNormalProbability();
            _rightNormalProbability = GetRightNormalProbability();

            _slope1 = GetSlope1();
            _slope2 = GetSlope2();
            _slope3 = GetSlope3();
            _slope4 = GetSlope4();

            _intercept1 = GetIntercept1();
            _intercept2 = GetIntercept2();
            _intercept3 = GetIntercept3();
            _intercept4 = GetIntercept4();

            CdfProbabilities = new List<double> { LEFT_TAIL, LEFT_TAIL + _leftNormalProbability, TOTAL_PROBABILITY - RIGHT_TAIL, TOTAL_PROBABILITY };
            Slopes = new List<double> { _slope1, _slope2, _slope3, _slope4 };
            Intercepts = new List<double> { _intercept1, _intercept2, _intercept3, _intercept4 };
            ALower = new List<double> { 0, LEFT_TAIL, LEFT_TAIL + _leftNormalProbability, TOTAL_PROBABILITY - RIGHT_TAIL };

            XLower = new List<double> { Minimum, Worst, Likely, Best };

            Mean = GetMean();
            Variance = GetVariance();
            Stdev = GetStandardDeviation();
            _moment3 = GetMoment(3);
            Skew = GetSkewness();
            Kurt = GetKurtosis();
        }

        public int Id { get; }

        public double Minimum { get; }
        public double Worst { get; }
        public double Likely { get; }
        public double Best { get; }
        public double Maximum { get; }

        public double HeightWorst { get; }
        public double HeightLikely { get; }
        public double HeightBest { get; }

        public double Mean { get; }
        private double Variance { get; }
        public double Stdev { get; }
        public double Skew { get; }
        public double Kurt { get; }

        [JsonIgnore]
        public virtual IList<double> CdfProbabilities { get; }

        public virtual double GetPrice(double uniformRandom, int index)
        {
            var mSim = Slopes[index];
            var bSim = Intercepts[index];
            var aLower = ALower[index];
            var xLower = XLower[index];
            var c1 = mSim / 2;
            var c2 = bSim;
            var c3 = aLower - uniformRandom - ((mSim / 2) * Math.Pow(xLower, 2) + bSim * xLower);
            var xSim = (-c2 + Math.Sqrt(Math.Pow(c2, 2) - 4 * c1 * c3)) / (2 * c1);
            return xSim;
        }

        private IList<double> Slopes { get; }
        private IList<double> Intercepts { get; }
        private IList<double> ALower { get; }
        private IList<double> XLower { get; }

        private double GetHeightWorst() { return (2 * LEFT_TAIL) / (Worst - Minimum); }
        private double GetHeightLikely()
        {
            return (((2 * NORMAL_PROBABILITY) - (HeightWorst * (Likely - Worst)) - (HeightBest * (Best - Likely))) / (Best - Worst));
        }
        private double GetHeightBest() { return (2 * RIGHT_TAIL) / (Maximum - Best); }

        private double GetLeftNormalProbability() { return (HeightWorst + HeightLikely) * (Likely - Worst) / 2; }
        private double GetRightNormalProbability() { return (HeightLikely + HeightBest) * (Best - Likely) / 2; }

        private double GetSlope1() { return HeightWorst / (Worst - Minimum); }
        private double GetSlope2() { return (HeightLikely - HeightWorst) / (Likely - Worst); }
        private double GetSlope3() { return (HeightBest - HeightLikely) / (Best - Likely); }
        private double GetSlope4() { return -HeightBest / (Maximum - Best); }

        private double GetIntercept1() { return HeightWorst - (_slope1 * Worst); }
        private double GetIntercept2() { return HeightLikely - (_slope2 * Likely); }
        private double GetIntercept3() { return HeightLikely - (_slope3 * Likely); }
        private double GetIntercept4() { return HeightBest - (_slope4 * Best); }

        private double GetMean()
        {
            return GetMoment(1);
        }
        private double GetVariance()
        {
            var moment2 = GetMoment(2);
            var variance = moment2 - Math.Pow(Mean, 2);
            return variance;
        }
        private double GetStandardDeviation()
        {
            return Math.Sqrt(Variance);
        }
        private double GetSkewness()
        {
            var skew = (_moment3 - 3 * Mean * Variance - Math.Pow(Mean, 3)) / (Math.Pow(Stdev, 3));
            return skew;
        }
        private double GetKurtosis()
        {
            var moment4 = GetMoment(4);
            var kurtosis = (moment4 - 4 * Mean * _moment3 + 6 * Math.Pow(Mean, 2) * Variance + 3 * Math.Pow(Mean, 4)) / (Math.Pow(Variance, 2)) - 3;
            return kurtosis;
        }
        private double GetMoment(int moment)
        {
            var component1 = ((_slope1 / (moment + 2)) * (Math.Pow(Worst, (moment + 2)) - Math.Pow(Minimum, (moment + 2))) + (_intercept1 / (moment + 1)) * (Math.Pow(Worst, (moment + 1)) - Math.Pow(Minimum, (moment + 1)))) / 100;
            var component2 = ((_slope2 / (moment + 2)) * (Math.Pow(Likely, (moment + 2)) - Math.Pow(Worst, (moment + 2))) + (_intercept2 / (moment + 1)) * (Math.Pow(Likely, (moment + 1)) - Math.Pow(Worst, (moment + 1)))) / 100;
            var component3 = ((_slope3 / (moment + 2)) * (Math.Pow(Best, (moment + 2)) - Math.Pow(Likely, (moment + 2))) + (_intercept3 / (moment + 1)) * (Math.Pow(Best, (moment + 1)) - Math.Pow(Likely, (moment + 1)))) / 100;
            var component4 = ((_slope4 / (moment + 2)) * (Math.Pow(Maximum, (moment + 2)) - Math.Pow(Best, (moment + 2))) + (_intercept4 / (moment + 1)) * (Math.Pow(Maximum, (moment + 1)) - Math.Pow(Best, (moment + 1)))) / 100;
            return component1 + component2 + component3 + component4;
        }

    }
}
