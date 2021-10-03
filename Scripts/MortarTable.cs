using Godot;
using System;

public class MortarTable : Table
{
	public override void Interact()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Working");
		GetNode<Timer>("Timer").Start();
		isWorking = true;
	}
}
