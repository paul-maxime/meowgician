using Godot;

public class PositionalAudio : AudioStreamPlayer2D
{
	private Node2D source;
	private Node2D listener;

	public override void _Ready()
	{
		source = GetParent<Node2D>();
		listener = GetNode<Node2D>("/root/Game/Hero");
	}

	public override void _Process(float delta)
	{
		GlobalPosition = (source.Position - listener.Position) + GetViewport().Size / 2;
	}
}
