namespace WhgVedit.Common;

using System.Numerics;
using Raylib_cs;
using WhgVedit.Types;

public static class Utils
{
	// Rounding function that favors positive infinity when met with a perfect tie.
	public static int Round(double value) => (int)Math.Floor(value + 0.5);
	public static Vector2i Round(Vector2 value) => new(Round(value.X), Round(value.Y));
	public static int Square(int value) => value * value;
	public static double Square(double value) => value * value;

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
	public static int LerpI(int start, int end, double ratio)
	{
		return Round(Lerp(start, end, ratio));
	}

	public static float LerpF(float start, float end, double ratio)
	{
		return (float)(start * (1 - ratio) + end * ratio);
	}

	public static double Lerp(double start, double end, double ratio)
	{
		return start * (1 - ratio) + end * ratio;
	}

	public static Vector2i Lerp2i(Vector2i start, Vector2i end, double ratio) => new(
		Round(Lerp(start.X, end.X, ratio)),
		Round(Lerp(start.Y, end.Y, ratio))
	);

	public static Color LerpColor(Color start, Color end, double ratio) => new(
		LerpI(start.R, end.R, ratio),
		LerpI(start.G, end.G, ratio),
		LerpI(start.B, end.B, ratio),
		LerpI(start.A, end.A, ratio)
	);

	public static Basis LerpBasis(Basis start, Basis end, double ratio) => new(
		LerpF(start.X.X, end.X.X, ratio),
		LerpF(start.X.Y, end.X.Y, ratio),
		LerpF(start.Y.X, end.Y.X, ratio),
		LerpF(start.Y.Y, end.Y.Y, ratio)
	);

	// To save resources (or out of my laziness), you're responsible for keeping lengths not negative.
	public static int FindRange(double position, List<double> rangeLengths)
	{
		for (int i = 0; i < rangeLengths.Count; i++)
		{
			position -= rangeLengths[i];

			if (position < 0) return i;
		}

		return -1; // When greater (or equal to) all ranges combined.
	}

	public static double RadToDeg(double rad) => rad / Math.Tau * 360;
	public static double DegToRad(double deg) => deg * Math.Tau / 360;
	public static bool AreColorsEqual(Color color1, Color color2)
	{
		return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B && color1.A == color2.A;
	}

	public static int SnapToGrid(int value, int step, int offset = 0)
	{
		return Round((value - offset) / (double)step) * step + offset;
	}
}
