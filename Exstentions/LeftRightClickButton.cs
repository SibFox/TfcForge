using Godot;

[GlobalClass]
public partial class LeftRightClickButton : Button
{
	[Signal]
	public delegate void LeftClickEventHandler(int num);
	[Signal]
	public delegate void RightClickEventHandler(int num);

	public override void _Ready()
	{
		GuiInput += OnGuiInput;
	}

	private void OnGuiInput(InputEvent e)
	{
		if (e is InputEventMouseButton && e.IsPressed())
		{
			switch ((e as InputEventMouseButton).ButtonIndex)
			{
				case MouseButton.Left:
					EmitSignal(SignalName.LeftClick);
					break;
				case MouseButton.Right:
					EmitSignal(SignalName.RightClick);
					break;
			}
		}
	}
}
