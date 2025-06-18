namespace WhgVedit.Engine;

public static class InputEngine
{
	public static List<InputAction> InputActions { get; } = new();

	public static InputAction? GetAction(string name)
		=> InputActions.FirstOrDefault(action => action.Name == name);

	public static void UpdateInputs()
	{
		foreach (InputAction action in InputActions)
		{
			action.Update();
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

	public static bool IsActionPressed(string actionName)
	{
		var successes = GetSuccesses(actionName);
		
		return successes[0].IsActive();
	}

	public static bool IsActionJustPressed(string actionName)
	{
		var successes = GetSuccesses(actionName);

		return successes[0].IsJustActivated();
	}
	public static int GetInputAxis(string negativeActionName, string positiveActionName)
	{
		return (IsActionPressed(positiveActionName) ? 1 : 0) - (IsActionPressed(negativeActionName) ? 1 : 0);
	}
	
	private static List<InputAction> GetSuccesses(string actionName)
	{
		List<InputAction> successes = InputActions.Where(x => x.Name == actionName).ToList();

		if (successes.Count == 0)
			throw new InvalidOperationException("No InputAction with the name '" + actionName + "' has been found.");
		return successes;
	}

}