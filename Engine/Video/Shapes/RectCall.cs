namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

class RectCall : ShapeCall
{
	public Rect2i Body { get; set; }

	public RectCall(int zIndex, Rect2i body, Color color)
	{
		ZIndex = zIndex;
		Body = body;
		Color = color;
	}

	public override void Execute()
	{
		VideoEngine.DrawRect2i(Body, Color);
	}
}