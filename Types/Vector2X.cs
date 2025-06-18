namespace WhgVedit.Types;

// Stands for "Vector2 Fixed," a vector made out of 2 fixed point values. The letter
// "F" wasn't chosen because Vector2F would easily be confused for "Vector2 Float."

using System.Numerics;

public readonly struct Vector2X
{
	// The values each struct stores.
	public readonly Fixed X;
	public readonly Fixed Y;

	// QoL getters.
	public readonly Vector2I Fraction => new(X.Fraction, Y.Fraction);
	public readonly Vector2I Rounded => new(X.Rounded, Y.Rounded);

	// Constructors.
	public Vector2X() { X = 0; Y = 0; }
	public Vector2X(int x, int y, bool fromSteps = false)
	{
		if (fromSteps) { X = new(x, true); Y = new(y, true); }
		else { X = x; Y = y; }
	}
	public Vector2X(Fixed x, Fixed y) { X = x; Y = y; }
	public Vector2X(float x, float y) { X = x; Y = y; }
	public Vector2X(double x, double y) { X = (Fixed)x; Y = (Fixed)y; }

	// Editing functions.
	public readonly Vector2X ApplyTransform(Transform2D transform)
	{
		return ApplyBasis(transform.Basis) + transform.Origin;
	}

	public readonly Vector2X ApplyBasis(Basis basis) => new(
		Math.Round(X * basis.X.X + Y * basis.X.Y),
		Math.Round(X * basis.Y.X + Y * basis.Y.Y)
	);

	public readonly Vector2X Clamp(Rect2I area) => Clamp(this, area);
	public static Vector2X Clamp(Vector2X original, Rect2I area) => new(
		Fixed.Clamp(original.X, area.Start.X, area.End.X),
		Fixed.Clamp(original.Y, area.Start.Y, area.End.Y)
	);

	// Conversion operators.

	public static explicit operator Vector2I(Vector2X a) => a.Rounded;
	public static implicit operator Vector2X(Vector2I a) => new(a.X, a.Y);

	public static explicit operator Vector2(Vector2X value) => new((float)value.X, (float)value.Y);
	public static implicit operator Vector2X(Vector2 value) => new(value.X, value.Y);

	// Other operators.

	public static Vector2X operator -(Vector2X a) => new(-a.X, -a.Y);

	public static Vector2X operator +(Vector2X a, Vector2X b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2X operator -(Vector2X a, Vector2X b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2X operator *(Vector2X a, Vector2X b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2X operator /(Vector2X a, Vector2X b) => new(a.X / b.X, a.Y / b.Y);

	public static Vector2X operator +(Vector2X a, Vector2I b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2X operator -(Vector2X a, Vector2I b) => new(a.X - b.X, a.Y - b.Y);
	public static Vector2X operator *(Vector2X a, Vector2I b) => new(a.X * b.X, a.Y * b.Y);
	public static Vector2X operator /(Vector2X a, Vector2I b) => new(a.X / b.X, a.Y / b.Y);

	public static Vector2X operator +(Vector2X a, int b) => new(a.X + b, a.Y + b);
	public static Vector2X operator -(Vector2X a, int b) => new(a.X - b, a.Y - b);
	public static Vector2X operator *(Vector2X a, int b) => new(a.X * b, a.Y * b);
	public static Vector2X operator /(Vector2X a, int b) => new(a.X / b, a.Y / b);

	public static Vector2X operator +(Vector2X a, float b) => new(a.X + b, a.Y + b);
	public static Vector2X operator -(Vector2X a, float b) => new(a.X - b, a.Y - b);
	public static Vector2X operator *(Vector2X a, float b) => new(a.X * b, a.Y * b);
	public static Vector2X operator /(Vector2X a, float b) => new(a.X / b, a.Y / b);

	public static Vector2X operator +(Vector2X a, double b) => new(a.X + b, a.Y + b);
	public static Vector2X operator -(Vector2X a, double b) => new(a.X - b, a.Y - b);
	public static Vector2X operator *(Vector2X a, double b) => new(a.X * b, a.Y * b);
	public static Vector2X operator /(Vector2X a, double b) => new(a.X / b, a.Y / b);

	public static bool operator ==(Vector2X a, Vector2X b) => a.X == b.X && a.Y == b.Y;
	public static bool operator !=(Vector2X a, Vector2X b) => !(a == b);

	public override readonly string ToString() => $"{{ X: {X}, Y: {Y} }}";

	public override bool Equals(object? obj)
	{
		if (obj is Vector2X other1) return this == other1;
		else if (obj is Vector2 other2) return this == other2;
		else if (obj is Vector2I other3) return this == other3;

		return false;
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}
}