using Godot;
using System;

public partial class BorderedIcon : TextureRect
{
	TextureRect IconNode => GetNode<TextureRect>("Icon");

	private Texture2D _icon;

	// public override void _Ready()
	// {
	// 	SetSizing();
	// }

	[Export]
	public Texture2D Icon
	{
		get => _icon;
		set
		{
			_icon = value;
			GetNode<TextureRect>("Icon").Texture = value;
			SetSizing();
		}
	}

	public override void _Ready()
	{
		// SetSizing();
		GetTree().ProcessFrame += SetSizing;
	}


	void SetSizing()
	{
		IconNode.SetAnchorsPreset(LayoutPreset.Center);
		IconNode.CustomMinimumSize = Size * .5625f;
		// IconNode.Size = IconNode.CustomMinimumSize;
		// IconNode.Size = Size * .5625f;
	}

	void OnVisibilityChanged()
	{
		IconNode.Texture = Icon;
		SetSizing();
	}

}
