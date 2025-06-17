using Newtonsoft.Json.Linq;
using Raylib_cs;
using WhgVedit.Engine.Video;
using WhgVedit.Objects.Shapes;
using WhgVedit.Types;

namespace WhgVedit.Objects;

public class Checkpoint : RectObject
{
	public override ZIndex ZIndex { get; set; } = new(-800);

	public Color Color = new(168, 252, 164);
	public float Brightness = 1;
	public Subpixel2 Center => Position + Size / 2;
	private readonly Timer activateTimer = new(20);
	
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
		VideoEngine.DrawRect2I(Body, new Color(Color.R * Brightness / 255f, Color.G * Brightness / 255f, Color.R * Brightness / 255f));
	}
	
	public override JObject ToJson()
	{
		JObject jObject = base.ToJson();
		jObject.Add("rect", new JArray(Position.X.Rounded, Position.Y.Rounded, Size.X, Size.Y));
		return jObject;
	}
}