namespace WhgVedit.Common;

using Raylib_cs;
using WhgVedit.Types;

static class Utils
{
	public static int GetInputAxis(KeyboardKey negativeAction, KeyboardKey positiveAction)
	{
		return (int)Raylib.IsKeyDown(positiveAction) - (int)Raylib.IsKeyDown(negativeAction);
	}

	public static double GetClosest(double interest, double a, double b)
	{
		if (interest == double.PositiveInfinity)
			return a > b ? a : b;

		if (interest == double.NegativeInfinity)
			return a < b ? a : b;

		return Math.Abs(interest - a) < Math.Abs(interest - b) ? a : b;
	}

	public static double PushToZero(double value, double amount)
	{
		if (value == 0)
			return 0;

		bool negative = value < 0;

		if (Math.Abs(value) <= amount && amount > 0)
			return 0;

		return negative ? value + amount : value - amount;
	}

	// Basically budget animation.
	public static int PingPong(int value, int scope)
	{
		value %= scope * 2;

		if (value >= scope)
			value = scope * 2 - value;

		return value;
	}
	// Float version. Felt like making it explicit instead of overloading.
	public static float PingPongF(float value, float scope)
	{
		value %= scope * 2;

		if (value >= scope)
			value = scope * 2 - value;

		return value;
	}

	// Linear interpolation.
	public static int LerpI(int start, int end, float ratio)
	{
		return (int)Math.Round(start * (1 - ratio) + end * ratio, MidpointRounding.ToPositiveInfinity);
	}
	
	public static int LerpIDouble(int start, int end, double ratio)
	{
		return (int)Math.Round(start * (1 - ratio) + end * ratio, MidpointRounding.ToPositiveInfinity);
	}

	public static float LerpF(float start, float end, float ratio)
	{
		return start * (1 - ratio) + end * ratio;
	}

	public static double LerpD(double start, double end, float ratio)
	{
		return start * (1 - ratio) + end * ratio;
	}

	public static Vector2i Lerp2i(Vector2i start, Vector2i end, float ratio) => new(
		LerpI(start.X, end.X, ratio),
		LerpI(start.Y, end.Y, ratio)
	);
	
	public static Vector2i Lerp2iDouble(Vector2i start, Vector2i end, double ratio) => new(
		LerpIDouble(start.X, end.X, ratio),
		LerpIDouble(start.Y, end.Y, ratio)
	);

	public static Color LerpColor(Color start, Color end, float ratio) => new(
		LerpI(start.R, end.R, ratio),
		LerpI(start.G, end.G, ratio),
		LerpI(start.B, end.B, ratio),
		LerpI(start.A, end.A, ratio)
	);

	public static Basis LerpBasis(Basis start, Basis end, float ratio) => new(
		LerpF(start.XX, end.XX, ratio),
		LerpF(start.XY, end.XY, ratio),
		LerpF(start.YX, end.YX, ratio),
		LerpF(start.YY, end.YY, ratio)
	);
}
