// ==============================
// Harvey Triana / Visionary-SAS
// ==============================
using System;
using System.Numerics;
using System.Windows.Forms;

namespace QuarticEcuation
{
    public partial class FormMain : Form
    {
        readonly QuarticSolver _solver = new QuarticSolver();

        Complex[] _roots;

        public FormMain()
        {
            InitializeComponent();

            buttonExecute.Click += (s, e) => Execute();
            buttonValidate.Click += (s, e) => ValidateResult();

            // samples
            SampleInput(1, -7, 13, 23, -78);
        }

        private void Execute()
        {
            _roots = _solver.Solve(A, B, C, D, E);
            if (_roots != null) {
                X1 = ComplexString(_roots[0]);
                X2 = ComplexString(_roots[1]);
                X3 = ComplexString(_roots[2]);
                X4 = ComplexString(_roots[3]);
            } else {
                X1 = "null";
                X2 = "null";
                X3 = "null";
                X4 = "null";
            }
        }

        private void SampleInput(double a, double b, double c, double d, double e)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        double A {
            get => double.Parse(textBoxA.Text);
            set => textBoxA.Text = value.ToString();
        }

        double B {
            get => double.Parse(textBoxB.Text);
            set => textBoxB.Text = value.ToString();
        }

        double C {
            get => double.Parse(textBoxC.Text);
            set => textBoxC.Text = value.ToString();
        }

        double D {
            get => double.Parse(textBoxD.Text);
            set => textBoxD.Text = value.ToString();
        }

        double E {
            get => double.Parse(textBoxE.Text);
            set => textBoxE.Text = value.ToString();
        }

        string X1 {
            get => textBoxX1.Text;
            set => textBoxX1.Text = value;
        }
        string X2 {
            get => textBoxX2.Text;
            set => textBoxX2.Text = value;
        }
        string X3 {
            get => textBoxX3.Text;
            set => textBoxX3.Text = value;
        }
        string X4 {
            get => textBoxX4.Text;
            set => textBoxX4.Text = value;
        }

        private bool ContainsNumber(string text)
        {
            if (!string.IsNullOrEmpty(text)) {
                foreach (var n in new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "9" }) {
                    if (text.Contains(n)) {
                        return true;
                    }
                }
            }
            return false;
        }

        string ComplexString(Complex z, int decimals = 4)
        {
            string s = $"N{decimals}";
            if (_solver.IsZero(z.Imaginary)) {
                return z.Real.ToString(s);
            }
            if (_solver.IsZero(z.Real)) {
                return z.Imaginary.ToString(s) + "i";
            }
            return (z.Real == 0 ? "" : z.Real.ToString(s)) +
                   (z.Imaginary >= 0 ? " + " : " - ") + Math.Abs(z.Imaginary).ToString(s) + "i";
        }

        void ValidateResult()
        {
            var s = string.Empty;
            if (ContainsNumber(X1) && ContainsNumber(X2) && ContainsNumber(X3) && ContainsNumber(X4)) {
                var a = A;
                var b = B;
                var c = C;
                var d = D;
                var e = E;
                s += "Validating ƒ(x) = 0";
                s += $"\nƒ({X1}) = {ComplexString(_solver.QuarticFunction(a, b, c, d, e, _roots[0]))}";
                s += $"\nƒ({X2}) = {ComplexString(_solver.QuarticFunction(a, b, c, d, e, _roots[1]))}";
                s += $"\nƒ({X3}) = {ComplexString(_solver.QuarticFunction(a, b, c, d, e, _roots[2]))}";
                s += $"\nƒ({X4}) = {ComplexString(_solver.QuarticFunction(a, b, c, d, e, _roots[3]))}";
            } else {
                s = "Missing result";
            }
            MessageBox.Show(s);
        }

    }
}
