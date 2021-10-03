using Godot;
using System;

public class MortarTable : Table
{
	MortarTable()
	{
		potionIndex = 3;
	}

	public override void Interact()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Working");
		GetNode<Timer>("Timer").Start();
		isWorking = true;
	}
}
