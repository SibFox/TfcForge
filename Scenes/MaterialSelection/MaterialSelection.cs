using Godot;
using System;

public partial class MaterialSelection : Control
{
	ItemList ItemList => GetNode<ItemList>("VBoxContainer/MaterialList");
	LineEdit IngotCostEdit => GetNode<LineEdit>("IngotCostEdit");

	public override void _Ready()
	{
		IngotCostEdit.Text = Global.GlobalConfig.GetValue("ingot", "cost", 100).ToString(); //ProjectSettings.GetSetting("global/IngotCost", 100).ToString();
	}


	void OnMaterialActivate(int index)
	{
		GD.Print("[MaterialSelection] " + ItemList.GetItemText(index));
		Global.OpenItemSelectionScene(ItemList.GetItemText(index));
	}

	void OnIngotCostTextChanged(string cstr)
	{
		if (!System.Text.RegularExpressions.Regex.IsMatch(cstr, "^[0-9]+$"))
		{
			IngotCostEdit.Text = cstr[..(cstr.Length - 1)];
			IngotCostEdit.CaretColumn = IngotCostEdit.Text.Length;
		}
	}

	void OnIngotCostTextSubmitted(string str)
	{
		Global.GlobalConfig.SetValue("ingot", "cost", str.ToInt());
		Global.GlobalConfig.Save("res://GlobalConfig.ini");
		GD.Print("[MaterialSelection] Ingot cost changed to " + str + $"; (Control value: {Global.GlobalConfig.GetValue("ingot", "cost")})");
	}
}
