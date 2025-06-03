namespace WhgVedit.Objects.Shapes;

// I'm not expecting shapes to be drawn by the scene, it seems
// cleaner to do it manually inside the parent objects' scripts.

using Raylib_cs;
using Types;
using WhgVedit.Engine.Video;

public class OutlinedRect : Object2D
{
	public Object2D? Parent { get; set; } // For getting dragged around.

	public Vector2i Size { get; set; }
	public Color OutlineColor { get; set; }
	public Color FillColor { get; set; }

	public bool IsVisible { get; set; } = true;
	public bool IsUI { get; set; }

	public int FillZ { get; set; } = 1;

	public Rect2i Body => new((Vector2i)Position, Size);

	public OutlinedRect(Rect2i body, Color outlineColor, Color fillColor, int zIndex = 0, Object2D? parent = null, bool isUI = false)
	{
		Position = body.Position;
		Size = body.Size;

		OutlineColor = outlineColor;
		FillColor = fillColor;

		ZIndex = zIndex;
		Parent = parent;

		// Extra
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
		Vector2i globalPos = (Vector2i)(Parent == null ? Position : Position + Parent.Position);

		VideoEngine.QueueOutlinedRect(
			ZIndex, ZIndex + FillZ,
			new Rect2i(globalPos, Size),
			OutlineColor, FillColor
		);
	}
}