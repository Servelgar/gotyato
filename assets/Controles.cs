using Godot;
using System;

public partial class Controles : Node2D
{
	[Export]
	public static int health = 10;
	[Export]
	public static int damage = 10;
	
	public static Boolean isAttacking = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		isAttacking = Input.IsActionJustPressed("ui_accept");
	}
}
