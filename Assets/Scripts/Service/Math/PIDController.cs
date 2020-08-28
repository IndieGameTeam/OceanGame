namespace GameServices.Math
{
    [System.Serializable]
    public class PIDController
    {
        public float P = 1.0F;
        public float I = 0.0F;
        public float D = 0.2F;

        private float error_old;
        private float error_sum;

        public float PID(float value, float target)
        {
            float output = 0f;

            float error = target - value;

            output += P * error;
            output += I * error_sum;
            output += D * (error - error_old);

            error_sum += error;
            error_old = error;

            return output;
        }
    }
}