using WhgVedit.Objects.Animation;

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

	// These will be imported from a level file in the future, maybe.
	public static readonly List<Wall> Walls = [
		new(525, 525, 246, 54),
		new(525, 621, 246, 54),
		new(537, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(730, 549, 30, 102),// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(634, 645, 30, 102)// { OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
	];

	public static readonly List<Enemy> Enemies =
	[
		new(252, 252),
		new(276, 252),
		new(252, 276),
		new(276, 276)
	];

	public Camera2D mainCamera = new() { Zoom = 1 };

	int time = 0;

	//bool zoomedOut = false;
	//ProtoKeyframe keyframe = new(new(72, 72), new(72 + 384, 72), 0.5f);

	readonly Enemy thiccEnemy = new(336, 336);

	private ProtoAnimation protoAnimation = new();
	private Enemy keyframeEnemyTest = new(480, 336);

	public Keyframe Keyframe1 = new Keyframe(0) { Position = new Vector2i(480, 336) };
	public Keyframe Keyframe2 = new Keyframe(2) { Position = new Vector2i(528, 336) };
	public Keyframe Keyframe3 = new Keyframe(3) { Position = new Vector2i(528, 336) };

	public void Ready()
	{
		Scene.Main = new([player]);

		Scene.Main.AddObjectsToGroups([..Enemies, thiccEnemy, keyframeEnemyTest], "Enemies");
		Scene.Main.AddObjectsToGroups([..Walls], "Walls");
		
		protoAnimation.Keyframes.AddRange([Keyframe1, Keyframe2, Keyframe3]);

		for (var index = 0; index < protoAnimation.Keyframes.Count; index++)
		{
			var keyframe = protoAnimation.Keyframes[index];
			Console.WriteLine($"Keyframe {index}: Duration: {keyframe.Duration}");
		}

		// Lots of debugging code here.
		
		/*Console.WriteLine("get length to index 0: " + protoAnimation.GetLengthTo(0)); // 0
		Console.WriteLine("get length to index 1: " + protoAnimation.GetLengthTo(1)); // 2
		Console.WriteLine("get length to index 1: " + protoAnimation.GetLengthTo(2)); // 5

		Console.WriteLine("get last keyframe index at t = 0: " + protoAnimation.GetLastKeyframeIndexAt(0)); //  0
		Console.WriteLine("get last keyframe index at t = 0.5: " + protoAnimation.GetLastKeyframeIndexAt(0.5)); // 0
		Console.WriteLine("get last keyframe index at t = 1: " + protoAnimation.GetLastKeyframeIndexAt(1)); // 0
		Console.WriteLine("get last keyframe index at t = 2: " + protoAnimation.GetLastKeyframeIndexAt(2)); // 1
		Console.WriteLine("get last keyframe index at t = 4: " + protoAnimation.GetLastKeyframeIndexAt(4)); // 1
		Console.WriteLine("get last keyframe index at t = 5: " + protoAnimation.GetLastKeyframeIndexAt(5)); // 2
		
		Console.WriteLine("Position at t = 0:" + protoAnimation.GetPosition(0)); // 480, 336
		Console.WriteLine("Position at t = 0.5:" + protoAnimation.GetPosition(0.5));
		Console.WriteLine("Position at t = 1:" + protoAnimation.GetPosition(1));
		Console.WriteLine("Position at t = 2:" + protoAnimation.GetPosition(2)); // 528, 336
		Console.WriteLine("Position at t = 4:" + protoAnimation.GetPosition(4)); // 528, 336
		Console.WriteLine("Position at t = 5:" + protoAnimation.GetPosition(5)); // 528, 336*/
		
		Scene.Main.Ready();
	}

	public void Process()
	{
		time++;

		// Moving this from Game.cs to Wall.cs involves programming keyframes.
		int wallOffset = Utils.PingPong(time * 7, 96);

		Walls[0].SetY(525 + wallOffset);
		Walls[1].SetY(621 + wallOffset);

		keyframeEnemyTest.Position = protoAnimation.GetPosition(time / 60.0);
		
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

		thiccEnemy.Radius = Utils.PingPong(time, 24) * 2 + 13;

		Scene.Main?.Draw();

		VideoEngine.Render();
	}
}