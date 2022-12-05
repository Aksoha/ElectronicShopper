using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectronicShopper.Extensions.System;

public static class EnumExtensions
{
    public static string? GetEnumDisplayName(this Enum e)
    {
        return e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name;
    }
}