using Godot;

public class Eye : Node2D
{
	private Vector2 randomTarget;
	private Node2D pupil;

	public override void _Ready()
	{
		base._Ready();
		pupil = GetNode<Node2D>("Pupil");
	}

	public void LookAtDirection(Vector2 direction)
	{
		pupil.Position = direction;
	}

	public void WoobleRandomly(float delta)
	{
		if (pupil.Position.DistanceTo(randomTarget) > 0.001)
		{
			pupil.Position = pupil.Position.MoveToward(randomTarget, delta * 10.0f);
		}
		else
		{
			randomTarget = new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1));
		}
	}
}
