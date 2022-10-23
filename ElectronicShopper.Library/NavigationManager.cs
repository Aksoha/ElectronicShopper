using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ElectronicShopper.Library;

public class Navigation : IDisposable
{
    
    private const int HistorySize = 256;
    private readonly NavigationManager _navigationManager;
    private readonly List<string> _history;
    
    public Navigation(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _history = new List<string>(HistorySize) { _navigationManager.Uri };
        _navigationManager.LocationChanged += OnLocationChanged;
    }
    
    public void NavigateTo(string url, bool forceLoad = false)
    {
        _navigationManager.NavigateTo(url, forceLoad);
    }

    private bool CanNavigateBack => _history.Count >= 2;
    
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
    
    private void EnsureSize()
    {
        if (_history.Count < HistorySize) return;
        _history.RemoveRange(0, _history.Count - HistorySize);
    }
    
    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}