namespace WhgVedit.Types;

public readonly struct Basis(float xx = 1, float xy = 0, float yx = 0, float yy = 1)
{
	// Main data.
	public readonly float XX = xx;
	public readonly float XY = xy;
	public readonly float YX = yx;
	public readonly float YY = yy;

	public readonly Basis StackedWith(Basis other)
	{
		throw new NotImplementedException("Multiplying basis types is not yet implemented.");
	}

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

	public override string ToString() => $"{{ X = X*{XX} + Y*{XY}, Y = X*{YX} + Y*{YY} }}";

	// $"{{ XX: {XX}, XY: {XY}, YX: {YX}, YY: {YY} }}"
	// $"{{ X = X*{XX} + Y*{XY}, Y = X*{YX} + Y*{YY} }}"
}