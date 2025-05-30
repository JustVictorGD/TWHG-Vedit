namespace WhgVedit.Types;

readonly struct Rect2i
{
	// Main data.
	public readonly Vector2i Position;
	public readonly Vector2i Size;


	// Alternative ways of getting the edges of the rectangle.
	public readonly Vector2i Start => Position;
	public readonly Vector2i End => Position + Size;
	public readonly Vector2i TopRight => new(End.X, Start.Y);
	public readonly Vector2i BottomLeft => new(Start.X, End.Y);


	// Constructors.
	public Rect2i() { Position = new(); Size = new(); }
	public Rect2i(Vector2i position, Vector2i size) { Position = position; Size = size; }
	public Rect2i(int x, int y, int width, int height)
	{
		Position = new(x, y);
		Size = new(width, height);
	}

	// Editing.
	public readonly Rect2i Move(int x, int y) => new(new(Position.X + x, Position.Y + y), Size);
	public readonly Rect2i Move(Vector2i amount) => Move(amount.X, amount.Y);

	public readonly Rect2i MoveTo(int x, int y) => new(new(x, y), Size);
	public readonly Rect2i MoveTo(Vector2i pos) => MoveTo(pos.X, pos.Y);

	public readonly Rect2i Resize(Vector2i amount) => new(Position, Size + amount);

	public bool Touches(Rect2i other, bool includeEdges = false)
	{
		throw new NotImplementedException();
	}

	public override string ToString() => string.Format($"{{ Position: {Position}, Size: {Size} }}");
}