using WhgVedit.Objects;

namespace WhgVedit.System;

class Scene
{
	public List<GameObject> GameObjects { get; set; } = [];
	public Dictionary<string, List<GameObject>> Groups { get; set; } = [];


	public void Ready()
	{
		foreach (GameObject gameObject in GameObjects)
			gameObject.Ready();
	}

	public void Update()
	{
		foreach (GameObject gameObject in GameObjects)
			gameObject.Update();
	}

	public void Draw()
	{
		foreach (GameObject gameObject in GameObjects)
			gameObject.Draw();
	}
}