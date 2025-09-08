using Godot;
using System;
using System.Text;
using static Godot.TranslationServer;

public partial class InspectItem : Control
{
    private BoxContainer MainContainer => GetNode<BoxContainer>("MainVBoxContainer");
    private BoxContainer ItemInfoContainer => MainContainer.GetNode<BoxContainer>("ItemInfoHBoxContainer");
    private BoxContainer InspectTabContainer => MainContainer.GetNode<BoxContainer>("InspectTabVBoxContainer");

    private BorderedIcon ItemIcon => ItemInfoContainer.GetNode<BorderedIcon>("ItemBorderedIcon");
    // private TextureRect ItemIcon => ItemInfoContainer.GetNode<TextureRect>("ItemIconBorder/ItemIcon");
    private Label ItemNameLabel => ItemInfoContainer.GetNode<Label>("ItemNameLabel");
    private Label CategoryLabel => ItemInfoContainer.GetNode<Label>("CategoryLabel");

    #region Recipe Vars
    // ------ Recipes ------
    #region Forge Recipe Vars
    // ---- Forge Recipe ----
    private BoxContainer ForgeRecipeContainer => InspectTabContainer.GetNode<BoxContainer>("ForgeRecipeVBoxContainer");

    private Label ForgeWorkResultLabel => ForgeRecipeContainer.GetNode<Label>("LabelHBoxContainer/MarginContainer/ForgeWorkResultLabel");
    private Label ActionWeightLabel => ForgeRecipeContainer.GetNode<Label>("ActionWeightMarginContainer/ActionWeightLabel");
    private Label MathLabel => ForgeRecipeContainer.GetNode<Label>("MathMarginContainer/MathLabel");
    private Label ActionsLabel => ForgeRecipeContainer.GetNode<Label>("ActionsMarginContainer/ActionsLabel");
    // ---- Forge Recipe ----
    #endregion

    #region Weld Recipe Vars
    // ---- Weld Recipe ----
    private BoxContainer WeldRecipeContainer => InspectTabContainer.GetNode<BoxContainer>("WeldRecipeVBoxContainer");
    private BoxContainer WeldIngredientsContainer => WeldRecipeContainer.GetNode<BoxContainer>("WeldIngredientsHBoxContainer");

    private BoxContainer FirstWeldItemContainer => WeldIngredientsContainer.GetNode<BoxContainer>("FirstItemHBoxContainer");
    private BoxContainer SecondWeldItemContainer => WeldIngredientsContainer.GetNode<BoxContainer>("SecondItemHBoxContainer");

    private Label FirstWeldItemLabel => FirstWeldItemContainer.GetNode<Label>("ItemNameLabel");
    private Label SecondWeldItemLabel => SecondWeldItemContainer.GetNode<Label>("ItemNameLabel");

    private BorderedIcon FirstWeldItemIcon => FirstWeldItemContainer.GetNode<BorderedIcon>("BorderedIcon");
    private BorderedIcon SecondWeldItemIcon => SecondWeldItemContainer.GetNode<BorderedIcon>("BorderedIcon");
    // ---- Weld Recipe ----
    #endregion
    // ------ Recipes ------
    #endregion

    #region Additional info Vars
    // ------ Additional Info ------
    private SplitContainer AdditionalInfoContainer => InspectTabContainer.GetNode<SplitContainer>("AdditionalInfoSplitContainer");

    #region Made from Vars
    // ---- Made From ----
    private BoxContainer MadeFromContainer => AdditionalInfoContainer.GetNode<BoxContainer>("MadeFromVBoxContainer");
    private BoxContainer ItemsContainer => MadeFromContainer.GetNode<BoxContainer>("ItemsHBoxContainer");
    private Label NotSpecifiedMadeFromLabel => MadeFromContainer.GetNode<Label>("NotSpecifiedLabel");

    private BoxContainer OriginalItemContainer => ItemsContainer.GetNode<BoxContainer>("OriginalItemHBoxContainer");
    private BoxContainer AdditionalItemContainer => ItemsContainer.GetNode<BoxContainer>("AdditionalItemHBoxContainer");

    private Label OriginalItemLabel => OriginalItemContainer.GetNode<Label>("ItemNameLabel");
    private Label AdditionalItemLabel => AdditionalItemContainer.GetNode<Label>("ItemNameLabel");

    private BorderedIcon OriginalItemIcon => OriginalItemContainer.GetNode<BorderedIcon>("BorderedIcon");
    private BorderedIcon AdditionalItemIcon => AdditionalItemContainer.GetNode<BorderedIcon>("BorderedIcon");
    // ---- Made From ----
    #endregion

    #region Melts into Vars
    // ---- Melts into ----
    private BoxContainer MeltsIntoContainer => AdditionalInfoContainer.GetNode<BoxContainer>("MeltsIntoVBoxContainer");
    private Label NotSpecifiedMeltsIntoLabel => MeltsIntoContainer.GetNode<Label>("NotSpecifiedLabel");

    private BoxContainer MoltenMetalContainer => MeltsIntoContainer.GetNode<BoxContainer>("MoltenMetalHBoxContainer");
    private Label MoltenMetalLabel => MoltenMetalContainer.GetNode<Label>("MetalNameLabel");
    private Label MillibucketsLabel => MoltenMetalContainer.GetNode<Label>("MillibucketsLabel");
    private BorderedIcon MoltenMetalIcon => MoltenMetalContainer.GetNode<BorderedIcon>("BorderedIcon");
    // ---- Melts into ----
    #endregion
    #endregion

    private Item _currentItem;
    int currentForgeWork;


    public void SetItem(Item item)
    {
        _currentItem = item;
        currentForgeWork = 0;

        ForgeRecipeContainer.Visible = WeldRecipeContainer.Visible = false;

        ItemIcon.Icon = _currentItem.Icon;
        // ItemIcon.Texture = _currentItem.Icon;
        ItemNameLabel.Text = _currentItem.Name;
        CategoryLabel.Text = ItemDatabase.ItemCategoryTRCodes[_currentItem.Category];

        if (item.WeldRecipe == null)
            SetForgeRecipe();
        else
            SetWendRecipe();

        SetAdditionalInfo();
    }

    #region Forge recipe
    void SetForgeRecipe()
    {
        ForgeRecipeContainer.Visible = true;

        StringBuilder actionWeightString = new();
        StringBuilder mathString = new();
        StringBuilder actionsString = new();

        ForgeRecipe recipe = _currentItem.ForgeRecipe;

        ForgeWorkResultLabel.Text = recipe.RequiredWork.ToString();

        //Shrink / Ужать
        for (int i = 0; i < recipe.Shrink; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.Shrink;
            if (i == 0)
            {
                actionWeightString.Append((int)ForgeDatabase.Action.Shrink);
                mathString.Append(currentForgeWork);
                actionsString.Append(Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Shrink]) + " x" + recipe.Shrink);
            }
            else
            {
                actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Shrink);
                mathString.Append(" > " + currentForgeWork);
            }
        }

        // Upset / Обжать
        for (int i = 0; i < recipe.Upset; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.Upset;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Upset]) + " x" + recipe.Upset);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Upset);
            mathString.Append(" > " + currentForgeWork);
        }

        // Bend / Изогнуть
        for (int i = 0; i < recipe.Bend; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.Bend;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Bend]) + " x" + recipe.Bend);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Bend);
            mathString.Append(" > " + currentForgeWork);
        }

        // Punch / Штамп
        for (int i = 0; i < recipe.Punch; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.Punch;

            if (i == 0)
                actionsString.Append(" + " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Punch]) + " x" + recipe.Punch);

            actionWeightString.Append(" + " + (int)ForgeDatabase.Action.Punch);
            mathString.Append(" > " + currentForgeWork);
        }

        // Weak hit / Слабый удар
        for (int i = 0; i < recipe.WeakHit; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.WeakHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.WeakHit]) + " x" + recipe.WeakHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.WeakHit));
            mathString.Append(" > " + currentForgeWork);
        }


        // Medium hit / Средний удар
        for (int i = 0; i < recipe.MediumHit; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.MediumHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.MediumHit]) + " x" + recipe.MediumHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.MediumHit));
            mathString.Append(" > " + currentForgeWork);
        }

        // Strong hit / Сильный удар
        for (int i = 0; i < recipe.StrongHit; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.StrongHit;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.StrongHit]) + " x" + recipe.StrongHit);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.StrongHit));
            mathString.Append(" > " + currentForgeWork);
        }

        // Draw / Протянуть
        for (int i = 0; i < recipe.Draw; i++)
        {
            currentForgeWork += (int)ForgeDatabase.Action.Draw;

            if (i == 0)
                actionsString.Append(" - " + Translate(ForgeDatabase.ActionTRCodes[ForgeDatabase.Action.Draw]) + " x" + recipe.Draw);

            actionWeightString.Append(" - " + Math.Abs((int)ForgeDatabase.Action.Draw));
            mathString.Append(" > " + currentForgeWork);
        }

        mathString.Append(" >>[ ");
        actionWeightString.Append(" >>[ ");
        actionsString.Append(" >>[ ");

        ushort actionAmount = 3;
        if (recipe.LastActions.SecondAction == ForgeDatabase.Action.None)
            actionAmount--;
        if (recipe.LastActions.ThirdAction == ForgeDatabase.Action.None)
            actionAmount--;
        for (int i = 1; i <= actionAmount; i++)
        {
            ForgeDatabase.Action action = ForgeDatabase.Action.None;
            switch (i)
            {
                case 1:
                    action = recipe.LastActions.FirstAction;
                    break;
                case 2:
                    action = recipe.LastActions.SecondAction;
                    break;
                case 3:
                    // if (recipe.LastActions.ThirdAction != ForgeDatabase.Action.None)
                        action = recipe.LastActions.ThirdAction;
                    break;
            }

            if (action < 0)
            {
                actionsString.Append("- ");
                actionWeightString.Append("- ");
            }
            else
            {
                actionsString.Append("+ ");
                actionWeightString.Append("+ ");
            }

            currentForgeWork += (int)action;

            mathString.Append("-> " + currentForgeWork + " ");
            actionWeightString.Append(Math.Abs((int)action) + " ");
            actionsString.Append(Translate(ForgeDatabase.ActionTRCodes[action]) + " ");
        }

        ActionWeightLabel.Text = actionWeightString.Append("[!]").ToString();
        MathLabel.Text = mathString.Append("[!]").ToString();
        ActionsLabel.Text = actionsString.Append("[!]").ToString();
    }
    #endregion

    #region Wend recipe

    void SetWendRecipe()
    {
        WeldRecipeContainer.Visible = true;

        WeldRecipe recipe = _currentItem.WeldRecipe;

        FirstWeldItemLabel.Text = recipe.FirstItem.Name;
        FirstWeldItemIcon.Icon = recipe.FirstItem.Icon;

        SecondWeldItemLabel.Text = recipe.SecondItem.Name;
        SecondWeldItemIcon.Icon = recipe.SecondItem.Icon;
    }

    #endregion

    #region Additional info
    void SetAdditionalInfo()
    {
        SetMadeFrom();
        SetMeltsInto();
    }

    #region  Made from
    void SetMadeFrom()
    {
        if ((_currentItem.MadeFrom == null) || (_currentItem.MadeFrom.OriginalItem == null && _currentItem.MadeFrom.AdditionalItem == null))
        {
            NotSpecifiedMadeFromLabel.Visible = true;
            ItemsContainer.Visible = false;
            return;
        }

        ItemsContainer.Visible = true;
        NotSpecifiedMadeFromLabel.Visible = false;
        AdditionalItemContainer.Visible = false;

        Item originalItem = _currentItem.MadeFrom.OriginalItem;
        Item additionalItem = _currentItem.MadeFrom.AdditionalItem;

        if (originalItem == null)
        {
            OriginalItemLabel.Text = additionalItem.Name;
            OriginalItemIcon.Icon = additionalItem.Icon;
        }
        else
        {
            OriginalItemLabel.Text = originalItem.Name;
            OriginalItemIcon.Icon = originalItem.Icon;

            if (additionalItem != null)
            {
                AdditionalItemContainer.Visible = true;
                AdditionalItemLabel.Text = additionalItem.Name;
                AdditionalItemIcon.Icon = additionalItem.Icon;
            }
        }
    }
    #endregion

    #region  Melts into
    void SetMeltsInto()
    {
        if (_currentItem.MeltsInto == null)
        {
            NotSpecifiedMeltsIntoLabel.Visible = true;
            MoltenMetalContainer.Visible = false;
            return;
        }

        MoltenMetalContainer.Visible = true;
        NotSpecifiedMeltsIntoLabel.Visible = false;

        MeltingRecipe metal = _currentItem.MeltsInto;

        MoltenMetalLabel.Text = metal.MeltsInto.Name;
        MoltenMetalIcon.Icon = metal.MeltsInto.Icon;
        MillibucketsLabel.Text = metal.Millibuckets + " Millibuckets";
    }
    #endregion
    #endregion

    void OnBackPressed()
    {
        Visible = false;

        Global.Main.ItemSelection.Visible = true;
	}
}
