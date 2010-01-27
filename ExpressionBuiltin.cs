namespace Expression
{
	using System;
	using System.Collections.Generic;

	public partial class Expression
	{
		public KeyValuePair<string, Parser>[] BuiltinParsers()
		{
			KeyValuePair<string, Parser>[] parsers = {
				// Constants
				new KeyValuePair<string, Parser>("true", parser_true),
				new KeyValuePair<string, Parser>("false", parser_false),
				new KeyValuePair<string, Parser>("pi", parser_pi),
				new KeyValuePair<string, Parser>("M_PI", parser_pi),
				new KeyValuePair<string, Parser>("e", parser_e),
				new KeyValuePair<string, Parser>("M_E", parser_e),
				new KeyValuePair<string, Parser>("gamma", parser_EulerMascheroni),
				new KeyValuePair<string, Parser>("EM", parser_EulerMascheroni),
				new KeyValuePair<string, Parser>("g", parser_g),
				new KeyValuePair<string, Parser>("atm", parser_atm),
				new KeyValuePair<string, Parser>("mu0", parser_mu0),
				new KeyValuePair<string, Parser>("hbar", parser_hbar),
				new KeyValuePair<string, Parser>("NA", parser_NA),
				new KeyValuePair<string, Parser>("KB", parser_KB),
				new KeyValuePair<string, Parser>("MB", parser_MB),
				new KeyValuePair<string, Parser>("GB", parser_GB),
				// Trigonometrical functions
				new KeyValuePair<string, Parser>("sin", parser_sin),
				new KeyValuePair<string, Parser>("cos", parser_cos),
				new KeyValuePair<string, Parser>("tg", parser_tg),
				new KeyValuePair<string, Parser>("tan", parser_tg),
				new KeyValuePair<string, Parser>("ctg", parser_ctg),
				new KeyValuePair<string, Parser>("cot", parser_ctg),
				new KeyValuePair<string, Parser>("sec", parser_sec),
				new KeyValuePair<string, Parser>("cosec", parser_cosec),
				new KeyValuePair<string, Parser>("csc", parser_cosec),
				new KeyValuePair<string, Parser>("Arcsin", parser_Arcsin),
				new KeyValuePair<string, Parser>("Asin", parser_Arcsin),
				new KeyValuePair<string, Parser>("Arccos", parser_Arccos),
				new KeyValuePair<string, Parser>("Acos", parser_Arccos),
				new KeyValuePair<string, Parser>("Arctg", parser_Arctg),
				new KeyValuePair<string, Parser>("Atan", parser_Arctg),
				new KeyValuePair<string, Parser>("Arcctg", parser_Arcctg),
				new KeyValuePair<string, Parser>("Acot", parser_Arcctg),
				new KeyValuePair<string, Parser>("Arcsec", parser_Arcsec),
				new KeyValuePair<string, Parser>("Asec", parser_Arcsec),
				new KeyValuePair<string, Parser>("Arccosec", parser_Arccosec),
				new KeyValuePair<string, Parser>("Acsc", parser_Arccosec),
				// Logarithmic and power functions
				new KeyValuePair<string, Parser>("sqrt", parser_sqrt),
				new KeyValuePair<string, Parser>("root", parser_root),
				new KeyValuePair<string, Parser>("pow", parser_pow),
				new KeyValuePair<string, Parser>("ln", parser_ln),
				new KeyValuePair<string, Parser>("lg", parser_lg),
				new KeyValuePair<string, Parser>("log", parser_log),
				new KeyValuePair<string, Parser>("exp", parser_exp),
				// Hyperbolic functions
				new KeyValuePair<string, Parser>("sh", parser_sh),
				new KeyValuePair<string, Parser>("Sinh", parser_sh),
				new KeyValuePair<string, Parser>("ch", parser_ch),
				new KeyValuePair<string, Parser>("Cosh", parser_ch),
				new KeyValuePair<string, Parser>("th", parser_th),
				new KeyValuePair<string, Parser>("Tanh", parser_th),
				new KeyValuePair<string, Parser>("cth", parser_cth),
				new KeyValuePair<string, Parser>("sch", parser_sch),
				new KeyValuePair<string, Parser>("csch", parser_csch),
				new KeyValuePair<string, Parser>("Arsh", parser_Arsh),
				new KeyValuePair<string, Parser>("Arth", parser_Arth),
				new KeyValuePair<string, Parser>("Arcth", parser_Arcth),
				// Trigonometric integral functions
				new KeyValuePair<string, Parser>("Si", parser_Si),
				new KeyValuePair<string, Parser>("Ci", parser_Ci),
				new KeyValuePair<string, Parser>("Ei", parser_Ei),
				new KeyValuePair<string, Parser>("li", parser_li),
				// Geometrical functions
				new KeyValuePair<string, Parser>("hypot", parser_hypot),
				// Statistical functions
				new KeyValuePair<string, Parser>("Amean", parser_Amean),
				new KeyValuePair<string, Parser>("Gmean", parser_Gmean),
				new KeyValuePair<string, Parser>("Hmean", parser_Hmean),
				new KeyValuePair<string, Parser>("Rmean", parser_Rmean),
				new KeyValuePair<string, Parser>("variance", parser_variance),
				new KeyValuePair<string, Parser>("deviation", parser_deviation),
				new KeyValuePair<string, Parser>("sigma", parser_deviation),
				// min, max, doz functions
				new KeyValuePair<string, Parser>("min", parser_min),
				new KeyValuePair<string, Parser>("max", parser_max),
				new KeyValuePair<string, Parser>("doz", parser_doz),
				// Functions of transformation
				new KeyValuePair<string, Parser>("rad", parser_rad),
				new KeyValuePair<string, Parser>("deg", parser_deg),
				new KeyValuePair<string, Parser>("int", parser_int),
				new KeyValuePair<string, Parser>("trunc", parser_int),
				new KeyValuePair<string, Parser>("ceil", parser_ceil),
				new KeyValuePair<string, Parser>("floor", parser_floor),
				new KeyValuePair<string, Parser>("round", parser_round),
				new KeyValuePair<string, Parser>("abs", parser_abs),
				new KeyValuePair<string, Parser>("sign", parser_sign),
			};
			return parsers;
		}

		// Constants

		protected double parser_true(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1;
		}

		protected double parser_false(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 0;
		}

		protected double parser_pi(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.PI;
		}

		protected double parser_e(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.E;
		}

		protected double parser_EulerMascheroni(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 0.5772156649015328606065120900824024310422;
		}

		protected double parser_g(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 9.80665;
		}

		protected double parser_atm(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 101325;
		}

		protected double parser_mu0(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 4e-7 * Math.PI;
		}

		protected double parser_hbar(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1.05457162853e-34;
		}

		protected double parser_NA(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 6.0221417930e23;
		}

		protected double parser_KB(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1024;
		}

		protected double parser_MB(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1024 * 1024;
		}

		protected double parser_GB(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1024 * 1024 * 1024;
		}

		// Trigonometrical functions

		protected double parser_sin(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Sin(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_cos(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Cos(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_tg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Tan(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_ctg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Tan(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_sec(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Cos(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_cosec(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Sin(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arcsin(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Asin(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arccos(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Acos(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arctg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Atan(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arcctg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Atan(1 / expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arcsec(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Acos(1 / expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arccosec(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Asin(1 / expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		// Logarithmic and power functions

		protected double parser_sqrt(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Sqrt(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_root(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (n < 2) args.Add(2);
			return Math.Pow(args[0], 1 / args[1]);
		}

		protected double parser_pow(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (n < 2) args.Add(2);
			return Math.Pow(args[0], args[1]);
		}

		protected double parser_ln(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Log(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_lg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Log10(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_log(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (n < 2) args.Add(Math.E);
			return Math.Log(args[0]) / Math.Log(args[1]);
		}

		protected double parser_exp(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Exp(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		// Hyperbolic functions

		protected double parser_sh(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Sinh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_ch(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Cosh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_th(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Tanh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_cth(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Tanh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_sch(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Cosh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_csch(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return 1 / Math.Sinh(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_Arsh(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);
			return Math.Log(x + Math.Sqrt(x * x + 1));
		}

		protected double parser_Arth(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);
			if (x <= -1 || x >= 1)
			{
				throw new ExpressException("Argument of Arth must be |x| >= 1");
			}
			return Math.Log((1 + x) / (1 - x)) / 2;
		}

		protected double parser_Arcth(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);
			if (x >= -1 || x <= 1)
			{
				throw new ExpressException("Argument of Arcth must be |x| <= 1");
			}
			return Math.Log((1 - x) / (1 + x)) / 2;
		}

		// Trigonometric integral functions

		protected double parser_Si(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);

			double x2 = -x * x,
					d = 0, s = x, l;
			double i = 1;
			do
			{
				l = d;
				d += s / i;
				i += 2;
				s *= x2 / (i - 1) / i;
			} while (l != d);
			return d;
		}

		protected double parser_Ci(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);

			double x2 = -x * x,
					d = 0.5772156649015328606065120900824024310422 + Math.Log(x), s = -x2 / 2, l;
			double i = 2;
			do
			{
				l = d;
				d += s / i;
				i += 2;
				s *= x2 / (i - 1) / i;
			} while (l != d);
			return d;
		}

		protected double parser_Ei(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);

			double d = 0.5772156649015328606065120900824024310422 + Math.Log(Math.Abs(x)), s = x, l;
			double i = 1;
			do
			{
				l = d;
				d += s / i;
				i++;
				s *= x / i;
			} while (l != d);
			return d;
		}

		protected double parser_li(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double x = expressFactor(ref express, ref curpos, ref lexeme, ref type);

			double d = 0.5772156649015328606065120900824024310422 + Math.Log(Math.Abs(x)), s = x, l;
			double i = 1;
			do
			{
				l = d;
				d += s / i;
				i++;
				s *= x / i;
			} while (l != d);
			return Math.Log(d);
		}

		// Geometrical functions

		protected double parser_hypot(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			foreach (double v in args)
			{
				d += v * v;
			}
			return Math.Sqrt(d);
		}

		// Statistical functions

		protected double parser_Amean(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Arithmetic mean\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				d += v;
			}
			return d / args.Count;
		}

		protected double parser_Gmean(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = 1;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Geometric mean\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				d *= v;
			}
			return Math.Pow(d, 1.0 / args.Count);
		}

		protected double parser_Hmean(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Harmonic mean\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				d += 1 / v;
			}
			return args.Count / d;
		}

		protected double parser_Rmean(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Root mean square\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				d += v * v;
			}
			return Math.Sqrt(d / args.Count);
		}

		protected double parser_variance(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double A = 0, d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Variance\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				A += v;
			}
			A /= args.Count;
			foreach (double v in args)
			{
				d += (v - A) * (v - A);
			}
			return d / args.Count;
		}

		protected double parser_deviation(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double A = 0, d = 0;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count == 0) throw new ExpressException("Function \"Standard deviation\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				A += v;
			}
			A /= args.Count;
			foreach (double v in args)
			{
				d += (v - A) * (v - A);
			}
			return Math.Sqrt(d / args.Count);
		}

		// min, max, doz functions

		protected double parser_min(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count > 0) d = args[0];
			else throw new ExpressException("Function \"Minimal\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				if (v < d) d = v;
			}
			return d;
		}

		protected double parser_max(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			List<double> args = new List<double>();
			expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (args.Count > 0) d = args[0];
			else throw new ExpressException("Function \"Maximal\" expects at last 1 argument", curpos, lexeme);
			foreach (double v in args)
			{
				if (v > d) d = v;
			}
			return d;
		}

		protected double parser_doz(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (n != 2) throw new ExpressException("Function expects 2 argument", curpos, lexeme);
			return args[0] >= args[1] ? args[0] - args[1] : 0;
		}

		// Functions of transformation

		protected double parser_rad(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return (long)expressFactor(ref express, ref curpos, ref lexeme, ref type) * Math.PI / 180;
		}

		protected double parser_deg(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return (long)expressFactor(ref express, ref curpos, ref lexeme, ref type) / Math.PI * 180;
		}

		protected double parser_int(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return (long)expressFactor(ref express, ref curpos, ref lexeme, ref type); ;
		}

		protected double parser_ceil(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Ceiling(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_floor(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Floor(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_round(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			if (n < 2) args.Add(0);
			if (args[1] >= 0)
			{
				return Math.Round(args[0], (int)args[1]);
			}
			else
			{
				double p = Math.Pow(10, -(int)args[1]);
				return Math.Round(args[0] / p) * p;
			}
		}

		protected double parser_abs(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Abs(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}

		protected double parser_sign(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			return Math.Sign(expressFactor(ref express, ref curpos, ref lexeme, ref type));
		}
	} // Expression
}
