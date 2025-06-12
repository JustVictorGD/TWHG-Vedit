namespace WhgVedit.Objects;

using Common;
using Objects.Shapes;
using Types;

public class Enemy : Object2D
{
	public const int DefaultRadius = 13;

	// Component. Set in Ready().
	public OutlinedCircle? Sprite { get; set; }

	public int Radius => Utils.Round(13 * Scale.X);

	public Circle Body => new((Vector2i)GetGlobalPosition(), Radius);
	public Circle Hitbox => new((Vector2i)GetGlobalPosition(), Radius - Wall.OutlineWidth);

	public Enemy()
	{
		// Outline and fill colors: Dark blue and transparent blue.
		Sprite = new(Radius, new(0, 0, 66), new(0, 0, 255));
		Sprite.SetParent(this);
	}

	public override void Draw()
	{
		if (Sprite != null) Sprite.Radius = Radius;

		Sprite?.Draw();
	}
}