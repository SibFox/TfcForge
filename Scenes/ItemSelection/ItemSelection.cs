using Godot;
using System;
using System.Text;

public partial class ItemSelection : Control
{
	private BoxContainer VBoxContainer => GetNode<BoxContainer>("VBoxContainer");
	private BoxContainer MetalInfoContainer => VBoxContainer.GetNode<BoxContainer>("MetalInfoHBoxContainer");

	private BorderedIcon MetalIcon => MetalInfoContainer.GetNode<BorderedIcon>("MetalBorderedIcon");
	// private TextureRect MetalIcon => MetalInfoContainer.GetNode<TextureRect>("MetalIconBorder/MetalIcon");
	private Label MetalName => MetalInfoContainer.GetNode<Label>("MetalNameLabel");
	private GridContainer ItemsContainer => VBoxContainer.GetNode<GridContainer>("ItemsControl/ItemsGridContainer");
	private OptionButton CategorySelectButton => MetalInfoContainer.GetNode<OptionButton>("CategoriesVBoxContainer/CategoriesSelect");

	// private Button AddNewForgeButton => ItemsContainer.GetNode<Button>("AddNewForgeButton");

	[Export]
	private Item selectedMetal;
	private string selectedMetalNameForSearch;

	public void SetMetal(Item _selectedMetal, string _metalName)
	{
		selectedMetal = _selectedMetal;
		selectedMetalNameForSearch = _metalName;
		GD.Print($"[ItemSelection] Selected name for search: {selectedMetalNameForSearch}");

		MetalIcon.Icon = selectedMetal.Icon;
		// MetalIcon.Texture = selectedMetal.Icon;
		MetalName.Text = selectedMetal.MetalName;

		LoadItems();
	}

	public void LoadItems(int index = -1)
	{
		foreach (var child in ItemsContainer.GetChildren())
		{
			child.Free();
		}

		StringBuilder fileNames = new();
		string[] itemFiles = DirAccess.GetFilesAt($"res://Content/Items/{selectedMetalNameForSearch}");
		GD.Print($"[ItemSelection] Metal: " + selectedMetal);

		foreach (var itemFile in itemFiles)
		{
			// if (itemFile == selectedMetalNameForSearch + "Ingot.tres")
			// 	continue;

			// GD.Print($"[ItemSelection] File name: {itemFile}");
			fileNames.Append(itemFile);

			if (ResourceLoader.Load($"res://Content/Items/{selectedMetalNameForSearch}/{itemFile}") is not Item item)
			{
				fileNames.Append("(Skipped); ");
				continue;
			}

			if (item.ShowInSelection && CategorySelectButton.GetSelectedId() == (int)ItemDatabase.ItemCategory.All || CategorySelectButton.GetSelectedId() == (int)item.Category)
			{
				ItemOptionsButton newOptionButton = GD.Load<PackedScene>("res://Scenes/UI/ItemOptionsButton.tscn").Instantiate<ItemOptionsButton>();
				newOptionButton.Item = item;

				newOptionButton.SizeFlagsHorizontal = (SizeFlags)6; //ShrinkCenter(4) + Expand(2)
				newOptionButton.SizeFlagsVertical = SizeFlags.ShrinkCenter;

				ItemsContainer.AddChild(newOptionButton);
			}

			fileNames.Append("; ");
		}

		GD.Print($"[ItemSelection] Loaded files with names({itemFiles.Length - 1}): " + fileNames.ToString());

		Button newForgeButton = ResourceLoader.Load<PackedScene>("res://Scenes/UI/AddNewForgeButton.tscn").Instantiate<Button>();
		newForgeButton.Pressed += OnAddNewForgePressed;
		newForgeButton.SizeFlagsHorizontal = (SizeFlags)6;
		newForgeButton.SizeFlagsVertical = SizeFlags.ShrinkCenter;

		ItemsContainer.AddChild(newForgeButton);

		ItemsContainer.Columns = Mathf.Clamp(ItemsContainer.GetChildCount() / 3, 3, 6);
	}

	void OnAddNewForgePressed()
	{

	}

	void OnBackPressed()
	{
		Global.OpenMaterialSelectionScene(this);
	}
}
