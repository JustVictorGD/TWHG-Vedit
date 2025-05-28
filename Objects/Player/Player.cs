namespace WhgVedit.Objects.Player;

using WhgVedit.Types;

// This is not an Object2D because the player's position uses subpixels, and it would be
// weird to override the Position property to Subpixel2 when it's expected to be Vector2i.
class Player : GameObject
{
	public Subpixel2 Position { get; set; } = new();
	public Vector2i Size { get; set; } = new(42, 42);
	public int Speed { get; set; } = 4;

	public Vector2i HalfSize => Size / 2;

	public Rect2i Body => new((Vector2i)Position + Size / 2, Size);


}