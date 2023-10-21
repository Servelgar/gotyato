using Godot;
using System;

public partial class enemigo : Node2D
{
	public static int enemy_health = 100;
	public static int enemy_max_health = 100;
	private ProgressBar vida = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		vida = GetNodeOrNull<ProgressBar>("GUI/EnemyHealth");
		vida.Value = enemy_max_health * 100f;
		if (vida == null){
			Console.WriteLine("es null");
		} else {
			Console.WriteLine("hay algo de vida aqui");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Controles.isAttacking){
			enemy_health -= Controles.damage;
			Console.WriteLine(enemy_health);
			vida.Value = enemy_health;
			Console.WriteLine(vida.Value);
			
		}
		if(enemy_health == 0){
			Console.WriteLine("muerto!!");
			QueueFree();
			enemy_health = -1;
		}
		//Console.WriteLine(GetNode("GUI/EnemyHealth"));
	}
	
	private void _on_enemy_health_value_changed(double value)
{
	// Replace with function body.
}
		
}







