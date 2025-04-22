using Godot;

public partial class MainMenu : Control
{
	// Metode ini akan dipanggil ketika scene di-load
	public override void _Ready()
	{
		// Menghubungkan sinyal secara otomatis jika belum dihubungkan di editor
		GetNode<Button>("VBoxContainer/BtnProject1").Pressed += _on_BtnProject1_pressed;
		GetNode<Button>("VBoxContainer/BtnProject2").Pressed += _on_BtnProject2_pressed;
		GetNode<Button>("VBoxContainer/BtnAbout").Pressed += _on_BtnAbout_pressed;
		GetNode<Button>("VBoxContainer/BtnGuide").Pressed += _on_BtnGuide_pressed;
		GetNode<Button>("VBoxContainer/BtnExit").Pressed += _on_BtnExit_pressed;

	}

	private void _on_BtnProject1_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Project1.tscn");
	}

	private void _on_BtnProject2_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Project2.tscn");
	}
	
	private void _on_BtnProject3_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Project3.tscn");
	}
	
	private void _on_BtnProject4_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Project4.tscn");
	}

	private void _on_BtnAbout_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/About.tscn");
	}

	private void _on_BtnGuide_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Guide.tscn");
	}

	private void _on_BtnExit_pressed()
	{
		GetTree().Quit();
	}
}
