using System.Numerics;
using Raylib_cs;
using WhgVedit.Objects;

namespace WhgVedit.System.Video;

class SceneCamera : GameObject
{
    public Vector2 Offset { get; set; }
    public Vector2 Target { get; set; }
    public float Rotation { get; set; }
    public float Zoom { get; set; }

    public Camera2D ToStruct() => new(Offset, Target, Rotation, Zoom);
}