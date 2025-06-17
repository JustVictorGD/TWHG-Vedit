namespace WhgVedit.Objects.Shapes;

using Raylib_cs;

using Engine.Video;
using Engine.Video.Shapes;
using Types;

public class SolidRect : RectObject
{
	public Color Color { get; set; }

	public SolidRect(Vector2I size, Color color, ZIndex zIndex = new(), bool isUI = false)
	{
		Size = size;
		Color = color;
		ZIndex = zIndex;
		IsUI = isUI;
	}

	public override void Draw()
	{
		if (IsVisible && !IsUI)
			PrivateDraw();
	}

	public override void DrawUI()
	{
		if (IsVisible && IsUI)
			PrivateDraw();
	}

	private void PrivateDraw()
	{
		VideoEngine.QueueDraw(new RectCall(GetGlobalZ(), Body, Color));
	}
}