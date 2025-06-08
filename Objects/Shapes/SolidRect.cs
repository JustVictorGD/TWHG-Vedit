namespace WhgVedit.Objects.Shapes;

using Raylib_cs;

using Engine.Video;
using Engine.Video.Shapes;
using Types;

public class SolidRect : Object2D
{
	public Vector2i Size { get; set; }
	public Color Color { get; set; }

	public bool IsVisible { get; set; } = true;
	public bool IsUI { get; set; }

	public Rect2i Body => new((Vector2i)Position, Size);

	public SolidRect(Rect2i body, Color color, int zIndex = 0, Object2D? parent = null, bool isUI = false)
	{
		Position = body.Position;
		Size = body.Size;
		Color = color;
		ZIndex = zIndex;
		Parent = parent;
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

		VideoEngine.QueueDraw(new RectCall(ZIndex, new Rect2i(globalPos, Size), Color));
	}
}