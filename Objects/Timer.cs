namespace WhgVedit.Objects;

// This class measures time in frames, not seconds.

public class Timer(int durationTicks = 1) : GameObject
{
	public double Time { get; private set; } = durationTicks;

	public double TimeLeft => durationTicks - Time;
	
	public bool IsActive { get; private set; } = false;
	public event Action? Timeout;

	public double Duration { get; } = durationTicks;
	public bool Debug { get; set; } = false;

	public override void Update()
	{
		if (Debug) Console.WriteLine($"Time: {Time}/{Duration}. Active: {IsActive}");

		if (!IsActive) return;

		Time--;

		if (Time <= 0)
		{
			IsActive = false;
			Timeout?.Invoke();

			if (Debug) Console.WriteLine("Timeout.");
		}
	}

	public void Start()
	{
		Time = Duration;
		IsActive = true;
	}
}