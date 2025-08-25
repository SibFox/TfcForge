using Godot;
using System;

public partial class ForgeProgressBar : Control
{
	TextureRect CurrentPointer => GetNode<TextureRect>("CurrentPointer");
	TextureRect RequiredPointer => GetNode<TextureRect>("RequiredPointer");
	TextureRect RequiredPointerNoLastActions => GetNode<TextureRect>("RequiredPointerNoLastActions");

	private Vector2 CurrentPointerOffset = new(-3, 1);
	private Vector2 RequiredPointerOffset = new(2, 1);

	public int RequiredProgress
	{
		set
		{
			RequiredPointer.Position = new Vector2(Math.Clamp(value, 0, 150), 0) + RequiredPointerOffset;
		}
	}
	public int RequiredProgressNoLastActions
	{
		set
		{
			RequiredPointerNoLastActions.Position = new Vector2(Math.Clamp(value, 0, 150), 0) + RequiredPointerOffset;
		}
	}
	public int CurrentProgress
	{
		set
		{
			CurrentPointer.Position = new Vector2(Math.Clamp(value, 0, 150), 0) + CurrentPointerOffset;
		}
	}
}
