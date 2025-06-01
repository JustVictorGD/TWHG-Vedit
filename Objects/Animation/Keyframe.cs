
namespace WhgVedit.Objects.Animation;

using System.Numerics;
using WhgVedit.Common;

// Unfinished to an unusable point.

// TODO: Make animations resemble those from Godot. Instead of always expecting position,
// rotation and scale value, property tracks should be optional and allow more types such as color.

using Types;

struct Keyframe
{
	public float Duration = 1; // Time after the previous keyframe, in seconds.
	public Vector2i Position = new();
	public float Rotation = 0;
	public Vector2 Scale = new();
	public Func<double, double> EasingFunc = Easings.Linear; // Easing between the previous keyframe's and this one's value, ranging from 0 to 1.
	// Easing is Linear by default. See list of built-in easings in Common/... (WIP)

	public Keyframe(float duration)
	{
		if (duration < 0)
			throw new ArgumentOutOfRangeException(nameof(duration), 
				"Keyframe duration cannot be negative.");
		
		Duration = duration;
	}
	
	/*public Keyframe(float duration, Vector2i position, float rotation, Vector2 scale)
	{
		Duration = duration;
		Position = position;
		Rotation = rotation;
		Scale = scale;
	}
	
	public Keyframe(float duration, Vector2i position, float rotation = 0)
		: this(duration, position, rotation, Vector2.One) {}
	
	public Keyframe(float duration = 1)
		: this(duration, Vector2i.Zero) {}*/
}