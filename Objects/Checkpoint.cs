using Raylib_cs;
using WhgVedit.Engine.Video;
using WhgVedit.Objects.Shapes;
using WhgVedit.Types;

namespace WhgVedit.Objects;

public class Checkpoint : Object2D
{
	
	public Vector2i Size = new();
	public Color Color = new(168, 252, 164);
	public float Brightness = 1;
	public Rect2i Body => new Rect2i((Vector2i)Position, Size);
	public Subpixel2 Center => Position + Size / 2;
	private Timer activateTimer = new(20);
	
	public Checkpoint(Subpixel2 position)
	{
		Position = position;
	}

	public Checkpoint()
	{
		Position = new Subpixel2(0, 0);
	}
	
	public override void Update()
	{
		activateTimer.Update();
		
		Player? player = (Player?)Scene?.GetObjectsInGroup("Player").FirstOrDefault();
		if (player is null) return;

		if (player.Body.Intersects(Body) && player.CurrentCheckpoint != this)
		{
			player.SetCheckpoint(this);
			activateTimer.Start();
		}

		if (activateTimer.IsActive)
		{
			Brightness = 0.5f + (float)activateTimer.Time / (float)(activateTimer.Duration * 2f);
		}
		else Brightness = 1;
	}

	public override void Draw()
	{
		VideoEngine.DrawRect2i(Body, new Color(Color.R * Brightness / 255f, Color.G * Brightness / 255f, Color.R * Brightness / 255f));
	}
}