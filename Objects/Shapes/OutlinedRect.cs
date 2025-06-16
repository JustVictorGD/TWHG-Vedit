namespace WhgVedit.Objects.Shapes;

// I'm not expecting shapes to be drawn by the scene, it seems
// cleaner to do it manually inside the parent objects' scripts.

using Raylib_cs;

using Engine.Video;
using Types;

public class OutlinedRect : RectObject
{
	public Color OutlineColor { get; set; }
	public Color FillColor { get; set; }

	public int FillZ { get; set; } = 1;

	public OutlinedRect(Vector2i size, Color outlineColor, Color fillColor, int outlineZ = 0, int fillZ = 1, bool isUI = false)
	{
		Size = size;

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
		VideoEngine.QueueOutlinedRect(GetGlobalZ(), FillZ, Body, OutlineColor, FillColor);
	}
}