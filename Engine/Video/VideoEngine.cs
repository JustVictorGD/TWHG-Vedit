namespace WhgVedit.Engine.Video;

using Raylib_cs;

using Engine.Video.Shapes;
using Objects;
using Types;

// Class description.

public static class VideoEngine
{
	private static readonly List<ShapeCall> drawCalls = [];

	public static void Render()
	{
		foreach (ShapeCall shapeCall in drawCalls.OrderBy(o => o.ZIndex))
			shapeCall.Execute();

		drawCalls.Clear();
	}

	public static void QueueDraw(ShapeCall drawCall)
	{
		drawCalls.Add(drawCall);
	}

	public static int QueueOutlinedRect(ZIndex outlineZ, ZIndex fillZ, Rect2I outer, Color outlineColor, Color fillColor)
	{
		fillZ += outlineZ;

		Rect2I inner = GetInner(outer);

		if (fillColor.A >= 255 && fillZ >= outlineZ)
		{
			QueueDraw(new RectCall(outlineZ, outer, outlineColor));
			QueueDraw(new RectCall(fillZ, inner, fillColor));

			// You can print the return values to debug which case gets chosen.
			return 0;
		}
		else if (inner.Size.X <= 0 || inner.Size.Y <= 0)
		{
			QueueDraw(new RectCall(outlineZ, outer, outlineColor));

			return 1;
		}
		else
		{
			QueueDraw(new OutlineCall(outlineZ, outer, outlineColor, inner));
			QueueDraw(new RectCall(fillZ, inner, fillColor));

			return 2;
		}
	}

	public static int QueueOutlinedCircle(ZIndex outlineZ, ZIndex fillZ, Circle shape, Color outlineColor, Color fillColor)
	{
		fillZ += outlineZ;

		int sides = (int)Math.Log2(shape.Radius + 1) * Game.CircleQuality;

		if (fillColor.A >= 255 && fillZ >= outlineZ)
		{
			QueueDraw(new CircleCall(fillZ, shape.Position, shape.Radius - Wall.OutlineWidth, fillColor, sides));
			QueueDraw(new CircleCall(outlineZ, shape, outlineColor, sides));

			return 0;
		}
		else if (shape.Radius <= Wall.OutlineWidth)
		{
			QueueDraw(new CircleCall(outlineZ, shape.Position, shape.Radius, outlineColor, sides));

			return 1;
		}
		else
		{
			QueueDraw(new RingCall(outlineZ, shape, outlineColor, sides));
			QueueDraw(new CircleCall(fillZ, shape.Position, shape.Radius - Wall.OutlineWidth, fillColor, sides));

			return 2;
		}
	}

	// Utility function for a frequent action throughout drawing.
	// Negative size may occur, but in drawing it's not a problem.
	public static Rect2I GetInner(Rect2I outer) => new(
		outer.Position + Wall.OutlineWidth,
		outer.Size - Wall.OutlineWidth * 2
	);

	// Utility draw functions.
	public static void DrawOutlinedRect(Rect2I dimensions, Color outlineColor, Color fillColor)
	{
		Rect2I inner = GetInner(dimensions);

		// If the fill color takes up zero area, just draw the outline.
		if (inner.Size.X <= 0 || inner.Size.Y <= 0)
			DrawRect2I(dimensions, outlineColor);

		// If fill color is completely opaque, there's no point in drawing a ring-like outline.
		else if (fillColor.A == 255)
		{
			DrawRect2I(dimensions, outlineColor);
			DrawRect2I(inner, fillColor);
		}
		else // This case is where things get fancy.
		{
			DrawOutline(dimensions, outlineColor, inner);
			DrawRect2I(inner, fillColor);
		}
	}

	public static void DrawOutline(Rect2I outer, Color color)
	{
		Rect2I inner = new(
			outer.Position + Wall.OutlineWidth,
			outer.Size - Wall.OutlineWidth * 2
		);

		DrawOutline(outer, color, inner);
	}
	public static void DrawOutline(Rect2I outer, Color color, Rect2I inner)
	{
		DrawRectFromCorners(outer.Start, inner.TopRight, color);
		DrawRectFromCorners(inner.Start, outer.BottomLeft, color);
		DrawRectFromCorners(inner.BottomLeft, outer.End, color);
		DrawRectFromCorners(inner.End, outer.TopRight, color);
	}

	public static void DrawRectFromCorners(Vector2I pos1, Vector2I pos2, Color color)
	{
		Vector2I start = new(
			Math.Min(pos1.X, pos2.X),
			Math.Min(pos1.Y, pos2.Y)
		);
		Vector2I size = new(
			Math.Abs(pos1.X - pos2.X),
			Math.Abs(pos1.Y - pos2.Y)
		);

		if (size.X != 0 && size.Y != 0)
			Raylib.DrawRectangle(start.X, start.Y, size.X, size.Y, color);
	}

	public static void DrawRect2I(Rect2I rect, Color color)
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