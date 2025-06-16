namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

public class CircleCall : ShapeCall
{
	public Circle Body { get; set; }
	public int Sides { get; set; }

	public CircleCall(ZIndex zIndex, Vector2i position, double radius, Color color, int sides)
	{
		ZIndex = zIndex;
		Color = color;
		Body = new(position, radius);
		Sides = sides;
	}

	public CircleCall(ZIndex zIndex, Circle body, Color color, int sides)
	{
		ZIndex = zIndex;
		Color = color;
		Body = body;
		Sides = sides;
	}

	public override void Execute()
	{
		Raylib.DrawPoly(Body.Position, Sides, (float)Body.Radius, 0, Color);
	}
}