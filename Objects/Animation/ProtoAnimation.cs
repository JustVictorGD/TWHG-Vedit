namespace WhgVedit.Objects.Animation;

// External namespaces.
using System.Numerics;

using Types;

// Unfinished to an unusable point.
// Refer to Keyframe.cs for the TODO on animation.

class ProtoAnimation : GameObject
{
	// Preventing unnecessary loops by caching which keyframes are worth caring about.

	// This is really complex (at least for now, as I don't have experience
	// handling data like this). Use ProtoKeyframe for testing movement for now.

	//private int _lastKeyframe = 0;
	//private int _lastProgress = 0;

	public List<Keyframe> Keyframes { get; set; } = [];
	//public double Length => Keyframes.Sum(k => k.Duration);

	public override void Update()
	{

	}

	public Vector2i GetPosition(int time)
	{
		return new();
	}

	public float GetRotation(int time) => throw new NotImplementedException();
	public Vector2 GetScale(int time) => throw new NotImplementedException();
}