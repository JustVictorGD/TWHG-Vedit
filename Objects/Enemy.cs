using Newtonsoft.Json.Linq;

namespace WhgVedit.Objects;

using Common;
using Objects.Shapes;
using Types;

public class Enemy : SpacialObject
{
	public override ZIndex ZIndex { get; set; } = new(400);

	public const int DefaultRadius = 13;

	// Component. Set in Ready().
	public OutlinedCircle? Sprite { get; set; }

	public int Radius => Utils.Round(13 * Scale.X);

	public Circle Body => new((Vector2I)GetGlobalPosition(), Radius);
	public Circle Hitbox => new((Vector2I)GetGlobalPosition(), Radius - Wall.OutlineWidth);

	public Enemy()
	{
		// Outline and fill colors: Dark blue and transparent blue.
		Sprite = new(Radius, new(0, 0, 66), new(0, 0, 255));
		AddChild(Sprite);
	}

	public override void Draw()
	{
		if (Sprite != null) Sprite.Radius = Radius;

		Sprite?.Draw();
	}

	public override JObject ToJson()
	{
		JObject jObject = base.ToJson();
		jObject.Add("position", new JArray(Position.X.Rounded, Position.Y.Rounded));
		return jObject;
	}
}