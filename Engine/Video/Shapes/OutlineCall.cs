namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

using Types;

public class OutlineCall : ShapeCall
{
	public Rect2i Body { get; set; }

	// Manual inner used to prevent just a bit of repeated logic.
	public Rect2i? Inner { get; set; }

	public OutlineCall(ZIndex zIndex, Rect2i body, Color color, Rect2i? inner = null)
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
			VideoEngine.DrawOutline(Body, Color, (Rect2i)Inner);
	}
}