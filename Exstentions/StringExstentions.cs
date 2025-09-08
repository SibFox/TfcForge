using Godot;

public static class StringExstensions
{
	public static string GetNameFromTransltaionCode(this string tr_str) => tr_str.Replace('.', ' ')
																				 .Replace("metal", " ")
																				 .Replace("item", " ")
																				 .Replace("tool", " ")
																				 .Replace("misc", " ")
																				 .Replace("equipment", " ")
																				 .Replace("name", " ")
																				 .Replace("weapon", " ")
																				 .Replace("decor", " ")
																				 .Replace("component", " ")
																				 .Replace('_', ' ')
																				 .ToPascalCase();
}
