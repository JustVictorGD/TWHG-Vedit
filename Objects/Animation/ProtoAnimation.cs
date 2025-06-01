namespace WhgVedit.Objects.Animation;

using System.Numerics;

using Types;
using Common;

// Unfinished to an almost unusable point.
// Refer to Keyframe.cs for the TODO on animation.

public class ProtoAnimation : GameObject
{
	// Preventing unnecessary loops by caching which keyframes are worth caring about.

	// This is really complex (at least for now, as I don't have experience
	// handling data like this). Use ProtoKeyframe for testing movement for now.

	//private int _lastKeyframe = 0;
	//private int _lastProgress = 0;

	public List<Keyframe> Keyframes { get; set; } = [];
	public double Length => Keyframes.Sum(k => k.Duration);

	public override void Update()
	{

	}

	public Vector2i GetPosition(double time)
	{
		if (time > Length)
			time %= Length;
		
		int index = GetLastKeyframeIndexAt(time);
		
		if (index < 0 || index > Keyframes.Count - 1) return Vector2i.Zero;
		Keyframe previous = Keyframes[index];

		if (index == Keyframes.Count - 1) return previous.Position; // Only true if time == length
		Keyframe next = Keyframes[index + 1];
		
		return Utils.Lerp2i(previous.Position, next.Position, 
			next.EasingFunc((time - GetLengthTo(index)) / next.Duration));
	}

	public double GetLengthTo(int index)
	{
		double sum = 0;
		for (int i = 0; i <= index; i++)
		{
			sum += Keyframes[i].Duration;
		}

		return sum;
	}
	public int GetLastKeyframeIndexAt(double time)
	{
		if (Keyframes.Count == 0) return -1;
		
		int index = 0;
		float currentLength = 0;
		while (index < Keyframes.Count - 1)
		{
			currentLength += Keyframes[index + 1].Duration;
			if (currentLength > time) break;
			
			index++;
		}

		return index;
	}

	public float GetRotation(int time) => throw new NotImplementedException();
	public Vector2 GetScale(int time) => throw new NotImplementedException();
}