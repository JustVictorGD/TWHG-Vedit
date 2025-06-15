using System.Numerics;
using Raylib_cs;

namespace WhgVedit.Engine.Video;

public static class RatioEnforcer
{
	public const int ScreenHeight = 720;
	public static float GoalRatio { get; set; } = 16 / 9f;
	public static Color MarginColor { get; set; } = Color.Black;

	private static Vector2 screenSize;
	private static Vector2 windowMargins;
	public static void CheckNewScreenSize(Vector2 newScreenSize)
	{
		if (screenSize == newScreenSize)
			return;
		else
			screenSize = newScreenSize;

		windowMargins = new Vector2(
			screenSize.X - screenSize.Y * GoalRatio,
			screenSize.Y - screenSize.X / GoalRatio
		) / 2;

		Vector2 goalScreenSize = new(ScreenHeight * GoalRatio, ScreenHeight);
		Vector2 screenScale = screenSize / goalScreenSize;

		// If false, the window is too tall instead.
		bool isScreenTooWide = (screenSize.X / screenSize.Y) > GoalRatio;
		float zoom = isScreenTooWide ? screenScale.Y : screenScale.X;

		// I should make a Camera class with individually mutable fields.
		Game.WorldCamera = new()
		{
			Offset = Game.WorldCamera.Offset,
			Zoom = zoom
		};
		Game.UICamera = new()
		{
			Zoom = zoom,
			Offset = isScreenTooWide ?
				new(windowMargins.X, 0) :
				new(0, windowMargins.Y)
		};
	}

	public static void DrawScreenMargins()
	{
		// Case 1: The window is too wide.
		if (screenSize.X > screenSize.Y * GoalRatio)
		{
			Raylib.DrawRectangle(
				0, 0,
				(int)windowMargins.X, (int)screenSize.Y,
			MarginColor);

			Raylib.DrawRectangle(
				(int)(screenSize.X - windowMargins.X) + 1, 0,
				(int)windowMargins.X + 1, (int)screenSize.Y + 1,
			MarginColor);
		}
		// Case 2: The window is too tall.
		else
		{
			Raylib.DrawRectangle(
				0, 0,
				(int)screenSize.X, (int)windowMargins.Y,
			MarginColor);

			Raylib.DrawRectangle(
				0, (int)(screenSize.Y - windowMargins.Y) + 1,
				(int)screenSize.X + 1, (int)windowMargins.Y + 1,
			MarginColor);
		}
	}

	private static Vector2 GetScreenSize() => new(
		Raylib.GetScreenWidth(),
		Raylib.GetScreenHeight()
	);
}