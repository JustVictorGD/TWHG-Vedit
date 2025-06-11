namespace WhgVedit.Types;

// Tutorial on 2D basis from Godot: https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html

// A 2D basis type is the rotation, scale and skew state of
// a 2D object encoded as a computer friendly 2x2 matrix.

// Basis contains the exact instructions on how to transform a
// 2D position, avoiding the need to do trigonometry for every
// object, and it can easily be stacked with other transforms.

// If "Vector" is any 2D position and "Basis" is any basis applied to that position,
// Vector.X = Vector.X * Basis.X.X + Vector.Y * Basis.X.Y
// Vector.Y = Vector.Y * Basis.Y.X + Vector.Y * Basis.Y.Y

using System.Numerics;
using System.Text;

using Common;

public readonly struct Basis
{
	public readonly Vector2 X;
	public readonly Vector2 Y;

	// These toggles don't directly affect the matrix, but they remove
	// ambiguity when splitting it into rotation, scale and skew. There would be
	// no difference between a 180 degree turn and a scale invertion otherwise.
	public readonly bool IsXInverted;
	public readonly bool IsYInverted;

	public static Basis Identity => new(1, 0, 0, 1);

	public Basis(float xx = 1, float xy = 0, float yx = 0, float yy = 1, bool isXInverted = false, bool isYInverted = false)
	{
		X = new(xx, xy);
		Y = new(yx, yy);
		IsXInverted = isXInverted;
		IsYInverted = isYInverted;
	}

	public Basis(Vector2 xVector, Vector2 yVector, bool isXInverted = false, bool isYInverted = false)
	{
		X = xVector;
		Y = yVector;
		IsXInverted = isXInverted;
		IsYInverted = isYInverted;
	}

	public static Basis FromScale(float x, float y) => new(x, 0, 0, y);

	public static Basis Stack(Basis first, Basis second) => new(
		first.X.X * second.X.X + first.X.Y * second.Y.X,
		first.X.X * second.X.Y + first.X.Y * second.Y.Y,
		first.Y.X * second.X.X + first.Y.Y * second.Y.X,
		first.Y.X * second.X.Y + first.Y.Y * second.Y.Y,
		first.IsXInverted != second.IsXInverted,
		first.IsYInverted != second.IsYInverted
	);

	public readonly Basis StackWith(Basis other) => Stack(this, other);

	public static Basis FromDeg(double degrees) => FromRad(degrees * MathF.Tau / 360);

	public static Basis FromRad(double radians)
	{
		float radiansF = (float)radians;
		return new(
			MathF.Cos(radiansF),
			MathF.Sin(radiansF),
			-MathF.Sin(radiansF),
			MathF.Cos(radiansF)
		);
	}

	public static Basis FromRotationAndScale(float degrees, float x, float y)
	{
		return FromScale(x, y).StackWith(FromDeg(degrees));
	}

	public static Basis FromRotationAndScale(float degrees, Vector2 scale) =>
		FromRotationAndScale(degrees, scale.X, scale.Y);

	public static Basis Construct(double angle, Vector2 scale, double skew)
	{
		return new(
			VectorFromAngle(angle) * scale.X, // X Vector.
			VectorFromAngle(angle + skew) * scale.Y, // Y vector.
			scale.X < 0, // IsXInverted
			scale.Y < 0 // IsYInverted
		);
	}

	// I can't guarantee that this math will work flawlessy.
	public void Split(out double rotation, out Vector2 scale, out double skew)
	{
		scale = new(
			X.Length() * (IsXInverted ? -1 : 1),
			Y.Length() * (IsYInverted ? -1 : 1)
		);

		rotation = Utils.RadToDeg(-Math.Atan2(-X.Y, X.X));
		skew = Utils.RadToDeg(Math.Atan2(Y.X, Y.Y)) + rotation;

		if (IsXInverted) rotation -= 180;
		if (IsXInverted != IsYInverted) skew -= 180;

		rotation = NormalizeAngle(rotation);
		skew = NormalizeAngle(skew);
	}

	private static Vector2 VectorFromAngle(double degrees)
	{
		double theta = Utils.DegToRad(degrees);

		return new(
			(float)Math.Cos(theta),
			(float)Math.Sin(theta)
		);
	}

	private static double NormalizeAngle(double degrees)
	{
		degrees %= 360;
		if (degrees <= -180) degrees += 360;
		if (degrees > 180) degrees -= 360;
		return degrees;
	}

	public override string ToString()
	{
		// I've heard that StringBuilder is better for memory.
		StringBuilder stringBuilder = new();

		stringBuilder.Append($"{{ XX: {X.X:F3}, XY: {X.Y:F3}, YX: {Y.X:F3}, YY: {Y.Y:F3}");

		if (IsXInverted) stringBuilder.Append(", -X");
		if (IsYInverted) stringBuilder.Append(", -Y");

		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}

	public string ToReadableString()
	{
		Split(out double rotation, out Vector2 scale, out double skew);

		return $"{{ Rotation: {rotation:F4}, Scale: {scale:F4}, Skew: {skew:F4} }}";
	}
}