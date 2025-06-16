namespace WhgVedit.Objects;

// Used for any axis-aligned rectangle shaped object.

using Types;

public class RectObject : SpacialObject
{
	public virtual Vector2i Size { get; set; } = new(42, 42);

	// In case of odd numbered size, this value rounds down.
	public Vector2i HalfSize => Size / 2;

	// Because the global position is used so often and
	// local so rarely, GlobalBody is just called Body.
	public Rect2i LocalBody => new((Vector2i)Position - HalfSize, Size);
	public Rect2i Body => new((Vector2i)GetGlobalPosition() - HalfSize, Size);
}