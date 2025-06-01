namespace WhgVedit.Common;

public static class Easings
{
	public static readonly Func<double, double> Constant =
		t => 0;
	public static readonly Func<double, double> Linear =
		t => t;
	public static readonly Func<double, double> SineInOut =
		t => -(Math.Cos(Math.PI * t) - 1) / 2;
}