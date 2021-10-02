using Godot;

public class DynamicLayerUpdater : Node
{
	private Node2D parent;

	public override void _Ready()
	{
		parent = this.GetParent<Node2D>();
	}

	public override void _Process(float delta)
	{
		parent.ZIndex = (int)parent.GlobalPosition.y;
	}
}
