using Godot;
using System;

public class FireTable : RigidBody2D
{
	public bool isWorking = false;
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");
	public void Interact()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Working");
		GetNode<Timer>("Timer").Start();
		isWorking = true;
	}

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
