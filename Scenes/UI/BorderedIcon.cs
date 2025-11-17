using Godot;
using System;

public partial class BorderedIcon : TextureRect
{
	TextureRect IconNode => GetNodeOrNull<TextureRect>("Icon");

	private Texture2D _icon;

	[Export]
	public Texture2D Icon
	{
		get => IconNode.Texture;
		set
		{
			if (IconNode != null)
			{
				_icon = value;
				IconNode.Texture = value;
				SetSizing();
			}
		}
	}

	public override void _Ready()
	{
		GetTree().ProcessFrame += SetSizing;
	}


	void SetSizing()
	{
		if (Icon != null)
        {
			IconNode.SetAnchorsPreset(LayoutPreset.Center);
			IconNode.CustomMinimumSize = Size * .5625f;
        }
	}

	void OnVisibilityChanged()
	{
		IconNode.Texture = Icon;
		SetSizing();
	}

}
