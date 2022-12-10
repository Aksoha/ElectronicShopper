using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectronicShopper.Extensions.System;

public static class EnumExtensions
{
    /// <summary>
    ///     Provides a name that was added by <see cref="DisplayAttribute" /> on an enum.
    /// </summary>
    /// <param name="e">The enum.</param>
    public static string? GetEnumDisplayName(this Enum e)
    {
        return e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name;
    }
}