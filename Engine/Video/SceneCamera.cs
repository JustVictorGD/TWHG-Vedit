namespace WhgVedit.Engine.Video;

using System.Numerics;
using Raylib_cs;

using Objects;

// Description.

class SceneCamera : GameObject
{
	public Vector2 Offset { get; set; }
	public Vector2 Target { get; set; }
	public float Rotation { get; set; }
	public float Zoom { get; set; }

	public Camera2D ToStruct() => new(Offset, Target, Rotation, Zoom);
}