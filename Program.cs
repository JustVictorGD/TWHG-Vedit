using System.Numerics;
using Raylib_cs;
using WhgVedit;

Game game = new();

Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

Raylib.InitWindow(1280, 720, "TWHG: Vedit");
Raylib.SetTargetFPS(60);


game.Ready();

while (!Raylib.WindowShouldClose())
{
	const float GoalRatio = 16 / 9f;

	Vector2 screenSize = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

	game.HandleWindowSize(screenSize, GoalRatio);
	game.Update();

	Raylib.BeginDrawing();
	Raylib.ClearBackground(new(0, 0, 64));

	// Complies with the camera.
	Raylib.BeginMode2D(Game.WorldCamera);
	game.Draw();

	// Ignores the camera.
	Raylib.BeginMode2D(Game.UICamera);
	game.DrawUI();

	Raylib.EndMode2D();
	game.DrawScreenMargins(screenSize, GoalRatio);
	Raylib.EndDrawing();
}

Raylib.CloseWindow();
