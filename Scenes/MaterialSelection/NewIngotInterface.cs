using Godot;
using static TfcForge.Common.Logger.Logger;

public partial class NewIngotInterface : Control
{
    [Signal]
    public delegate void SaveEventHandler(MetalInfo info);
    [Signal]
    public delegate void CancelEventHandler();

    LineEdit MetalName => GetNode<LineEdit>("%NameEdit");
    BorderedIcon MetalIcon => GetNode<BorderedIcon>("%MetalIcon");
    FileDialog IconSelect => GetNode<FileDialog>("%IconSelect");


    void OnItemIconClick(InputEvent ev)
    {
        if (ev is InputEventMouseButton && ev.IsPressed())
        {
            IconSelect.Visible = true;
        }
    }

    void OnIconSelected(string path)
	{
		LogInfo(nameof(NewIngotInterface), nameof(OnIconSelected)).AddLine("Path to sprite", path).Push();
	    MetalIcon.Icon = GD.Load<Texture2D>(path);
		IconSelect.Visible = false;
	}

    void OnSaveButtonPressed()
    {
        EmitSignal(SignalName.Save, new MetalInfo
        {
            Name = MetalName.Text.ToResourceName(),
            Icon = MetalIcon.Icon
        });
        MetalName.Text = null;
        MetalIcon.Icon = null;
    }

    void OnCancelButtonPressed()
    {
        EmitSignal(SignalName.Cancel);
        MetalName.Text = null;
        MetalIcon.Icon = null;
    }

    void OnMetalNameTextChanged(string cstr)
	{
		if (!System.Text.RegularExpressions.Regex.IsMatch(cstr, "^[a-z_]+$"))
		{
			MetalName.Text = cstr[..^1];
			MetalName.CaretColumn = MetalName.Text.Length;
		}
	}

    public partial class MetalInfo : RefCounted
    {
        public string Name;
        public string NameNoCode => Name.GetNameFromTransltaionCode();
        public Texture2D Icon;
    }
}
