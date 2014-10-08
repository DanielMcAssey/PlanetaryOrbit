using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace PlanetaryOrbit.Core.Physics
{
    public struct Vector3D : IEquatable<Vector3D>
    {
        public Vector3D(double value)
            : this()
        {
            X = Y = Z = value;
        }
        public Vector3D(Vector3 source)
            : this()
        {
            X = source.X;
            Y = source.Y;
            Z = source.Z;
        }
        public Vector3D(double x, double y, double z)
            : this()
        {

            X = x;
            Y = y;
            Z = z;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public override string ToString()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", new object[] { X.ToString(currentCulture), Y.ToString(currentCulture), Z.ToString(currentCulture) });
        }
        public static void Subtract(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D();
            result.X = v1.X - v2.X;
            result.Y = v1.Y - v2.Y;
            result.Z = v1.Z - v2.Z;
        }
        public static Vector3D Subtract(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X - v2.X;
            vector.Y = v1.Y - v2.Y;
            vector.Z = v1.Z - v2.Z;
            return vector;
        }
        public static void SmoothStep(ref Vector3D v1, ref Vector3D v2, double amount, out Vector3D result)
        {
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            result = new Vector3D();
            result.X = v1.X + ((v2.X - v1.X) * amount);
            result.Y = v1.Y + ((v2.Y - v1.Y) * amount);
            result.Z = v1.Z + ((v2.Z - v1.Z) * amount);
        }
        public static Vector3D SmoothStep(Vector3D v1, Vector3D v2, double amount)
        {
            Vector3D vector = new Vector3D();
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            vector.X = v1.X + ((v2.X - v1.X) * amount);
            vector.Y = v1.Y + ((v2.Y - v1.Y) * amount);
            vector.Z = v1.Z + ((v2.Z - v1.Z) * amount);
            return vector;
        }
        public static void Reflect(ref Vector3D vector, ref Vector3D normal, out Vector3D result)
        {
            double num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            result = new Vector3D();
            result.X = vector.X - ((2f * num) * normal.X);
            result.Y = vector.Y - ((2f * num) * normal.Y);
            result.Z = vector.Z - ((2f * num) * normal.Z);
        }
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            Vector3D vector2 = new Vector3D();

            double num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            vector2.X = vector.X - ((2f * num) * normal.X);
            vector2.Y = vector.Y - ((2f * num) * normal.Y);
            vector2.Z = vector.Z - ((2f * num) * normal.Z);
            return vector2;
        }
        public static Vector3D operator -(Vector3D value)
        {
            Vector3D vector = new Vector3D();
            vector.X = -value.X;
            vector.Y = -value.Y;
            vector.Z = -value.Z;
            return vector;
        }
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X - v2.X;
            vector.Y = v1.Y - v2.Y;
            vector.Z = v1.Z - v2.Z;
            return vector;
        }
        public static Vector3D operator *(double scaleFactor, Vector3D value)
        {
            Vector3D vector = new Vector3D();
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            vector.Z = value.Z * scaleFactor;
            return vector;
        }
        public static Vector3D operator *(Vector3D value, double scaleFactor)
        {
            Vector3D vector = new Vector3D();
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            vector.Z = value.Z * scaleFactor;
            return vector;
        }
        public static Vector3D operator *(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X * v2.X;
            vector.Y = v1.Y * v2.Y;
            vector.Z = v1.Z * v2.Z;
            return vector;
        }
        public static bool operator !=(Vector3D v1, Vector3D v2)
        {
            if ((v1.X == v2.X) && (v1.Y == v2.Y))
            {
                return !(v1.Z == v2.Z);
            }
            return true;
        }
        public static bool operator ==(Vector3D v1, Vector3D v2)
        {
            return (((v1.X == v2.X) && (v1.Y == v2.Y)) && (v1.Z == v2.Z));
        }
        public static Vector3D operator /(Vector3D value, double divider)
        {
            Vector3D vector = new Vector3D();
            double num = 1f / divider;
            vector.X = value.X * num;
            vector.Y = value.Y * num;
            vector.Z = value.Z * num;
            return vector;
        }
        public static Vector3D operator /(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X / v2.X;
            vector.Y = v1.Y / v2.Y;
            vector.Z = v1.Z / v2.Z;
            return vector;
        }
        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X + v2.X;
            vector.Y = v1.Y + v2.Y;
            vector.Z = v1.Z + v2.Z;
            return vector;
        }
        public static implicit operator Vector3(Vector3D v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }
        public static void Normalize(ref Vector3D value, out Vector3D result)
        {

            double num2 = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
            double num = 1f / ((double)Math.Sqrt((double)num2));
            result = new Vector3D();
            result.X = value.X * num;
            result.Y = value.Y * num;
            result.Z = value.Z * num;
        }
        public static Vector3D Normalize(Vector3D value)
        {
            var vector = new Vector3D();
            var num2 = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
            var num = 1f / (Math.Sqrt(num2));
            vector.X = value.X * num;
            vector.Y = value.Y * num;
            vector.Z = value.Z * num;
            return vector;
        }
        public void Normalize()
        {
            var num2 = ((X * X) + (Y * Y)) + (Z * Z);
            var num = 1f / Math.Sqrt(num2);
            X *= num;
            Y *= num;
            Z *= num;
        }
        public static void Negate(ref Vector3D value, out Vector3D result)
        {
            result = new Vector3D();
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }
        public static Vector3D Negate(Vector3D value)
        {
            Vector3D vector = new Vector3D();
            vector.X = -value.X;
            vector.Y = -value.Y;
            vector.Z = -value.Z;
            return vector;
        }
        public static void Multiply(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D();
            result.X = v1.X * v2.X;
            result.Y = v1.Y * v2.Y;
            result.Z = v1.Z * v2.Z;
        }
        public static void Multiply(ref Vector3D v1, double scaleFactor, out Vector3D result)
        {
            result = new Vector3D();
            result.X = v1.X * scaleFactor;
            result.Y = v1.Y * scaleFactor;
            result.Z = v1.Z * scaleFactor;
        }
        public static Vector3D Multiply(Vector3D v1, double scaleFactor)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X * scaleFactor;
            vector.Y = v1.Y * scaleFactor;
            vector.Z = v1.Z * scaleFactor;
            return vector;
        }
        public static Vector3D Multiply(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X * v2.X;
            vector.Y = v1.Y * v2.Y;
            vector.Z = v1.Z * v2.Z;
            return vector;
        }
        public static void Min(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D();
            result.X = (v1.X < v2.X) ? v1.X : v2.X;
            result.Y = (v1.Y < v2.Y) ? v1.Y : v2.Y;
            result.Z = (v1.Z < v2.Z) ? v1.Z : v2.Z;
        }
        public static Vector3D Min(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = (v1.X < v2.X) ? v1.X : v2.X;
            vector.Y = (v1.Y < v2.Y) ? v1.Y : v2.Y;
            vector.Z = (v1.Z < v2.Z) ? v1.Z : v2.Z;
            return vector;
        }
        public static void Max(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D();
            result.X = (v1.X > v2.X) ? v1.X : v2.X;
            result.Y = (v1.Y > v2.Y) ? v1.Y : v2.Y;
            result.Z = (v1.Z > v2.Z) ? v1.Z : v2.Z;
        }
        public static Vector3D Max(Vector3D v1, Vector3D v2)
        {
            Vector3D vector = new Vector3D();
            vector.X = (v1.X > v2.X) ? v1.X : v2.X;
            vector.Y = (v1.Y > v2.Y) ? v1.Y : v2.Y;
            vector.Z = (v1.Z > v2.Z) ? v1.Z : v2.Z;
            return vector;
        }
        public static void Lerp(ref Vector3D v1, ref Vector3D v2, double amount, out Vector3D result)
        {
            result = new Vector3D();
            result.X = v1.X + ((v2.X - v1.X) * amount);
            result.Y = v1.Y + ((v2.Y - v1.Y) * amount);
            result.Z = v1.Z + ((v2.Z - v1.Z) * amount);
        }
        public static Vector3D Lerp(Vector3D v1, Vector3D v2, double amount)
        {
            Vector3D vector = new Vector3D();
            vector.X = v1.X + ((v2.X - v1.X) * amount);
            vector.Y = v1.Y + ((v2.Y - v1.Y) * amount);
            vector.Z = v1.Z + ((v2.Z - v1.Z) * amount);
            return vector;
        }
        public double LengthSquared()
        {
            return (((X * X) + (Y * Y)) + (Z * Z));
        }
        public double Length()
        {
            return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }
        public static void Hermite(ref Vector3D v1, ref Vector3D tangent1, ref Vector3D v2, ref Vector3D tangent2, double amount, out Vector3D result)
        {
            double num = amount * amount;
            double num2 = amount * num;
            double num6 = ((2f * num2) - (3f * num)) + 1f;
            double num5 = (-2f * num2) + (3f * num);
            double num4 = (num2 - (2f * num)) + amount;
            double num3 = num2 - num;
            result = new Vector3D();
            result.X = (((v1.X * num6) + (v2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
            result.Y = (((v1.Y * num6) + (v2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
            result.Z = (((v1.Z * num6) + (v2.Z * num5)) + (tangent1.Z * num4)) + (tangent2.Z * num3);
        }
        public static Vector3D Hermite(Vector3D v1, Vector3D tangent1, Vector3D v2, Vector3D tangent2, double amount)
        {
            Vector3D vector = new Vector3D();
            double num = amount * amount;
            double num2 = amount * num;
            double num6 = ((2f * num2) - (3f * num)) + 1f;
            double num5 = (-2f * num2) + (3f * num);
            double num4 = (num2 - (2f * num)) + amount;
            double num3 = num2 - num;
            vector.X = (((v1.X * num6) + (v2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
            vector.Y = (((v1.Y * num6) + (v2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
            vector.Z = (((v1.Z * num6) + (v2.Z * num5)) + (tangent1.Z * num4)) + (tangent2.Z * num3);
            return vector;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }
        #region Equals
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector3D)
            {
                flag = Equals((Vector3D)obj);
            }
            return flag;
        }


        public bool Equals(Vector3D other)
        {
            return (((X == other.X) && (Y == other.Y)) && (Z == other.Z));
        }
        #endregion

        #region Dot
        public static void Dot(ref Vector3D v1, ref Vector3D v2, out double result)
        {
            result = ((v1.X * v2.X) + (v1.Y * v2.Y)) + (v1.Z * v2.Z);
        }

        public static double Dot(Vector3D v1, Vector3D v2)
        {
            return (((v1.X * v2.X) + (v1.Y * v2.Y)) + (v1.Z * v2.Z));
        }
        #endregion

        #region Divide
        public static void Divide(ref Vector3D v1, double v2, out Vector3D result)
        {
            double num = 1f / v2;
            result = new Vector3D(v1.X * num, v1.Y * num, v1.Z * num);
        }


        public static void Divide(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3D Divide(Vector3D v1, double v2)
        {
            var num = 1f / v2;
            return new Vector3D(v1.X * num, v1.Y * num, v1.Z * num);
        }

        public static Vector3D Divide(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }
        #endregion

        #region Distance
        public static void DistanceSquared(ref Vector3D v1, ref Vector3D v2, out double result)
        {
            double numX = v1.X - v2.X;
            double numY = v1.Y - v2.Y;
            double numZ = v1.Z - v2.Z;
            result = ((numX * numX) + (numY * numY)) + (numZ * numZ);
        }

        public static double DistanceSquared(Vector3D v1, Vector3D v2)
        {
            double numX = v1.X - v2.X;
            double numY = v1.Y - v2.Y;
            double numZ = v1.Z - v2.Z;
            return (((numX * numX) + (numY * numY)) + (numZ * numZ));
        }

        public static void Distance(ref Vector3D v1, ref Vector3D v2, out double result)
        {
            double numX = v1.X - v2.X;
            double numY = v1.Y - v2.Y;
            double numZ = v1.Z - v2.Z;
            double numSum = ((numX * numX) + (numY * numY)) + (numZ * numZ);
            result = (double)Math.Sqrt((double)numSum);
        }

        public static double Distance(Vector3D v1, Vector3D v2)
        {
            double numX = v1.X - v2.X;
            double numY = v1.Y - v2.Y;
            double numZ = v1.Z - v2.Z;
            double numSum = ((numX * numX) + (numY * numY)) + (numZ * numZ);
            return (double)Math.Sqrt((double)numSum);
        }
        #endregion

        #region Cross
        public static void Cross(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
        {
            double numX = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
            double numY = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
            double numZ = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
            result = new Vector3D(numX, numY, numZ);
        }


        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            Vector3D vector = new Vector3D();
            vector.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
            vector.Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
            vector.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
            return vector;
        }
        #endregion

        #region Clamp
        public static void Clamp(ref Vector3D v1, ref Vector3D min, ref Vector3D max, out Vector3D result)
        {
            double x = v1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            double y = v1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            double z = v1.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;

            result = new Vector3D(x, y, z);
        }

        public static Vector3D Clamp(Vector3D v1, Vector3D min, Vector3D max)
        {
            Vector3D vector = new Vector3D();
            double x = v1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            double y = v1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            double z = v1.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;
            vector.X = x;
            vector.Y = y;
            vector.Z = z;
            return vector;
        }
        #endregion

        #region CatmullRom
        public static void CatmullRom(ref Vector3D v1, ref Vector3D v2, ref Vector3D v3, ref Vector3D v4, double amount, out Vector3D result)
        {
            double num = amount * amount;
            double num2 = amount * num;
            result = new Vector3D();
            result.X = 0.5f * ((((2f * v2.X) + ((-v1.X + v3.X) * amount)) + (((((2f * v1.X) - (5f * v2.X)) + (4f * v3.X)) - v4.X) * num)) + ((((-v1.X + (3f * v2.X)) - (3f * v3.X)) + v4.X) * num2));
            result.Y = 0.5f * ((((2f * v2.Y) + ((-v1.Y + v3.Y) * amount)) + (((((2f * v1.Y) - (5f * v2.Y)) + (4f * v3.Y)) - v4.Y) * num)) + ((((-v1.Y + (3f * v2.Y)) - (3f * v3.Y)) + v4.Y) * num2));
            result.Z = 0.5f * ((((2f * v2.Z) + ((-v1.Z + v3.Z) * amount)) + (((((2f * v1.Z) - (5f * v2.Z)) + (4f * v3.Z)) - v4.Z) * num)) + ((((-v1.Z + (3f * v2.Z)) - (3f * v3.Z)) + v4.Z) * num2));
        }

        public static Vector3D CatmullRom(Vector3D v1, Vector3D v2, Vector3D v3, Vector3D v4, double amount)
        {
            Vector3D vector = new Vector3D();
            double num = amount * amount;
            double num2 = amount * num;
            vector.X = 0.5f * ((((2f * v2.X) + ((-v1.X + v3.X) * amount)) + (((((2f * v1.X) - (5f * v2.X)) + (4f * v3.X)) - v4.X) * num)) + ((((-v1.X + (3f * v2.X)) - (3f * v3.X)) + v4.X) * num2));
            vector.Y = 0.5f * ((((2f * v2.Y) + ((-v1.Y + v3.Y) * amount)) + (((((2f * v1.Y) - (5f * v2.Y)) + (4f * v3.Y)) - v4.Y) * num)) + ((((-v1.Y + (3f * v2.Y)) - (3f * v3.Y)) + v4.Y) * num2));
            vector.Z = 0.5f * ((((2f * v2.Z) + ((-v1.Z + v3.Z) * amount)) + (((((2f * v1.Z) - (5f * v2.Z)) + (4f * v3.Z)) - v4.Z) * num)) + ((((-v1.Z + (3f * v2.Z)) - (3f * v3.Z)) + v4.Z) * num2));
            return vector;
        }
        #endregion

        #region Barycentric
        public static void Barycentric(ref Vector3D v1, ref Vector3D v2, ref Vector3D v3, double amount1, double amount2, out Vector3D result)
        {
            result = new Vector3D();
            result.X = (v1.X + (amount1 * (v2.X - v1.X))) + (amount2 * (v3.X - v1.X));
            result.Y = (v1.Y + (amount1 * (v2.Y - v1.Y))) + (amount2 * (v3.Y - v1.Y));
            result.Z = (v1.Z + (amount1 * (v2.Z - v1.Z))) + (amount2 * (v3.Z - v1.Z));
        }


        public static Vector3D Barycentric(Vector3D v1, Vector3D v2, Vector3D v3, double amount1, double amount2)
        {
            Vector3D vector = new Vector3D();
            vector.X = (v1.X + (amount1 * (v2.X - v1.X))) + (amount2 * (v3.X - v1.X));
            vector.Y = (v1.Y + (amount1 * (v2.Y - v1.Y))) + (amount2 * (v3.Y - v1.Y));
            vector.Z = (v1.Z + (amount1 * (v2.Z - v1.Z))) + (amount2 * (v3.Z - v1.Z));
            return vector;
        }
        #endregion

        #region Add
        public static void Add(ref Vector3D v1, ref Vector3D v2, out Vector3D result)
        {
            result = new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }


        public static Vector3D Add(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        #endregion

        static Vector3D()
        {
            _unitX = new Vector3D(Vector3.UnitX);
            _unitY = new Vector3D(Vector3.UnitY);
            _unitZ = new Vector3D(Vector3.UnitZ);

            _backward = new Vector3D(Vector3.Backward);
            _down = new Vector3D(Vector3.Down);
            _forward = new Vector3D(Vector3.Forward);
            _left = new Vector3D(Vector3.Left);
            _right = new Vector3D(Vector3.Right);
            _one = new Vector3D(Vector3.One);
            _right = new Vector3D(Vector3.Right);
            _up = new Vector3D(Vector3.Up);
            _zero = new Vector3D(Vector3.Zero);
        }

        public static Vector3D Backward
        {
            get
            {
                return _backward;
            }
        }

        public static Vector3D Down
        {
            get
            {
                return _down;
            }
        }

        public static Vector3D Forward
        {
            get
            {
                return _forward;
            }
        }
        public static Vector3D Left
        {
            get
            {
                return _left;
            }
        }

        public static Vector3D One
        {
            get
            {
                return _one;
            }
        }

        public static Vector3D Right
        {
            get
            {
                return _right;
            }
        }

        public static Vector3D UnitX
        {
            get
            {
                return _unitX;
            }
        }

        public static Vector3D UnitY
        {
            get
            {
                return _unitY;
            }
        }

        public static Vector3D UnitZ
        {
            get
            {
                return _unitZ;
            }
        }

        public static Vector3D Up
        {
            get
            {
                return _up;
            }
        }

        public static Vector3D Zero
        {
            get
            {
                return _zero;
            }
        }


        private static Vector3D _backward;
        private static Vector3D _down;
        private static Vector3D _forward;
        private static Vector3D _left;
        private static Vector3D _one;
        private static Vector3D _right;
        private static Vector3D _unitX;
        private static Vector3D _unitY;
        private static Vector3D _unitZ;
        private static Vector3D _up;
        private static Vector3D _zero;
    }
}
