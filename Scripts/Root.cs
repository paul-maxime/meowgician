using Godot;
using System;

public class Root : Node2D
{
	public override void _EnterTree()
	{
		base._EnterTree();
		GD.Randomize();
	}
}
