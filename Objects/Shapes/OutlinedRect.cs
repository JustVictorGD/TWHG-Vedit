namespace WhgVedit.Objects.Shapes;

// I'm not expecting shapes to be drawn by the scene, it seems
// cleaner to do it manually inside the parent objects' scripts.

using Raylib_cs;

using Engine.Video;
using Types;

public class OutlinedRect : SpacialObject
{
	public Vector2i Size { get; set; }
	public Rect2i Body => new((Vector2i)Position, Size);

	public Color OutlineColor { get; set; }
	public Color FillColor { get; set; }

	public int FillZ { get; set; } = 1;

	public OutlinedRect(Rect2i body, Color outlineColor, Color fillColor, int outlineZ = 0, int fillZ = 1, bool isUI = false)
	{
		Position = body.Position;
		Size = body.Size;

		OutlineColor = outlineColor;
		FillColor = fillColor;

		ZIndex = outlineZ;
		FillZ = fillZ;

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

		VideoEngine.QueueOutlinedRect(
			GetGlobalZ(), FillZ,
			new Rect2i(globalPos, Size),
			OutlineColor, FillColor
		);
	}
}