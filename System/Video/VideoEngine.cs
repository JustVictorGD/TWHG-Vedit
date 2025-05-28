using Raylib_cs;
using WhgVedit.Objects;
using WhgVedit.Types;

namespace WhgVedit.Video;

class VideoEngine
{
	const int OutlineWidth = 6;

	private static VideoEngine _instance = new();

	private readonly List<ShapeCall> _drawCalls = [];

	public static void QueueDraw(ShapeCall drawCall)
	{
		_instance._drawCalls.Add(drawCall);
	}

	public static void Render()
	{
		foreach (ShapeCall shapeCall in _instance._drawCalls.OrderBy(o => o.ZIndex))
			shapeCall.Execute();

		_instance._drawCalls.Clear();
	}


	// Utility draw functions.


	public static void DrawOutlinedRectangle(Rect2i dimensions, Color outlineColor, Color fillColor)
	{
		Rect2i inner = new(
		    dimensions.Position + Wall.OutlineWidth,
		    dimensions.Size - Wall.OutlineWidth * 2
		);

		// If the fill color takes up zero area, just draw the outline.
		if (inner.Size.X <= 0 || inner.Size.Y <= 0)
			DrawRect2i(dimensions, outlineColor);

		// If fill color is completely opaque, there's no point in drawing a ring-like outline.
		else if (fillColor.A == 255)
		{
			DrawRect2i(dimensions, outlineColor);
			DrawRect2i(inner, fillColor);
		}
		else // This case is where things get fancy.
		{
			DrawOutline(dimensions, outlineColor, inner);
			DrawRect2i(inner, fillColor);
		}
	}

	public static void DrawOutline(Rect2i outer, Color color)
	{
		Rect2i inner = new(
			outer.Position + Wall.OutlineWidth,
			outer.Size - Wall.OutlineWidth * 2
		);

		DrawOutline(outer, color, inner);
	}
	public static void DrawOutline(Rect2i outer, Color color, Rect2i inner)
	{
		DrawRectFromCorners(outer.Start, inner.TopRight, color);
		DrawRectFromCorners(inner.Start, outer.BottomLeft, color);
		DrawRectFromCorners(inner.BottomLeft, outer.End, color);
		DrawRectFromCorners(inner.End, outer.TopRight, color);
	}

	public static void DrawRectFromCorners(Vector2i pos1, Vector2i pos2, Color color)
	{
		Vector2i start = new(
			Math.Min(pos1.X, pos2.X),
			Math.Min(pos1.Y, pos2.Y)
		);
		Vector2i size = new(
			Math.Abs(pos1.X - pos2.X),
			Math.Abs(pos1.Y - pos2.Y)
		);

		if (size.X != 0 && size.Y != 0)
			Raylib.DrawRectangle(start.X, start.Y, size.X, size.Y, color);
	}

	public static void DrawRect2i(Rect2i rect, Color color)
	{
		Raylib.DrawRectangle(
			rect.Position.X,
			rect.Position.Y,
			rect.Size.X,
			rect.Size.Y,
			color
		);
	}
}