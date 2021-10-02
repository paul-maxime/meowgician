using Godot;
using System;

public class FlickeringLight : Light2D
{
	private double elapsedTime;

	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		this.elapsedTime += delta * 2.0;
		this.TextureScale = 0.9f + (float)Math.Sin(this.elapsedTime) * 0.1f;
	}
}
