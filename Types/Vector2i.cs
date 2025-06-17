using System.Numerics;
using WhgVedit.Common;

namespace WhgVedit.Types;

public readonly struct Vector2I
{
	public readonly int X;
	public readonly int Y;

	public static Vector2I Zero => new();
	public static Vector2I One => new(1, 1);

	// Constructors.
	public Vector2I() { X = 0; Y = 0; }

	public Vector2I(int v) { X = v; Y = v; }
	public Vector2I(double v) { X = Utils.Round(v); Y = Utils.Round(v); }

	public Vector2I(int x, int y) { X = x; Y = y; }
	public Vector2I(double x, double y) { X = Utils.Round(x); Y = Utils.Round(y); }


	// Editing functions. These don't edit this instance, only create new structs.

	public readonly Vector2I Clamp(int left, int top, int right, int bottom) => new(
		Math.Clamp(X, left, right),
		Math.Clamp(Y, top, bottom)
	);

	public readonly Vector2I Clamp(Vector2I min, Vector2I max) => Clamp(min.X, min.Y, max.X, max.Y);
	public readonly Vector2I Clamp(Rect2I area) => Clamp(area.Start, area.End);

	public readonly Vector2I Move(int x, int y) => new(X + x, Y + y);
	public readonly Vector2I Move(Vector2I amount) => Move(amount.X, amount.Y);

	public readonly Vector2I Lerp(Vector2I other, float time) => new(
		Utils.LerpI(X, other.X, time),
		Utils.LerpI(Y, other.Y, time)
	);

	public readonly Vector2I SnapToGrid(Vector2I size, Vector2I offset)
	{
		return new(
			Utils.SnapToGrid(X, size.X, offset.X),
			Utils.SnapToGrid(Y, size.Y, offset.Y)
		);
	}

	public readonly Vector2I SnapToGrid(Vector2I size) => SnapToGrid(size, new());
	public readonly Vector2I SnapToGrid(int size, int offset = 0) => SnapToGrid(new(size));


	// Warning: Make sure to preserve the original copy. Constantly rounding
	// floating point operations to integers will result in A LOT of distortion.
	public readonly Vector2I ApplyTransform(Transform2D transform)
	{
		return (Vector2I)(ApplyBasis(transform.Basis) + transform.Origin);
	}

	public readonly Vector2I ApplyBasis(Basis basis) => new(
		Utils.Round(X * basis.X.X + Y * basis.X.Y),
		Utils.Round(X * basis.Y.X + Y * basis.Y.Y)
	);

	// Conversion operators.

	public static implicit operator Vector2(Vector2I a) => new(a.X, a.Y);

	public static explicit operator Vector2I(Vector2 a) => new(
		(int)Math.Round(a.X, MidpointRounding.ToPositiveInfinity),
		(int)Math.Round(a.Y, MidpointRounding.ToPositiveInfinity)
	);

	// Other operators.

	public static Vector2I operator -(Vector2I a) => new(-a.X, -a.Y);

	public static Vector2I operator +(Vector2I a, int b) => new(a.X + b, a.Y + b);
	public static Vector2I operator -(Vector2I a, int b) => new(a.X - b, a.Y - b);
	public static Vector2I operator *(Vector2I a, int b) => new(a.X * b, a.Y * b);
	public static Vector2I operator /(Vector2I a, int b) => new(a.X / b, a.Y / b);

	public static Subpixel2 operator +(Vector2I a, double b) => new(a.X + b, a.Y + b);
	public static Subpixel2 operator -(Vector2I a, double b) => new(a.X - b, a.Y - b);
	public static Subpixel2 operator *(Vector2I a, double b) => new(a.X * b, a.Y * b);
	public static Subpixel2 operator /(Vector2I a, double b) => new(a.X / b, a.Y / b);

	public static Vector2I operator +(Vector2I a, Vector2I b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2I operator -(Vector2I a, Vector2I b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2I operator *(Vector2I a, Vector2I b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2I operator /(Vector2I a, Vector2I b) => new(a.X / b.X, a.Y / b.Y);

	// Comparison operators.

	public static bool operator <(Vector2I a, Vector2I b) => a.X < b.X && a.Y < b.Y;
	public static bool operator >(Vector2I a, Vector2I b) => a.X > b.X && a.Y > b.Y;

	public static bool operator <=(Vector2I a, Vector2I b) => a.X <= b.X && a.Y <= b.Y;
	public static bool operator >=(Vector2I a, Vector2I b) => a.X >= b.X && a.Y >= b.Y;
	
	public override string ToString() => $"{{ X: {X}, Y: {Y} }}";
}