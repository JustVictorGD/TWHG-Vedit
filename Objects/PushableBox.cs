namespace WhgVedit.Objects;

using Common;
using Types;

public class PushableBox : RectObject
{
	public bool IsActive { get; set; } = true;
	public Vector2I LastMovement { get; set; } = new();

	public PushableBox(int width, int height)
	{
		Size = new(width, height);
	}

	public PushableBox(Vector2I size)
	{
		Size = size;
	}

	public Vector2I SuggestSinglePush(Wall wall)
	{
		if (!IsActive) return Vector2I.Zero;

		int top_overlap = wall.Body.Start.Y - Body.End.Y;
		int left_overlap = wall.Body.Start.X - Body.End.X;
		int bottom_overlap = wall.Body.End.Y - Body.Position.Y;
		int right_overlap = wall.Body.End.X - Body.Position.X;

		if (left_overlap > 0 || right_overlap < 0 || top_overlap > 0 || bottom_overlap < 0)
			return new();

		Vector2I push = new(
			(int)Utils.GetClosest(0, left_overlap, right_overlap),
			(int)Utils.GetClosest(0, top_overlap, bottom_overlap)
		);

		if (Math.Abs(push.X) > Math.Abs(push.Y))
			push = new(0, push.Y);
		else
			push = new(push.X, 0);

		return push;
	}

	public Vector2I SuggestWallPushes(List<Wall> walls)
	{
		if (!IsActive) return Vector2I.Zero;

		Vector2I original_position = Body.Position;
		Rect2I tempBody = Body;

		foreach (Wall wall in walls)
			tempBody = new(
				tempBody.Position + SuggestSinglePush(wall),
				tempBody.Size
			);

		return tempBody.Position - original_position;
	}
}