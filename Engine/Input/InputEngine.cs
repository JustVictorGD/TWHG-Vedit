namespace WhgVedit.Engine;

public static class InputEngine
{
	public static List<InputAction> InputActions { get; } = new();

	public static InputAction? GetAction(string name)
		=> InputActions.FirstOrDefault(action => action.Name == name);

	public static void CheckInputs()
	{
		foreach (InputAction action in InputActions)
		{
			action.CheckInputs();
		}
	}

	public static void AddActions(List<InputAction> actions)
	{
		foreach (InputAction action in actions)
		{
			if (InputActions.Any(a => a.Name == action.Name))
				throw new ArgumentException($"Action with name {action.Name} already exists.");
		}
		InputActions.AddRange(actions);
	}
	
	public static int GetInputAxis(string negativeActionName, string positiveActionName)
	{
		InputAction? negative = GetAction(negativeActionName);
		InputAction? positive = GetAction(positiveActionName);
		
		if (negative is null || positive is null) return 0;
		
		return (positive.IsActive ? 1 : 0) - (negative.IsActive ? 1 : 0);
	}
}