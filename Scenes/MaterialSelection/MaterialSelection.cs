using Godot;
using static TfcForge.Common.Logger.Logger;

public partial class MaterialSelection : Control
{
	ItemList ItemList => GetNode<ItemList>("VBoxContainer/MaterialList");
	LineEdit IngotCostEdit => GetNode<LineEdit>("IngotCostEdit");



	public override void _Ready()
	{
		IngotCostEdit.Text = Global.IngotCost.ToString();
	}



	void OnMaterialActivate(int index)
	{
		LogInfo(nameof(MaterialSelection)).AddLine("Chosen metal:", ItemList.GetItemText(index)).Push();
		Global.OpenItemSelectionScene(ItemList.GetItemText(index));
	}

	void OnIngotCostTextChanged(string cstr)
	{
		if (!System.Text.RegularExpressions.Regex.IsMatch(cstr, "^[0-9]+$"))
		{
			IngotCostEdit.Text = cstr[..^1];
			IngotCostEdit.CaretColumn = IngotCostEdit.Text.Length;
		}
	}

	void OnIngotCostTextSubmitted(string str)
	{
		Global.IngotCost = str.ToInt();
	}
}
