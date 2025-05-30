using WhgVedit.Types;

namespace WhgVedit.Objects;

class Object2D : GameObject
{
	public int ZIndex { get; set; } = 0;
	public Subpixel2 Position { get; set; } = new();
}