using Godot;

public class FireTable : Table
{
	FireTable()
	{
		potionIndex = 1;
	}

	public override void Interact()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Working");
		GetNode<Timer>("Timer").Start();
		isWorking = true;
	}
}
