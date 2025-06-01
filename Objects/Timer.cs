namespace WhgVedit.Objects;

// This class measures time in frames, not seconds.

class Timer(int durationTicks = 1) : GameObject
{
	private double time = durationTicks;
	private bool isActive = false;
	public event Action? Timeout;

	public double Duration { get; set; } = durationTicks;
	public bool Debug { get; set; } = false;

	public override void Update()
	{
		if (Debug) Console.WriteLine($"Time: {time}/{Duration}. Active: {isActive}");

		if (!isActive) return;

		time--;

		if (time <= 0)
		{
			isActive = false;
			Timeout?.Invoke();

			if (Debug) Console.WriteLine("Timeout.");
		}
	}

	public void Start()
	{
		time = Duration;
		isActive = true;
	}
}