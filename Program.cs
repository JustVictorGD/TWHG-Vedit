using Raylib_cs;
using WhgVedit;
using WhgVedit.Engine.Video;

Game game = new();

Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

Raylib.InitWindow(1280, 720, "TWHG: Vedit");
Raylib.SetTargetFPS(60);

game.Ready();

while (!Raylib.WindowShouldClose())
{
	RatioEnforcer.CheckNewScreenSize(new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

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
	RatioEnforcer.DrawScreenMargins();

	Raylib.EndDrawing();
}

Raylib.CloseWindow();
