namespace WhgVedit.Engine.UI;

using Objects;
using Types;

public class Slider(int x, int y, int width, int height, bool isUI = true) : Button(x, y, width, height, isUI)
{
	private Vector2i mousePosOffset = new();
	public bool CanMoveX = true;
	public bool CanMoveY = true;

	public override void Press()
	{
		mousePosOffset = (Vector2i)Position - (Vector2i)Game.GetMouseWorldPosition();
	}

	public override void Update()
	{
		base.Update();

		if (IsDown)
		{
			if (CanMoveX) SetX(Game.GetMouseWorldPosition().X + mousePosOffset.X);
			if (CanMoveY) SetY(Game.GetMouseWorldPosition().Y + mousePosOffset.Y);
		}
	}
}