using Godot;

public abstract class Table : KinematicBody2D
{
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");

	public bool isWorking = false;

	public abstract void Interact();

	public void _on_Timer_timeout()
	{
		isWorking = false;
		var potionInstance = potion.Instance<Potion>();
		potionInstance.init(new Vector2(0, 0), 0);
		potionInstance.AddToGroup("potions");
		potionInstance.AddToGroup("selectable");
		AddChild(potionInstance);
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}
}