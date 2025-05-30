namespace WhgVedit;

using System.Numerics;
using Raylib_cs;

using Common;
using Engine.Video;
using Objects;
using Objects.Animation;
using Objects.Player;
using Types;

class Game
{
	public const int TileSize = 48;
	public static int CircleQuality = 5;

	public Dictionary<string, List<GameObject>> Groups { get; set; } = [];

	Vector2 camera_pos = new();
	readonly Vector2i areaSize = new(32, 20); // Original: 32, 20
	Color tile1 = new(0xDD, 0xDD, 0xFF);
	Color tile2 = new(0xF7, 0xF7, 0xFF);

	readonly Player player = new() { Position = new(480, 384), Speed = 8 };

	readonly List<Wall> walls = [
		new(525, 525, 246, 54),
		new(525, 621, 246, 54),
		new(537, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(730, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(634, 645, 30, 102)// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
	];

	public Camera2D mainCamera = new() { Zoom = 1 };

	int time = 0;
	//bool zoomedOut = false;

	ProtoKeyframe keyframe = new(new(72, 72), new(72 + 384, 72), 0.5f);

	readonly List<Enemy> enemies = [
		new(252, 252),
		new(276, 252),
		new(252, 276),
		new(276, 276),
	];
	readonly Enemy thiccEnemy = new(336, 336);

	public void Ready()
	{
		List<Subpixel> subpixels = [
			4,
			new(Subpixel.MinFromWhole(4), true),
			new(Subpixel.MaxFromWhole(4), true),
		];

		foreach (Subpixel subpixel in subpixels)
			Console.WriteLine($"{subpixel.Steps}, {subpixel.Rounded}, {subpixel.Fraction}");

		for (int i = 0; i < 24; i++)
			Console.WriteLine($"{i - 12} -> {Utils.PushToZero(i - 12, 5)}");
	}

	// I'm not proud of camera work here. Stuff in this function is messy in general.
	public void Process()
	{
		const float Drag = 0.0625f;

		time++;

		player.Position += new Subpixel2(
			player.Speed * Utils.GetInputAxis(KeyboardKey.Left, KeyboardKey.Right),
			player.Speed * Utils.GetInputAxis(KeyboardKey.Up, KeyboardKey.Down)
		);

		Vector2 screenSize = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

		camera_pos = camera_pos * (1 - Drag) + (Vector2)player.Position * Drag;
		mainCamera.Offset = -camera_pos * mainCamera.Zoom + screenSize / 2;
		

		int wallOffset = Utils.PingPong(time * 7, 96);

		walls[0].Body = walls[0].Body.MoveTo(525, 525 + wallOffset);
		walls[1].Body = walls[1].Body.MoveTo(525, 621 + wallOffset);

		player.Position += Collision.SuggestWallPushes(player.Body, walls);

		player.Position = player.Position.Clamp(new(-player.HalfSize, areaSize * 48 - player.Size));
	}


	public void Draw()
	{
		// Floor.
		for (int x = 0; x < areaSize.X; x++)
		{
			for (int y = 0; y < areaSize.Y; y++)
			{
				Color tileColor = (x + y) % 2 == 0 ? tile1 : tile2;

				Raylib.DrawRectangle(
					x * TileSize, y * TileSize,
					TileSize, TileSize,
					tileColor
				);
			}
		}

		foreach (Wall wall in walls)
			VideoEngine.DrawRect2i(wall.Body, wall.OutlineColor);

		foreach (Wall wall in walls)
		{
			Rect2i inner = new(
				wall.Body.Position + Wall.OutlineWidth,
				wall.Body.Size - Wall.OutlineWidth * 2
			);
			VideoEngine.DrawRect2i(inner, wall.FillColor);
		}

		foreach (Enemy enemy in enemies)
			enemy.Draw();

		thiccEnemy.Radius = Utils.PingPong(time, 24) * 2 + 13;
		thiccEnemy.Draw();

		Color outline = new(102, 0, 0);
		Color fill = new(255, 0, 0);

		VideoEngine.DrawOutlinedRectangle(player.Body, outline, fill);



		int offset = Utils.PingPong(time * 2, 72);


		Vector2i pos = keyframe.GetPos(time / 60f);

		Raylib.DrawPoly(pos, 16, 24, 0, Color.Red);


		VideoEngine.Render();
	}
}