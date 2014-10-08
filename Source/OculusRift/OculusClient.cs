using Microsoft.Xna.Framework;
using System;

namespace OculusRift.Oculus
{
    public class OculusClient
    {
        public float DistK0 { get; private set; }
        public float DistK1 { get; private set; }
        public float DistK2 { get; private set; }
        public float DistK3 { get; private set; }

        public OculusClient()
        {
            OculusRiftDevice.Initialize();
            DistK0 = OculusRiftDevice.DistK0;
            DistK1 = OculusRiftDevice.DistK1;
            DistK2 = OculusRiftDevice.DistK2;
            DistK3 = OculusRiftDevice.DistK3;
        }

        public static Quaternion GetOrientation()
        {
            Quaternion q = new Quaternion();
            OculusRiftDevice.GetOrientation(ref q);
            return q;
        }

        public static Quaternion GetPredictedOrientation()
        {
            Quaternion q = new Quaternion();
            OculusRiftDevice.GetPredictedOrientation(ref q);
            return q;
        }

        public static Vector2 GetScreenResolution()
        {
            Vector2 resolution = new Vector2();
            int h = 0;
            int v = 0;
            OculusRiftDevice.GetScreenResolution(ref h, ref v);
            resolution.X = (float)h;
            resolution.Y = (float)v;
            return resolution;
        }

        public static Vector2 GetScreenSize()
        {
            Vector2 screenSize = new Vector2();
            float h = 0;
            float v = 0;
            OculusRiftDevice.GetScreenSize(ref h, ref v);
            screenSize.X = h;
            screenSize.Y = v;
            return screenSize;
        }

        public static float GetEyeToScreenDistance()
        {
            float d = 0;
            OculusRiftDevice.GetEyeToScreenDistance(ref d);
            return d;
        }

        public static float GetLensSeparationDistance()
        {
            float d = 0;
            OculusRiftDevice.GetLensSeparationDistance(ref d);
            return d;
        }

        public static float GetInterpupillaryDistance()
        {
            float d = 0;
            OculusRiftDevice.GetInterpupillaryDistance(ref d);
            return d;
        }

        public static bool ResetSensorOrientation(int sensorID)
        {
            OculusRiftDevice.ResetSensorOrientation(sensorID);
            return true;
        }

        public static bool SetSensorPredictionTime(int sensorID, float predictionTime)
        {
            OculusRiftDevice.SetSensorPredictionTime(sensorID, predictionTime);
            return true;
        }

    }

    public static class Helpers
    {
        public static Vector3 ToEulerAngles(this Quaternion q)
        {
            Vector3 euler = Vector3.UnitX;
            double sqw = q.W * q.W;
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            double test = q.X * q.Y + q.Z * q.W;
            if (test > 0.499 * unit)
            { // singularity at north pole
                euler.X = (float)(2 * Math.Atan2(q.X, q.W));
                euler.Y = (float)Math.PI / 2;
                euler.Z = 0;
            }
            else if (test < -0.499 * unit)
            { // singularity at south pole
                euler.X = (float)(-2 * Math.Atan2(q.X, q.W));
                euler.Y = (float)-Math.PI / 2;
                euler.Z = 0;
            }
            else
            {
                euler.X = (float)Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z, sqx - sqy - sqz + sqw);
                euler.Y = -(float)Math.Asin(2 * test / unit);
                euler.Z = -(float)Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, -sqx + sqy - sqz + sqw);
            }
            //adjust due to quaternion weirdness from Rift
            //TODO check if persists with the new SDK
            float temp = euler.Y;
            euler.X += (float)Math.PI;
            euler.Y = euler.Z;
            euler.Z = temp;
            return euler;
        }
    }
}
