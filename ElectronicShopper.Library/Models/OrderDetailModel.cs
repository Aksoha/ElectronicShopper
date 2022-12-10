using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ElectronicShopper.Library.Models;

/// <summary>
///     Represents a details of a purchase for one type of a product.
/// </summary>
/// <seealso cref="OrderModel" />
public class OrderDetailModel : IDbEntity, INotifyPropertyChanged
{
    private int _quantity;

    /// <summary>
    ///     Id of a product that was purchased.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    ///     Name of a product that was purchased.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;


    /// <summary>
    ///     Quantity of purchased product.
    /// </summary>
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

    /// <summary>
    ///     Price for which one product was purchased.
    /// </summary>
    /// <seealso cref="InventoryModel.Price"/>
    public decimal PricePerItem { get; set; }

    public int? Id { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;


    /// <summary>
    ///     Invokes <see cref="PropertyChanged" />.
    /// </summary>
    /// <param name="propertyName">Name of property which field has changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    /// <summary>
    ///     Compares <paramref name="field" /> and <paramref name="value" />.
    ///     If object are not equal sets a field to be equal to <paramref name="value" /> and invokes
    ///     <see cref="PropertyChanged" />.
    /// </summary>
    /// <param name="field">Field that is to be updated.</param>
    /// <param name="value">New value of the <paramref name="field" />.</param>
    /// <param name="propertyName">Name of property whose field is to be updated.</param>
    /// <typeparam name="T">The type of the object to set.</typeparam>
    /// <returns><see langword="true" />If field was updated otherwise <see langword="false" />.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}