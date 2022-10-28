using Godot;

public class PositionalAudio : AudioStreamPlayer2D
{
	private Node2D source;
	private Node2D listener;
	private Node2D camera;

	public override void _Ready()
	{
		source = GetParent<Node2D>();
		listener = GetNode<Node2D>("/root/Game/Hero");
		camera = GetNode<Node2D>("/root/Game/Camera2D");
	}

	public override void _Process(float delta)
	{
		GlobalPosition = (source.Position - listener.Position) + camera.Position;
	}
}
