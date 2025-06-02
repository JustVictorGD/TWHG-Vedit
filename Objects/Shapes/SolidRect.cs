namespace WhgVedit.Objects.Shapes;

using Raylib_cs;
using Types;
using WhgVedit.Engine.Video;
using WhgVedit.Engine.Video.Shapes;

public class SolidRect : Object2D
{
	public Vector2i Size { get; set; }
	public Color Color { get; set; }
	public bool IsUI { get; set; }

	public Rect2i Body => new((Vector2i)Position, Size);

	public SolidRect(Rect2i body, Color color, bool isUI)
	{
		Position = body.Position;
		Size = body.Size;
		Color = color;
		IsUI = isUI;
	}

	public override void Draw()
	{
		if (!IsUI)
			VideoEngine.QueueDraw(new RectCall(ZIndex, Body, Color));
	}

	public override void DrawUI()
	{
		if (IsUI)
			VideoEngine.QueueDraw(new RectCall(ZIndex, Body, Color));
	}
}