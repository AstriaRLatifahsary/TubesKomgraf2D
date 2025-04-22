using Godot;

public partial class Guide : Node2D
{
	public override void _Ready()
	{
	}

	private void _on_BtnBack_pressed()
	{
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/MainMenu.tscn");
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.Print("Scene Tidak Ada");
		}
	}
}
