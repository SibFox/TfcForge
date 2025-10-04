using Godot;
using System;
using System.Text;

public partial class InspectItem : Control
{
    private BoxContainer MainContainer => GetNode<BoxContainer>("MainVBoxContainer");
    private BoxContainer ItemInfoContainer => MainContainer.GetNode<BoxContainer>("ItemInfoHBoxContainer");
    private BoxContainer InspectTabContainer => MainContainer.GetNode<BoxContainer>("InspectTabVBoxContainer");

    private BorderedIcon ItemIcon => ItemInfoContainer.GetNode<BorderedIcon>("ItemBorderedIcon");
    private Label ItemNameLabel => ItemInfoContainer.GetNode<Label>("ItemNameLabel");
    private Label CategoryLabel => ItemInfoContainer.GetNode<Label>("CategoryLabel");

    private ForgeRecipeContainer ForgeRecipeContainer => InspectTabContainer.GetNode<ForgeRecipeContainer>("ForgeRecipeContainer");
    private AdditionalInfoContainer AdditionalInfoContainer => InspectTabContainer.GetNode<AdditionalInfoContainer>("AdditionalInfoContainer");

    private WeldRecipeContainer WeldRecipeContainer => InspectTabContainer.GetNode<WeldRecipeContainer>("WeldRecipeContainer");

    public int CurrentForgeWork { get; set; }

    private Item _currentItem;
    public Item CurrentItem
    {
        get => _currentItem;
        set
        {
            _currentItem = value;
            CurrentForgeWork = 0;

            ForgeRecipeContainer.Visible = WeldRecipeContainer.Visible = false;

            ItemIcon.Icon = value.Icon;
            ItemNameLabel.Text = value.Name;
            CategoryLabel.Text = ItemDatabase.ItemCategoryTRCodes[value.Category];

            if (value.WeldRecipe == null)
                ForgeRecipeContainer.SetForgeRecipe();
            else
                WeldRecipeContainer.SetWendRecipe();

            AdditionalInfoContainer.SetAdditionalInfo();
        }
    }

    void OnBackPressed()
    {
        Visible = false;
        Global.Main.ItemSelection.Visible = true;
	}
}
