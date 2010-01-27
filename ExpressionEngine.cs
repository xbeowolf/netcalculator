namespace Expression
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Exceptions in expressions
	/// </summary>
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

	/// <summary>
	/// Type of expression lexeme
	/// </summary>
	public enum Lexeme
	{
		Unknown, End,
		NumDecimal, NumHexadecimal,
		Parser, Variable, Function,
		OpComma, OpBoolean, OpRelational, OpBitwise, OpAdditive, OpMultiplicative, OpPower, OpPrefixPostfix, OpBrackets,
	};

	public delegate double Parser(ref string express, ref int curpos, ref string lexeme, ref Lexeme type);

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

	public partial class Expression
	{
		/// <summary>
		/// Operators buit-in lexemes
		/// </summary>
		static public readonly KeyValuePair<Lexeme, string[]>[] BuiltinOp =
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
		/// Set of builtin and user defined parsers
		/// </summary>
		public List<KeyValuePair<string, Parser>> Parsers;

		/// <summary>
		/// Set of user defined variables
		/// </summary>
		public List<KeyValuePair<string, double>> Variables;

		/// <summary>
		/// Set of user defined functions
		/// </summary>
		public List<KeyValuePair<string, string[]>> Functions;

		/// <summary>
		/// Creates new empty object
		/// </summary>
		public Expression()
		{
			Parsers = new List<KeyValuePair<string, Parser>>(BuiltinParsers());
			Variables = new List<KeyValuePair<string, double>>();
			Functions = new List<KeyValuePair<string, string[]>>();
		}

		/// <summary>
		/// Creates new object with users data
		/// </summary>
		/// <param name="str">Given array of user defined variables</param>
		/// <param name="funks">Given array of user defined functions</param>
		public Expression(List<KeyValuePair<string, double>> vars, List<KeyValuePair<string, string[]>> funks)
		{
			Parsers = new List<KeyValuePair<string, Parser>>();
			Variables = vars;
			Functions = funks;
		}

		/// <summary>
		/// Calculate an expression
		/// </summary>
		/// <returns>Result of calculation</returns>
		public string Evaluate(string express, int radix)
		{
			List<double> args = new List<double>();
			int n = Evaluate(args, express);

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
					strarg = args[i].ToString();
				}
				res += strarg;
				if (i < n - 1) res += ", ";
			}
			return res;
		}

		public double Evaluate(string express)
		{
			List<double> args = new List<double>();
			int n = Evaluate(args, express);
			return n > 0 ? args[0] : 0;
		}

		public int Evaluate(List<double> args, string express)
		{
			int curpos = 0;
			string lexeme;
			Lexeme type;

			extractLexem(ref express, ref curpos, out lexeme, out type);
			int n = expressList(args, ref express, ref curpos, ref lexeme, ref type);

			return n;
		}

		/// <summary>
		/// Evaluates a list of expressions encounted with a commas
		/// </summary>
		/// <param name="args">Array with results of calculation</param>
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Number of elements in list</returns>
		protected int expressList(List<double> args, ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			if (type == Lexeme.End) return 0;

			double d;
			int i = 0;

			d = expressBoolean(ref express, ref curpos, ref lexeme, ref type);
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
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = expressBoolean(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressBoolean(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressRelational(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpBoolean)
			{
				switch (lexeme)
				{
					case "&&":
					case "and":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref express, ref curpos, ref lexeme, ref type);
							d = (d != 0 && d2 != 0) ? 1 : 0;
							break;
						}

					case "||":
					case "or":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref express, ref curpos, ref lexeme, ref type);
							d = (d != 0 || d2 != 0) ? 1 : 0;
							break;
						}

					case "@@":
					case "xor":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref express, ref curpos, ref lexeme, ref type);
							d = ((d != 0) != (d2 != 0)) ? 1 : 0;
							break;
						}

					case "<->":
					case "equ":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref express, ref curpos, ref lexeme, ref type);
							d = ((d != 0) == (d2 != 0)) ? 1 : 0;
							break;
						}

					case "->":
					case "imp":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							double d2 = expressRelational(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressRelational(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressBitwise(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpRelational)
			{
				switch (lexeme)
				{
					case "<=":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d <= expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case ">=":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d >= expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "<":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d < expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case ">":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d > expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "==":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d == expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
							break;
						}

					case "!=":
					case "<>":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = d != expressBitwise(ref express, ref curpos, ref lexeme, ref type) ? 1 : 0;
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressBitwise(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressAdditive(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpBitwise)
			{
				switch (lexeme)
				{
					case "&":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = (int)d & (int)expressAdditive(ref express, ref curpos, ref lexeme, ref type);
							break;
						}

					case "|":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = (int)d | (int)expressAdditive(ref express, ref curpos, ref lexeme, ref type);
							break;
						}

					case "@":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = (int)d ^ (int)expressAdditive(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressAdditive(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressMultiplicative(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpAdditive)
			{
				switch (lexeme)
				{
					case "+":
					case "plus":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d += expressMultiplicative(ref express, ref curpos, ref lexeme, ref type);
							break;
						}

					case "-":
					case "minus":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d -= expressMultiplicative(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressMultiplicative(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressPower(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpMultiplicative)
			{
				switch (lexeme)
				{
					case "*":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d *= expressPower(ref express, ref curpos, ref lexeme, ref type);
							break;
						}

					case "/":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d /= expressPower(ref express, ref curpos, ref lexeme, ref type);
							break;
						}

					case "%":
					case "mod":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d %= expressPower(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPower(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressPrefix(ref express, ref curpos, ref lexeme, ref type);
			while (type == Lexeme.OpPower)
			{
				switch (lexeme)
				{
					case "^":
						{
							extractLexem(ref express, ref curpos, out lexeme, out type);
							d = Math.Pow(d, expressPrefix(ref express, ref curpos, ref lexeme, ref type));
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPrefix(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			switch (lexeme)
			{
				case "!":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						d = expressPostfix(ref express, ref curpos, ref lexeme, ref type) == 0 ? 1 : 0;
						break;
					}

				default:
					d = expressPostfix(ref express, ref curpos, ref lexeme, ref type);
					break;
			}
			return d;
		}

		/// <summary>
		/// Evalutes postfix operations
		/// </summary>
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressPostfix(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d = expressSign(ref express, ref curpos, ref lexeme, ref type);
			switch (lexeme)
			{
				case "!":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressSign(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			int sign = +1;
			switch (lexeme)
			{
				case "+":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						sign = +1;
						break;
					}

				case "-":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						sign = -1;
						break;
					}
			}
			return sign * expressFactor(ref express, ref curpos, ref lexeme, ref type);
		}

		/// <summary>
		/// Evaluates expression with a brackets
		/// </summary>
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressFactor(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			List<double> args = new List<double>();
			int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
			return n > 0 ? args[0] : 0;
		}

		/// <summary>
		/// Evaluates expression with a brackets
		/// </summary>
		/// <param name="args">Array with results of calculation</param>
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected int expressFactor(List<double> args, ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			double d;
			int i = 0;

			switch (lexeme)
			{
				case "(":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						i = expressList(args, ref express, ref curpos, ref lexeme, ref type);
						if (lexeme == ")") extractLexem(ref express, ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \")\" is expected", curpos, lexeme);
						break;
					}

				case "[":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						i = expressList(args, ref express, ref curpos, ref lexeme, ref type);
						if (lexeme == "]") extractLexem(ref express, ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \"]\" is expected", curpos, lexeme);
						break;
					}

				case "{":
					{
						extractLexem(ref express, ref curpos, out lexeme, out type);
						i = expressList(args, ref express, ref curpos, ref lexeme, ref type);
						if (lexeme == "}") extractLexem(ref express, ref curpos, out lexeme, out type);
						else throw new ExpressException("Closing bracket \"}\" is expected", curpos, lexeme);
						break;
					}

				default:
					d = expressLexem(ref express, ref curpos, ref lexeme, ref type);
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
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		/// <returns>Result of calculation</returns>
		protected double expressLexem(ref string express, ref int curpos, ref string lexeme, ref Lexeme type)
		{
			int curpos0 = curpos;
			string lexeme0 = lexeme;
			Lexeme type0 = type;
			extractLexem(ref express, ref curpos, out lexeme, out type);
			switch (type0)
			{
				case Lexeme.NumDecimal:
					return Convert.ToDouble(lexeme0.Replace('.', ','));
				case Lexeme.NumHexadecimal:
					return ParseHex(lexeme0);

				case Lexeme.Parser:
					foreach (KeyValuePair<string, Parser> v in Parsers)
						if (v.Key == lexeme0) return v.Value(ref express, ref curpos, ref lexeme, ref type);
					throw new ExpressException("Invalid parser", curpos0, lexeme0);

				case Lexeme.Variable:
					foreach (KeyValuePair<string, double> v in Variables)
						if (v.Key == lexeme0) return v.Value;
					throw new ExpressException("Invalid variable", curpos0, lexeme0);

				case Lexeme.Function:
					foreach (KeyValuePair<string, string[]> v in Functions)
						if (v.Key == lexeme0)
						{
							double d;
							List<double> args = new List<double>();
							if (v.Value.Length > 1)
							{
								int n = expressFactor(args, ref express, ref curpos, ref lexeme, ref type);
								if (n < v.Value.Length - 1) throw new ExpressException("Too few arguments to function", curpos0, lexeme0);
							}
							int given = 0;
							try
							{
								for (; given < v.Value.Length - 1; given++)
									Variables.Insert(given, new KeyValuePair<string, double>(v.Value[given + 1], args[given]));
								d = Evaluate(v.Value[0]);
							}
							catch (Exception)
							{
								throw;
							}
							finally
							{
								Variables.RemoveRange(0, given);
							}
							return d;
						}
					throw new ExpressException("Invalid function", curpos0, lexeme0);

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
		/// Extracts new lexeme and adjust current position
		/// </summary>
		/// <param name="express">Expression string to convert</param>
		/// <param name="curpos">Current position of string expression processing</param>
		/// <param name="lexeme">Current processing lexeme</param>
		/// <param name="type">Type of current processing lexeme</param>
		protected void extractLexem(ref string express, ref int curpos, out string lexeme, out Lexeme type)
		{
			// Skip space
			skipSpace(ref express, ref curpos);

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

			// Extract parser
			foreach (KeyValuePair<string, Parser> v in Parsers)
			{
				if (String.Compare(express, curpos, v.Key, 0, v.Key.Length) == 0 &&
					!(curpos + v.Key.Length < express.Length && isAlphaNumericChar(express[curpos + v.Key.Length])))
				{
					curpos += v.Key.Length;
					type = Lexeme.Parser;
					lexeme = v.Key;
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

			// Extract function
			foreach (KeyValuePair<string, string[]> v in Functions)
			{
				if (String.Compare(express, curpos, v.Key, 0, v.Key.Length) == 0 &&
					!(curpos + v.Key.Length < express.Length && isAlphaNumericChar(express[curpos + v.Key.Length])))
				{
					curpos += v.Key.Length;
					type = Lexeme.Function;
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

		static internal void skipSpace(ref string express, ref int curpos)
		{
			while (curpos < express.Length && express[curpos] == ' ') curpos++;
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

		/// <summary>
		/// Converts hexadecimal value from string to double
		/// </summary>
		/// <param name="hex">Hexadecimal string value representation</param>
		/// <returns>Double value representation</returns>
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
	} // Expression
}