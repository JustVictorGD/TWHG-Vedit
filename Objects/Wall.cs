namespace WhgVedit.Objects;

using Raylib_cs;

using Engine.Video;
using Types;

class Wall : Object2D
{
	public const int OutlineWidth = 6;
	public const int HalfWidth = OutlineWidth / 2;

	// TODO: Cache the individual rects that make up the outline IF the fill
	// is completely opaque, to avoid recomputing them every frame. They
	// should only be recomputed when resizing the wall, not moving it.
	private readonly List<Rect2i> outlineRects = [];


	public Color OutlineColor { get; set; } = new(72, 72, 102);
	public Color FillColor { get; set; } = new(179, 179, 255);

	public Vector2i Size { get; set; }

	// QoL getters.
	public Vector2i Start => (Vector2i)Position;
	public Vector2i End => Start + Size;
	public Rect2i Body => new(Start, Size);

	public Wall() { }

	public Wall(int x, int y, int width, int height)
	{
		Position = new(x, y);
		Size = new(width, height);
	}

	public Wall(Vector2i position, Vector2i size)
	{
		Position = position;
		Size = size;
	}

	public override void Draw()
	{
		VideoEngine.QueueOutlinedRect(ZIndex, ZIndex + 1, Body, OutlineColor, FillColor);
	}
}