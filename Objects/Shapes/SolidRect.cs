namespace WhgVedit.Objects.Shapes;

using Raylib_cs;

using Engine.Video;
using Engine.Video.Shapes;
using Types;

public class SolidRect : SpacialObject
{
	public Vector2i Size { get; set; }
	public Color Color { get; set; }

	public Rect2i Body => new((Vector2i)Position, Size);

	public SolidRect(int zIndex, Rect2i body, Color color, bool isUI = false)
	{
		Position = body.Position;
		Size = body.Size;
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
		Vector2i globalPos = (Vector2i)GetGlobalPosition();

		VideoEngine.QueueDraw(new RectCall(GetGlobalZ(), new Rect2i(globalPos, Size), Color));
	}
}