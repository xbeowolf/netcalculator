namespace Expression
{
	using System;
	using System.Collections.Generic;

	public class ExpressException : ApplicationException
	{
		/// <summary>
		/// Position of error in expression
		/// </summary>
		public int endpos = -1;
		public string lexeme = "";

		public ExpressException() : base() { }
		public ExpressException(string message) : base(message) { }
		public ExpressException(string message, int ep, string s) : base(message) { endpos = ep; lexeme = s; }
		public ExpressException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		public ExpressException(string message, Exception innerException) : base(message, innerException) { }
	}

	public partial class Expression
	{
		/// <summary>
		/// Type of expression lexeme
		/// </summary>
		public enum Lexeme {
			Unknown, End,
			NumDecimal, NumHexadecimal,
			Variable,
			OpComma, OpBoolean, OpRelational, OpBitwise, OpAdditive, OpMultiplicative, OpPower, OpPrefixPostfix, OpBrackets,
			Term,
		};

		delegate double Parser(ref int curpos, ref string lexeme, ref Lexeme type);

		/// <summary>
		/// Pair to represent key and value with full access
		/// </summary>
		/// <typeparam name="T1">Key type</typeparam>
		/// <typeparam name="T2">Value type</typeparam>
		public class KeyValuePair<T1, T2>
		{
			public KeyValuePair(T1 k, T2 v)
			{
				Key = k;
				Value = v;
			}

			public T1 Key;
			public T2 Value;
		};

		/// <summary>
		/// Operators buit-in lexemes
		/// </summary>
		static public KeyValuePair<Lexeme, string[]>[] BuiltinOp =
		{
			// Comma operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpComma, new string[] {
				",",
				"comma",
			}),
			// Boolean operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpBoolean, new string[] {
				"&&", "||", "@@", "->", "<->",
				"and", "or", "xor", "imp", "equ",
			}),
			// Relational operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpRelational, new string[] {
				"<>",
				"<=", ">=", "<", ">", "==", "!=",
			}),
			// Bitwise operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpBitwise, new string[] {
				"&", "|", "@",
			}),
			// Additive operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpAdditive, new string[] {
				"+", "-",
				"plus", "minus",
			}),
			// Multiplicative operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpMultiplicative, new string[] {
				"*", "/", "%",
				"mod",
			}),
			// Power operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpPower, new string[] {
				"^",
			}),
			// Prefix/postfix operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpPrefixPostfix, new string[] {
				"!",
			}),
			// Brackets operators
			new KeyValuePair<Lexeme, string[]>(Lexeme.OpBrackets, new string[] {
				"(", ")",
				"[", "]",
				"{", "}"
			}),
		};

		/// <summary>
		/// Constants and functions built-in lexemes
		/// </summary>
		static public string[] BuiltinTerm = {
			// Constants
			"true", "false",
			"pi", "e", "C",
			"g",
			"KB", "MB", "GB",
			// Trigonometrical functions
			"sin", "cos", "tg", "ctg", "sec", "cosec",
			"Arcsin", "Arccos", "Arctg", "Arcctg", "Arcsec", "Arccosec",
			// Logarithmic and power functions
			"sqrt", "root", "pow", "ln", "lg", "log", "exp",
			// Hyperbolic functions
			"sh", "ch", "th", "cth", "sch", "csch", "Arsh", "Arth", "Arcth",
			// Trigonometric integral functions
			"Si", "Ci", "Ei", "li",
			//"erf", "erfc", "sinc",
			// Gaussian
			"Gauss",
			// Geometrical functions
			"hypot",
			// Statistical functions
			"Amean", "Gmean", "Hmean", "Rmean",
			"variance", "deviation",
			// min, max, doz functions
			"min", "max", "doz",
			// Functions of transformation
			"rad", "deg",
			"int", "ceil", "floor", "round", "abs", "sign",
		};

		/// <summary>
		/// String with expression
		/// </summary>
		public string express;

		/// <summary>
		/// Set of user defined variables
		/// </summary>
		public List<KeyValuePair<string, double>> Variables;

		/// <summary>
		/// Sets an empty expressions and permits up to 10 listed items
		/// </summary>
		public Expression()
		{
			express = "";
			Variables = new List<KeyValuePair<string, double>>();
		}

		/// <summary>
		/// Sets an expressions to given string and permits up to 10 listed items
		/// </summary>
		/// <param name="str">Initial expression</param>
		public Expression(string str)
		{
			express = str;
			Variables = new List<KeyValuePair<string, double>>();
		}

		/// <summary>
		/// Calculate an expression
		/// </summary>
		/// <returns>Result of calculation</returns>
		public string Evaluate(int radix)
		{
			List<double> args = new List<double>();
			int curpos = 0;
			string lexeme;
			Lexeme type;

			extractLexem(ref curpos, out lexeme, out type);
			int n = expressList(args, ref curpos, ref lexeme, ref type);

			string res = "", strarg;
			for (int i = 0; i < n; i++)
			{
				if (radix != 10)
				{
					char c;
					strarg = "";
					for (long d = (long)args[i]; d != 0; d /= radix)
					{
						c = (char)(d % radix);
						c += (char)(c < 10 ? '0' - 0 : 'A' - 10);
						strarg = c + strarg;
					}
					strarg = "0x" + strarg;
				} else {
					strarg = Convert.ToString(args[i]);
				}
				res += strarg;
				if (i < n - 1) res += ", ";
			}
			return res;
		}

		/// <summary>
		/// Evaluates a list of expressions encounted with a commas
		/// </summary>
		/// <param name="args">Array with results of calculation</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Number of elements in list</returns>
		protected int expressList(List<double> args, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			if (type == Lexeme.End) return 0;

			double d;
			int i = 0;

			d = expressBoolean(ref curpos, ref lexeme, ref type);
			if (i < args.Count) args[i] = d;
			else args.Add(d);
			i++;

			while (type == Lexeme.OpComma)
			{
				switch (lexeme)
				{
					case ",":
					case "comma":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = expressBoolean(ref curpos, ref lexeme, ref type);
							if (i < args.Count) args[i] = d;
							else args.Add(d);
							i++;
							break;
						}

					default:
						throw new ExpressException("Invalid comma operator", curpos, lexeme);
				}
			}
			return i;
		}

		/// <summary>
		/// Evaluates expression with boolean operations
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressBoolean(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressRelational(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpBoolean)
			{
				switch (lexeme)
				{
					case "&&":
					case "and":
						{
							extractLexem(ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref curpos, ref lexeme, ref type);
							d = (d != 0 && d2 != 0) ? 1 : 0;
							break;
						}

					case "||":
					case "or":
						{
							extractLexem(ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref curpos, ref lexeme, ref type);
							d = (d != 0 || d2 != 0) ? 1 : 0;
							break;
						}

					case "@@":
					case "xor":
						{
							extractLexem(ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref curpos, ref lexeme, ref type);
							d = ((d != 0) != (d2 != 0)) ? 1 : 0;
							break;
						}

					case "<->":
					case "equ":
						{
							extractLexem(ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref curpos, ref lexeme, ref type);
							d = ((d != 0) == (d2 != 0)) ? 1 : 0;
							break;
						}

					case "->":
					case "imp":
						{
							extractLexem(ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref curpos, ref lexeme, ref type);
							d = (d != 0 && d2 == 0) ? 0 : 1;
							break;
						}

					default:
						throw new ExpressException("Invalid boolean operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evaluates expression with relational operations
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressRelational(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressBitwise(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpRelational)
			{
				switch (lexeme)
				{
					case "<=":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d <= expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case ">=":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d >= expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "<":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d < expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case ">":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d > expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "==":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d == expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "!=":
					case "<>":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = d != expressBitwise(ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					default:
						throw new ExpressException("Invalid relational operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evaluates expression with bitwise operations: & and, | or, @ xor
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressBitwise(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressAdditive(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpBitwise)
			{
				switch (lexeme)
				{
					case "&":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = (int)d & (int)expressAdditive(ref curpos, ref lexeme, ref type);
							break;
						}

					case "|":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = (int)d | (int)expressAdditive(ref curpos, ref lexeme, ref type);
							break;
						}

					case "@":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = (int)d ^ (int)expressAdditive(ref curpos, ref lexeme, ref type);
							break;
						}

					default:
						throw new ExpressException("Invalid bitwise operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evaluates expression with additive operations: + add, - sub
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressAdditive(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressMultiplicative(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpAdditive)
			{
				switch (lexeme)
				{
					case "+":
					case "plus":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d += expressMultiplicative(ref curpos, ref lexeme, ref type);
							break;
						}

					case "-":
					case "minus":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d -= expressMultiplicative(ref curpos, ref lexeme, ref type);
							break;
						}

					default:
						throw new ExpressException("Invalid additive operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evaluates expression with multiplicative operations: * multiplication, / division, % modulus
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressMultiplicative(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressPower(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpMultiplicative)
			{
				switch (lexeme)
				{
					case "*":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d *= expressPower(ref curpos, ref lexeme, ref type);
							break;
						}

					case "/":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d /= expressPower(ref curpos, ref lexeme, ref type);
							break;
						}

					case "%":
					case "mod":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d %= expressPower(ref curpos, ref lexeme, ref type);
							break;
						}

					default:
						throw new ExpressException("Invalid multiplicative operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evaluates expression with power operations: ^ power
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPower(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressPrefix(ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpPower)
			{
				switch (lexeme)
				{
					case "^":
						{
							extractLexem(ref curpos, out lexeme, out type);
							d = Math.Pow(d, expressPrefix(ref curpos, ref lexeme, ref type));
							break;
						}

					default:
						throw new ExpressException("Invalid power operator", curpos, lexeme);
				}
			}
			return d;
		}

		/// <summary>
		/// Evalutes prefix operations
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPrefix(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			switch (lexeme)
			{
				case "!":
					{
						extractLexem(ref curpos, out lexeme, out type);
						d = expressPostfix(ref curpos, ref lexeme, ref type) == 0 ? 1 : 0;
						break;
					}

				default:
					d = expressPostfix(ref curpos, ref lexeme, ref type);
					break;
			}
			return d;
		}

		/// <summary>
		/// Evalutes postfix operations
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPostfix(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressSign(ref curpos, ref lexeme, ref type);
			switch (lexeme)
			{
				case "!":
					{
						extractLexem(ref curpos, out lexeme, out type);
						double n = d;
						if (d - (int)d == 0)
						{
							d = 1;
							for (int i = 1; i <= n; d *= i, i++) { }
						}
						else
						{
							double[] num = {
								1.0, 1.0, 1.0, -139.0,
								-571.0, 163879.0, 5246819.0, -534703531.0,
								-4483131259.0, 432261921612371.0, 6232523202521089.0, -25834629665134204969.0,
								-1579029138854919086429.0, 746590869962651602203151.0, 1511513601028097903631961.0, -8849272268392873147705987190261.0,
								-142801712490607530608130701097701.0
							};
							double[] den = {
								1.0, 12.0, 288.0, 51840.0,
								2488320.0, 209018880.0, 75246796800.0, 902961561600.0,
								86684309913600.0, 514904800886784000.0, 86504006548979712000.0, 13494625021640835072000.0,
								9716130015581401251840000.0, 116593560186976815022080000.0, 2798245444487443560529920000.0, 299692087104605205332754432000000.0,
								57540880724084199423888850944000000.0
							};
							d = Math.Sqrt(2 * Math.PI * n) * Math.Pow(n / Math.E, n);
							double s = 0;
							for (int i = 0; i < 17; i++)
							{
								s += num[i]/den[i]*Math.Pow(n, -i);
							}
							d *= s;
						}
						break;
					}
			}
			return d;
		}

		/// <summary>
		/// Evaluates a term with a sign
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressSign(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			int sign = +1;
			switch (lexeme)
			{
				case "+":
					{
						extractLexem(ref curpos, out lexeme, out type);
						sign = +1;
						break;
					}

				case "-":
					{
						extractLexem(ref curpos, out lexeme, out type);
						sign = -1;
						break;
					}
			}
			return sign * expressFactor(ref curpos, ref lexeme, ref type);
		}

		/// <summary>
		/// Evaluates expression with a brackets
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressFactor(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>(1);
			expressFactor(args, ref curpos, ref lexeme, ref type);
			return args[0];
		}

		/// <summary>
		/// Evaluates expression with a brackets
		/// </summary>
		/// <param name="args">Array with results of calculation</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected int expressFactor(List<double> args, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			int i = 0;

			switch (lexeme)
			{
				case "(":
					{
						extractLexem(ref curpos, out lexeme, out type);
						i = expressList(args, ref curpos, ref lexeme, ref type);
						if (lexeme == ")") extractLexem(ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \")\" is expected", curpos, lexeme);
						break;
					}

				case "[":
					{
						extractLexem(ref curpos, out lexeme, out type);
						i = expressList(args, ref curpos, ref lexeme, ref type);
						if (lexeme == "]") extractLexem(ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \"]\" is expected", curpos, lexeme);
						break;
					}

				case "{":
					{
						extractLexem(ref curpos, out lexeme, out type);
						i = expressList(args, ref curpos, ref lexeme, ref type);
						if (lexeme == "}") extractLexem(ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \"}\" is expected", curpos, lexeme);
						break;
					}

				default:
					d = expressLexem(ref curpos, ref lexeme, ref type);
					if (i < args.Count) args[i] = d;
					else args.Add(d);
					i++;
					break;
			}
			return i;
		}

		/// <summary>
		/// Evaluates an term: it can be floating or a function call
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressLexem(ref int curpos, ref string lexeme, ref Lexeme type)
		{
			int curpos0 = curpos;
			string lexeme0 = lexeme;
			Lexeme type0 = type;
			extractLexem(ref curpos, out lexeme, out type);
			switch (type0)
			{
				case Lexeme.NumDecimal:
					return Convert.ToDouble(lexeme0.Replace('.', ','));
				case Lexeme.NumHexadecimal:
					return ParseHex(lexeme0);

				case Lexeme.Variable:
					foreach (KeyValuePair<string, double> v in Variables)
						if (v.Key == lexeme0) return v.Value;
					throw new ExpressException("Invalid variable", curpos0, lexeme0);

				case Lexeme.Term:
					return expressTerm(lexeme0, ref curpos, ref lexeme, ref type);

				case Lexeme.OpComma:
					throw new ExpressException("Unexpected comma operator", curpos0, lexeme0);
				case Lexeme.OpBoolean:
					throw new ExpressException("Unexpected boolean operator", curpos0, lexeme0);
				case Lexeme.OpRelational:
					throw new ExpressException("Unexpected relational operator", curpos0, lexeme0);
				case Lexeme.OpBitwise:
					throw new ExpressException("Unexpected bitwise operator", curpos0, lexeme0);
				case Lexeme.OpAdditive:
					throw new ExpressException("Unexpected additive operator", curpos0, lexeme0);
				case Lexeme.OpMultiplicative:
					throw new ExpressException("Unexpected multiplicative operator", curpos0, lexeme0);
				case Lexeme.OpPower:
					throw new ExpressException("Unexpected power operator", curpos0, lexeme0);
				case Lexeme.OpPrefixPostfix:
					throw new ExpressException("Unexpected prefix or posfix operator", curpos0, lexeme0);
				case Lexeme.OpBrackets:
					throw new ExpressException("Unexpected brackets operator", curpos0, lexeme0);

				case Lexeme.Unknown:
					throw new ExpressException("Undefined lexeme, constant or function, or floating decimal wanted here", curpos0, lexeme0);
				case Lexeme.End:
				default:
					throw new ExpressException("Unexpected end of expression");
			}
		}

		/// <summary>
		/// Evaluates expression on a given term
		/// </summary>
		/// <param name="term">Name of builtin function or constant for wich expression woll be interpreted as arguments</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressTerm(string term, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			// Express builtin terms
			switch (term)
			{
				// Constants

				case "true":
					{
						return 1;
					}

				case "false":
					{
						return 0;
					}

				case "pi":
				case "M_PI":
					{
						return Math.PI;
					}

				case "e":
				case "M_E":
					{
						return Math.E;
					}

				case "C":
				case "M_C":
					{
						return 0.5772156649015328606065120900824024310422;
					}

				case "g":
					{
						return 9.80665;
					}

				case "KB":
					{
						return 1024;
					}

				case "MB":
					{
						return 1024 * 1024;
					}

				case "GB":
					{
						return 1024 * 1024 * 1024;
					}

				// Trigonometrical functions

				case "sin":
					{
						return Math.Sin(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "cos":
					{
						return Math.Cos(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "tg":
				case "tan":
					{
						return Math.Tan(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "ctg":
				case "cot":
					{
						return 1 / Math.Tan(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "sec":
					{
						return 1 / Math.Cos(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "cosec":
				case "csc":
					{
						return 1 / Math.Sin(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arcsin":
				case "Asin":
					{
						return Math.Asin(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arccos":
				case "Acos":
					{
						return Math.Acos(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arctg":
				case "Atan":
					{
						return Math.Atan(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arcctg":
				case "Acot":
					{
						return Math.Atan(1 / expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arcsec":
				case "Asec":
					{
						return Math.Acos(1 / expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arccosec":
				case "Acsc":
					{
						return Math.Asin(1 / expressFactor(ref curpos, ref lexeme, ref type));
					}

				// Logarithmic and sedate functions

				case "sqrt":
					{
						return Math.Sqrt(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "root":
					{
						List<double> args = new List<double>();
						int n = expressFactor(args, ref curpos, ref lexeme, ref type);
						if (n < 2) args.Add(2);
						return Math.Pow(args[0], 1 / args[1]);
					}

				case "pow":
					{
						List<double> args = new List<double>();
						int n = expressFactor(args, ref curpos, ref lexeme, ref type);
						if (n < 2) args.Add(2);
						return Math.Pow(args[0], args[1]);
					}

				case "ln":
					{
						return Math.Log(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "lg":
					{
						return Math.Log10(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "log":
					{
						List<double> args = new List<double>();
						int n = expressFactor(args, ref curpos, ref lexeme, ref type);
						if (n < 2) args.Add(Math.E);
						return Math.Log(args[0]) / Math.Log(args[1]);
					}

				case "exp":
					{
						return Math.Exp(expressFactor(ref curpos, ref lexeme, ref type));
					}

				// Hyperbolic functions

				case "sh":
				case "Sinh":
					{
						return Math.Sinh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "ch":
				case "Cosh":
					{
						return Math.Cosh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "th":
				case "Tanh":
					{
						return Math.Tanh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "cth":
					{
						return 1 / Math.Tanh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "sch":
					{
						return 1 / Math.Cosh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "csch":
					{
						return 1 / Math.Sinh(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "Arsh":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);
						return Math.Log(x + Math.Sqrt(x * x + 1));
					}

				case "Arth":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);
						if (x <= -1 || x >= 1)
						{
							throw new ExpressException("Argument of Arth must be |x| >= 1");
						}
						return Math.Log((1 + x) / (1 - x)) / 2;
					}

				case "Arcth":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);
						if (x >= -1 || x <= 1)
						{
							throw new ExpressException("Argument of Arcth must be |x| <= 1");
						}
						return Math.Log((1 - x) / (1 + x)) / 2;
					}

				// Trigonometric integral functions

				case "Si":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);

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

				case "Ci":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);

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

				case "Ei":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);

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

				case "li":
					{
						double x = expressFactor(ref curpos, ref lexeme, ref type);

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

				case "hypot":
					{
						double d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						foreach (double v in args)
						{
							d += v * v;
						}
						return Math.Sqrt(d);
					}

				// Statistical functions

				case "Amean":
					{
						double d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count == 0) throw new ExpressException("Function \"Arithmetic mean\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							d += v;
						}
						return d / args.Count;
					}

				case "Gmean":
					{
						double d = 1;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count == 0) throw new ExpressException("Function \"Geometric mean\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							d *= v;
						}
						return Math.Pow(d, 1.0 / args.Count);
					}

				case "Hmean":
					{
						double d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count == 0) throw new ExpressException("Function \"Harmonic mean\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							d += 1/v;
						}
						return args.Count / d;
					}

				case "Rmean":
					{
						double d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count == 0) throw new ExpressException("Function \"Root mean square\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							d += v * v;
						}
						return Math.Sqrt(d / args.Count);
					}

				case "variance":
					{
						double A = 0, d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
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

				case "deviation":
				case "sigma":
					{
						double A = 0, d = 0;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count == 0) throw new ExpressException("Function \"Standard deviation\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							A += v;
						}
						A /= args.Count;
						foreach (double v in args)
						{
							d += (v - A)*(v - A);
						}
						return Math.Sqrt(d / args.Count);
					}

				// min, max, doz functions

				case "min":
					{
						double d;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count > 0) d = args[0];
						else throw new ExpressException("Function \"Minimal\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							if (v < d) d = v;
						}
						return d;
					}

				case "max":
					{
						double d;
						List<double> args = new List<double>();
						expressFactor(args, ref curpos, ref lexeme, ref type);
						if (args.Count > 0) d = args[0];
						else throw new ExpressException("Function \"Maximal\" expects at last 1 argument", curpos, lexeme);
						foreach (double v in args)
						{
							if (v > d) d = v;
						}
						return d;
					}

				case "doz":
					{
						List<double> args = new List<double>();
						int n = expressFactor(args, ref curpos, ref lexeme, ref type);
						if (n != 2) throw new ExpressException("Function expects 2 argument", curpos, lexeme);
						return args[0] >= args[1] ? args[0] - args[1] : 0;
					}

				// Functions of transformation

				case "rad":
					{
						return (long)expressFactor(ref curpos, ref lexeme, ref type) * Math.PI / 180;
					}

				case "deg":
					{
						return (long)expressFactor(ref curpos, ref lexeme, ref type) / Math.PI * 180;
					}

				case "int":
				case "trunc":
					{
						return (long)expressFactor(ref curpos, ref lexeme, ref type); ;
					}

				case "ceil":
					{
						return Math.Ceiling(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "floor":
					{
						return Math.Floor(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "round":
					{
						List<double> args = new List<double>();
						int n = expressFactor(args, ref curpos, ref lexeme, ref type);
						if (n < 2) args.Add(0);
						if (args[1] >= 0)
						{
							return Math.Round(args[0], (int)args[1]);
						}
						else
						{
							double p = Math.Pow(10, -(int)args[1]);
							return Math.Round(args[0]/p)*p;
						}
					}

				case "abs":
					{
						return Math.Abs(expressFactor(ref curpos, ref lexeme, ref type));
					}

				case "sign":
					{
						return Math.Sign(expressFactor(ref curpos, ref lexeme, ref type));
					}
			}

			// Express variables
			foreach (KeyValuePair<string, double> v in Variables)
				if (v.Key == term) return v.Value;

			throw new ExpressException("Unexpected term", curpos, lexeme);
		}

		/// <summary>
		/// Extracts new lexeme and adjust current position
		/// </summary>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		public void extractLexem(ref int curpos, out string lexeme, out Lexeme type)
		{
			// Skip space
			while (curpos < express.Length && express[curpos] == ' ') curpos++;

			// Test on end of line
			if (curpos >= express.Length)
			{
				lexeme = "";
				type = Lexeme.End;
				return;
			}

			// Extract operator
			for( int i = 0; i < BuiltinOp.Length; i++ )
			{
				for( int j = 0; j < BuiltinOp[i].Value.Length; j++ )
				{
					if (String.Compare(express, curpos, BuiltinOp[i].Value[j], 0, BuiltinOp[i].Value[j].Length) == 0)
					{
						curpos += BuiltinOp[i].Value[j].Length;
						lexeme = BuiltinOp[i].Value[j];
						type = BuiltinOp[i].Key;
						return;
					}
				}
			}

			// Extract builtin term
			for (int i = 0; i < BuiltinTerm.Length; i++)
			{
				if (String.Compare(express, curpos, BuiltinTerm[i], 0, BuiltinTerm[i].Length) == 0 &&
						!(curpos + BuiltinTerm[i].Length < express.Length && isAlphaNumericChar(express[curpos + BuiltinTerm[i].Length])))
				{
					curpos += BuiltinTerm[i].Length;
					type = Lexeme.Term;
					lexeme = BuiltinTerm[i];
					return;
				}
			}

			// Extract variable
			foreach (KeyValuePair<string, double> v in Variables)
			{
				if (String.Compare(express, curpos, v.Key, 0, v.Key.Length) == 0 &&
					!(curpos + v.Key.Length < express.Length && isAlphaNumericChar(express[curpos + v.Key.Length])))
				{
					curpos += v.Key.Length;
					type = Lexeme.Variable;
					lexeme = v.Key;
					return;
				}
			}

			// Extract decimal, hexadecimal or unknown word
			int start = curpos;
			bool isfunc = isAlphaChar(express[curpos]);
			if (isfunc) // extract unknown word
			{
				while (curpos < express.Length && isAlphaNumericChar(express[curpos])) curpos++;
				type = Lexeme.Unknown;
			}
			else // extract decimal or hexadecimal
			{
				if (String.Compare(express, curpos, "0x", 0, 2, true) == 0) // extract hexadecimal
				{
					curpos += 2;
					bool complete = curpos >= express.Length;
					while (!complete)
					{
						if ((express[curpos] >= '0' && express[curpos] <= '9') || (express[curpos] >= 'A' && express[curpos] <= 'F') || (express[curpos] >= 'a' && express[curpos] <= 'f'))
						{
							curpos++;
						}
						else complete = true;
						complete |= curpos >= express.Length;
					}
					type = Lexeme.NumHexadecimal;
				}
				else // extract decimal
				{
					bool hasM = false, hasP = false, hasE = false, hasS = false;
					bool complete = curpos >= express.Length;
					while (!complete)
					{
						if (express[curpos] >= '0' && express[curpos] <= '9')
						{
							hasM = true;
							if (hasE) hasP = true;
							curpos++;
						}
						else
							if ((express[curpos] == ',' || express[curpos] == '.') &&
									curpos + 1 < express.Length &&
									(express[curpos + 1] >= '0' && express[curpos + 1] <= '9'))
							{
								if (!hasM || hasP || hasE || hasS) throw new ExpressException("Unexpected \",\" in floating-point value", curpos + 1, ",");
								else curpos++;
								hasP = true;
							}
							else
								if (express[curpos] == 'E' || express[curpos] == 'e')
								{
									if (!hasM || hasE) throw new ExpressException("Unexpected \"E\" in floating-point value", curpos + 1, "E");
									else curpos++;
									hasE = true;
								}
								else
									if (express[curpos] == '+' || express[curpos] == '-')
									{
										if (!hasE || hasS) complete = true;
										else curpos++;
										hasS = true;
									}
									else complete = true;
						complete |= curpos >= express.Length;
					}
					type = Lexeme.NumDecimal;
				}
			}
			lexeme = express.Substring(start, curpos - start);
			return;
		}

		/// <summary>
		/// Checkup a symbol on a letter set
		/// </summary>
		/// <param name="ch">Checked symbol</param>
		/// <returns>true if it's a function name symbol</returns>
		static internal bool isAlphaChar(char ch)
		{
			return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '_';
		}

		/// <summary>
		/// Checkup a symbol on a letter set or it's a digit
		/// </summary>
		/// <param name="ch">Checked symbol</param>
		/// <returns>true if it's a function name symbol</returns>
		static internal bool isAlphaNumericChar(char ch)
		{
			return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '_' || (ch >= '0' && ch <= '9');
		}

		static internal double ParseHex(string hex)
		{
			double d = 0;
			for (int i = 2; i < hex.Length; i++)
			{
				if (hex[i] >= '0' && hex[i] <= '9')
				{
					d *= 16;
					d += hex[i] - '0' + 0;
				}
				else if (hex[i] >= 'A' && hex[i] <= 'F')
				{
					d *= 16;
					d += hex[i] - 'A' + 10;
				}
				else if (hex[i] >= 'a' && hex[i] <= 'f')
				{
					d *= 16;
					d += hex[i] - 'a' + 10;
				}
				else break;
			}
			return d;
		}
	}
}