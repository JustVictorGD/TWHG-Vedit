namespace WhgVedit.Types;

public readonly struct Rect2I
{
	// Main data.
	public readonly Vector2I Position;
	public readonly Vector2I Size;


	// Alternative ways of getting the edges of the rectangle.
	public readonly Vector2I Start => Position;
	public readonly Vector2I End => Position + Size;
	public readonly Vector2I TopRight => new(End.X, Start.Y);
	public readonly Vector2I BottomLeft => new(Start.X, End.Y);


	// Constructors.
	public Rect2I() { Position = new(); Size = new(); }
	public Rect2I(Vector2I position, Vector2I size) { Position = position; Size = size; }
	public Rect2I(int x, int y, int width, int height)
	{
		Position = new(x, y);
		Size = new(width, height);
	}

	// Collision.
	public bool Intersects(Rect2I other)
	{
		bool overlapOnX = Position.X < other.End.X && other.Position.X < End.X;
		bool overlapOnY = Position.Y < other.End.Y && other.Position.Y < End.Y;

		return overlapOnX && overlapOnY;
	}

	public bool Intersects(Circle circle)
	{
		Vector2I nearestPoint = circle.Position.Clamp(this);

		int squaredDictance = (int)(
			Math.Pow(nearestPoint.X - circle.Position.X, 2) +
			Math.Pow(nearestPoint.Y - circle.Position.Y, 2)
		);

		return squaredDictance < Math.Pow(circle.Radius, 2);
	}

	// Editing.
	public readonly Rect2I Move(int x, int y) => new(new(Position.X + x, Position.Y + y), Size);
	public readonly Rect2I Move(Vector2I amount) => Move(amount.X, amount.Y);

	public readonly Rect2I MoveTo(int x, int y) => new(new(x, y), Size);
	public readonly Rect2I MoveTo(Vector2I pos) => MoveTo(pos.X, pos.Y);

	public readonly Rect2I Resize(Vector2I amount) => new(Position, Size + amount);

	public override string ToString() => $"{{ Position: {Position}, Size: {Size} }}";
}