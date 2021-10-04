using Godot;
using Godot.Collections;
using System;

public class ApparitionAnimation : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

	public void Play()
	{
		foreach(var child in GetChildren())
		{
			if (child is Smoke smoke)
			{
				smoke.GetNode<AnimationPlayer>("AnimationPlayer").Play("smoke");
			}
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
