namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

class CircleCall : ShapeCall
{
	public Vector2i Position { get; set; }
	public float Radius { get; set; }

	public CircleCall(int zIndex, Color color, Vector2i position, float radius)
	{
		ZIndex = zIndex;
		Color = color;
		Position = position;
		Radius = radius;
	}

	public override void Execute()
	{
		int sides = (int)MathF.Log2(Radius + 1) * Game.CircleQuality;

		Raylib.DrawPoly(Position, sides, Radius, 0, Color);
	}
}