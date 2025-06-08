namespace WhgVedit.Types;

// TODO: Stacking math.

public readonly struct Transform2D
{
	public readonly Subpixel2 Origin;
	public readonly Basis Basis;

	public static Transform2D Identity => new(0, 0, 1, 0, 0, 1);

	public Transform2D(Subpixel2 origin, Basis basis)
	{
		Origin = origin;
		Basis = basis;
	}

	public Transform2D(int posX, int posY, Basis basis)
	{
		Origin = new(posX, posY);
		Basis = basis;
	}

	public Transform2D(int posX, int posY, float xx, float xy, float yx, float yy)
	{
		Origin = new(posX, posY);
		Basis = new(xx, xy, yx, yy);
	}

	public static Transform2D Stack(Transform2D first, Transform2D second)
	{
		Basis combinedBasis = Basis.Stack(first.Basis, second.Basis);

		Subpixel2 transformedOrigin = new(
			first.Basis.XX * second.Origin.X + first.Basis.XY * second.Origin.Y + first.Origin.X,
			first.Basis.YX * second.Origin.X + first.Basis.YY * second.Origin.Y + first.Origin.Y
		);

		return new Transform2D(transformedOrigin, combinedBasis);
	}

	public readonly Transform2D StackWith(Transform2D other) => Stack(this, other);

	public override string ToString()
	{
		return $"{{ Origin: {Origin}, Basis: {Basis} }}";
	}
}