using Godot;
using System;

public class IngredientSolid : KinematicBody2D
{
	public uint IngredientType { get; private set; }
	
	public void Init(Vector2 position, uint index)
	{
		this.Position = position;
		IngredientType = index;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(index * 8f, 0, 8f, 8f);
		this.GetNode<Sprite>("Outline").RegionRect = new Rect2(index * 10f, 0, 10f, 10f);
	}
}
