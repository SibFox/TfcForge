using Godot;
using System;

public partial class ActionHBoxContainer : HBoxContainer
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

	private HBoxContainer PositiveActionsContainer => GetNode<HBoxContainer>("ActionVBoxContainer/PositiveHBoxContainer");
	private HBoxContainer NegativeActionsContainer => GetNode<HBoxContainer>("ActionVBoxContainer/NegativeHBoxContainer");

	void OnActionClick(int strength)
	{
		if ((CurrentProgress >= 0 && CurrentProgress <= 150) ||
			(CurrentProgress < 0 && strength > 0) ||
			(CurrentProgress > 150 && strength < 0))
		{
			CurrentProgress += strength;
			switch (strength)
			{
                // Left Clicks
				case 16:
					CurrentForgeRecipe.Shrink++;
					PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = CurrentForgeRecipe.Shrink.ToString();
					break;
				case 13:
					CurrentForgeRecipe.Upset++;
					PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = CurrentForgeRecipe.Upset.ToString();
					break;
				case 7:
					CurrentForgeRecipe.Bend++;
					PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = CurrentForgeRecipe.Bend.ToString();
					break;
				case 2:
					CurrentForgeRecipe.Punch++;
					PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = CurrentForgeRecipe.Punch.ToString();
					break;
				case -3:
					CurrentForgeRecipe.WeakHit++;
					NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = CurrentForgeRecipe.WeakHit.ToString();
					break;
				case -6:
					CurrentForgeRecipe.MediumHit++;
					NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = CurrentForgeRecipe.MediumHit.ToString();
					break;
				case -9:
					CurrentForgeRecipe.StrongHit++;
					NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = CurrentForgeRecipe.StrongHit.ToString();
					break;
				case -15:
					CurrentForgeRecipe.Draw++;
					NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = CurrentForgeRecipe.Draw.ToString();
					break;

				// Right Clicks
				case 15:
					// CurrentForgeRecipe.Draw--;
					NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = (--CurrentForgeRecipe.Draw).ToString();
					break;
				case 9:
					// CurrentForgeRecipe.StrongHit--;
					NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = (--CurrentForgeRecipe.StrongHit).ToString();
					break;
				case 6:
					// CurrentForgeRecipe.MediumHit--;
					NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = (--CurrentForgeRecipe.MediumHit).ToString();
					break;
				case 3:
					// CurrentForgeRecipe.WeakHit--;
					NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = (--CurrentForgeRecipe.WeakHit).ToString();
					break;
				case -2:
					// CurrentForgeRecipe.Punch++;
					PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = (--CurrentForgeRecipe.Punch).ToString();
					break;
				case -7:
					// CurrentForgeRecipe.Bend++;
					PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = (--CurrentForgeRecipe.Bend).ToString();
					break;
				case -13:
					// CurrentForgeRecipe.Upset++;
					PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = (--CurrentForgeRecipe.Upset).ToString();
					break;
				case -16:
					// CurrentForgeRecipe.Shrink++;
					PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = (--CurrentForgeRecipe.Shrink).ToString();
					break;
			}
		}
	}
}
