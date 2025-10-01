using Godot;
using System.Text;

public partial class Forge : Control
{
	private ForgeRecipe _currentForgeRecipe = new();
	private LastShowForgeActions _lastForgeActions;

	public ForgeRecipe CurrentForgeRecipe
	{
		get => _currentForgeRecipe;
		set => _currentForgeRecipe = value;
	}

	public LastShowForgeActions LastForgeActions
	{
		get => _lastForgeActions;
		set => _lastForgeActions = value;
	}

	private string _lastForgeActionsResourcePath;	

	private int _currentProgress = 0;
	public int CurrentProgress
	{
		get => _currentProgress;
		set
		{
			_currentProgress = value;

			CurrentProgressNumber.Text = _currentProgress.ToString("000");

			if (_currentProgress < 0 || _currentProgress > 150)
			{
				CurrentProgressNumber.Text = "ui.forge.broke";
			}

			ProgressBar.CurrentProgress = _currentProgress;
		}
	}

	BoxContainer InterfaceContainer => GetNode<BoxContainer>("ForgeBG/InterfaceVBoxContainer");
	BoxContainer InfoContainer => InterfaceContainer.GetNode<BoxContainer>("InfoHBoxContainer");

	ForgeActionsContainer ActionsContainer => InterfaceContainer.GetNode<ForgeActionsContainer>("ForgeActionsContainer");
	BoxContainer PositiveActionsContainer => ActionsContainer.PositiveActionsContainer;
	BoxContainer NegativeActionsContainer => ActionsContainer.NegativeActionsContainer;
	
	LastActionsContainer LastActionsContainer => InfoContainer.GetNode<LastActionsContainer>("LastActionsContainer");
	OptionButton VariantsMenu => LastActionsContainer.VariantsMenu;
	BorderedIcon FirstActionIcon => LastActionsContainer.FirstActionIcon;
	BorderedIcon SecondActionIcon => LastActionsContainer.SecondActionIcon;
	BorderedIcon ThirdActionIcon => LastActionsContainer.ThirdActionIcon;

	BorderedIcon ItemIcon => GetNode<BorderedIcon>("%ItemIcon");
	LineEdit ItemName => GetNode<LineEdit>("%ItemName");
	SpinBox ForgeGoal => GetNode<SpinBox>("%ForgeGoal");
	SpinBox IngotAmount => GetNode<SpinBox>("%IngotAmount");
	FileDialog IconSelect => GetNode<FileDialog>("%IconSelect");	

	Label CurrentProgressNumber => GetNode<Label>("%CurrentProgressNumber");
	Label GoalNumber => GetNode<Label>("%GoalNumber");
	ForgeProgressBar ProgressBar => GetNode<ForgeProgressBar>("%ForgeProgressBar");

	private Item _selectedItem;

	public Item SelectedItem
	{
		get => _selectedItem;
		set
		{
			_selectedItem = value;
			CurrentForgeRecipe = value.ForgeRecipe == null ? new ForgeRecipe() { LastActions = new() } :
															value.ForgeRecipe.Duplicate() as ForgeRecipe;
			
			LastForgeActions = value.LastForgeActions == null ? new() : value.LastForgeActions.Duplicate() as LastShowForgeActions;
			if (value.LastForgeActions != null)
			{
				_lastForgeActionsResourcePath = value.LastForgeActions.ResourcePath;
				string[] LastActionSplittedPath = _lastForgeActionsResourcePath.Split('/');
				string LastActionFileName = LastActionSplittedPath[^1].Replace(".tres", "");
				GD.Print("[Forge] Last Actions File Name: " + LastActionFileName);
				for (int i = 0; i < VariantsMenu.ItemCount; i++)
				{
					if (VariantsMenu.GetItemText(i) == LastActionFileName)
					{
						VariantsMenu.Select(i);
						break;
					}
					else
						VariantsMenu.Select(-1);
				}
			}

			IngotAmount.Value = 1;
			if (value.MeltsInto != null)
				if (value.MeltsInto.MeltsInto != null)
					IngotAmount.Value = value.MeltsInto.Ingots;

			ItemIcon.Icon = value.Icon;
			ItemName.Text = value.Name;
			ForgeGoal.Value = CurrentForgeRecipe.RequiredWork;
			LastActionsContainer.SetLastForgeActions();
			LastActionsContainer.LookForHitTypeInLastActions();

			RecalculateRequiredProgressBar();

			NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = CurrentForgeRecipe.WeakHit.ToString();
			NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = CurrentForgeRecipe.MediumHit.ToString();
			NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = CurrentForgeRecipe.StrongHit.ToString();
			NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = CurrentForgeRecipe.Draw.ToString();
			PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = CurrentForgeRecipe.Punch.ToString();
			PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = CurrentForgeRecipe.Bend.ToString();
			PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = CurrentForgeRecipe.Upset.ToString();
			PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = CurrentForgeRecipe.Shrink.ToString();

			CurrentProgress = CurrentForgeRecipe.WeakHit * -3 +
							  CurrentForgeRecipe.MediumHit * -6 +
							  CurrentForgeRecipe.StrongHit * -9 +
							  CurrentForgeRecipe.Draw * -15 +
							  CurrentForgeRecipe.Punch * 2 +
							  CurrentForgeRecipe.Bend * 7 +
							  CurrentForgeRecipe.Upset * 13 +
							  CurrentForgeRecipe.Shrink * 16;
		}
	}



	public override void _Ready()
	{
		StringBuilder fileNames = new();
		string[] actionFiles = DirAccess.GetFilesAt($"res://Content/Last Shown Forge Actions");

		foreach (string actionFile in actionFiles)
		{
			fileNames.Append(actionFile);

			VariantsMenu.AddItem(actionFile.Replace(".tres", ""));

			fileNames.Append("; ");
		}

		GD.Print("[Forge] Loaded Last Actions: " + fileNames.ToString());
	}

	void OnItemIconClick(InputEvent ev)
	{
		if (ev is InputEventMouseButton && ev.IsPressed())
		{
			IconSelect.Visible = true;
		}
	}

	void OnIconSelected(string path)
	{
		GD.Print("[Forge/IconSelect] Path to sprite: " + path);
		ItemIcon.Icon = GD.Load<Texture2D>(path);
		IconSelect.Visible = false;
	}

	void ForgeGoalValueChanged(float value)
	{
		CurrentForgeRecipe.RequiredWork = (int)value;
		RecalculateRequiredProgressBar();
	}

	public void RecalculateRequiredProgressBar()
	{
		if (CurrentForgeRecipe != null)
		{
			ProgressBar.RequiredProgress = (int)ForgeGoal.Value;
			if (CurrentForgeRecipe != null && CurrentForgeRecipe.LastActions != null)
			{
				int goal =	(int)ForgeGoal.Value
							- (int)CurrentForgeRecipe.LastActions.FirstAction
							- (int)CurrentForgeRecipe.LastActions.SecondAction
							- (int)CurrentForgeRecipe.LastActions.ThirdAction;
				ProgressBar.RequiredProgressNoLastActions = goal;
				GoalNumber.Text = goal.ToString("000");											
			}
		}
	}

	// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	// ~~~~~~~~~~~~~~~~~~~~ Item Resource Save ~~~~~~~~~~~~~~~~~~~~~
	void OnSaveButtonPressed()
	{
		string oldPath = SelectedItem.ResourcePath;
		if (oldPath.Length < 1)
			oldPath = Global.Paths.Items +
					SelectedItem.MetalName.GetNameFromTransltaionCode() +
					$"/{SelectedItem.Name.GetNameFromTransltaionCode()}.tres";
		string newPath = Global.Paths.Items +
						SelectedItem.MetalName.GetNameFromTransltaionCode() +
						$"/{ItemName.Text.GetNameFromTransltaionCode()}.tres";

		if (SelectedItem.Name != ItemName.Text && FileAccess.FileExists(newPath))
		{
			GD.PushError("[Forge/Save] Item already present.");
			return;
		}

		SelectedItem.Name = ItemName.Text;
		SelectedItem.ForgeRecipe = CurrentForgeRecipe;
		SelectedItem.LastForgeActions = LastForgeActions;
		SelectedItem.Icon = ItemIcon.Icon;

		SelectedItem.MeltsInto ??= new()
		{
			MeltsInto = GD.Load<Item>(Global.Paths.Items + 
									SelectedItem.MetalName.GetNameFromTransltaionCode() + 
									"/Ingot.tres").MeltsInto.MeltsInto,
			Ingots = (float)IngotAmount.Value
		};

		GD.Print("[Forge/Save] Item save resource path: " + newPath);
		if (ResourceSaver.Save(SelectedItem, newPath) != Error.Ok)
		{
			GD.PushError("[Forge/Save] Item couldn't be saved!");
			return;
		}
		GD.Print("[Forge/Save] Item successfully saved");

		if (newPath != oldPath && FileAccess.FileExists(oldPath))
		{
			if (DirAccess.RemoveAbsolute(oldPath) != Error.Ok)
				GD.Print("[Forge/Save] Old resource file was not deleted");
			else
				GD.Print("[Forge/Save] Old resource file successfully deleted");
					
		}

		Global.Main.ItemSelection.AddToCache(SelectedItem);
		_selectedItem = null;
		Visible = false;
		Global.Main.ItemSelection.LoadItemsFromCache();
		Global.Main.ItemSelection.Visible = true;
		
	}

	void OnCancelButtonPressed()
	{
		GD.Print("[Forge/Save] Item editing canceled");
		_selectedItem = null;
		Visible = false;
		Global.Main.ItemSelection.Visible = true;
	}
}
