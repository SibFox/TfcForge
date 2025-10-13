using Godot;

public partial class LastActionsContainer : VBoxContainer
{
    private Forge Forge => GetParent().GetParent().GetParent().GetParent<Forge>();

    public OptionButton VariantsMenu => GetNode<OptionButton>("VariantsHBoxContainer/Variants");

    public BoxContainer IconsContainer => GetNode<BoxContainer>("IconsHBoxContainer");
    public BorderedIcon FirstActionIcon => IconsContainer.GetNode<BorderedIcon>("FirstAction");
    public BorderedIcon SecondActionIcon => IconsContainer.GetNode<BorderedIcon>("SecondAction");
    public BorderedIcon ThirdActionIcon => IconsContainer.GetNode<BorderedIcon>("ThirdAction");

    private TextureButton FirstWeakHitButton => FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton");
    private TextureButton FirstMediumHitButton => FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton");
    private TextureButton FirstStrongHitButton => FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton");

    private TextureButton SecondWeakHitButton => SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton");
    private TextureButton SecondMediumHitButton => SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton");
    private TextureButton SecondStrongHitButton => SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton");

    private TextureButton ThirdWeakHitButton => ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton");
    private TextureButton ThirdMediumHitButton => ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton");
    private TextureButton ThirdStrongHitButton => ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton");



    private ForgeRecipe CurrentForgeRecipe
    {
        get => Forge.CurrentForgeRecipe;
        set => Forge.CurrentForgeRecipe = value;
    }

    private LastShowForgeActions LastForgeActions
    {
        get => Forge.LastForgeActions;
        set => Forge.LastForgeActions = value;
    }

    void HitLastActionButtonPressed(int place, int strength)
    {
        switch (place)
        {
            case 1:
                CurrentForgeRecipe.LastActions.FirstAction = strength switch
                {
                    1 => ForgeDatabase.Action.WeakHit,
                    2 => ForgeDatabase.Action.MediumHit,
                    3 => ForgeDatabase.Action.StrongHit,
                    _ => ForgeDatabase.Action.WeakHit
                };
            break;
            case 2:
                CurrentForgeRecipe.LastActions.SecondAction = strength switch
                {
                    1 => ForgeDatabase.Action.WeakHit,
                    2 => ForgeDatabase.Action.MediumHit,
                    3 => ForgeDatabase.Action.StrongHit,
                    _ => ForgeDatabase.Action.WeakHit
                };
            break;
            case 3:
                CurrentForgeRecipe.LastActions.ThirdAction = strength switch
                {
                    1 => ForgeDatabase.Action.WeakHit,
                    2 => ForgeDatabase.Action.MediumHit,
                    3 => ForgeDatabase.Action.StrongHit,
                    _ => ForgeDatabase.Action.WeakHit
                };
            break;
        }
        Forge.RecalculateRequiredProgressBar();
    }

    void OnVariantSelected(int index)
    {
        string fileName = VariantsMenu.GetItemText(index) + ".tres";
        LastForgeActions = GD.Load<LastShowForgeActions>(Global.Paths.LastForgeActions + fileName);

        CurrentForgeRecipe.LastActions.FirstAction = LastForgeActions.FirstAction switch
        {
            ForgeDatabase.ShownAction.Draw => ForgeDatabase.Action.Draw,
            ForgeDatabase.ShownAction.Hit => ForgeDatabase.Action.WeakHit,
            ForgeDatabase.ShownAction.Punch => ForgeDatabase.Action.Punch,
            ForgeDatabase.ShownAction.Bend => ForgeDatabase.Action.Bend,
            ForgeDatabase.ShownAction.Upset => ForgeDatabase.Action.Upset,
            ForgeDatabase.ShownAction.Shrink => ForgeDatabase.Action.Shrink,
            _ => ForgeDatabase.Action.None
        };

        CurrentForgeRecipe.LastActions.SecondAction = LastForgeActions.SecondAction switch
        {
            ForgeDatabase.ShownAction.Draw => ForgeDatabase.Action.Draw,
            ForgeDatabase.ShownAction.Hit => ForgeDatabase.Action.WeakHit,
            ForgeDatabase.ShownAction.Punch => ForgeDatabase.Action.Punch,
            ForgeDatabase.ShownAction.Bend => ForgeDatabase.Action.Bend,
            ForgeDatabase.ShownAction.Upset => ForgeDatabase.Action.Upset,
            ForgeDatabase.ShownAction.Shrink => ForgeDatabase.Action.Shrink,
            _ => ForgeDatabase.Action.None
        };

        CurrentForgeRecipe.LastActions.ThirdAction = LastForgeActions.ThirdAction switch
        {
            ForgeDatabase.ShownAction.Draw => ForgeDatabase.Action.Draw,
            ForgeDatabase.ShownAction.Hit => ForgeDatabase.Action.WeakHit,
            ForgeDatabase.ShownAction.Punch => ForgeDatabase.Action.Punch,
            ForgeDatabase.ShownAction.Bend => ForgeDatabase.Action.Bend,
            ForgeDatabase.ShownAction.Upset => ForgeDatabase.Action.Upset,
            ForgeDatabase.ShownAction.Shrink => ForgeDatabase.Action.Shrink,
            _ => ForgeDatabase.Action.None
        };

        Forge.RecalculateRequiredProgressBar();
        SetLastForgeActions();
    }

    public void SetLastForgeActions()
    {
        FirstActionIcon.Icon = GetActionIcon(LastForgeActions.FirstAction);
        SecondActionIcon.Icon = GetActionIcon(LastForgeActions.SecondAction);
        ThirdActionIcon.Icon = GetActionIcon(LastForgeActions.ThirdAction);

        if (LastForgeActions.FirstAction == ForgeDatabase.ShownAction.Hit)
            FirstActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = true;
        else
            FirstActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = false;

        if (LastForgeActions.SecondAction == ForgeDatabase.ShownAction.Hit)
            SecondActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = true;
        else
            SecondActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = false;

        if (LastForgeActions.ThirdAction == ForgeDatabase.ShownAction.Hit)
            ThirdActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = true;
        else
            ThirdActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = false;

        FirstWeakHitButton.ButtonPressed =
        SecondWeakHitButton.ButtonPressed =
        ThirdWeakHitButton.ButtonPressed = true;
    }

    Texture2D GetActionIcon(ForgeDatabase.ShownAction action) => action switch
    {
        ForgeDatabase.ShownAction.Draw => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Draw.png"),
        ForgeDatabase.ShownAction.Hit => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Hit.png"),
        ForgeDatabase.ShownAction.Punch => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Punch.png"),
        ForgeDatabase.ShownAction.Bend => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Bend.png"),
        ForgeDatabase.ShownAction.Upset => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Upset.png"),
        ForgeDatabase.ShownAction.Shrink => GD.Load<Texture2D>(Global.Paths.ForgeSprites + "Shrink.png"),
        _ => null,
    };
    
    public void LookForHitTypeInLastActions()
	{
		switch (CurrentForgeRecipe.LastActions.FirstAction)
		{
			case ForgeDatabase.Action.WeakHit:
				FirstWeakHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				FirstMediumHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				FirstStrongHitButton.ButtonPressed = true;
				break;
		}

		switch (CurrentForgeRecipe.LastActions.SecondAction)
		{
			case ForgeDatabase.Action.WeakHit:
				SecondWeakHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				SecondMediumHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				SecondStrongHitButton.ButtonPressed = true;
				break;
		}

		switch (CurrentForgeRecipe.LastActions.ThirdAction)
		{
			case ForgeDatabase.Action.WeakHit:
				ThirdWeakHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				ThirdMediumHitButton.ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				ThirdStrongHitButton.ButtonPressed = true;
				break;
		}
	}    
}
