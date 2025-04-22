using Godot;

public partial class Project1 : Button
{

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
