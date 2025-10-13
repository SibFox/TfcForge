using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TfcForge.Common.Logger.Logger;

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

	private Item selectedMetal;
	public Item SelectedMetal
	{
		get => selectedMetal;
		set
		{
			selectedMetal = value;
			LogInfo(nameof(ItemSelection)).AddLine("Selected name for search:", value.MetalName).Push();

			MetalIcon.Icon = value.Icon;
			MetalName.Text = value.MetalName;

			LoadCache();
			LoadItemsFromCache();
			PageSelectorContainer.SelectedPage = 1;
		}
	}

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
		string metalNameTransltaionCode = SelectedMetal.MetalName.GetNameFromTransltaionCode();
		string[] itemFiles = DirAccess.GetFilesAt(Global.Paths.Items + $"{metalNameTransltaionCode}");
		LogInfo(nameof(ItemSelection)).AddLine("Metal:", SelectedMetal).Push();

		foreach (string itemFile in itemFiles)
		{
			fileNames.Append(itemFile);
			string itemPath = Global.Paths.Items + $"{metalNameTransltaionCode}/{itemFile}";
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

		LogInfo(nameof(ItemSelection)).AddLine($"Loaded files with names({itemFiles.Length - 1}):", fileNames).Push();
	}

	public void LoadItemsFromCache(int index = -1)
	{
		foreach (var child in ItemsContainer.GetChildren())
		{
			child.Free();
		}

		if (index > -1)
			PageSelectorContainer.SelectedPage = 1;
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
		Global.OpenForgeSceneWithNewItem(SelectedMetal.MetalName);
	}

	void OnBackPressed()
	{
		ClearCache();
		Global.OpenMaterialSelectionScene(this);
	}
}
