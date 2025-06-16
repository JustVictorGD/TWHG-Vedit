namespace WhgVedit.Objects.Animation;

public class AnimationPlayer : GameObject
{
	public Animation? Animation { get; private set; }
	public double Time;

	public AnimationPlayer(Animation animation)
	{
		Animation = animation;
	}

	public AnimationPlayer()
	{
		Animation = null;
	}

	public void SetAnimation(Animation animation)
	{
		if (Animation is null)
			Animation = animation;
	}

	public override void Update()
	{
		if (Animation is null) return;
		
		GameObject? parent = Parent;
		if (parent is SpacialObject obj)
		{
			obj.Position = Animation.GetPosition(Time);
			obj.Rotation = Animation.GetRotation(Time);
			obj.Scale = Animation.GetScale(Time);
		}
		
		// Apply position, rotation, scale to parent
		Time += 1/60.0;
	}
}