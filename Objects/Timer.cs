namespace WhgVedit.Objects;

// This class measures time in frames, not seconds.

public class Timer(int durationTicks = 1) : GameObject
{
	public double TimeLeft { get; private set; } = durationTicks;

	public double Time => durationTicks - TimeLeft;
	
	public bool IsActive { get; private set; } = false;
	public event Action? Timeout;

	public double Duration { get; } = durationTicks;
	public bool Debug { get; set; } = false;

	public override void Update()
	{
		if (Debug) Console.WriteLine($"Time: {TimeLeft}/{Duration}. Active: {IsActive}");

		if (!IsActive) return;

		TimeLeft--;

		if (TimeLeft <= 0)
		{
			IsActive = false;
			Timeout?.Invoke();

			if (Debug) Console.WriteLine("Timeout.");
		}
	}

	public void Start()
	{
		TimeLeft = Duration;
		IsActive = true;
	}
}