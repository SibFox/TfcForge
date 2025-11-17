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
	Window NewIngotWindow => GetNode<Window>("NewIngotWindow");

	LinkedList<Item> IngotsCache = [];

	public override void _Ready()
	{
		IngotCostEdit.Text = Global.Config.IngotCost.ToString();

		NewIngotWindow.Visible = false;

		LoadCache();

		LogInfo(nameof(MaterialSelection), nameof(_Ready))
			.AddLine("MaterialSelection initilized").Push();
	}

	void LoadCache()
	{
		IngotsCache.Clear();
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
		LoadListFromCache();
	}
	
	void LoadListFromCache()
	{
		ItemList.Clear();
		foreach (Item ingot in IngotsCache)
		{
			ItemList.AddItem(ingot.MetalName, ingot.Icon);
		}

		ItemList.AddItem("ui.m_select.new_ingot", GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Add New Forge Button.png"));

		LogInfo(nameof(MaterialSelection), nameof(LoadListFromCache))
			.AddLine("Cache successfully loaded").Push();
    }

	void OnMaterialActivate(int index)
	{
		if (index == ItemList.ItemCount - 1)
        {
			NewIngotWindow.Visible = true;
			return;
        }
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

	void OnNewIngotSave(NewIngotInterface.MetalInfo info)
	{
		string metalName = "metal." + info.Name;

		AlloyIngredient[] alloyIngredients = [];

		string savePath = Global.Paths.MoltenMetals + info.NameNoCode + ".tres";
		MoltenMetal moltenMetal = new()
		{
			Name = metalName,
			Icon = info.Icon,
			AlloyIngredients = alloyIngredients,
			ResourcePath = savePath
		};

		Error moltenMetalSave = ResourceSaver.Save(moltenMetal, savePath);
		if (moltenMetalSave != Error.Ok)
		{
			LogErr(nameof(MaterialSelection), nameof(OnNewIngotSave))
				.AddLine("Failed to save new Molten Metal")
				.AddLine("Path:", savePath).Push();
			return;
		}
		LogInfo(nameof(MaterialSelection), nameof(OnNewIngotSave))
				.AddLine("New Molten Metal successfully saved").Push();
		
		string savePathIngot = Global.Paths.Items + info.NameNoCode + "/Ingot.tres";
		Item newIngot = new()
		{
			Name = "item.ingot",
			Category = ItemDatabase.ItemCategory.Metal,
			Icon = info.Icon,
			MetalName = metalName,
			MeltsInto = new()
			{
				MeltsInto = ResourceLoader.Load<MoltenMetal>(savePath, "MoltenMetal"),
				Ingots = 1.0f
			},
			ShowInSelection = false,
			ResourcePath = savePathIngot
		};

		DirAccess.MakeDirAbsolute(Global.Paths.Items + info.NameNoCode);
		Error newIngotsave = ResourceSaver.Save(newIngot, savePathIngot);
		if (newIngotsave != Error.Ok)
		{
			LogErr(nameof(MaterialSelection), nameof(OnNewIngotSave))
				.AddLine("Failed to save new Ingot Item")
				.AddLine("Path:", savePathIngot).Push();
			DirAccess.RemoveAbsolute(Global.Paths.MoltenMetals + info.NameNoCode + ".tres");
			DirAccess.RemoveAbsolute(Global.Paths.Items + info.NameNoCode);
			return;
		}
		LogInfo(nameof(MaterialSelection), nameof(OnNewIngotSave))
				.AddLine("New Ingot Item successfully saved").Push();

		NewIngotWindow.Visible = false;

		LoadCache();
	}
	
	void OnNewIngotCancel()
    {
		NewIngotWindow.Visible = false;
    }
}
