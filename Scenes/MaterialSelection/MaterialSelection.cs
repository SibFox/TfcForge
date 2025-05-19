using Godot;
using System;

public partial class MaterialSelection : Control
{
	ItemList ItemList => GetNode<ItemList>("VBoxContainer/MaterialList");

	void OnMaterialActivate(int index)
	{
		GD.Print("[MaterialSelection] " + ItemList.GetItemText(index));
		Global.OpenItemSelectionScene(ItemList.GetItemText(index));
	}
}
