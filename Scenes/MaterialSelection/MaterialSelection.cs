using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using System.Text;
using static TfcForge.Common.Logger.Logger;

public partial class MaterialSelection : Control
{
	ItemList ItemList => GetNode<ItemList>("VBoxContainer/MaterialList");
	LineEdit IngotCostEdit => GetNode<LineEdit>("IngotCostEdit");

	LinkedList<Item> IngotsCache = [];

	public override void _Ready()
	{
		IngotCostEdit.Text = Global.Config.IngotCost.ToString();

		LoadCache();
		LoadCacheToList();

		LogInfo(nameof(MaterialSelection), nameof(_Ready))
			.AddLine("MaterialSelection initilized").Push();
	}

	void LoadCache()
	{
		List<string> ItemsDir = DirAccess.Open(Global.Paths.Items).GetDirectories().ToList();
		StringBuilder stringBuilder = new();
		foreach (string dir in ItemsDir)
		{
			Item ingot = GD.Load<Item>(Global.Paths.Items + dir + "/Ingot.tres");
			if (ingot == null)
			{
				LogErr(nameof(MaterialSelection), nameof(LoadCache))
					.AddLine("Path '" + Global.Paths.Items + dir + "' don't have an Ingot item").Push();
			}
			else
			{
				stringBuilder.Append(dir + ", ");
				IngotsCache.AddLast(ingot);                
            }
		}
		LogInfo(nameof(MaterialSelection), nameof(LoadCache)).AddLine($"Loaded ingots:", stringBuilder).Push();
	}
	
	void LoadCacheToList()
    {
		ItemList.Clear();
		foreach (Item ingot in IngotsCache)
		{
			ItemList.AddItem(ingot.MetalName, ingot.Icon);
		}
		LogInfo(nameof(MaterialSelection), nameof(LoadCacheToList))
			.AddLine("Cache successfully loaded").Push();
    }

	void OnMaterialActivate(int index)
	{
		LogInfo(nameof(MaterialSelection), nameof(OnMaterialActivate)).AddLine("Chosen metal:", ItemList.GetItemText(index)).Push();
		Global.OpenItemSelectionScene(IngotsCache.ElementAt(index));
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
		Global.Config.IngotCost = str.ToInt();
	}
}
