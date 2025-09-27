using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class ItemSelection : Control
{
	private const int MAX_ITEMS_ON_PAGE = 14;

	private BoxContainer VBoxContainer => GetNode<BoxContainer>("VBoxContainer");
	private BoxContainer MetalInfoContainer => VBoxContainer.GetNode<BoxContainer>("MetalInfoHBoxContainer");

	private BorderedIcon MetalIcon => MetalInfoContainer.GetNode<BorderedIcon>("MetalBorderedIcon");
	private Label MetalName => MetalInfoContainer.GetNode<Label>("MetalNameLabel");
	private GridContainer ItemsContainer => VBoxContainer.GetNode<GridContainer>("ItemsControl/ItemsGridContainer");
	private OptionButton CategorySelectButton => MetalInfoContainer.GetNode<OptionButton>("CategoriesVBoxContainer/CategoriesSelect");

	private PageSelector PageSelectorContainer => VBoxContainer.GetNode<PageSelector>("PageSelectorHBoxContainer");

	private LinkedList<Item> ItemsCache = new();
	private List<Item> CategorizedItems = new();


	[Export]
	private Item selectedMetal;
	private string selectedMetalNameForSearch;

	public void SetMetal(Item _selectedMetal, string _metalName)
	{
		selectedMetal = _selectedMetal;
		selectedMetalNameForSearch = _metalName;
		GD.Print($"[ItemSelection] Selected name for search: {selectedMetalNameForSearch}");

		MetalIcon.Icon = selectedMetal.Icon;
		MetalName.Text = selectedMetal.MetalName;

		LoadCache();
		LoadItemsFromCache();
		PageSelectorContainer.SelectedPage = 1;
	}

	int хуй = 69; // Объект истории со времён пары истории России

	public void ClearCache()
	{
		ItemsCache.Clear();
	}

	public void AddToCache(Item newItem)
	{
		if (ItemsCache.Contains(newItem))
			return;

		ItemsCache.AddLast(newItem);
	}

	public byte GetMaxItemPages() => (byte)Mathf.Ceil((float)CategorizedItems.Count / MAX_ITEMS_ON_PAGE);

	void LoadCache()
	{
		StringBuilder fileNames = new();
		string[] itemFiles = DirAccess.GetFilesAt(Global.Paths.Items + $"{selectedMetalNameForSearch}");
		GD.Print($"[ItemSelection] Metal: " + selectedMetal);

		foreach (string itemFile in itemFiles)
		{
			fileNames.Append(itemFile);
			string itemPath = Global.Paths.Items + $"{selectedMetalNameForSearch}/{itemFile}";
			if (ResourceLoader.Load(itemPath) is not Item item)
			{
				fileNames.Append("(Skipped); ");
				continue;
			}

			if (item.ShowInSelection)
				ItemsCache.AddLast(item);

			fileNames.Append("; ");
		}
		
		CategorizedItems = ItemsCache.ToList();

		GD.Print($"[ItemSelection] Loaded files with names({itemFiles.Length - 1}): " + fileNames.ToString());
	}

	public void LoadItemsFromCache(int index = -1)
	{
		foreach (var child in ItemsContainer.GetChildren())
		{
			child.Free();
		}
		
		byte selectedPage = PageSelectorContainer.SelectedPage;

		CategorizedItems = CategorySelectButton.GetSelectedId() switch
		{
			(int)ItemDatabase.ItemCategory.All => ItemsCache.ToList(),
			_ => ItemsCache.Where(i => (int)i.Category == CategorySelectButton.GetSelectedId()).ToList()
		};

		for (int i = MAX_ITEMS_ON_PAGE * (selectedPage - 1);
			i < Mathf.Clamp(MAX_ITEMS_ON_PAGE * selectedPage, 0, CategorizedItems.Count); i++)
		{
			Item item = CategorizedItems.ElementAt(i);

			ItemOptionsButton newOptionButton = GD.Load<PackedScene>(Global.Paths.UI + "ItemOptionsButton.tscn").Instantiate<ItemOptionsButton>();
			newOptionButton.Item = item;

			newOptionButton.SizeFlagsHorizontal = (SizeFlags)6; //ShrinkCenter(4) + Expand(2)
			newOptionButton.SizeFlagsVertical = SizeFlags.ShrinkCenter;

			ItemsContainer.AddChild(newOptionButton);
		}

		Button newForgeButton = ResourceLoader.Load<PackedScene>(Global.Paths.UI + "AddNewForgeButton.tscn").Instantiate<Button>();
		newForgeButton.Pressed += OnAddNewForgePressed;
		newForgeButton.SizeFlagsHorizontal = (SizeFlags)6;
		newForgeButton.SizeFlagsVertical = SizeFlags.ShrinkCenter;

		ItemsContainer.AddChild(newForgeButton);

		PageSelectorContainer.Visible = CategorizedItems.Count > MAX_ITEMS_ON_PAGE;
	}

	void OnAddNewForgePressed()
	{
		Global.OpenForgeSceneWithNewItem(selectedMetal.MetalName);
	}

	void OnBackPressed()
	{
		ClearCache();
		Global.OpenMaterialSelectionScene(this);
	}
}
