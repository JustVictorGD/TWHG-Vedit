namespace WhgVedit.Engine.UI;

using Raylib_cs;

using WhgVedit.Types;



public class Slider(int x, int y, int width, int height) : Button(x, y, width, height)
{
	private Vector2i mousePosOffset = new();
	public bool CanMoveX = true;
	public bool CanMoveY = true;

	public override void Press()
	{
		mousePosOffset = (Vector2i)Position - (Vector2i)Raylib.GetMousePosition();
	}

	public override void Update()
	{
		base.Update();

		if (IsDown)
		{
			if (CanMoveX) SetX(Raylib.GetMousePosition().X + mousePosOffset.X);
			if (CanMoveY) SetY(Raylib.GetMousePosition().Y + mousePosOffset.Y);
		}
	}
}