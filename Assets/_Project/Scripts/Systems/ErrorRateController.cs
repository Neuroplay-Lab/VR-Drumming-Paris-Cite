using System.Collections.Generic;
using System.Linq;

namespace _Project.Scripts.Systems
{
    public class ErrorRateController
    {
        private const int MaxErrorRates = 20;
        private static ErrorRateController _instance;

        private static readonly Queue<float> ErrorRates = new Queue<float>();

        private float _averageErrorRate;

        public static ErrorRateController Instance
        {
            get
            {
                if (_instance == null) _instance = new ErrorRateController();
                return _instance;
            }
        }


        public void AddErrorRate(float errorRate)
        {
            // Debug.Log("Error rate: " + errorRate);
            if (ErrorRates.Count >= MaxErrorRates)
                ErrorRates.Dequeue();
            ErrorRates.Enqueue(errorRate);
            UpdateAverageErrorRate();
            // Debug.Log($"Average Error: {Math.Abs(_averageErrorRate / 0.5f * 100):F2}%");
        }

        private void UpdateAverageErrorRate()
        {
            if (ErrorRates.Count == 0) return;
            _averageErrorRate = ErrorRates.Average();
        }

        public float GetAverageErrorRate()
        {
            return _averageErrorRate;
        }

        public void ClearErrorRates()
        {
            ErrorRates.Clear();
        }

        public float GetLastErrorRate()
        {
            if (ErrorRates.Count == 0) return 0;
            return ErrorRates.Last();
        }
    }
}