namespace WhgVedit.Types;

// Fixed point number where the smallest interval is 1/256.

public readonly struct Fixed
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
	public Fixed() { Steps = 0; }
	public Fixed(int value, bool fromSteps = false)
	{
		if (fromSteps)
			Steps = value;
		else
			Steps = value * StepsPerInteger;
	}
	public Fixed(double value) { Steps = (int)(value * StepsPerInteger); }

	public static int MinFromWhole(int value) => value * 256 + Min;
	public static int MaxFromWhole(int value) => value * 256 + Max;

	// The arguments are whole pixels!
	public static Fixed Clamp(Fixed original, int min, int max)
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
		if (obj is Fixed other)
			return Steps == other.Steps;
		else
			return false;
	}

	public override readonly int GetHashCode() => Steps;

	// HERE COMES THE WALL OF TEXT!

	public static Fixed operator -(Fixed a) => new(-a.Steps, true);

	// Conversion operators.

	public static explicit operator int(Fixed value) => value.Rounded;
	public static implicit operator float(Fixed value) => (float)value.Value;
	public static implicit operator double(Fixed value) => value.Value;

	public static implicit operator Fixed(int value) => new(value);
	public static implicit operator Fixed(float value) => new(value);
	public static explicit operator Fixed(double value) => new(value);

	// Fixed + Fixed operators.

	public static Fixed operator +(Fixed a, Fixed b) => new(a.Steps + b.Steps, true);
	public static Fixed operator -(Fixed a, Fixed b) => new(a.Steps - b.Steps, true);
	public static Fixed operator *(Fixed a, Fixed b) => new((int)(a.Steps * b.Value), true);
	public static Fixed operator /(Fixed a, Fixed b) => new((int)(a.Steps / b.Value), true);

	public static bool operator ==(Fixed a, Fixed b) => a.Steps == b.Steps;
	public static bool operator !=(Fixed a, Fixed b) => !(a == b);

	// Equality operators.
	private static bool Equality(Fixed a, double b) => a.Value == b;

	public static bool operator ==(Fixed a, int b) => Equality(a, b);
	public static bool operator ==(Fixed a, float b) => Equality(a, b);
	public static bool operator ==(Fixed a, double b) => Equality(a, b);
	public static bool operator ==(int a, Fixed b) => Equality(b, a);
	public static bool operator ==(float a, Fixed b) => Equality(b, a);
	public static bool operator ==(double a, Fixed b) => Equality(b, a);

	public static bool operator !=(Fixed a, int b) => !Equality(a, b);
	public static bool operator !=(Fixed a, float b) => !Equality(a, b);
	public static bool operator !=(Fixed a, double b) => !Equality(a, b);
	public static bool operator !=(int a, Fixed b) => !Equality(b, a);
	public static bool operator !=(float a, Fixed b) => !Equality(b, a);
	public static bool operator !=(double a, Fixed b) => !Equality(b, a);

	// Addition operators.
	private static Fixed Add(Fixed a, double b) => new(a.Value + b);

	public static Fixed operator +(Fixed a, int b) => Add(a, b);
	public static Fixed operator +(Fixed a, float b) => Add(a, b);
	public static Fixed operator +(Fixed a, double b) => Add(a, b);
	public static Fixed operator +(int a, Fixed b) => Add(b, a);
	public static Fixed operator +(float a, Fixed b) => Add(b, a);
	public static Fixed operator +(double a, Fixed b) => Add(b, a);

	// Subtraction operators.
	private static Fixed Subtract(Fixed a, double b) => new(a.Value - b);

	public static Fixed operator -(Fixed a, int b) => Subtract(a, b);
	public static Fixed operator -(Fixed a, float b) => Subtract(a, b);
	public static Fixed operator -(Fixed a, double b) => Subtract(a, b);
	public static Fixed operator -(int a, Fixed b) => Subtract(b, a);
	public static Fixed operator -(float a, Fixed b) => Subtract(b, a);
	public static Fixed operator -(double a, Fixed b) => Subtract(b, a);

	// Multiplication operators.
	private static Fixed Multiply(Fixed a, double b) => new(a.Value * b);

	public static Fixed operator *(Fixed a, int b) => Multiply(a, b);
	public static Fixed operator *(Fixed a, float b) => Multiply(a, b);
	public static Fixed operator *(Fixed a, double b) => Multiply(a, b);
	public static Fixed operator *(int a, Fixed b) => Multiply(b, a);
	public static Fixed operator *(float a, Fixed b) => Multiply(b, a);
	public static Fixed operator *(double a, Fixed b) => Multiply(b, a);

	// Division operators.

	private static Fixed Divide(Fixed a, double b) => new(a.Value / b);

	public static Fixed operator /(Fixed a, int b) => Divide(a, b);
	public static Fixed operator /(Fixed a, float b) => Divide(a, b);
	public static Fixed operator /(Fixed a, double b) => Divide(a, b);
	public static Fixed operator /(int a, Fixed b) => Divide(b, a);
	public static Fixed operator /(float a, Fixed b) => Divide(b, a);
	public static Fixed operator /(double a, Fixed b) => Divide(b, a);
}

