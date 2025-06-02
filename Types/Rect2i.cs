namespace WhgVedit.Types;

public readonly struct Rect2i
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

	// Collision.
	public bool Intersects(Rect2i other)
	{
		bool overlapOnX = Position.X < other.End.X && other.Position.X < End.X;
		bool overlapOnY = Position.Y < other.End.Y && other.Position.Y < End.Y;

		return overlapOnX && overlapOnY;
	}

	public bool Intersects(Circle circle)
	{
		Vector2i nearestPoint = circle.Position.Clamp(this);

		int squaredDictance = (int)(
			Math.Pow(nearestPoint.X - circle.Position.X, 2) +
			Math.Pow(nearestPoint.Y - circle.Position.Y, 2)
		);

		return squaredDictance < Math.Pow(circle.Radius, 2);
	}

	// Editing.
	public readonly Rect2i Move(int x, int y) => new(new(Position.X + x, Position.Y + y), Size);
	public readonly Rect2i Move(Vector2i amount) => Move(amount.X, amount.Y);

	public readonly Rect2i MoveTo(int x, int y) => new(new(x, y), Size);
	public readonly Rect2i MoveTo(Vector2i pos) => MoveTo(pos.X, pos.Y);

	public readonly Rect2i Resize(Vector2i amount) => new(Position, Size + amount);

	public override string ToString() => $"{{ Position: {Position}, Size: {Size} }}";
}