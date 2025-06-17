namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;
using WhgVedit.Objects;

public class RingCall : ShapeCall
{
	public Circle Body { get; set; }
	public int Sides { get; set; }

	public RingCall(ZIndex zIndex, Color color, Vector2I position, float radius, int sides)
	{
		ZIndex = zIndex;
		Color = color;
		Body = new(position, radius);
		Sides = sides;
	}

	public RingCall(ZIndex zIndex, Circle body, Color color, int sides)
	{
		ZIndex = zIndex;
		Color = color;
		Body = body;
		Sides = sides;
	}

	public override void Execute()
	{
		Raylib.DrawRing(
			Body.Position,
			(float)Body.Radius - Wall.OutlineWidth,
			(float)Body.Radius,
			0, 360, Sides, Color
		);
	}
}