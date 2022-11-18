using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ElectronicShopper.Library.Models;

public class OrderDetailModel : IDbEntity, INotifyPropertyChanged
{
    public int? Id { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Range(0, double.PositiveInfinity)]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value == _quantity) return;
            _quantity = value;
            OnPropertyChanged();
        }
    }

    public decimal PricePerItem { get; set; }
    private int _quantity;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}