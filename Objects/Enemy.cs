using System.Numerics;
using Raylib_cs;
using WhgVedit.Common;
using WhgVedit.Engine.Video;
using WhgVedit.Engine.Video.Shapes;
using WhgVedit.Types;

namespace WhgVedit.Objects;

public class Enemy : Object2D
{
	public const int DefaultRadius = 13;

	public int Radius => Utils.Round(13 * Scale.X);

	public Color OutlineColor { get; set; } = new(0, 0, 66);
	public Color FillColor { get; set; } = new(0, 0, 255);

	public Circle Body => new((Vector2i)Position, Radius - Wall.OutlineWidth);

	public Enemy(int posX, int posY)
	{
		Position = new(posX, posY);
	}

	public Enemy(Subpixel2 position)
	{
		Position = position;
	}

	public override void Draw()
	{
		VideoEngine.QueueDraw(new CircleCall(ZIndex,
			OutlineColor, (Vector2i)Position, Radius));

		VideoEngine.QueueDraw(new CircleCall(ZIndex + 1,
			FillColor, (Vector2i)Position, Radius - 6));
	}
}