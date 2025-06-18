namespace WhgVedit.Objects;

// Used for any axis-aligned rectangle shaped object.

using Types;

public class RectObject : SpacialObject
{
	public virtual Vector2I Size { get; set; } = new(42, 42);

	// In case of odd numbered size, this value rounds down.
	public Vector2I HalfSize => Size / 2;

	// Because the global position is used so often and
	// local so rarely, GlobalBody is just called Body.
	public Rect2I LocalBody => new((Vector2I)Position - HalfSize, Size);
	public Rect2I Body => new((Vector2I)GetGlobalPosition() - HalfSize, Size);
}