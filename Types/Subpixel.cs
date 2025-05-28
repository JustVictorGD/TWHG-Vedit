namespace WhgVedit.Types;

readonly struct Subpixel
{
	// Main data. 256 steps equal one pixel.
	public readonly int Steps;


	// QoL constants.
	public const int StepsPerInteger = 256;
	public const float StepSize = 1f / StepsPerInteger;
	public const int Min = StepsPerInteger / -2;
	public const int Max = StepsPerInteger / 2 - 1;


	// QoL getters.
	public readonly double Value => Steps * StepSize;
	public readonly int Rounded => (int)Math.Floor(Value + 0.5);
	public readonly int Fraction => Steps - Rounded * StepsPerInteger;


	// Constructors.
	public Subpixel() { Steps = 0; }
	public Subpixel(int value, bool fromSteps = false)
	{
		if (fromSteps)
			Steps = value;
		else
			Steps = value * StepsPerInteger;
	}
	public Subpixel(double value) { Steps = (int)(value * StepsPerInteger); }

	public static int MinFromWhole(int value) => value * 256 + Min;
	public static int MaxFromWhole(int value) => value * 256 + Max;

	// The arguments are whole pixels!
	public static Subpixel Clamp(Subpixel original, int min, int max)
	{
		return new(Math.Clamp(
		    original.Steps,
		    MinFromWhole(min),
		    MaxFromWhole(max)
		), true);
	}

	public override readonly string ToString() => Value.ToString();

	public override readonly bool Equals(object? obj)
	{
		if (obj is Subpixel other)
			return Steps == other.Steps;
		else
			return false;
	}

	public override readonly int GetHashCode() => Steps;

	// HERE COMES THE WALL OF TEXT!

	public static Subpixel operator -(Subpixel a) => new(-a.Steps, true);

	// Conversion operators.

	public static explicit operator int(Subpixel value) => value.Rounded;
	public static implicit operator float(Subpixel value) => (float)value.Value;
	public static implicit operator double(Subpixel value) => value.Value;

	public static implicit operator Subpixel(int value) => new(value);
	public static implicit operator Subpixel(float value) => new(value);
	public static explicit operator Subpixel(double value) => new(value);

	// Subpixel + Subpixel operators.

	public static Subpixel operator +(Subpixel a, Subpixel b) => new(a.Steps + b.Steps, true);
	public static Subpixel operator -(Subpixel a, Subpixel b) => new(a.Steps - b.Steps, true);
	public static Subpixel operator *(Subpixel a, Subpixel b) => new((int)(a.Steps * b.Value), true);
	public static Subpixel operator /(Subpixel a, Subpixel b) => new((int)(a.Steps / b.Value), true);

	public static bool operator ==(Subpixel a, Subpixel b) => a.Steps == b.Steps;
	public static bool operator !=(Subpixel a, Subpixel b) => !(a == b);

	// Equality operators.
	private static bool Equality(Subpixel a, double b) => a.Value == b;

	public static bool operator ==(Subpixel a, int b) => Equality(a, b);
	public static bool operator ==(Subpixel a, float b) => Equality(a, b);
	public static bool operator ==(Subpixel a, double b) => Equality(a, b);
	public static bool operator ==(int a, Subpixel b) => Equality(b, a);
	public static bool operator ==(float a, Subpixel b) => Equality(b, a);
	public static bool operator ==(double a, Subpixel b) => Equality(b, a);

	public static bool operator !=(Subpixel a, int b) => !Equality(a, b);
	public static bool operator !=(Subpixel a, float b) => !Equality(a, b);
	public static bool operator !=(Subpixel a, double b) => !Equality(a, b);
	public static bool operator !=(int a, Subpixel b) => !Equality(b, a);
	public static bool operator !=(float a, Subpixel b) => !Equality(b, a);
	public static bool operator !=(double a, Subpixel b) => !Equality(b, a);

	// Addition operators.
	private static Subpixel Add(Subpixel a, double b) => new(a.Value + b);

	public static Subpixel operator +(Subpixel a, int b) => Add(a, b);
	public static Subpixel operator +(Subpixel a, float b) => Add(a, b);
	public static Subpixel operator +(Subpixel a, double b) => Add(a, b);
	public static Subpixel operator +(int a, Subpixel b) => Add(b, a);
	public static Subpixel operator +(float a, Subpixel b) => Add(b, a);
	public static Subpixel operator +(double a, Subpixel b) => Add(b, a);

	// Subtraction operators.
	private static Subpixel Subtract(Subpixel a, double b) => new(a.Value - b);

	public static Subpixel operator -(Subpixel a, int b) => Subtract(a, b);
	public static Subpixel operator -(Subpixel a, float b) => Subtract(a, b);
	public static Subpixel operator -(Subpixel a, double b) => Subtract(a, b);
	public static Subpixel operator -(int a, Subpixel b) => Subtract(b, a);
	public static Subpixel operator -(float a, Subpixel b) => Subtract(b, a);
	public static Subpixel operator -(double a, Subpixel b) => Subtract(b, a);

	// Multiplication operators.
	private static Subpixel Multiply(Subpixel a, double b) => new(a.Value * b);

	public static Subpixel operator *(Subpixel a, int b) => Multiply(a, b);
	public static Subpixel operator *(Subpixel a, float b) => Multiply(a, b);
	public static Subpixel operator *(Subpixel a, double b) => Multiply(a, b);
	public static Subpixel operator *(int a, Subpixel b) => Multiply(b, a);
	public static Subpixel operator *(float a, Subpixel b) => Multiply(b, a);
	public static Subpixel operator *(double a, Subpixel b) => Multiply(b, a);

	// Division operators.

	private static Subpixel Divide(Subpixel a, double b) => new(a.Value / b);

	public static Subpixel operator /(Subpixel a, int b) => Divide(a, b);
	public static Subpixel operator /(Subpixel a, float b) => Divide(a, b);
	public static Subpixel operator /(Subpixel a, double b) => Divide(a, b);
	public static Subpixel operator /(int a, Subpixel b) => Divide(b, a);
	public static Subpixel operator /(float a, Subpixel b) => Divide(b, a);
	public static Subpixel operator /(double a, Subpixel b) => Divide(b, a);
}

