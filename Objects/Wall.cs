using Newtonsoft.Json.Linq;
using WhgVedit.Common;

namespace WhgVedit.Objects;

using Raylib_cs;

using Engine.Video;
using Types;

public class Wall : RectObject
{
	public static readonly ZIndex DefaultZ = new(1000);
	public override ZIndex ZIndex { get; set; } = new(1000);

	public const int OutlineWidth = 6;
	public const int HalfWidth = OutlineWidth / 2;

	// TODO: Cache the individual rects that make up the outline IF the fill
	// is completely opaque, to avoid recomputing them every frame. They
	// should only be recomputed when resizing the wall, not moving it.
	private readonly List<Rect2I> outlineRects = [];

	public Color OutlineColor { get; set; } = new(72, 72, 102);
	public Color FillColor { get; set; } = new(179, 179, 255);

	public Wall() { }

	public Wall(int x, int y, int width, int height)
	{
		Position = new(x, y);
		Size = new(width, height);
	}

	public Wall(Vector2I position, Vector2I size)
	{
		Position = position;
		Size = size;
	}

	public override void Draw()
	{
		VideoEngine.QueueOutlinedRect(ZIndex, new(1), Body, OutlineColor, FillColor);
	}
	
	public override JObject ToJson()
	{
		JObject jObject = base.ToJson();
		jObject.Add("rect", new JArray(Position.X.Rounded, Position.Y.Rounded, Size.X, Size.Y));
		if (ZIndex != DefaultZ)
		{
			jObject.Add("zIndex", ZIndex.Value);
		}

		if (Utils.AreColorsEqual(OutlineColor, new(72, 72, 102)))
		{
			jObject.Add("outlineColor",
				new JArray { OutlineColor.R, OutlineColor.G, OutlineColor.B });
		}

		if (Utils.AreColorsEqual(FillColor, new(179, 179, 255)))
		{
			jObject.Add("fillColor", 
				new JArray { FillColor.R, FillColor.G, FillColor.B });
		}
		return jObject;
	}
}