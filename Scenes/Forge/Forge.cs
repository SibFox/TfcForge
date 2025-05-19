using Godot;
using System;

public partial class Forge : Control
{
	/* 	TODO
		Создать ресурс ForgeRecipe, который будет хранить количество необходимых действий для доведения
		предмета до успешной ковки. Этот же самый ресурс будет сохраняться в предмете или отдельно(?).
		Будет представлен не в виде: 16 + 16 + 16 + 13 + 7 + 7 + 2 - 9 +_ 2 - 6 + 13
		А в виде: 	Shrink - 3
					Crimp - 1
					Bend - 2
					Stamp - 1
					WeakHit - 0
					MediumHit - 0
					StrongHit - 1
		Содержит исключительно необходимые дейтсвия для доведения до точки необходимых последних действий,
		поскольку последние действия представлены отдельным ресурсом.
	*/

	public Item SelectedItem;
	
	private ForgeRecipe _currentForgeRecipe;

	Panel ForgeBG => GetNode<Panel>("ForgeBG");

	Label CurrentProgressNumber => ForgeBG.GetNode<Label>("ActionHBoxContainer/CurrentForgeProgressVBoxContainer/CurrentProgressNumbers");

	private int _currentProgress = 0;

	int CurrentProgress 
	{
		get => 0;
		set 
		{
			// GD.Print($"[Forge] {value}///{_currentProgress}" );
			_currentProgress += value;
			
			CurrentProgressNumber.Text = _currentProgress.ToString();

			if (_currentProgress < 0 || _currentProgress > 150)
			{
				// _currentProgress = 0;
				CurrentProgressNumber.Text = "TR_FORGE_BROKE";
			}
		}
	}

	public void OpenWithNewForgeRecipe()
	{
		_currentForgeRecipe = new ForgeRecipe();
	}

	void OnWeakHitPressed()		// -3
	{
		CurrentProgress -= 3;
		_currentForgeRecipe.WeakHit++;
	}

	void OnMediumHitPressed()	// -6
	{
		CurrentProgress -= 6;
		_currentForgeRecipe.MediumHit++;
	}
	
	void OnStrongHitPressed()	// -9
	{
		CurrentProgress -= 9;
		_currentForgeRecipe.StrongHit++;
	}

	void OnPullPressed()		// -15
	{
		CurrentProgress -= 15;
		_currentForgeRecipe.Pull++;
	}

	void OnStampPressed()		// +2
	{
		CurrentProgress += 2;
		_currentForgeRecipe.Stamp++;
	}

	void OnBendPressed()		// +7
	{
		CurrentProgress += 7;
		_currentForgeRecipe.Bend++;
	}

	void OnCrimpPressed()		// +13
	{
		CurrentProgress += 13;
		_currentForgeRecipe.Crimp++;
	}

	void OnShrinkPressed()		// +16
	{
		CurrentProgress += 16;
		_currentForgeRecipe.Shrink++;
	}
	
}
