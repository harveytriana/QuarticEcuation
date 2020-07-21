// ================================================
// Harvey Triana / Visionary-SAS
// This returns an array of the Complexes.
// Of course, a real number is a subset of the 
// complexes. When 'i' is zero, take it like a Real
// ================================================
using System;
using System.Numerics;

namespace QuarticEcuation
{
    public class CubicSolver
    {
        const double CLOSETOZERO = 1.0E-16;

        public Complex[] Solve(double a, double b, double c, double d)
        {
            if (a == 0) {
                return null;
            }

            var result = new Complex[3];

            double j, k, l, p, q, t;
            double z1 = 0, z2 = 0, z3 = 0, i1 = 0, i2 = 0, i3 = 0;

            // constants
            j = b / a;
            k = c / a;
            l = d / a;
            p = -(j * j / 3.0) + k;
            q = (2.0 / 27.0 * j * j * j) - (j * k / 3.0) + l;
            t = q * q / 4.0 + p * p * p / 27.0;

            // force to zero if it is very close to zero
            if (Math.Abs(t) < CLOSETOZERO) {
                t = 0;
            }
            // There are three cases according to the value of t
            if (t > 0) {// one real, two complexs
                // real root
                z1 = CubeRoot(-q / 2.0 + Math.Sqrt(t))
                   + CubeRoot(-q / 2.0 - Math.Sqrt(t));
                // two complex roots
                z2 = -z1 / 2.0;
                z3 = z2; // conjugated
                // imaginary
                var i = Math.Sqrt(Math.Abs(Math.Pow(z1 / 2.0, 2.0) + q / z1));
                i1 = 0;
                i2 = i;
                i3 = -i;
            }
            if (t == 0) {// three real roots, at least two equal
                z1 = 2.0 * CubeRoot(-q / 2.0);
                z2 = -z1 / 2.0 + Math.Sqrt(Math.Pow(z1 / 2.0, 2.0) + q / z1);
                z3 = -z1 / 2.0 - Math.Sqrt(Math.Pow(z1 / 2.0, 2.0) + q / z1);
            }
            if (t < 0) {// three real roots
                var x = -q / 2.0;
                var y = Math.Sqrt(-t); // make t positive
                var angle = Math.Atan(y / x);
                if (q > 0) {// if q > 0 the angle becomes 2 * PI - angle
                    angle =  Math.PI - angle;
                }
                z1 = 2.0 * Math.Sqrt(-p / 3.0) * Math.Cos(angle / 3.0);
                z2 = 2.0 * Math.Sqrt(-p / 3.0) * Math.Cos((angle + 2.0 * Math.PI) / 3.0);
                z3 = 2.0 * Math.Sqrt(-p / 3.0) * Math.Cos((angle + 4.0 * Math.PI) / 3.0);
            }
            result[0] = new Complex(z1 - j / 3.0, i1);
            result[1] = new Complex(z2 - j / 3.0, i2);
            result[2] = new Complex(z3 - j / 3.0, i3);

            return result;
        }

        private double CubeRoot(double number)
        {
            if (number < 0) {
                return -Math.Pow(-number, 1.0 / 3.0);
            }
            return Math.Pow(number, 1.0 / 3.0);
        }
    }
}
