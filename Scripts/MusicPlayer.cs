using Godot;
using System;

public class MusicPlayer : AudioStreamPlayer
{
	private Cauldron cauldron;
	private float targetPitch = 1.0f;

	public override void _Ready()
	{
		base._Ready();
		cauldron = GetNode<Cauldron>("/root/Game/Cauldron");
	}
	
	public override void _Process(float delta)
	{
		base._Process(delta);

		targetPitch = 1.0f + Math.Max(cauldron.Instability - 0.5f, 0.0f);

		this.PitchScale = Mathf.MoveToward(this.PitchScale, targetPitch, delta * 0.1f);
	}
}
