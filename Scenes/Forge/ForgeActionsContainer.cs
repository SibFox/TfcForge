using Godot;

public partial class ForgeActionsContainer : HBoxContainer
{
	private Forge Forge => GetParent().GetParent().GetParent<Forge>();

	private ForgeRecipe CurrentForgeRecipe
	{
		get => Forge.CurrentForgeRecipe;
		set => Forge.CurrentForgeRecipe = value;
	}
	
	private int CurrentProgress
	{
		get => Forge.CurrentProgress;
		set => Forge.CurrentProgress = value;
	}

	public HBoxContainer PositiveActionsContainer => GetNode<HBoxContainer>("ActionVBoxContainer/PositiveHBoxContainer");
	public HBoxContainer NegativeActionsContainer => GetNode<HBoxContainer>("ActionVBoxContainer/NegativeHBoxContainer");



	void OnActionClick(int strength)
	{
		if ((CurrentProgress >= 0 && CurrentProgress <= 150) ||
			(CurrentProgress < 0 && strength > 0) ||
			(CurrentProgress > 150 && strength < 0))
		{
			switch (strength)
			{
                // Left Clicks
				case 16:
                    CurrentProgress += strength;
					PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = (++CurrentForgeRecipe.Shrink).ToString();
				break;
				case 13:
                    CurrentProgress += strength;
					PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = (++CurrentForgeRecipe.Upset).ToString();
				break;
				case 7:
                    CurrentProgress += strength;
					PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = (++CurrentForgeRecipe.Bend).ToString();
				break;
				case 2:
                    CurrentProgress += strength;
					PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = (++CurrentForgeRecipe.Punch).ToString();
				break;
				case -3:
                    CurrentProgress += strength;
					NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = (++CurrentForgeRecipe.WeakHit).ToString();
				break;
				case -6:
                    CurrentProgress += strength;
					NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = (++CurrentForgeRecipe.MediumHit).ToString();
				break;
				case -9:
                    CurrentProgress += strength;
					NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = (++CurrentForgeRecipe.StrongHit).ToString();
				break;
				case -15:
                    CurrentProgress += strength;
					NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = (++CurrentForgeRecipe.Draw).ToString();
				break;

				// Right Clicks
				case 15:
                    if (CurrentForgeRecipe.Draw > 0)
                    {
                        CurrentProgress += strength;
					    NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = (--CurrentForgeRecipe.Draw).ToString();
                    }
				break;
				case 9:
                    if (CurrentForgeRecipe.StrongHit > 0)
                    {
                        CurrentProgress += strength;
                        NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = (--CurrentForgeRecipe.StrongHit).ToString();
                    }
				break;
				case 6:
                    if (CurrentForgeRecipe.MediumHit > 0)
                    {
                        CurrentProgress += strength;
					    NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = (--CurrentForgeRecipe.MediumHit).ToString();
                    }
				break;
				case 3:
                    if (CurrentForgeRecipe.WeakHit > 0)
                    {
                        CurrentProgress += strength;
					    NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = (--CurrentForgeRecipe.WeakHit).ToString();
                    }
				break;
				case -2:
                    if (CurrentForgeRecipe.Punch > 0)
                    {
                        CurrentProgress += strength;
					    PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = (--CurrentForgeRecipe.Punch).ToString();
                    }
				break;
				case -7:
                    if (CurrentForgeRecipe.Bend > 0)
                    {
                        CurrentProgress += strength;
					    PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = (--CurrentForgeRecipe.Bend).ToString();
                    }
				break;
				case -13:
                    if (CurrentForgeRecipe.Upset > 0)
                    {
                        CurrentProgress += strength;
					    PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = (--CurrentForgeRecipe.Upset).ToString();
                    }
				break;
				case -16:
                    if (CurrentForgeRecipe.Shrink > 0)
                    {
                        CurrentProgress += strength;
					    PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = (--CurrentForgeRecipe.Shrink).ToString();
                    }
				break;
			}
		}
	}
}
