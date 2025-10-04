using Godot;

public partial class AdditionalInfoContainer : SplitContainer
{
    private InspectItem InspectItem => GetParent().GetParent().GetParent<InspectItem>();

    private Item CurrentItem
    {
        get => InspectItem.CurrentItem;
        set => InspectItem.CurrentItem = value;
    }

    #region Made from Vars
    // ---- Made From ----
    private BoxContainer MadeFromContainer => GetNode<BoxContainer>("MadeFromVBoxContainer");
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
    private BoxContainer MeltsIntoContainer => GetNode<BoxContainer>("MeltsIntoVBoxContainer");
    private Label NotSpecifiedMeltsIntoLabel => MeltsIntoContainer.GetNode<Label>("NotSpecifiedLabel");

    private BoxContainer MoltenMetalContainer => MeltsIntoContainer.GetNode<BoxContainer>("MoltenMetalHBoxContainer");
    private Label MoltenMetalLabel => MoltenMetalContainer.GetNode<Label>("MetalNameLabel");
    private Label MillibucketsLabel => MoltenMetalContainer.GetNode<Label>("MillibucketsLabel");
    private BorderedIcon MoltenMetalIcon => MoltenMetalContainer.GetNode<BorderedIcon>("BorderedIcon");
    // ---- Melts into ----
    #endregion


    #region Additional info
    public void SetAdditionalInfo()
    {
        SetMadeFrom();
        SetMeltsInto();
    }

    #region  Made from
    void SetMadeFrom()
    {
        if ((CurrentItem.MadeFrom == null) || (CurrentItem.MadeFrom.OriginalItem == null && CurrentItem.MadeFrom.AdditionalItem == null))
        {
            NotSpecifiedMadeFromLabel.Visible = true;
            ItemsContainer.Visible = false;
            return;
        }

        ItemsContainer.Visible = true;
        NotSpecifiedMadeFromLabel.Visible = false;
        AdditionalItemContainer.Visible = false;

        Item originalItem = CurrentItem.MadeFrom.OriginalItem;
        Item additionalItem = CurrentItem.MadeFrom.AdditionalItem;

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
        if (CurrentItem.MeltsInto == null)
        {
            NotSpecifiedMeltsIntoLabel.Visible = true;
            MoltenMetalContainer.Visible = false;
            return;
        }
        if (CurrentItem.MeltsInto.MeltsInto == null)
        {
            NotSpecifiedMeltsIntoLabel.Visible = true;
            MoltenMetalContainer.Visible = false;
            return;
        }
        
        MoltenMetalContainer.Visible = true;
        NotSpecifiedMeltsIntoLabel.Visible = false;

        MeltingRecipe metal = CurrentItem.MeltsInto;

        MoltenMetalLabel.Text = metal.MeltsInto.Name;
        MoltenMetalIcon.Icon = metal.MeltsInto.Icon;
        MillibucketsLabel.Text = Mathf.Snapped(metal.Ingots * Global.IngotCost, 0.01) + " " + TranslationServer.Translate("ui.mb_long");
    }
    #endregion
    #endregion
}
