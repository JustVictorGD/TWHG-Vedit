using System.Numerics;

namespace WhgVedit.Types;

public readonly struct Subpixel2
{
	// The values each struct stores.
	public readonly Subpixel X;
	public readonly Subpixel Y;

	// QoL getters.
	public readonly Vector2i Fraction => new(X.Fraction, Y.Fraction);
	public readonly Vector2i Rounded => new(X.Rounded, Y.Rounded);

	// Constructors.
	public Subpixel2() { X = 0; Y = 0; }
	public Subpixel2(int x, int y, bool fromSteps = false)
	{
		if (fromSteps) { X = new(x, true); Y = new(y, true); }
		else { X = x; Y = y; }
	}
	public Subpixel2(Subpixel x, Subpixel y) { X = x; Y = y; }
	public Subpixel2(float x, float y) { X = x; Y = y; }
	public Subpixel2(double x, double y) { X = (Subpixel)x; Y = (Subpixel)y; }

	// Editing functions.
	public readonly Subpixel2 Clamp(Rect2i area) => Clamp(this, area);
	public static Subpixel2 Clamp(Subpixel2 original, Rect2i area) => new(
		Subpixel.Clamp(original.X, area.Start.X, area.End.X),
		Subpixel.Clamp(original.Y, area.Start.Y, area.End.Y)
	);

	// Looking for Move()? Just add two Subpixel2 values together.

	public override readonly string ToString() => $"{{ X = {X}, Y = {Y} }}";


	// Conversion operators.

	public static explicit operator Vector2i(Subpixel2 a) => a.Rounded;
	public static implicit operator Subpixel2(Vector2i a) => new(a.X, a.Y);

	// Other operators.

	public static Subpixel2 operator -(Subpixel2 a) => new(-a.X, -a.Y);

	public static Subpixel2 operator +(Subpixel2 a, Subpixel2 b) => new(a.X + b.X, a.Y + b.Y);
	public static Subpixel2 operator -(Subpixel2 a, Subpixel2 b) => new(a.X - b.X, a.Y - b.Y);
	public static Subpixel2 operator *(Subpixel2 a, Subpixel2 b) => new(a.X * b.X, a.Y * b.Y);
	public static Subpixel2 operator /(Subpixel2 a, Subpixel2 b) => new(a.X / b.X, a.Y / b.Y);

	public static Subpixel2 operator +(Subpixel2 a, Vector2i b) => new(a.X + b.X, a.Y + b.Y);
	public static Subpixel2 operator -(Subpixel2 a, Vector2i b) => new(a.X - b.X, a.Y - b.Y);
	public static Subpixel2 operator *(Subpixel2 a, Vector2i b) => new(a.X * b.X, a.Y * b.Y);
	public static Subpixel2 operator /(Subpixel2 a, Vector2i b) => new(a.X / b.X, a.Y / b.Y);

	public static Subpixel2 operator +(Subpixel2 a, int b) => new(a.X + b, a.Y + b);
	public static Subpixel2 operator -(Subpixel2 a, int b) => new(a.X - b, a.Y - b);
	public static Subpixel2 operator *(Subpixel2 a, int b) => new(a.X * b, a.Y * b);
	public static Subpixel2 operator /(Subpixel2 a, int b) => new(a.X / b, a.Y / b);

	public static Subpixel2 operator +(Subpixel2 a, float b) => new(a.X + b, a.Y + b);
	public static Subpixel2 operator -(Subpixel2 a, float b) => new(a.X - b, a.Y - b);
	public static Subpixel2 operator *(Subpixel2 a, float b) => new(a.X * b, a.Y * b);
	public static Subpixel2 operator /(Subpixel2 a, float b) => new(a.X / b, a.Y / b);

	public static Subpixel2 operator +(Subpixel2 a, double b) => new(a.X + b, a.Y + b);
	public static Subpixel2 operator -(Subpixel2 a, double b) => new(a.X - b, a.Y - b);
	public static Subpixel2 operator *(Subpixel2 a, double b) => new(a.X * b, a.Y * b);
	public static Subpixel2 operator /(Subpixel2 a, double b) => new(a.X / b, a.Y / b);

	public static explicit operator Vector2(Subpixel2 value) => new((float)value.X, (float)value.Y);
}