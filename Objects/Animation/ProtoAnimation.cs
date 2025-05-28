
using System.Numerics;
using WhgVedit.Types;

namespace WhgVedit.Objects.Animation;

// Unfinished to an unusable point.

// TODO: Make animations resemble those from Godot. Instead of always expecting position,
// rotation and scale value, property tracks should be optional and allow more types such as color.

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