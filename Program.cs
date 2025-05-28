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
	game.Process();

	Raylib.BeginDrawing();
	Raylib.ClearBackground(Color.Black);
	Raylib.BeginMode2D(game.mainCamera);

	game.Draw();

	Raylib.EndMode2D();
	Raylib.EndDrawing();
}

Raylib.CloseWindow();
