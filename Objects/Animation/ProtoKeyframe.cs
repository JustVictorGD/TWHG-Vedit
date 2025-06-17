namespace WhgVedit.Objects.Animation;

using Common;
using Types;

// Placeholder implementation of cycles. Replace with Godot-like animations later.
// Refer to Keyframe.cs for the TODO on animation.

public struct ProtoKeyframe(Vector2I pos1, Vector2I pos2, float length)
{
	public float Length = length;
	public Vector2I Pos1 = pos1;
	public Vector2I Pos2 = pos2;

	public readonly Vector2I GetPos(float time)
	{
		float relativeTime = Utils.PingPongF(time, Length);

		return Utils.Lerp2I(Pos1, Pos2, relativeTime);
	}
}