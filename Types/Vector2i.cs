using System.Numerics;

namespace WhgVedit.Types;

readonly struct Vector2i
{
	public readonly int X;
	public readonly int Y;

	public static Vector2i Zero => new();
	public static Vector2i One => new(1, 1);

	// Constructors.
	public Vector2i() { X = 0; Y = 0; }
	public Vector2i(int x, int y) { X = x; Y = y; }


	// Editing functions. These don't edit this instance, only create new structs.

	public readonly Vector2i Clamp(int left, int top, int right, int bottom) => new(
		Math.Clamp(X, left, right),
		Math.Clamp(Y, top, bottom)
	);

	public readonly Vector2i Clamp(Vector2i min, Vector2i max) => Clamp(min.X, min.Y, max.X, max.Y);
	public readonly Vector2i Clamp(Rect2i area) => Clamp(area.Start, area.End);

	public readonly Vector2i Move(int x, int y) => new(X + x, Y + y);
	public readonly Vector2i Move(Vector2i amount) => Move(amount.X, amount.Y);

	public readonly Vector2i Lerp(Vector2i other, float time) => new(

	);

	// Warning: Make sure to preserve the original copy. Constantly rounding
	// floating point operations to integers will result in A LOT of distortion.
	public readonly Vector2i ApplyBasis(Basis basis) => new(
		(int)Math.Round(X * basis.XX + Y * basis.XY),
		(int)Math.Round(X * basis.YX + Y * basis.YY)
	);

	public override string ToString() => $"{{ X: {X}, Y: {Y} }}";

	// Conversion operators.

	public static implicit operator Vector2(Vector2i a) => new(a.X, a.Y);

	public static explicit operator Vector2i(Vector2 a) => new(
		(int)Math.Round(a.X, MidpointRounding.ToPositiveInfinity),
		(int)Math.Round(a.Y, MidpointRounding.ToPositiveInfinity)
	);

	// Other operators.

	public static Vector2i operator -(Vector2i a) => new(-a.X, -a.Y);

	public static Vector2i operator +(Vector2i a, int b) => new(a.X + b, a.Y + b);
	public static Vector2i operator -(Vector2i a, int b) => new(a.X - b, a.Y - b);
	public static Vector2i operator *(Vector2i a, int b) => new(a.X * b, a.Y * b);
	public static Vector2i operator /(Vector2i a, int b) => new(a.X / b, a.Y / b);

	public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2i operator -(Vector2i a, Vector2i b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2i operator *(Vector2i a, Vector2i b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2i operator /(Vector2i a, Vector2i b) => new(a.X / b.X, a.Y / b.Y);

	// Comparison operators.

	public static bool operator <(Vector2i a, Vector2i b) => a.X < b.X && a.Y < b.Y;
	public static bool operator >(Vector2i a, Vector2i b) => a.X > b.X && a.Y > b.Y;

	public static bool operator <=(Vector2i a, Vector2i b) => a.X <= b.X && a.Y <= b.Y;
	public static bool operator >=(Vector2i a, Vector2i b) => a.X >= b.X && a.Y >= b.Y;
}