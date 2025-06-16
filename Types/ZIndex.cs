namespace WhgVedit.Types;

public readonly struct ZIndex(int category = 0, int layer = 0) : IComparable<ZIndex>
{
	private const int Short = 65536; // Amount of layers in a category.
	private const int HalfShort = Short / 2; // 32768

	public readonly short Category = (short)category;
	public readonly short Layer = (short)layer;

	public int Value => Category * Short + Layer + HalfShort;

	// Conversion operators.
	public static explicit operator int(ZIndex v) => v.Value;

	public static explicit operator ZIndex(int v) => new(
		FloorDiv(v, Short),
		v % Short - HalfShort
	);

	// Arithmetic operators.
	public static ZIndex operator +(ZIndex a, ZIndex b) => (ZIndex)(a.Value + b.Value);
	public static ZIndex operator -(ZIndex a, ZIndex b) => (ZIndex)(a.Value - b.Value);

	// Comparison operators.
	public static bool operator >(ZIndex a, ZIndex b) => a.Value > b.Value;
	public static bool operator >=(ZIndex a, ZIndex b) => a.Value >= b.Value;
	public static bool operator <=(ZIndex a, ZIndex b) => a.Value <= b.Value;
	public static bool operator <(ZIndex a, ZIndex b) => a.Value < b.Value;

	public static bool operator ==(ZIndex a, ZIndex b) => a.Value == b.Value;
	public static bool operator !=(ZIndex a, ZIndex b) => a.Value != b.Value;

	public override bool Equals(object? obj)
	{
		if (obj is ZIndex zIndex) return Value == zIndex.Value;
		return false;
	}

	public override int GetHashCode() => Value;

	public override string ToString() => $"{{ {Category}, {Layer} }}";

	private static int FloorDiv(int a, int b)
	{
		int result = a / b;
		if (a < 0 && a % b != 0) result--;
		return result;
	}

	// Used to make ZIndex directly usable for List.OrderBy().
	public int CompareTo(ZIndex other) => Value.CompareTo(other.Value);
}