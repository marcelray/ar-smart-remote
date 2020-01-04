using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumUtils
{
	// Source: https://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp
	public static string GetDescription(this Enum value)
	{            
		FieldInfo field = value.GetType().GetField(value.ToString());

		DescriptionAttribute attribute
				= Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
					as DescriptionAttribute;

		return attribute == null ? value.ToString() : attribute.Description;
	}
}