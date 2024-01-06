using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts.Systems
{
    /// <summary>
    ///     Collects the error rate of drum hits and then stores values to
    ///     calculate the average error rate based on a set number of
    ///     previous hits.
    /// </summary>
    public class ErrorRateController
    {
        private const int MaxErrorRates = 20; // how many prev hits to consider
        private static ErrorRateController _instance;

        private static readonly Queue<float> ErrorRates = new Queue<float>(); // holds previous error rates

        private float _averageErrorRate;

        public static ErrorRateController Instance
        {
            get
            {
                if (_instance == null) _instance = new ErrorRateController();
                return _instance;
            }
        }

        /// <summary>
        ///     Registers new error rates and updates the <c>ErrorRates</c> queue
        ///     appropriately
        /// </summary>
        /// <param name="errorRate">The error rate to register</param>
        public void AddErrorRate(float errorRate)
        {
            if (ErrorRates.Count >= MaxErrorRates)
                ErrorRates.Dequeue();
            ErrorRates.Enqueue(errorRate);
            UpdateAverageErrorRate();
        }

        /// <summary>
        ///     Calculates the average error rate based on all values in the
        ///     <c>ErrorRates</c> queue
        /// </summary>
        private void UpdateAverageErrorRate()
        {
            if (ErrorRates.Count == 0) return;
            _averageErrorRate = ErrorRates.Average();
        }

        /// <summary>
        ///     Allows the average error rate to be read by other scripts
        /// </summary>
        /// <returns>The average Error Rate</returns>
        public float GetAverageErrorRate()
        {
            return _averageErrorRate;
        }

        /// <summary>
        ///     Removes existing error rates from the <c>ErrorRates</c> queue
        /// </summary>
        public void ClearErrorRates()
        {
            ErrorRates.Clear();
        }

        /// <returns>The most recently recorded error rate</returns>
        public float GetLastErrorRate()
        {
            if (ErrorRates.Count == 0) return 0;
            return ErrorRates.Last();
        }
    }
}