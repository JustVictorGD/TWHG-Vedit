namespace WhgVedit.Objects.Animation;

// Unfinished to an unusable point.

// TODO: Make animations resemble those from Godot. Instead of always expecting position,
// rotation and scale value, property tracks should be optional and allow more types such as color.

using Types;

struct Keyframe
{
	public float Duration = 1;
	public Vector2i Position = new();
	//public float Rotation = 0;
	//public Vector2 Scale = new();

	public Keyframe() { }
}