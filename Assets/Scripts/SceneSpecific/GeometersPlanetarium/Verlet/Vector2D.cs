// Type: UnityEngine.Vector2
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Program Files (x86)\Unity\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
    public struct Vector2d
    {
        public const double kEpsilon = 1E-05d;
        public double x;
        public double y;

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
        }

        public Vector2d normalized
        {
            get
            {
                var vector2d = new Vector2d(x, y);
                vector2d.Normalize();
                return vector2d;
            }
        }

        public double magnitude => Mathd.Sqrt(x * x + y * y);

        public double sqrMagnitude => x * x + y * y;

        public static Vector2d zero => new Vector2d(0.0d, 0.0d);

        public static Vector2d one => new Vector2d(1d, 1d);

        public static Vector2d up => new Vector2d(0.0d, 1d);

        public static Vector2d right => new Vector2d(1d, 0.0d);

        public Vector2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2d(Vector3d v)
        {
            return new Vector2d(v.x, v.y);
        }

        public static implicit operator Vector3d(Vector2d v)
        {
            return new Vector3d(v.x, v.y, 0.0d);
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x + b.x, a.y + b.y);
        }

        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x - b.x, a.y - b.y);
        }

        public static Vector2d operator -(Vector2d a)
        {
            return new Vector2d(-a.x, -a.y);
        }

        public static Vector2d operator *(Vector2d a, double d)
        {
            return new Vector2d(a.x * d, a.y * d);
        }

        public static Vector2d operator *(float d, Vector2d a)
        {
            return new Vector2d(a.x * d, a.y * d);
        }

        public static Vector2d operator /(Vector2d a, double d)
        {
            return new Vector2d(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2d lhs, Vector2d rhs)
        {
            return SqrMagnitude(lhs - rhs) < 0.0 / 1.0;
        }

        public static bool operator !=(Vector2d lhs, Vector2d rhs)
        {
            return SqrMagnitude(lhs - rhs) >= 0.0 / 1.0;
        }

        public void Set(double new_x, double new_y)
        {
            x = new_x;
            y = new_y;
        }

        public static Vector2d Lerp(Vector2d from, Vector2d to, double t)
        {
            t = Mathd.Clamp01(t);
            return new Vector2d(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
        }

        public static Vector2d MoveTowards(Vector2d current, Vector2d target, double maxDistanceDelta)
        {
            var vector2 = target - current;
            var magnitude = vector2.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0.0d)
                return target;
            return current + vector2 / magnitude * maxDistanceDelta;
        }

        public static Vector2d Scale(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x * b.x, a.y * b.y);
        }

        public void Scale(Vector2d scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public void Normalize()
        {
            var magnitude = this.magnitude;
            if (magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = zero;
        }

        public override string ToString()
        {
            /*
      string fmt = "({0:D1}, {1:D1})";
      object[] objArray = new object[2];
      int index1 = 0;
      // ISSUE: variable of a boxed type
      __Boxed<double> local1 = (ValueType) this.x;
      objArray[index1] = (object) local1;
      int index2 = 1;
      // ISSUE: variable of a boxed type
      __Boxed<double> local2 = (ValueType) this.y;
      objArray[index2] = (object) local2;
      */
            return "not implemented";
        }

        public string ToString(string format)
        {
            /* TODO:
      string fmt = "({0}, {1})";
      object[] objArray = new object[2];
      int index1 = 0;
      string str1 = this.x.ToString(format);
      objArray[index1] = (object) str1;
      int index2 = 1;
      string str2 = this.y.ToString(format);
      objArray[index2] = (object) str2;
      */
            return "not implemented";
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2d))
                return false;
            var vector2d = (Vector2d) other;
            if (x.Equals(vector2d.x))
                return y.Equals(vector2d.y);
            return false;
        }

        public static double Dot(Vector2d lhs, Vector2d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        public static double Angle(Vector2d from, Vector2d to)
        {
            return Mathd.Acos(Mathd.Clamp(Dot(from.normalized, to.normalized), -1d, 1d)) * 57.29578d;
        }

        public static double Distance(Vector2d a, Vector2d b)
        {
            return (a - b).magnitude;
        }

        public static Vector2d ClampMagnitude(Vector2d vector, double maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
                return vector.normalized * maxLength;
            return vector;
        }

        public static double SqrMagnitude(Vector2d a)
        {
            return a.x * a.x + a.y * a.y;
        }

        public double SqrMagnitude()
        {
            return x * x + y * y;
        }

        public static Vector2d Min(Vector2d lhs, Vector2d rhs)
        {
            return new Vector2d(Mathd.Min(lhs.x, rhs.x), Mathd.Min(lhs.y, rhs.y));
        }

        public static Vector2d Max(Vector2d lhs, Vector2d rhs)
        {
            return new Vector2d(Mathd.Max(lhs.x, rhs.x), Mathd.Max(lhs.y, rhs.y));
        }
    }
}