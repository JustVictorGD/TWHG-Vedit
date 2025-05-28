namespace WhgVedit.Types;

struct Basis(float xx = 1, float xy = 0, float yx = 0, float yy = 1)
{
    // Main data.
    public float XX = xx;
    public float XY = xy;
    public float YX = yx;
    public float YY = yy;
}