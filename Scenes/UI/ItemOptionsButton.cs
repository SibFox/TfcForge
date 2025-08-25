using Godot;
using System;

public partial class ItemOptionsButton : Control
{
	private Item _item;

	[Export]
	public Item Item 
	{ 
		get => _item; 
		set 
		{
			_item = value;
			SetInfoes();
		} 
	}

	private BorderedIcon ItemIcon => GetNode<BorderedIcon>("BorderedIcon");
	// private TextureRect ItemIcon => GetNode<TextureRect>("Border/ItemIcon");
	private TextureButton InspectButton => GetNode<TextureButton>("ButtonControl/InspectButton");
	private TextureButton RedactButton => GetNode<TextureButton>("ButtonControl/RedactButton");
	
	void SetInfoes()
	{
		ItemIcon.Icon = Item.Icon;
		ItemIcon.TooltipText = Item.Name;
	// GetNode<TextureRect>("Border").TooltipText = Item.Name

		InspectButton.Disabled = !((Item.WeldRecipe == null && Item.LastForgeActions != null && Item.ForgeRecipe != null && Item.ForgeRecipe.LastActions != null) ||
		(Item.WeldRecipe != null && Item.LastForgeActions == null && Item.ForgeRecipe == null));
	}

	void OnInspectPressed()
	{
		Global.OpenInspectItemScene(_item);
	}

	void OnRedactPressed()
	{
		Global.OpenForgeScene(_item.MetalName, _item.Name);
	}
}
