namespace WhgVedit.Objects.Animation;

public class AnimationPlayer(Animation animation) : GameObject
{
	public Animation Animation { get; private set; } = animation;
	public double Time;

	public override void Update()
	{
		GameObject? parent = Parent;
		if (parent is Object2D obj)
		{
			obj.Position = Animation.GetPosition(Time);
			obj.Rotation = Animation.GetRotation(Time);
			obj.Scale = Animation.GetScale(Time);
		}
		
		// Apply position, rotation, scale to parent
		Time += 1/60.0;
	}
}