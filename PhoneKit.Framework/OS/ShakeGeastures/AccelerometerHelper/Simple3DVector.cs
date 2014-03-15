/* 
    Copyright (c) 2012 - 2013 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://code.msdn.microsoft.com/wpapps
  
*/
using System;


namespace Microsoft.Phone.Applications.Common
{
    public class Simple3DVector
    {
        /// <summary>
        /// X-axis coordinate
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Y-axis coordinate
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Z-axis coordinate
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// Default constructor
        /// Creates a null vector
        /// </summary>
        public Simple3DVector(){}

        /// <summary>
        /// Cector constructor from 3 double values
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        public Simple3DVector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Cloning constructor
        /// </summary>
        /// <param name="v">Vector to clone</param>
        public Simple3DVector(Simple3DVector v)
        {
            if (v != null)
            {
                X = v.X;
                Y = v.Y;
                Z = v.Z;
            }
        }

        /// <summary>
        /// Override the ToString method to display vector in suitable format:
        /// </summary>
        public override string ToString()
        {
            return (String.Format("({0:0.000},{1:0.000},{2:0.000})", X, Y, Z));
        }

        /// <summary>
        /// Overload (==) operator for 2 vectors
        /// </summary>
        public static bool operator ==(Simple3DVector v1, Simple3DVector v2)
        {
            if (Object.ReferenceEquals(v1, v2))
            { // if both are null, or both are same instance, return true
                return true;
            }
            
            if (((object) v1 == null) || ((object) v2 == null))
            { // if one is null, but not both, return false
                return false;
            }

            return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
        }

        /// <summary>
        /// Overload (!=) operator for 2 vectors
        /// </summary>
        public static bool operator !=(Simple3DVector v1, Simple3DVector v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Override the Object.Equals(object o) method:
        /// </summary>
        public override bool Equals(object o)
        {
            if (o is Simple3DVector)
            {
                return (bool)(this == (Simple3DVector)o);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Override the Object.Equals(object o) method:
        /// </summary>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Overload (+) operator for 2 vectors
        /// </summary>
        public static Simple3DVector operator +(Simple3DVector v1, Simple3DVector v2)
        {
            return new Simple3DVector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Overload (-) operator for 2 vectors
        /// </summary>
        public static Simple3DVector operator -(Simple3DVector v1, Simple3DVector v2)
        {
            return new Simple3DVector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Overload (*) operator for 2 vectors
        /// </summary>
        public static Simple3DVector operator *(Simple3DVector v1, Simple3DVector v2)
        {
            return new Simple3DVector(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        /// <summary>
        /// Overload (*) operator for a vector and double (scaling)
        /// </summary>
        public static Simple3DVector operator *(Simple3DVector v, double d)
        {
            return new Simple3DVector(d * v.X, d * v.Y, d * v.Z);
        }

        /// <summary>
        /// Overload (/) operator for a vector and double (scaling)
        /// </summary>
        public static Simple3DVector operator /(Simple3DVector v, double d)
        {
            return new Simple3DVector(v.X / d, v.Y / d, v.Z / d);
        }

        private double? _magnitude = null;

        /// <summary>
        /// Get Magnitude of vector
        /// </summary>
        public double Magnitude
        {
            get
            {
                if (_magnitude == null)
                {
                    _magnitude = Math.Sqrt(X * X + Y * Y + Z * Z);
                }
                return _magnitude.Value;
            }
        }
    }
}
