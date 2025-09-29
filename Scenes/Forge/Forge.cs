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

	ForgeActionsContainer ActionsContainer => InterfaceContainer.GetNode<ForgeActionsContainer>("ForgeActionsContainer");
	BoxContainer PositiveActionsContainer => ActionsContainer.PositiveActionsContainer;
	BoxContainer NegativeActionsContainer => ActionsContainer.NegativeActionsContainer;
	BoxContainer InfoContainer => InterfaceContainer.GetNode<BoxContainer>("InfoHBoxContainer");
	BoxContainer LastActionsContainer => InfoContainer.GetNode<BoxContainer>("LastActionsVBoxContainer");
	BoxContainer ActionIconsContainer => LastActionsContainer.GetNode<BoxContainer>("IconsHBoxContainer");
	OptionButton VariantsMenu => LastActionsContainer.GetNode<OptionButton>("VariantsHBoxContainer/Variants");

	BorderedIcon ItemIcon => GetNode<BorderedIcon>("%ItemIcon");
	LineEdit ItemName => GetNode<LineEdit>("%ItemName");
	SpinBox ForgeGoal => GetNode<SpinBox>("%ForgeGoal");
	SpinBox IngotAmount => GetNode<SpinBox>("%IngotAmount");
	FileDialog IconSelect => GetNode<FileDialog>("%IconSelect");

	BorderedIcon FirstActionIcon => ActionIconsContainer.GetNode<BorderedIcon>("FirstAction");
	BorderedIcon SecondActionIcon => ActionIconsContainer.GetNode<BorderedIcon>("SecondAction");
	BorderedIcon ThirdActionIcon => ActionIconsContainer.GetNode<BorderedIcon>("ThirdAction");

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
						VariantsMenu.Select(i);
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
			SetLastForgeActions();
			LookForHitTypeInLastActions();

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

	#region Last Actions Hit

	void HitLastActionButtonPressed(int place, int strength)
	{
		GD.Print($"[Forge/HitButton] Место: {place};\tСила: {strength}");

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
		RecalculateRequiredProgressBar();
	}

	#endregion


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
			ForgeDatabase.ShownAction.None => ForgeDatabase.Action.None,
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
			ForgeDatabase.ShownAction.None => ForgeDatabase.Action.None,
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
			ForgeDatabase.ShownAction.None => ForgeDatabase.Action.None,
			_ => ForgeDatabase.Action.None
		};

		RecalculateRequiredProgressBar();
		SetLastForgeActions();
	}

	// Где это используется вообще?
	void ForgeGoalValueChanged(float value)
	{
		CurrentForgeRecipe.RequiredWork = (int)value;
		RecalculateRequiredProgressBar();
	}

	void SetLastForgeActions()
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
			// DirAccess.Open(Global.Paths.Items +
			// 				SelectedItem.MetalName.GetNameFromTransltaionCode())
			// 				.Remove(SelectedItem.Name + ".tres")
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

	void LookForHitTypeInLastActions()
	{
		switch (CurrentForgeRecipe.LastActions.FirstAction)
		{
			case ForgeDatabase.Action.WeakHit:
				FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				FirstActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton").ButtonPressed = true;
				break;
		}

		switch (CurrentForgeRecipe.LastActions.SecondAction)
		{
			case ForgeDatabase.Action.WeakHit:
				SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				SecondActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton").ButtonPressed = true;
				break;
		}

		switch (CurrentForgeRecipe.LastActions.ThirdAction)
		{
			case ForgeDatabase.Action.WeakHit:
				ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/WeakButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.MediumHit:
				ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/MediumButton").ButtonPressed = true;
				break;
			case ForgeDatabase.Action.StrongHit:
				ThirdActionIcon.GetNode<TextureButton>("HitHBoxContainer/StrongButton").ButtonPressed = true;
				break;
		}
	}

}
