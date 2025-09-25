using Godot;
using System.Collections;
using System.Diagnostics;
using System.Text;

public partial class Forge : Control
{
	private ForgeRecipe _currentForgeRecipe = new();
	private LastShowForgeActions _lastForgeActions;

	private string _lastForgeActionsResourcePath;

	BoxContainer InterfaceContainer => GetNode<BoxContainer>("ForgeBG/InterfaceVBoxContainer");
	BoxContainer ForgeProgressContainer => InterfaceContainer.GetNode<BoxContainer>("ActionHBoxContainer/CurrentForgeProgressVBoxContainer");
	BoxContainer PositiveActionsContainer => InterfaceContainer.GetNode<BoxContainer>("ActionHBoxContainer/ActionVBoxContainer/PositiveHBoxContainer");
	BoxContainer NegativeActionsContainer => InterfaceContainer.GetNode<BoxContainer>("ActionHBoxContainer/ActionVBoxContainer/NegativeHBoxContainer");
	BoxContainer InfoContainer => InterfaceContainer.GetNode<BoxContainer>("InfoHBoxContainer");
	BoxContainer LastActionsContainer => InfoContainer.GetNode<BoxContainer>("LastActionsVBoxContainer");
	BoxContainer ActionIconsContainer => LastActionsContainer.GetNode<BoxContainer>("IconsHBoxContainer");
	OptionButton VariantsMenu => LastActionsContainer.GetNode<OptionButton>("VariantsHBoxContainer/Variants");

	BorderedIcon ItemIcon => GetNode<BorderedIcon>("%ItemIcon");
	Label ItemName => GetNode<Label>("%ItemName");
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
			_currentForgeRecipe = value.ForgeRecipe == null ? new ForgeRecipe() { LastActions = new() } :
															  value.ForgeRecipe.Duplicate() as ForgeRecipe;
			
			_lastForgeActions = value.LastForgeActions == null ? new() : value.LastForgeActions.Duplicate() as LastShowForgeActions;
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
				}
			}

			if (value.MeltsInto != null)
				if (value.MeltsInto.MeltsInto != null)
					IngotAmount.Value = value.MeltsInto.Ingots;

			ItemIcon.Icon = value.Icon;
			ItemName.Text = value.Name;
			ForgeGoal.Value = _currentForgeRecipe.RequiredWork;
			SetLastForgeActions();
			LookForHitTypeInLastActions();

			RecalculateRequiredProgressBar();

			NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = _currentForgeRecipe.WeakHit.ToString();
			NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = _currentForgeRecipe.MediumHit.ToString();
			NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = _currentForgeRecipe.StrongHit.ToString();
			NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = _currentForgeRecipe.Draw.ToString();
			PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = _currentForgeRecipe.Punch.ToString();
			PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = _currentForgeRecipe.Bend.ToString();
			PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = _currentForgeRecipe.Upset.ToString();
			PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = _currentForgeRecipe.Shrink.ToString();

			_currentProgress = 0;
			CurrentProgress = _currentForgeRecipe.WeakHit * -3 +
							  _currentForgeRecipe.MediumHit * -6 +
							  _currentForgeRecipe.StrongHit * -9 +
							  _currentForgeRecipe.Draw * -15 +
							  _currentForgeRecipe.Punch * 2 +
							  _currentForgeRecipe.Bend * 7 +
							  _currentForgeRecipe.Upset * 13 +
							  _currentForgeRecipe.Shrink * 16;
		}
	}

	private int _currentProgress = 0;

	int CurrentProgress
	{
		get => 0;
		set
		{
			// GD.Print($"[Forge] {value}///{_currentProgress}" );
			_currentProgress += value;

			CurrentProgressNumber.Text = _currentProgress.ToString("000");

			if (_currentProgress < 0 || _currentProgress > 150)
			{
				CurrentProgressNumber.Text = "ui.forge.broke";
			}

			ProgressBar.CurrentProgress = _currentProgress;
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
	}

	public void OpenWithNewForgeRecipe()
	{
		_currentForgeRecipe = new ForgeRecipe();
	}


	#region Forge Clicks
	//// Left Clicks
	void OnLeftWeakHitPressed()     // -3
	{
		if (_currentProgress > 0)
		{
			CurrentProgress -= 3;
			_currentForgeRecipe.WeakHit++;
			NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = _currentForgeRecipe.WeakHit.ToString();
		}
	}

	void OnLeftMediumHitPressed()   // -6
	{
		if (_currentProgress > 0)
		{
			CurrentProgress -= 6;
			_currentForgeRecipe.MediumHit++;
			NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = _currentForgeRecipe.MediumHit.ToString();
		}
	}

	void OnLeftStrongHitPressed()   // -9
	{
		if (_currentProgress > 0)
		{
			CurrentProgress -= 9;
			_currentForgeRecipe.StrongHit++;
			NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = _currentForgeRecipe.StrongHit.ToString();
		}
	}

	void OnLeftDrawPressed()        // -15
	{
		if (_currentProgress > 0)
		{
			CurrentProgress -= 15;
			_currentForgeRecipe.Draw++;
			NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = _currentForgeRecipe.Draw.ToString();
		}
	}

	void OnLeftPunchPressed()       // +2
	{
		if (_currentProgress < 150)
		{
			CurrentProgress += 2;
			_currentForgeRecipe.Punch++;
			PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = _currentForgeRecipe.Punch.ToString();
		}
	}

	void OnLeftBendPressed()        // +7
	{
		if (_currentProgress < 150)
		{
			CurrentProgress += 7;
			_currentForgeRecipe.Bend++;
			PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = _currentForgeRecipe.Bend.ToString();
		}
	}

	void OnLeftUpsetPressed()       // +13
	{
		if (_currentProgress < 150)
		{
			CurrentProgress += 13;
			_currentForgeRecipe.Upset++;
			PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = _currentForgeRecipe.Upset.ToString();
		}
	}

	void OnLeftShrinkPressed()      // +16
	{
		if (_currentProgress < 150)
		{
			CurrentProgress += 16;
			_currentForgeRecipe.Shrink++;
			PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = _currentForgeRecipe.Shrink.ToString();
		}
	}


	//// Right Clicks
	void OnRightWeakHitPressed()    // +3
	{
		if (_currentForgeRecipe.WeakHit > 0)
		{
			CurrentProgress += 3;
			_currentForgeRecipe.WeakHit--;
			NegativeActionsContainer.GetNode<Label>("WeakHit/Amount").Text = _currentForgeRecipe.WeakHit.ToString();
		}
	}

	void OnRightMediumHitPressed()  // +6
	{
		if (_currentForgeRecipe.MediumHit > 0)
		{
			CurrentProgress += 6;
			_currentForgeRecipe.MediumHit--;
			NegativeActionsContainer.GetNode<Label>("MediumHit/Amount").Text = _currentForgeRecipe.MediumHit.ToString();
		}
	}

	void OnRightStrongHitPressed()  // +9
	{
		if (_currentForgeRecipe.StrongHit > 0)
		{
			CurrentProgress += 9;
			_currentForgeRecipe.StrongHit--;
			NegativeActionsContainer.GetNode<Label>("StrongHit/Amount").Text = _currentForgeRecipe.StrongHit.ToString();
		}
	}

	void OnRightDrawPressed()       // +15
	{
		if (_currentForgeRecipe.Draw > 0)
		{
			CurrentProgress += 15;
			_currentForgeRecipe.Draw--;
			NegativeActionsContainer.GetNode<Label>("Draw/Amount").Text = _currentForgeRecipe.Draw.ToString();
		}
	}

	void OnRightPunchPressed()      // -2
	{
		if (_currentForgeRecipe.Punch > 0)
		{
			CurrentProgress -= 2;
			_currentForgeRecipe.Punch--;
			PositiveActionsContainer.GetNode<Label>("Punch/Amount").Text = _currentForgeRecipe.Punch.ToString();
		}
	}

	void OnRightBendPressed()       // -7
	{
		if (_currentForgeRecipe.Bend > 0)
		{
			CurrentProgress -= 7;
			_currentForgeRecipe.Bend--;
			PositiveActionsContainer.GetNode<Label>("Bend/Amount").Text = _currentForgeRecipe.Bend.ToString();
		}
	}

	void OnRightUpsetPressed()      // -13
	{
		if (_currentForgeRecipe.Upset > 0)
		{
			CurrentProgress -= 13;
			_currentForgeRecipe.Upset--;
			PositiveActionsContainer.GetNode<Label>("Upset/Amount").Text = _currentForgeRecipe.Upset.ToString();
		}
	}

	void OnRightShrinkPressed()     // -16
	{
		if (_currentForgeRecipe.Shrink > 0)
		{
			CurrentProgress -= 16;
			_currentForgeRecipe.Shrink--;
			PositiveActionsContainer.GetNode<Label>("Shrink/Amount").Text = _currentForgeRecipe.Shrink.ToString();
		}
	}
	#endregion

	#region Last Actions Hit

	void HitLastActionButtonPressed(int place, int strength)
	{
		GD.Print($"[Forge/HitButton] Место: {place};\tСила: {strength}");

		switch (place)
		{
			case 1:
				_currentForgeRecipe.LastActions.FirstAction = strength switch
				{
					1 => ForgeDatabase.Action.WeakHit,
					2 => ForgeDatabase.Action.MediumHit,
					3 => ForgeDatabase.Action.StrongHit,
					_ => ForgeDatabase.Action.WeakHit
				};
				break;
			case 2:
				_currentForgeRecipe.LastActions.SecondAction = strength switch
				{
					1 => ForgeDatabase.Action.WeakHit,
					2 => ForgeDatabase.Action.MediumHit,
					3 => ForgeDatabase.Action.StrongHit,
					_ => ForgeDatabase.Action.WeakHit
				};
				break;
			case 3:
				_currentForgeRecipe.LastActions.ThirdAction = strength switch
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
		_lastForgeActions = GD.Load<LastShowForgeActions>(Global.Paths.LastForgeActions + fileName);

		_currentForgeRecipe.LastActions.FirstAction = _lastForgeActions.FirstAction switch
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

		_currentForgeRecipe.LastActions.SecondAction = _lastForgeActions.SecondAction switch
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

		_currentForgeRecipe.LastActions.ThirdAction = _lastForgeActions.ThirdAction switch
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

	void ForgeGoalValueChanged(float value)
	{
		_currentForgeRecipe.RequiredWork = (int)value;
		RecalculateRequiredProgressBar();
	}

	void SetLastForgeActions()
	{
		FirstActionIcon.Icon = GetActionIcon(_lastForgeActions.FirstAction);
		SecondActionIcon.Icon = GetActionIcon(_lastForgeActions.SecondAction);
		ThirdActionIcon.Icon = GetActionIcon(_lastForgeActions.ThirdAction);

		if (_lastForgeActions.FirstAction == ForgeDatabase.ShownAction.Hit)
			FirstActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = true;
		else
			FirstActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = false;

		if (_lastForgeActions.SecondAction == ForgeDatabase.ShownAction.Hit)
			SecondActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = true;
		else
			SecondActionIcon.GetNode<BoxContainer>("HitHBoxContainer").Visible = false;

		if (_lastForgeActions.ThirdAction == ForgeDatabase.ShownAction.Hit)
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



	// ~~~~~~~~~~~~~~~~~~~~ Item Resource Save ~~~~~~~~~~~~~~~~~~~~
	void OnSaveButtonPressed()
	{
		_selectedItem.Name = ItemName.Text;
		_selectedItem.ForgeRecipe = _currentForgeRecipe;
		_selectedItem.LastForgeActions = _lastForgeActions;

		if (_selectedItem.MeltsInto == null)
		{
            _selectedItem.MeltsInto = new()
            {
                MeltsInto = GD.Load<MoltenMetal>(Global.Paths.MoltenMetals +
                            TranslationServer.Translate(_selectedItem.MetalName))
            };
        }
		_selectedItem.MeltsInto.Ingots = (float)IngotAmount.Value;

		_selectedItem.Icon = ItemIcon.Icon;
		GD.Print("[Forge/Save] Item resource path: " + _selectedItem.ResourcePath);
		ResourceSaver.Save(_selectedItem, _selectedItem.ResourcePath);
		_selectedItem = null;
		Visible = false;
		Global.Main.ItemSelection.LoadItemsFromCache();
		Global.Main.ItemSelection.Visible = true;
	}

	void OnCancelButtonPressed()
	{
		_selectedItem = null;
		Visible = false;
		Global.Main.ItemSelection.Visible = true;
	}


	void RecalculateRequiredProgressBar()
	{
		ProgressBar.RequiredProgress = (int)ForgeGoal.Value;
		if (_currentForgeRecipe != null && _currentForgeRecipe.LastActions != null)
		{
			int goal =	(int)ForgeGoal.Value
						- (int)_currentForgeRecipe.LastActions.FirstAction
						- (int)_currentForgeRecipe.LastActions.SecondAction
						- (int)_currentForgeRecipe.LastActions.ThirdAction;
			ProgressBar.RequiredProgressNoLastActions = goal;
			GoalNumber.Text = goal.ToString("000");											
		}
	}

	void LookForHitTypeInLastActions()
	{
		switch (_currentForgeRecipe.LastActions.FirstAction)
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

			switch (_currentForgeRecipe.LastActions.SecondAction)
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

			switch (_currentForgeRecipe.LastActions.ThirdAction)
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
