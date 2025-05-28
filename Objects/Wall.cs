using Raylib_cs;

namespace WhgVedit.Objects;

using WhgVedit.Types;

class Wall
{
	public const int OutlineWidth = 6;
	public const int HalfWidth = OutlineWidth / 2;

	private readonly List<Rect2i> _outlineRects = [];

	public Rect2i Body { get; set; }

	public Vector2i Position => Body.Position;
	public Vector2i Size => Body.Size;
	public Vector2i End => Position + Size;


	public Color OutlineColor { get; set; } = new(72, 72, 102, 255);
	public Color FillColor { get; set; } = new(179, 179, 255, 255);

	public Wall() { }

	public Wall(int x, int y, int width, int height)
	{
		Body = new(x, y, width, height);
	}

	public Wall(Vector2i position, Vector2i size)
	{
		Body = new(position, size);
	}
}