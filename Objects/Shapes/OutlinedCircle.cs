namespace WhgVedit.Objects.Shapes;

// I'm not expecting shapes to be drawn by the scene, it seems
// cleaner to do it manually inside the parent objects' scripts.

using Raylib_cs;

using Engine.Video;
using Types;

public class OutlinedCircle : SpacialObject
{
	public double Radius { get; set; }
	public Circle Body => new((Vector2I)GetGlobalPosition(), Radius);

	public Color OutlineColor { get; set; }
	public Color FillColor { get; set; }

	public ZIndex FillZ { get; set; } = new(1);

	public OutlinedCircle(double radius, Color outlineColor, Color fillColor, ZIndex outlineZ = new(), ZIndex fillZ = new(), bool isUI = false)
	{
		Radius = radius;

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
		VideoEngine.QueueOutlinedCircle(
			GetGlobalZ(), FillZ,
			Body,
			OutlineColor, FillColor
		);
	}
}