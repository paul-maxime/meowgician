using Godot;
using System;

public class Game : Node2D
{
	public override void _EnterTree()
	{
		base._EnterTree();
		GD.Randomize();
	}
}
