using WhgVedit.Common;

namespace WhgVedit.Types;

public readonly struct Circle
{
	public readonly Vector2I Position;
	public readonly double Radius;

	public Circle(int x, int y, double radius)
	{
		Position = new(x, y);
		Radius = radius;
	}

	public Circle(Vector2I position, double radius)
	{
		Position = position;
		Radius = radius;
	}

	public bool Intersects(Circle other)
	{
		int squaredDistance = Utils.Square(Position.X - other.Position.X) +
				Utils.Square(Position.Y - other.Position.Y);

		return squaredDistance < Utils.Square(Radius + other.Radius);
	}

	public bool Intersects(Rect2I rect)
	{
		return rect.Intersects(this);
	}
}