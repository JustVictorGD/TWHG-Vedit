namespace WhgVedit.Objects;

// You may see that the position sometimes ends in one half.
// This means that its position is one subpixel away from
// crossing into the pixel one above or one to the left.

using Raylib_cs;

using Common;
using Engine.Video;
using Types;

class Player : Object2D
{
	public Vector2i Size { get; set; } = new(42, 42);
	public int Speed { get; set; } = 4;
	public Color OutlineColor { get; set; } = new(102, 0, 0);
	public Color FillColor { get; set; } = new(255, 0, 0);

	public Vector2i HalfSize => Size / 2;

	public Rect2i Body => new((Vector2i)Position - Size / 2, Size);


	public override void Update()
	{
		Position += new Vector2i(
			Speed * Utils.GetInputAxis(KeyboardKey.Left, KeyboardKey.Right),
			Speed * Utils.GetInputAxis(KeyboardKey.Up, KeyboardKey.Down)
		);

		// TODO: Make walls set subpixels to min and max values just like the world border.
		Position += Collision.SuggestWallPushes(Body, Game.Walls);

		Position = Position.Clamp(new(HalfSize, Game.AreaSize * 48 - Size));

		// Un-comment to see subpixels change between 0, -128 and 127.
		//Console.WriteLine(Position.Fraction);
	}

	public override void Draw()
	{
		VideoEngine.QueueOutlinedRect(ZIndex, ZIndex + 1, Body, OutlineColor, FillColor);
	}

	public bool TouchesEnemy(Enemy enemy)
	{
		Circle circle = enemy.Body;

		Vector2i nearestPoint = circle.Position.Clamp(Body);

		int squaredDictance = (int)(
			Math.Pow(nearestPoint.X - circle.Position.X, 2) +
			Math.Pow(nearestPoint.Y - circle.Position.Y, 2)
		);

		return squaredDictance < Math.Pow(circle.Radius, 2);
	}
}