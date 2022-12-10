using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ElectronicShopper.Library;

/// <inheritdoc cref="NavigationManager" />
public class Navigation : IDisposable
{
    /// <summary>
    ///     Maximum number of steps back that can be taken by the <see cref="NavigateTo" />.
    /// </summary>
    private const int HistorySize = 256;

    /// <summary>
    ///     history containing last N numbers of URI, where N = <see cref="HistorySize" />.
    /// </summary>
    private readonly List<string> _history;

    private readonly NavigationManager _navigationManager;

    public Navigation(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _history = new List<string>(HistorySize) { _navigationManager.Uri };
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private bool CanNavigateBack => _history.Count >= 2;

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }


    /// <inheritdoc cref="NavigationManager.NavigateTo(string,bool)" />
    public void NavigateTo(string uri, bool forceLoad = false)
    {
        _navigationManager.NavigateTo(uri, forceLoad);
    }


    /// <summary>
    ///     Navigates to previous URI.
    /// </summary>
    public void NavigateBack()
    {
        if (CanNavigateBack == false)
            return;

        var backPageUrl = _history[^2];
        _history.RemoveRange(_history.Count - 2, 2);
        _navigationManager.NavigateTo(backPageUrl);
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        EnsureSize();
        _history.Add(e.Location);
    }


    /// <summary>
    ///     Ensures that the size of <see cref="_history" /> never exceeds <see cref="HistorySize" />
    ///     by removing oldest item from the history when needed.
    /// </summary>
    private void EnsureSize()
    {
        if (_history.Count < HistorySize) return;
        _history.RemoveRange(0, _history.Count - HistorySize);
    }
}