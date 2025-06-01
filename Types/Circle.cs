namespace WhgVedit.Types;

public readonly struct Circle(Vector2i position, double radius)
{
	public readonly Vector2i Position = position;
	public readonly double Radius = radius;
}