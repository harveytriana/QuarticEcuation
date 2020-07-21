// ================================================
// Harvey Triana / Visionary-SAS
// Quartic Equations
// References:
// Paper: A Note on the Solution of Quartic Equations
// By Herbert E. Salzer Quartic Equations
// Am. Math Society Proceedings, 1959.
// Modified here for support all cases.
// Others
// https://en.wikipedia.org/wiki/Quadratic_equation
// ================================================
using System;
using System.Collections.Generic;
using System.Numerics;

namespace QuarticEcuation
{
    public class QuarticSolver
    {
        readonly CubicSolver _cubicSolver = new CubicSolver();

        /// <summary>
        /// Solve:  ax^4 + bx^3 + cx^2 + dx + e
        /// </summary>
        /// <returns>Complex[4]</returns>
        public Complex[] Solve(double a, double b, double c, double d, double e)
        {
            if (a == 0) {
                return null;
            }

            double r1, r2, r3, r4, i1, i2, i3, i4;

            double A = b / a;
            double B = c / a;
            double C = d / a;
            double D = e / a;

            // First, get the resolvent cubic equation, x^3 + C2x^2 + C1x + C0 = 0 (C sufix)
            double C3 = 1;
            double C2 = -B;
            double C1 = A * C - 4.0 * D;
            double C0 = D * (4.0 * B - A * A) - C * C;
            double m, n, x1;

            // solve cubic
            var cubicRoots = _cubicSolver.Solve(C3, C2, C1, C0);

            if (IsZero(cubicRoots[0].Imaginary)) {
                x1 = cubicRoots[0].Real;
            } else if (IsZero(cubicRoots[1].Imaginary)) {
                x1 = cubicRoots[1].Real;
            } else if (IsZero(cubicRoots[2].Imaginary)) {
                x1 = cubicRoots[2].Real;
            } else {
                return null;
            }

            m = Math.Sqrt(Math.Abs(A * A / 4.0 - B + x1));
            if (IsZero(m)) {
                m = 0;
                n = Math.Sqrt(x1 * x1 / 4.0 - D);
            } else {
                n = (A * x1 - 2.0 * C) / (4.0 * m);
            }
            double alpha = A * A / 2.0 - x1 - B;
            double beta = 4.0 * n - A * m;
            double t1 = alpha + beta;
            double t2 = alpha - beta;
            double gamma = Math.Sqrt(Math.Abs(t1));
            double delta = Math.Sqrt(Math.Abs(t2));

            if (t1 < 0 && t2 >= 0) {// gamma is imag and delta is real
                r1 = (-A / 2.0 + m) / 2.0; // imag and r2=Conjugate(r1)
                i1 = gamma / 2.0;
                r2 = r1;
                i2 = -i1;
                r3 = (-A / 2.0 - m + delta) / 2.0; // real
                i3 = 0;
                r4 = (-A / 2.0 - m - delta) / 2.0; // real
                i4 = 0;
            } else if (t1 < 0 && t2 < 0) { // gamma and delta are imag
                r1 = (-A / 2.0 + m) / 2.0;
                i1 = gamma / 2.0;
                r2 = r1;
                i2 = -i1;
                r3 = (-A / 2.0 - m) / 2.0;
                i3 = delta / 2.0;
                r4 = r3;
                i4 = -i3;
            } else if (t1 >= 0 && t2 < 0) {// gamma is real and delta is imag
                r1 = (-A / 2.0 + m + gamma) / 2.0;
                i1 = 0;
                r2 = (-A / 2.0 + m - gamma) / 2.0;
                i2 = 0;
                r3 = (-A / 2.0 + m) / 2.0;
                i3 = delta / 2.0;
                r4 = r3;
                i4 = -i3;
            } else {// gamma and delta are reals, then all roots are reals
                r1 = (-A / 2.0 + m + gamma) / 2.0;
                i1 = 0;
                r2 = (-A / 2.0 - m + delta) / 2.0;
                i2 = 0;
                r3 = (-A / 2.0 + m - gamma) / 2.0;
                i3 = 0;
                r4 = (-A / 2.0 - m - delta) / 2.0;
                i4 = 0;
            }
            var result = new List<Complex>
            {
                new Complex(r1, i1),
                new Complex(r2, i2),
                new Complex(r3, i3),
                new Complex(r4, i4)
            };
            return result.ToArray();
        }

        #region Utilities
        const double CLOSETOZERO = 1.0E-6;

        public bool IsZero(double number)
        {
            return Math.Abs(number) < CLOSETOZERO;
        }

        public Complex QuarticFunction(double a, double b, double c, double d, double e, Complex root)
        {
            return a * Complex.Pow(root, 4) + b * Complex.Pow(root, 3) + c * Complex.Pow(root, 2) + d * root + e;
        }

        // for real world numbers
        public double QuarticFunction(double a, double b, double c, double d, double e, double root)
        {
            return a * Math.Pow(root, 4) + b * Math.Pow(root, 3) + c * Math.Pow(root, 2) + d * root + e;
        }

        public bool IsSolved(double a, double b, double c, double d, double e, Complex[] roots)
        {
            if (roots == null) {
                return false;
            }
            foreach (var r in roots) {
                var z = QuarticFunction(a, b, c, d, e, r);
                if ((IsZero(z.Real) && IsZero(z.Imaginary)) == false) {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
