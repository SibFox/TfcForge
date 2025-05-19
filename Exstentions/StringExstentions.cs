using Godot;

public static class StringExstensions
{
	public static string GetNameFromTransltaionCode(this string tr_str) => tr_str.Replace("TR_", "").Replace('_', ' ').ToPascalCase();
}
