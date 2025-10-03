using Godot;

public partial class WeldRecipeContainer : VBoxContainer
{
    private InspectItem InspectItem => GetParent().GetParent().GetParent<InspectItem>();

    private Item CurrentItem
    {
        get => InspectItem.CurrentItem;
        set => InspectItem.CurrentItem = value;
    }

    private BoxContainer WeldIngredientsContainer => GetNode<BoxContainer>("WeldIngredientsHBoxContainer");

    private BoxContainer FirstWeldItemContainer => WeldIngredientsContainer.GetNode<BoxContainer>("FirstItemHBoxContainer");
    private BoxContainer SecondWeldItemContainer => WeldIngredientsContainer.GetNode<BoxContainer>("SecondItemHBoxContainer");

    private Label FirstWeldItemLabel => FirstWeldItemContainer.GetNode<Label>("ItemNameLabel");
    private Label SecondWeldItemLabel => SecondWeldItemContainer.GetNode<Label>("ItemNameLabel");

    private BorderedIcon FirstWeldItemIcon => FirstWeldItemContainer.GetNode<BorderedIcon>("BorderedIcon");
    private BorderedIcon SecondWeldItemIcon => SecondWeldItemContainer.GetNode<BorderedIcon>("BorderedIcon");
        
    public void SetWendRecipe()
    {
        Visible = true;

        WeldRecipe recipe = CurrentItem.WeldRecipe;

        FirstWeldItemLabel.Text = recipe.FirstItem.Name;
        FirstWeldItemIcon.Icon = recipe.FirstItem.Icon;

        SecondWeldItemLabel.Text = recipe.SecondItem.Name;
        SecondWeldItemIcon.Icon = recipe.SecondItem.Icon;
    }
}
