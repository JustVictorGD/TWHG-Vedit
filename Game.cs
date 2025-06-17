namespace WhgVedit;

using Raylib_cs;
using System.Numerics;

using Common;
using Engine;
using Engine.Video;
using Json;
using Objects;
using Objects.Animation;
using Objects.UI;
using Types;

public class Game
{
	int time = 0;

	private const int cursorGridSize = 12;
	public const int TileSize = 48;
	public static readonly Vector2I AreaSize = new(32, 20); // WHG 4 standard: 32, 20.
	
	// Rendering.
	public static int CircleQuality { get; set; } = 5;
	Vector2 camera_pos = new();
	public static Camera2D WorldCamera { get; set; } = new() { Zoom = 1 };
	public static Camera2D UICamera { get; set; } = new() { Zoom = 1, Target = new(640, 360) };
	Color tile1 = new(0xDD, 0xDD, 0xFF);
	Color tile2 = new(0xF7, 0xF7, 0xFF);
	// Rendering end.

	readonly Player player = new() { Position = new(480, 384) };

	public List<GameObject> NewObjects { get; set; } = [];

	public List<GameObject> ObjectsToRemove { get; set; } = [];
	
	// These will be imported from a level file.
	public List<GameObject> GameObjects { get; set; }

	public static List<Wall> Walls = [];
	public static List<Enemy> Enemies = [];

	//bool zoomedOut = false;
	//ProtoKeyframe keyframe = new(new(72, 72), new(72 + 384, 72), 0.5f);

	/*private static Animation thiccEnemyAnimation = new();
	private AnimationPlayer thiccEnemyAnimationPlayer = new(thiccEnemyAnimation);
	readonly Enemy thiccEnemy = new(336, 336);

	public Keyframe keyframe4 = new Keyframe(0) { Position = new(336, 336), Scale = Vector2.One };
	public Keyframe keyframe5 = new Keyframe(1) { Position = new(336, 336), Scale = 5 * Vector2.One };
		
	private static Animation keyframeEnemyAnimation = new();
	private AnimationPlayer keyframeEnemyAnimationPlayer = new(keyframeEnemyAnimation);
	private Enemy keyframeEnemyTest = new(480, 336);

	private Keyframe keyframe1 = new Keyframe(0) { Position = new Vector2i(480, 336), Scale = Vector2.One, EasingFunc = Easings.SineInOut };
	private Keyframe keyframe2 = new Keyframe(0.5f) { Position = new Vector2i(960, 336), Scale = 10 * Vector2.One, EasingFunc = Easings.SineInOut };
	private Keyframe keyframe3 = new Keyframe(1) { Position = new Vector2i(480, 336), Scale = Vector2.One, EasingFunc = Easings.SineInOut };
	*/
	readonly List<Button> buttons = [
		new Slider(128, 128, 48, 48),
		new Slider(256, 128, 48, 48) { ZIndex = new(-16) },
		new Slider(256, 256, 48, 48, false),
		new Slider(384, 256, 48, 48, false) { ZIndex = new(-16) }
	];

	public static List<Checkpoint> Checkpoints = [];
	/*private Checkpoint checkpoint = new(new Subpixel2(480, 96)) { Size = new Vector2i(96, 96) };
	private Checkpoint checkpoint2 = new(new Subpixel2(672, 96)) { Size = new Vector2i(96, 96) };
	*/


	public static Vector2 GetMousePosition(bool isUI)
	{
		Camera2D camera = isUI ? UICamera : WorldCamera;
		return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
	}

	private readonly List<CursorCatcher> cursorAreas = [
		new(new(1536, 960), false, "Floor"),
		new(new(96, 96), false, "Checkpoint 1"),
		new(new(96, 96), false, "Checkpoint 2"),
	];
	
	public void Ready()
	{
		ObjectParser parser = new("Json/Scene.json");
		parser.Parse();
		GameObjects = parser.GetObjects();
		Walls = parser.GetObjectsOfType<Wall>();
		Enemies = parser.GetObjectsOfType<Enemy>();
		Checkpoints = parser.GetObjectsOfType<Checkpoint>();

		Scene.Main = new([]);

		// Add parsed objects
		Scene.Main.AddObjectsToGroups(Walls, "Walls");
		Scene.Main.AddObjectsToGroups(Enemies, "Enemies");
		Scene.Main.AddObjectsToGroups(Checkpoints, "Checkpoints");

		var animationPlayers = parser.GetObjectsOfType<AnimationPlayer>();
		Scene.Main.AddObjectsToGroups(animationPlayers, "AnimationPlayers");

		Scene.Main.AddObjectsToGroups([player], "Player");
		//Scene.Main.AddObjectsToGroups([.. Walls], "Walls");
		Scene.Main.AddObjectsToGroups([.. buttons], "Buttons");

		Scene.Main.AddObjectsToGroups([.. cursorAreas], "CursorAreas"); // Beta.

		/*keyframeEnemyAnimation.Keyframes.AddRange([keyframe1, keyframe2, keyframe3]);
		Scene.Main.AddObject(keyframeEnemyAnimationPlayer);
		keyframeEnemyAnimationPlayer.Parent = keyframeEnemyTest;

		thiccEnemyAnimation.Keyframes.AddRange([keyframe4, keyframe5]);
		Scene.Main.AddObject(thiccEnemyAnimationPlayer);
		thiccEnemyAnimationPlayer.Parent = thiccEnemy;*/

		/*for (var index = 0; index < keyframeEnemyAnimation.Keyframes.Count; index++)
		{
			var keyframe = keyframeEnemyAnimation.Keyframes[index];
			Console.WriteLine($"Keyframe {index}: Duration: {keyframe.Duration}");
		}*/

		InputAction leftAction = new("Left", [KeyboardKey.A, KeyboardKey.Left]);
		InputAction rightAction = new("Right", [KeyboardKey.D, KeyboardKey.Right]);
		InputAction upAction = new("Up", [KeyboardKey.W, KeyboardKey.Up]);
		InputAction downAction = new("Down", [KeyboardKey.S, KeyboardKey.Down]);

		// Not sure about the naming of these 2 actions yet. Also escape still closes the game.
		InputAction returnAction = new("Return", [KeyboardKey.Escape, KeyboardKey.Backspace]);
		InputAction continueAction = new("Continue", [KeyboardKey.Space, KeyboardKey.Enter]);

		InputAction saveAction = new("Save", [KeyboardKey.E]);


		InputEngine.AddActions([leftAction, rightAction, upAction, downAction, returnAction, continueAction, saveAction]);

		Scene.Main.Ready();
	}


	public void Update()
	{
		List<Enemy> enemies = Scene.Main != null ? [.. Scene.Main.GetObjectsInGroup("Enemies").Cast<Enemy>()] : [];

		Vector2I mousePos = Utils.Round(GetMousePosition(false)).SnapToGrid(cursorGridSize);

		if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Scene.Main != null)
		{
			Enemy newEnemy = new() { Position = mousePos, Groups = ["Enemies"] };
			Scene.Main.AddObjectsToGroups([newEnemy], "Enemies");
			NewObjects.Add(newEnemy);
		}

		if (Raylib.IsMouseButtonDown(MouseButton.Right) && Scene.Main != null)
		{
			foreach (Enemy enemy in enemies)
				if (enemy.Position == mousePos)
				{
					ObjectsToRemove.Add(enemy);
					Scene.Main.RemoveObject(enemy);
					NewObjects.Remove(enemy);
				}
			
		}
		
		time++;

		// Moving this from Game.cs to Wall.cs involves programming keyframes.
		int wallOffset = Utils.PingPong(time * 7, 96);

		Walls[0].SetY(408 + wallOffset);
		Walls[1].SetY(504 + wallOffset);

		//keyframeEnemyTest.Position = _animation.GetPosition(time / 60.0);
		//keyframeEnemyTest.Radius = Utils.Round(13 * (_animation.GetScale(time / 60.0).X + 1));

		InputEngine.CheckInputs();

		HandleCursorAreas();
		HandleButtons();

		Scene.Main?.Update();

		if (Scene.Main == null) return;

		foreach (GameObject @object in Scene.Main.GetObjectsInGroup("Enemies"))
			if (@object is Enemy enemy && player.Body.Intersects(enemy.Hitbox))
				player.Die();

		if (InputEngine.GetAction("Save").IsActive)
		{
			ObjectSaver saver = new("Json/Scene.json", Scene.Main.GameObjects, []);

			saver.AddNewObjectsToJson(NewObjects);
			saver.DeleteObjectsFromJson(ObjectsToRemove);
			saver.Save();
			
			NewObjects = [];
			ObjectsToRemove = [];
			
			Console.WriteLine("SAVED!");
		}
			
			
	}

	// Draw calls in this function comply with the camera.
	public void Draw()
	{
		// Camera. Not too proud of the messy looking math.
		const float Drag = 0.0625f;

		Vector2 screenSize = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

		camera_pos = camera_pos * (1 - Drag) + (Vector2)player.Position * Drag;

		WorldCamera = new()
		{
			Offset = -camera_pos * WorldCamera.Zoom + screenSize / 2,
			Zoom = WorldCamera.Zoom
		};

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

		//thiccEnemy.Scale = Utils.PingPong(time, 24) * Vector2.One;

		Scene.Main?.Draw();
		VideoEngine.Render();

		Vector2I mousePos = Utils.Round(GetMousePosition(false)).SnapToGrid(cursorGridSize);
		Raylib.DrawCircle(mousePos.X, mousePos.Y, 13, new(255, 192, 96, 128));
	}

	// Draw calls in this function ignore the camera.
	public void DrawUI()
	{
		Scene.Main?.DrawUI();

		VideoEngine.Render();
	}

	// Beta.
	private void HandleCursorAreas()
	{
		foreach (CursorCatcher catcher in cursorAreas.OrderBy(x => x.ZIndex).Reverse())
			if (catcher.IsUnderCursor())
			{
				catcher.IsFocused = true;
				break;
			}
	}

	private void HandleButtons()
	{
		List<Button> focusedButtons = [];

		foreach (Button button in buttons)
		{
			button.IsFocused = false;

			if (button.IsUnderCursor())
				focusedButtons.Add(button);
		}

		focusedButtons = [.. focusedButtons
			.OrderBy(x => x.IsUI)
			.ThenBy(x => x.ZIndex)
		];

		if (focusedButtons.Count > 0)
			// Objects later in the array have higher Z order.
			focusedButtons[^1].IsFocused = true;
	}
}