using Godot;

public abstract class Table : RigidBody2D
{
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");

	public bool isWorking = false;

	public abstract void Interact();

	public void _on_Timer_timeout()
	{
		isWorking = false;
		var potionInstance = potion.Instance();
		potionInstance.AddToGroup("potions");
		potionInstance.AddToGroup("selectable");
		GetNode("/root/Root/Potions").AddChild(potionInstance);
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}
}