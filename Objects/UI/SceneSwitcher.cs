namespace WhgVedit.Objects.UI;

// Primitive instance of the Button class being extended to add functionality.
// Doesn't contain any text, but it looks red and it voids the scene when pressed.

using Raylib_cs;

using Engine;
using Engine.Video;

public class SceneSwitcher(int x, int y, int width, int height, bool isUI = true) : Button(x, y, width, height, isUI)
{
	public Scene? TargetScene { get; set; }

	public override void Confirm(MouseButton mouseButton)
	{
		if (mouseButton == MouseButton.Left)
			Scene.Main = TargetScene;
	}

	public override void PrivateDraw()
	{
		if (!Listener.IsHeld)
			VideoEngine.DrawOutlinedRect(Body, new(102, 0, 0), new(102, 0, 0, 128));
		else
			VideoEngine.DrawOutlinedRect(Body, new(255, 0, 0), new(255, 0, 0, 128));
	}
}