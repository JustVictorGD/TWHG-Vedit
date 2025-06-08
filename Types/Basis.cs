using System.Numerics;

namespace WhgVedit.Types;

public readonly struct Basis(float xx = 1, float xy = 0, float yx = 0, float yy = 1)
{
	// These 4 values tell how any 2D vector would get transformed once
	// this basis is applied to it. You can read these symbols right to
	// left. Value of XY can be read as "Vector.X = Vector.Y * XY.Value."
	public readonly float XX = xx;
	public readonly float XY = xy;
	public readonly float YX = yx;
	public readonly float YY = yy;

	public static Basis FromScale(float x, float y) => new(x, 0, 0, y);

	public static Basis Stack(Basis first, Basis second) => new(
		first.XX * second.XX + first.XY * second.YX,
		first.XX * second.XY + first.XY * second.YY,
		first.YX * second.XX + first.YY * second.YX,
		first.YX * second.XY + first.YY * second.YY
	);

	public readonly Basis StackWith(Basis other) => Stack(this, other);

	public static Basis FromDeg(float degrees) => FromRad(degrees * MathF.Tau / 360);

	public static Basis FromRad(float radians)
	{
		return new(
			MathF.Cos(radians),
			MathF.Sin(radians),
			-MathF.Sin(radians),
			MathF.Cos(radians)
		);
	}

	public static Basis FromRotationAndScale(float degrees, Vector2 scale) =>
		FromRotationAndScale(degrees, scale.X, scale.Y);

	public static Basis FromRotationAndScale(float degrees, float x, float y)
	{
		return FromScale(x, y).StackWith(FromDeg(degrees));
	}

	//public override string ToString() => $"{{ X = X*{XX} + Y*{XY}, Y = X*{YX} + Y*{YY} }}";
	public override string ToString() => $"{{ XX: {XX:F3}, XY: {XY:F3}, YX: {YX:F3}, YY: {YY:F3} }}";
}