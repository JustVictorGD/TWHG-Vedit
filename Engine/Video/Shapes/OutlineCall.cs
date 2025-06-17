namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

public class OutlineCall : ShapeCall
{
	public Rect2I Body { get; set; }

	// Manual inner used to prevent just a bit of repeated logic.
	public Rect2I? Inner { get; set; }

	public OutlineCall(ZIndex zIndex, Rect2I body, Color color, Rect2I? inner = null)
	{
		ZIndex = zIndex;
		Body = body;
		Color = color;
		Inner = inner;
	}

	public override void Execute()
	{
		if (Inner == null)
			VideoEngine.DrawOutline(Body, Color);
		else
			VideoEngine.DrawOutline(Body, Color, (Rect2I)Inner);
	}
}