using Godot;

public class Hero : KinematicBody2D
{
	[Export] public int speed = 200;

	private bool hasPotion = false;

	public Vector2 velocity = new Vector2();

	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}

	private Node2D GetClosestItemSelectable()
	{
		Godot.Collections.Array items;
		if (!hasPotion)
		{
			items = GetTree().GetNodesInGroup("potions");
		}
		else
		{
			items = new Godot.Collections.Array { GetNode<Node2D>("/root/Root/Cauldron") };
		}

		if (items.Count == 0)
		{
			return null;
		}
		Node2D nearestItem = (Node2D)items[0];
		foreach (Node2D item in items)
		{
			if (item.GlobalPosition.DistanceTo(this.GlobalPosition) < nearestItem.GlobalPosition.DistanceTo(this.GlobalPosition))
			{
				nearestItem = item;
			}
		}
		return nearestItem;
	}

	private void HideSelectablesOutline()
	{
		var items = GetTree().GetNodesInGroup("selectable");
		foreach (Node2D item in items)
		{
			item.GetNode<Sprite>("Outline").Visible = false;
		}
	}

	private void PickPotion(RigidBody2D potion)
	{
		hasPotion = true;
		potion.RemoveFromGroup("shakable");
		potion.GetNode<Sprite>("Outline").Visible = false;
		potion.GetParent().RemoveChild(potion);
		AddChild(potion);
		potion.Name = "Potion";
		potion.Mode = RigidBody2D.ModeEnum.Static;
		potion.Position = new Vector2(0, -4 * 16);
	}

	private void DropPotion()
	{
		hasPotion = false;
		var potion = GetNode<Potion>("Potion");
		potion.QueueFree();
	}

	private void DropPotionOnGround()
	{
		hasPotion = false;
		var potion = GetNode<Potion>("Potion");
		potion.AddToGroup("shakable");
		potion.GetNode<Sprite>("Outline").Visible = true;
		potion.GetParent().RemoveChild(potion);
		GetNode("/root/Root/Potions").AddChild(potion);
		potion.Position = this.Position + new Vector2(0, -4 * 16);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("ui_accept"))
		{
			var closestItemSelectable = GetClosestItemSelectable();
			if (closestItemSelectable != null && closestItemSelectable.GlobalPosition.DistanceTo(this.GlobalPosition) < 80)
			{
				if (closestItemSelectable is Potion potion)
				{
					PickPotion(potion);
				}
				else
				{
					DropPotion();
				}
			}
			else if (hasPotion)
			{
				DropPotionOnGround();
			}
		}
	}

	public void GetVelocity()
	{
		velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
			velocity.x += 1;

		if (Input.IsActionPressed("ui_left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("ui_down"))
			velocity.y += 1;

		if (Input.IsActionPressed("ui_up"))
			velocity.y -= 1;

		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (velocity != new Vector2())
		{
			HideSelectablesOutline();
			var closestItemSelectable = GetClosestItemSelectable();
			if (closestItemSelectable != null && closestItemSelectable.GlobalPosition.DistanceTo(this.GlobalPosition) < 80)
			{
				closestItemSelectable.GetNode<Sprite>("Outline").Visible = true;
			}
			if (velocity.y > 0)
			{
				animationPlayer.Play("Walk");
			}
			else if (velocity.y < 0)
			{
				animationPlayer.Play("Walk back");
			}
			if (velocity.x > 0)
			{
				animationPlayer.Play("Walk right");
			}
			else if (velocity.x < 0)
			{
				animationPlayer.Play("Walk left");
			}
		}
		else
		{
			if (animationPlayer.CurrentAnimation == "Walk")
			{
				animationPlayer.Play("Idle");
			}
			else if (animationPlayer.CurrentAnimation == "Walk back")
			{
				animationPlayer.Play("Idle back");
			}
			else if (animationPlayer.CurrentAnimation == "Walk left")
			{
				animationPlayer.Play("Idle left");
			}
			else if (animationPlayer.CurrentAnimation == "Walk right")
			{
				animationPlayer.Play("Idle right");
			}
		}

		velocity = velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		GetVelocity();
		velocity = MoveAndSlide(velocity, infiniteInertia: false);
	}
}
