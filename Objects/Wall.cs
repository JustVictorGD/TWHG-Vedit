using Raylib_cs;

namespace WhgVedit.Objects;

using WhgVedit.Types;

class Wall : Object2D
{
	public const int OutlineWidth = 6;
	public const int HalfWidth = OutlineWidth / 2;

	// TODO: Cache.
	private readonly List<Rect2i> _outlineRects = [];


	public Color OutlineColor { get; set; } = new(72, 72, 102, 255);
	public Color FillColor { get; set; } = new(179, 179, 255, 255);

	public Vector2i Size { get; set; }

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
}