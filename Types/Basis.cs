using System.Numerics;
using WhgVedit.Common;

// Tutorial on rotation matrices from Godot: https://docs.godotengine.org/en/stable/tutorials/math/matrices_and_transforms.html

// A basis type can represent any combination of rotation,
// scale and skew in a way that's easy to stack.

// In this manual example, "Vector" is any 2D position and
// "Basis" is any basis that gets applied to that position.

// Vector.X = Vector.X * Basis.X.X + Vector.Y * Basis.X.Y
// Vector.Y = Vector.Y * Basis.Y.X + Vector.Y * Basis.Y.Y

namespace WhgVedit.Types;

public readonly struct Basis(float xx = 1, float xy = 0, float yx = 0, float yy = 1)
{
	public readonly Vector2 X = new(xx, xy);
	public readonly Vector2 Y = new(yx, yy);

	public static Basis Identity => new(1, 0, 0, 1);

	public static Basis FromScale(float x, float y) => new(x, 0, 0, y);

	public static Basis Stack(Basis first, Basis second) => new(
		first.X.X * second.X.X + first.X.Y * second.Y.X,
		first.X.X * second.X.Y + first.X.Y * second.Y.Y,
		first.Y.X * second.X.X + first.Y.Y * second.Y.X,
		first.Y.X * second.X.Y + first.Y.Y * second.Y.Y
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


	// I did many tests, but I can't guarantee that this math will work flawlessy.
	public void Split(out double angle, out Vector2 scale, out double skew)
	{
		angle = Utils.RadToDeg(-Math.Atan2(X.X, X.Y)) + 90;
		scale = new(X.Length(), Y.Length());

		skew = Utils.RadToDeg(Math.Atan2(Y.X, Y.Y)) + angle;
		skew = NormalizeAngle(skew);

		if (Math.Abs(skew) >= 90)
		{
			scale.Y *= -1;
			skew = NormalizeAngle(skew + 180);
		}
	}

	private static double NormalizeAngle(double degrees)
	{
		degrees %= 360;
		if (degrees <= -180) degrees += 360;
		if (degrees > 180) degrees -= 360;
		return degrees;
	}

	//public override string ToString() => $"{{ X = X*{XX} + Y*{XY}, Y = X*{YX} + Y*{YY} }}";
	public override string ToString() => $"{{ XX: {X.X:F3}, XY: {X.Y:F3}, YX: {Y.X:F3}, YY: {Y.Y:F3} }}";
}