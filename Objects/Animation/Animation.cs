namespace WhgVedit.Objects.Animation;

using System.Numerics;

using Types;
using Common;

// Unfinished to an almost unusable point.
// Refer to Keyframe.cs for the TODO on animation.

public class Animation : GameObject
{
	// Preventing unnecessary loops by caching which keyframes are worth caring about.

	// This is really complex (at least for now, as I don't have experience
	// handling data like this). Use ProtoKeyframe for testing movement for now.

	//private int _lastKeyframe = 0;
	//private int _lastProgress = 0;
	public string Name { get; set; } = "";
	public List<Keyframe> Keyframes { get; set; } = [];
	public double Length => Keyframes.Sum(k => k.Duration);

	public override void Update()
	{

	}

	public Vector2i GetPosition(double time)
	{
		if (time > Length)
			time %= Length;
		
		int[] indicesInBetween = GetKeyframeIndicesInBetween(time);
		if (indicesInBetween.Length == 0) return new Vector2i(0, 0);

		Keyframe previous = Keyframes[indicesInBetween[0]];
		if (indicesInBetween.Length == 1) return previous.Position;

		Keyframe next = Keyframes[indicesInBetween[1]];
		
		return Utils.Lerp2i(previous.Position, next.Position, 
			next.EasingFunc((time - GetLengthTo(indicesInBetween[0])) / next.Duration));
	}
	
	public float GetRotation(double time)
	{
		if (time > Length)
			time %= Length;
		
		int[] indicesInBetween = GetKeyframeIndicesInBetween(time);
		if (indicesInBetween.Length == 0) return 0;
		
		Keyframe previous = Keyframes[indicesInBetween[0]];
		if (indicesInBetween.Length == 1) return previous.Rotation;

		Keyframe next = Keyframes[indicesInBetween[1]];
		
		return Utils.LerpF(previous.Rotation, next.Rotation, 
			(float)next.EasingFunc((time - GetLengthTo(indicesInBetween[0])) / next.Duration));
	}
	
	public Vector2 GetScale(double time)
	{
		if (time > Length)
			time %= Length;
		
		int[] indicesInBetween = GetKeyframeIndicesInBetween(time);
		if (indicesInBetween.Length == 0) return Vector2.Zero;
		
		Keyframe previous = Keyframes[indicesInBetween[0]];
		if (indicesInBetween.Length == 1) return previous.Scale;

		Keyframe next = Keyframes[indicesInBetween[1]];
		
		return Vector2.Lerp(previous.Scale, next.Scale, 
			(float)next.EasingFunc((time - GetLengthTo(indicesInBetween[0])) / next.Duration));
	}

	public int[] GetKeyframeIndicesInBetween(double time)
	{
		int index = GetLastKeyframeIndexAt(time);
		
		// If array.Length == 0 then the index is out of bounds (it's not between any keyframes)
		if (index < 0 || index > Keyframes.Count - 1) return [];
		Keyframe previous = Keyframes[index];

		// If array.Length == 1 then the index is the last one (there's no next one)
		if (index == Keyframes.Count - 1) return [index];
		Keyframe next = Keyframes[index + 1];

		// If array.Length == 2 then proceed normally.
		return [index, index + 1];
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