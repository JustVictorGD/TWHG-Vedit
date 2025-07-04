namespace WhgVedit.Objects;

// You may see that the position sometimes ends in one half.
// This means that its position is one subpixel away from
// crossing into the pixel one above or one to the left.

using Raylib_cs;

using Engine;
using Objects.Shapes;
using Types;

public class Player : RectObject
{
	public override ZIndex ZIndex { get; set; } = new(600);

	// Components. Set in Ready().
	public OutlinedRect? Sprite { get; set; }
	public PushableBox PushableBox { get; set; }

	// Spacial properties.
	public override Vector2I Size { get; set; } = new(42, 42);
	public int Speed { get; set; } = 4;
	
	// Color.
	public Color OutlineColor { get; set; } = new(102, 0, 0);
	public Color FillColor { get; set; } = new(255, 0, 0);
	// Death transparency.
	public float OutlineOpacity { get; set; } = 1;
	public float FillOpacity { get; set; } = 1;

	// State.
	public bool CanMove { get; set; } = true;
	public bool IsDead { get; set; }

	public Checkpoint? CurrentCheckpoint { get; set; }

	// Death and respawn animations.
	private const int FadeOutTicks = 15;
	private const int RespawnTicks = 35;
	private const int FadeInTicks = 10;
	private Timer fadeOutTimer = new(FadeOutTicks);
	private Timer respawnTimer = new(RespawnTicks);
	private Timer fadeInTimer = new(FadeInTicks);

	public Player()
	{
		Sprite = new(Size, OutlineColor, FillColor);
		AddChild(Sprite);

		PushableBox = new(Size);
		AddChild(PushableBox);
	}

	public override void Update()
	{
		fadeOutTimer.Update();
		respawnTimer.Update();
		fadeInTimer.Update();

		if (CanMove && !IsDead)
			Position += new Vector2I(
				Speed * InputEngine.GetInputAxis("Left", "Right"),
				Speed * InputEngine.GetInputAxis("Up", "Down")
			);

		// TODO: Make walls set subpixels to min and max values just like the world border.
		Position += PushableBox.SuggestWallPushes(Game.Walls);

		Position = Position.Clamp(new(HalfSize, Game.AreaSize * 48 - Size));

		if (IsDead)
		{
			FillOpacity = (float)fadeOutTimer.TimeLeft / FadeOutTicks;
			OutlineOpacity = (float)fadeOutTimer.TimeLeft / FadeOutTicks;
		}

		if (fadeInTimer.IsActive)
		{
			FillOpacity = (float)fadeInTimer.Time / FadeInTicks;
			OutlineOpacity = (float)fadeInTimer.Time / FadeInTicks;
		}
	}


	public override void Draw()
	{
		if (Sprite != null)
		{
			Sprite.OutlineColor = new Color(OutlineColor.R / 255f, OutlineColor.G / 255f, OutlineColor.B / 255f, OutlineColor.A / 255f * OutlineOpacity);
			Sprite.FillColor = new Color(FillColor.R / 255f, FillColor.G / 255f, FillColor.B / 255f, FillColor.A / 255f * FillOpacity);

			Sprite.Draw();
		}
		
	}

	public bool TouchesEnemy(Enemy enemy)
	{
		Circle circle = enemy.Body;

		Vector2I nearestPoint = circle.Position.Clamp(Body);

		int squaredDictance = (int)(
			Math.Pow(nearestPoint.X - circle.Position.X, 2) +
			Math.Pow(nearestPoint.Y - circle.Position.Y, 2)
		);

		return squaredDictance < Math.Pow(circle.Radius, 2);
	}

	public void Die()
	{
		if (IsDead) return;
		fadeOutTimer.Start();
		respawnTimer.Start();

		IsDead = true;
		respawnTimer.Timeout += Respawn;
	}

	public void Respawn()
	{
		fadeInTimer.Start();
		// This is for if the opacity doesn't get set to 1 when the timer ends.
		fadeInTimer.Timeout += () =>
		{
			FillOpacity = 1;
			OutlineOpacity = 1;
		};
		// Position = GetCurrentCheckpoint().Position;
		if (CurrentCheckpoint != null) Position = CurrentCheckpoint.Center;
		else Position = new(480, 384);
		IsDead = false;
		FillOpacity = 1;
		OutlineOpacity = 1;
	}

	public void SetCheckpoint(Checkpoint checkpoint)
	{
		CurrentCheckpoint = checkpoint;
		
	}
}