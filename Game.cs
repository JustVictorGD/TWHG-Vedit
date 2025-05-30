namespace WhgVedit;

using System.Numerics;
using Raylib_cs;

using Common;
using Engine.Video;
using Objects;
using Types;
using WhgVedit.Engine;

class Game
{
	public const int TileSize = 48;
	public static int CircleQuality = 5;
	public static readonly Vector2i AreaSize = new(32, 20); // WHG 4 standard: 32, 20.


	Vector2 camera_pos = new();
	
	Color tile1 = new(0xDD, 0xDD, 0xFF);
	Color tile2 = new(0xF7, 0xF7, 0xFF);

	readonly Player player = new() { Position = new(480, 384) };

	public static readonly List<Wall> Walls = [
		new(525, 525, 246, 54),
		new(525, 621, 246, 54),
		new(537, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(730, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(634, 645, 30, 102)// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
	];

	public Camera2D mainCamera = new() { Zoom = 1 };

	int time = 0;

	//bool zoomedOut = false;
	//ProtoKeyframe keyframe = new(new(72, 72), new(72 + 384, 72), 0.5f);

	readonly Enemy thiccEnemy = new(336, 336);

	public void Ready()
	{
		Scene.Main = new([
			player,
			thiccEnemy,
			new Enemy(252, 252),
			new Enemy(276, 252),
			new Enemy(252, 276),
			new Enemy(276, 276),
		]);

		Scene.Main.Ready();
	}

	public void Process()
	{
		time++;

		// Moving this from Game.cs to Wall.cs involves programming keyframes.
		int wallOffset = Utils.PingPong(time * 7, 96);

		Walls[0].SetY(525 + wallOffset);
		Walls[1].SetY(621 + wallOffset);

		Scene.Main?.Update();
	}

	public void Draw()
	{
		// Camera. Not too proud of the messy looking math.
		const float Drag = 0.0625f;

		Vector2 screenSize = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

		camera_pos = camera_pos * (1 - Drag) + (Vector2)player.Position * Drag;
		mainCamera.Offset = -camera_pos * mainCamera.Zoom + screenSize / 2;

		// Floor.
		for (int x = 0; x < AreaSize.X; x++)
		{
			for (int y = 0; y < AreaSize.Y; y++)
			{
				Color tileColor = (x + y) % 2 == 0 ? tile1 : tile2;

				Raylib.DrawRectangle(
					x * TileSize, y * TileSize,
					TileSize, TileSize,
					tileColor
				);
			}
		}

		// TODO: Give Walls a Draw() function.

		foreach (Wall wall in Walls)
			VideoEngine.DrawRect2i(wall.Body, wall.OutlineColor);

		foreach (Wall wall in Walls)
		{
			Rect2i inner = new(
				wall.Body.Position + Wall.OutlineWidth,
				wall.Body.Size - Wall.OutlineWidth * 2
			);
			VideoEngine.DrawRect2i(inner, wall.FillColor);
		}

		thiccEnemy.Radius = Utils.PingPong(time, 24) * 2 + 13;

		Scene.Main?.Draw();

		VideoEngine.Render();
	}
}