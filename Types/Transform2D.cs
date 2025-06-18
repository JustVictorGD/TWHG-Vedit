using System.Numerics;

// Check Basis.cs for some explanation on the math involved.

namespace WhgVedit.Types;

public readonly struct Transform2D
{
	public readonly Vector2X Origin;
	public readonly Basis Basis;
	public readonly Vector2 X => Basis.X;
	public readonly Vector2 Y => Basis.Y;

	public static Transform2D Identity => new(0, 0, 1, 0, 0, 1);

	public Transform2D(Vector2X origin, Basis basis)
	{
		Origin = origin;
		Basis = basis;
	}

	public Transform2D(double posX, double posY, Basis basis)
	{
		Origin = new(posX, posY);
		Basis = basis;
	}

	public Transform2D(double posX, double posY, float xx, float xy, float yx, float yy)
	{
		Origin = new(posX, posY);
		Basis = new(xx, xy, yx, yy);
	}

	public static Transform2D Stack(Transform2D first, Transform2D second)
	{
		Basis combinedBasis = Basis.Stack(first.Basis, second.Basis);

		Vector2X transformedOrigin = new(
			first.X.X * second.Origin.X + first.X.Y * second.Origin.Y + first.Origin.X,
			first.Y.X * second.Origin.X + first.Y.Y * second.Origin.Y + first.Origin.Y
		);

		return new Transform2D(transformedOrigin, combinedBasis);
	}

	public readonly Transform2D StackWith(Transform2D other) => Stack(this, other);

	public override string ToString()
	{
		return $"{{ Origin: {Origin}, X: {X}, Y: {Y} }}";
	}
}