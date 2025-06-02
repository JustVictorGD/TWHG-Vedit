using WhgVedit.Objects.Animation;

namespace WhgVedit;

using Newtonsoft.Json;
using Raylib_cs;
using System.Numerics;

using Common;
using Engine;
using Engine.UI;
using Engine.Video;
using Objects;
using Objects.Shapes;
using Types;

public class Game
{
	public const int TileSize = 48;
	public static int CircleQuality { get; set; } = 5;
	public static readonly Vector2i AreaSize = new(32, 20); // WHG 4 standard: 32, 20.


	Vector2 camera_pos = new();

	Color tile1 = new(0xDD, 0xDD, 0xFF);
	Color tile2 = new(0xF7, 0xF7, 0xFF);

	readonly Player player = new() { Position = new(480, 384) };

	// These will be imported from a level file in the future, maybe.
	public static readonly List<Wall> Walls = [
		new(525, 525, 246, 54),
		new(525, 621, 246, 54),
		new(537, 549, 30, 102) { ZIndex = 12, OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(730, 549, 30, 102) { ZIndex = 12, OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(634, 645, 30, 102) { ZIndex = 12, OutlineColor = new(0, 102, 0), FillColor = new(0, 255, 0) },
		new(189, 93, 54, 54),
	];

	public static readonly List<Enemy> Enemies =
	[
		new(252, 252),
		new(276, 252),
		new(252, 276),
		new(276, 276),

		new(168, 140),
		new(168, 196),
	];

	public Camera2D mainCamera = new() { Zoom = 1 };

	int time = 0;

	//bool zoomedOut = false;
	//ProtoKeyframe keyframe = new(new(72, 72), new(72 + 384, 72), 0.5f);

	readonly Enemy thiccEnemy = new(336, 336);

	private ProtoAnimation protoAnimation = new();
	private Enemy keyframeEnemyTest = new(480, 336);

	public Keyframe Keyframe1 = new Keyframe(0) { Position = new Vector2i(480, 336), EasingFunc = Easings.SineInOut };
	public Keyframe Keyframe2 = new Keyframe(0.5f) { Position = new Vector2i(960, 336), EasingFunc = Easings.SineInOut };
	public Keyframe Keyframe3 = new Keyframe(1) { Position = new Vector2i(480, 336), EasingFunc = Easings.SineInOut };

	readonly List<Button> buttons = [
		new(80, 80, 64, 64),
		new(80, 160, 64, 64),
		new(160, 80, 64, 64),
		new(160, 160, 64, 64),
		new(240, 80, 64, 64),
		new(320, 80, 64, 64),

		new Slider(500, 500, 96, 96),
		new SceneSwitcher(64, 500, 128, 64)
	];

	private Checkpoint checkpoint = new(new Subpixel2(480, 96)) { Size = new Vector2i(96, 96) };
	private Checkpoint checkpoint2 = new(new Subpixel2(672, 96)) { Size = new Vector2i(96, 96) };
	
	public void Ready()
	{
		Scene.Main = new([]);

		Scene.Main.AddObjectsToGroups([player], "Player");
		Scene.Main.AddObjectsToGroups([.. Enemies, thiccEnemy, keyframeEnemyTest], "Enemies");
		Scene.Main.AddObjectsToGroups([.. Walls], "Walls");
		Scene.Main.AddObjectsToGroups([.. buttons], "Buttons");
		Scene.Main.AddObjectsToGroups([checkpoint, checkpoint2], "Checkpoints");

		protoAnimation.Keyframes.AddRange([Keyframe1, Keyframe2, Keyframe3]);

		for (var index = 0; index < protoAnimation.Keyframes.Count; index++)
		{
			var keyframe = protoAnimation.Keyframes[index];
			Console.WriteLine($"Keyframe {index}: Duration: {keyframe.Duration}");
		}

		// JSON to Runtime test.
		string json = File.ReadAllText("Json/Rectangles.json");

		List<ColorRect>? rects = JsonConvert.DeserializeObject<List<ColorRect>>(json);
		List<SolidRect> rectangles = [];

		if (rects != null)
			foreach (ColorRect rect in rects)
			{
				rectangles.Add(new(rect.GetRect(), rect.GetColor(), true));
			}
		
		// The red, green, blue and white rectangles
		// in the corner of the screen are from this.
		Scene.Main.AddObjectsToGroups([.. rectangles], "Rectangles");

		InputAction leftAction = new("Left", [KeyboardKey.A, KeyboardKey.Left]);
		InputAction rightAction = new("Right", [KeyboardKey.D, KeyboardKey.Right]);
		InputAction upAction = new("Up", [KeyboardKey.W, KeyboardKey.Up]);
		InputAction downAction = new("Down", [KeyboardKey.S, KeyboardKey.Down]);
		
		InputEngine.AddActions([leftAction, rightAction, upAction, downAction]);

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

		InputEngine.CheckInputs();
		Scene.Main?.Update();

		if (Scene.Main == null) return;

		foreach (GameObject @object in Scene.Main.GetObjectsInGroup("Enemies"))
			if (@object is Enemy enemy && player.Body.Intersects(enemy.Body))
				player.Die();
	}

	// Draw calls in this function comply with the camera.
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

	// Draw calls in this function ignore the camera.
	public void DrawUI()
	{
		Scene.Main?.DrawUI();

		VideoEngine.Render();
	}
}