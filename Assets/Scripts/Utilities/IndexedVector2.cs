using System;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    public struct IndexedVector2
    {
        public Vector2 vector;
        public int index;

        public float x
        {
            get => vector.x;
            set => vector.x = value;
        }

        public float y
        {
            get => vector.y;
            set => vector.y = value;
        }

        public IndexedVector2(float x, float y)
        {
            vector = new Vector2(x, y);
            index = 0;
        }
        
        public IndexedVector2(float x, float y, int index)
        {
            vector = new Vector2(x, y);
            this.index = index;
        }
        
        public IndexedVector2(Vector2 vector, int index)
        {
            this.vector = new Vector2(vector.x, vector.y);
            this.index = index;
        }
        
        public IndexedVector2(Vector2 vector)
        {
            this.vector = new Vector2(vector.x, vector.y);
            index = 0;
        }
        
        public IndexedVector2(Vector3 vector, int index)
        {
            this.vector = new Vector2(vector.x, vector.y);
            this.index = index;
        }
        
        public IndexedVector2(Vector3 vector)
        {
            this.vector = new Vector2(vector.x, vector.y);
            this.index = 0;
        }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return this.x;
                if (index == 1)
                    return this.y;
                throw new IndexOutOfRangeException("Invalid Vector2 index!");
            }
            set
            {
                if (index != 0)
                {
                    if (index != 1)
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                    this.y = value;
                }
                else
                    this.x = value;
            }
        }

        /// <summary>
        ///   <para>Set x and y components of an existing Vector2.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void Set(float newX, float newY)
        {
            this.x = newX;
            this.y = newY;
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static IndexedVector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new IndexedVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static IndexedVector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return new IndexedVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        /// <summary>
        ///   <para>Moves a point current towards target.</para>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        public static IndexedVector2 MoveTowards(
            IndexedVector2 current,
            IndexedVector2 target,
            float maxDistanceDelta)
        {
            float num1 = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
            if ((double) num3 == 0.0 || (double) maxDistanceDelta >= 0.0 &&
                (double) num3 <= (double) maxDistanceDelta * (double) maxDistanceDelta)
                return target;
            float num4 = (float) Math.Sqrt((double) num3);
            return new IndexedVector2(current.x + num1 / num4 * maxDistanceDelta,
                current.y + num2 / num4 * maxDistanceDelta);
        }

        /// <summary>
        ///   <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static IndexedVector2 Scale(Vector2 a, Vector2 b)
        {
            return new IndexedVector2(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        ///   <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector2 scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
        }

        /// <summary>
        ///   <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        public void Normalize()
        {
            float magnitude = this.magnitude;
            if ((double) magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vector2.zero;
        }

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                Vector2 vector2 = new Vector2(this.x, this.y);
                vector2.Normalize();
                return vector2;
            }
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return $"Vector: {vector}, Index: {index}";
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        /// <summary>
        ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            if (!(other is Vector2))
                return false;
            return this.Equals((IndexedVector2) other);
        }

        public bool Equals(IndexedVector2 other)
        {
            return (double) this.x == (double) other.x && (double) this.y == (double) other.y;
        }

        /// <summary>
        ///   <para>Reflects a vector off the vector defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static IndexedVector2 Reflect(Vector2 inDirection, Vector2 inNormal)
        {
            float num = -2f * Vector2.Dot(inNormal, inDirection);
            return new IndexedVector2(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        /// <summary>
        ///   <para>Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.</para>
        /// </summary>
        /// <param name="inDirection">The input direction.</param>
        /// <returns>
        ///   <para>The perpendicular direction.</para>
        /// </returns>
        public static IndexedVector2 Perpendicular(IndexedVector2 inDirection)
        {
            return new IndexedVector2(-inDirection.y, inDirection.x, inDirection.index);
        }

        /// <summary>
        ///   <para>Dot Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static float Dot(IndexedVector2 lhs, IndexedVector2 rhs)
        {
            return (float) ((double) lhs.x * (double) rhs.x + (double) lhs.y * (double) rhs.y);
        }

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude
        {
            get { return (float) Math.Sqrt((double) this.x * (double) this.x + (double) this.y * (double) this.y); }
        }

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public float sqrMagnitude
        {
            get { return (float) ((double) this.x * (double) this.x + (double) this.y * (double) this.y); }
        }

        /// <summary>
        ///   <para>Returns the unsigned angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        public static float Angle(IndexedVector2 from, IndexedVector2 to)
        {
            float num = (float) Math.Sqrt((double) from.sqrMagnitude * (double) to.sqrMagnitude);
            if ((double) num < 1.00000000362749E-15)
                return 0.0f;
            return (float) Math.Acos((double) Mathf.Clamp(Vector2.Dot(from.vector, to.vector) / num, -1f, 1f)) *
                   57.29578f;
        }

        /// <summary>
        ///   <para>Returns the signed angle in degrees between from and to.</para>
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        public static float SignedAngle(IndexedVector2 from, IndexedVector2 to)
        {
            return Angle(from, to) *
                   Mathf.Sign((float) ((double) from.x * (double) to.y - (double) from.y * (double) to.x));
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float Distance(IndexedVector2 a, IndexedVector2 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        public static IndexedVector2 ClampMagnitude(IndexedVector2 vector, float maxLength)
        {
            float sqrMagnitude = vector.sqrMagnitude;
            if ((double) sqrMagnitude <= (double) maxLength * (double) maxLength)
                return vector;
            float num1 = (float) Math.Sqrt((double) sqrMagnitude);
            float num2 = vector.x / num1;
            float num3 = vector.y / num1;
            return new IndexedVector2(num2 * maxLength, num3 * maxLength, vector.index);
        }

        public static float SqrMagnitude(IndexedVector2 a)
        {
            return (float) ((double) a.x * (double) a.x + (double) a.y * (double) a.y);
        }

        public float SqrMagnitude()
        {
            return (float) ((double) this.x * (double) this.x + (double) this.y * (double) this.y);
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static IndexedVector2 Min(IndexedVector2 lhs, IndexedVector2 rhs)
        {
            return new IndexedVector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), lhs.index);
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static IndexedVector2 Max(IndexedVector2 lhs, IndexedVector2 rhs)
        {
            return new IndexedVector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), lhs.index);
        }

        //    ADDITION
        public static IndexedVector2 operator +(IndexedVector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x + b.x, a.y + b.y, a.index);
        }
        
        public static IndexedVector2 operator +(IndexedVector2 a, Vector2 b)
        {
            return new IndexedVector2(a.x + b.x, a.y + b.y, a.index);
        }
        
        public static IndexedVector2 operator +(Vector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x + b.x, a.y + b.y, b.index);
        }
        
        public static IndexedVector2 operator +(IndexedVector2 a, Vector3 b)
        {
            return new IndexedVector2(a.x + b.x, a.y + b.y, a.index);
        }
        
        public static IndexedVector2 operator +(Vector3 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x + b.x, a.y + b.y, b.index);
        }

        //    SUBTRACTION
        
        public static IndexedVector2 operator -(IndexedVector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x - b.x, a.y - b.y, a.index);
        }
        
        public static IndexedVector2 operator -(IndexedVector2 a, Vector2 b)
        {
            return new IndexedVector2(a.x - b.x, a.y - b.y, a.index);
        }
        
        public static IndexedVector2 operator -(Vector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x - b.x, a.y - b.y, b.index);
        }
        
        public static IndexedVector2 operator -(IndexedVector2 a, Vector3 b)
        {
            return new IndexedVector2(a.x - b.x, a.y - b.y, a.index);
        }
        
        public static IndexedVector2 operator -(Vector3 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x - b.x, a.y - b.y, b.index);
        }
        
        //    MULTIPLICATION

        public static IndexedVector2 operator *(IndexedVector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x * b.x, a.y * b.y, a.index);
        }

        public static IndexedVector2 operator /(IndexedVector2 a, IndexedVector2 b)
        {
            return new IndexedVector2(a.x / b.x, a.y / b.y, a.index);
        }

        public static IndexedVector2 operator -(IndexedVector2 a)
        {
            return new IndexedVector2(-a.x, -a.y, a.index);
        }

        public static IndexedVector2 operator *(IndexedVector2 a, float d)
        {
            return new IndexedVector2(a.x * d, a.y * d, a.index);
        }

        public static IndexedVector2 operator *(float d, IndexedVector2 a)
        {
            return new IndexedVector2(a.x * d, a.y * d, a.index);
        }

        public static IndexedVector2 operator /(IndexedVector2 a, float d)
        {
            return new IndexedVector2(a.x / d, a.y / d, a.index);
        }

        public static bool operator ==(IndexedVector2 lhs, IndexedVector2 rhs)
        {
            float num1 = lhs.x - rhs.x;
            float num2 = lhs.y - rhs.y;
            return (double) num1 * (double) num1 + (double) num2 * (double) num2 < 9.99999943962493E-11;
        }

        public static bool operator !=(IndexedVector2 lhs, IndexedVector2 rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator IndexedVector2(Vector2 v)
        {
            return new IndexedVector2(v.x, v.y);
        }
        
        public static implicit operator IndexedVector2(Vector3 v)
        {
            return new IndexedVector2(v.x, v.y);
        }

        public static implicit operator Vector3(IndexedVector2 v)
        {
            return new Vector3(v.x, v.y, 0.0f);
        }
        
        public static implicit operator Vector2(IndexedVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
    }
}