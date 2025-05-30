namespace WhgVedit.Objects.Animation;

using Common;
using Types;

// Placeholder implementation of cycles. Replace with Godot-like animations later.
// Refer to Keyframe.cs for the TODO on animation.

struct ProtoKeyframe(Vector2i pos1, Vector2i pos2, float length)
{
	public float Length = length;
	public Vector2i Pos1 = pos1;
	public Vector2i Pos2 = pos2;

	public readonly Vector2i GetPos(float time)
	{
		float relativeTime = Utils.PingPongF(time, Length);

		return Utils.Lerp2i(Pos1, Pos2, relativeTime);
	}
}