namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

public class RectCall : ShapeCall
{
	public Rect2I Body { get; set; }

	public RectCall(ZIndex zIndex, Rect2I body, Color color)
	{
		ZIndex = zIndex;
		Body = body;
		Color = color;
	}

	public override void Execute()
	{
		VideoEngine.DrawRect2I(Body, Color);
	}
}