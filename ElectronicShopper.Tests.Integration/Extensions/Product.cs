using Bogus;

namespace ElectronicShopper.Tests.Integration.Extensions;

internal class Product : DataSet
{


    private static readonly string[] Categories =
    {
        "Mobile Devices",
        "Wearables",
        "TV",
        "Monitors",
        "Laptops",
        "Tablets",
        "Computers",
        "Printers",
        "Scanners"
    };
    
    private static readonly List<(string Key, List<string> Value)> Properties = new()
    {
        ("Screen size", new List<string> {"21\""}),
        ("Screen size", new List<string> {"27\""}),
        ("Screen size", new List<string> {"32\""}),
        ("RAM", new List<string> {"1x8GB"}),
        ("RAM", new List<string> {"2x8GB"}),
        ("RAM", new List<string> {"1x16GB"}),
        ("RAM", new List<string> {"2x16GB"}),
        ("RAM", new List<string> {"1x32GB"}),
        ("RAM", new List<string> {"2x32GB"}),
        ("Storage", new List<string> {"256GB HDD"}),
        ("Storage", new List<string> {"512GB HDD"}),
        ("Storage", new List<string> {"1TB HDD"}),
        ("Storage", new List<string> {"128GB SSD"}),
        ("Storage", new List<string> {"256GB SSD"}),
        ("Storage", new List<string> {"512GB SSD"}),
        ("Storage", new List<string> {"128GB SSD", "512GB HDD"}),
        ("Storage", new List<string> {"128GB SSD", "1TB HDD"}),
        ("Storage", new List<string> {"256GB SSD", "512GB HDD"}),
        ("Storage", new List<string> {"256GB SSD", "1TB HDD"}),
        ("Weight", new List<string> {"1.9kg"}),
        ("Weight", new List<string> {"4.6kg"}),
        ("Weight", new List<string> {"13.2kg"}),
        ("Weight", new List<string> {"3.3kg"}),
        ("CPU", new List<string> {"Intel Core i5-10400F"}),
        ("CPU", new List<string> {"Intel® Core™ i7-13700KF"}),
        ("CPU", new List<string> {"AMD Ryzen 7 5800X3D"}),
        ("CPU", new List<string> {"Intel Core i3-12100F"}),
        ("Refresh rate", new List<string> {"60Hz"}),
        ("Refresh rate", new List<string> {"100Hz"}),
        ("Refresh rate", new List<string> {"120Hz"}),
        ("Refresh rate", new List<string> {"240Hz"}),
        ("Resolution", new List<string> {"1920x1080"}),
        ("Resolution", new List<string> {"2560x1440"}),
        ("Resolution", new List<string> {"3840x2160"}),
        ("Resolution", new List<string> {"3440x1440"}),
        ("Ports", new List<string> {"1x USB 2.0"}),
        ("Ports", new List<string> {"2x USB 2.0"}),
        ("Ports", new List<string> {"1x USB 3.2"}),
        ("Ports", new List<string> {"2x USB 3.2"}),
        ("Ports", new List<string> {"1x USB 3.2", "1x USB 2.0"}),
        ("Ports", new List<string> {"1x USB 3.2", "2x USB 2.0"}),
        ("Ports", new List<string> {"2x USB 3.2", "1x Thunderbolt 4"}),
        ("Response time", new List<string> {"1ms"}),
        ("Response time", new List<string> {"2ms"}),
        ("Response time", new List<string> {"5ms"}),
    };
    
    private static readonly List<string> Templates = Properties.Select(x => x.Key).Distinct().ToList();

    public new string Category()
    {
        return Random.ArrayElement(Categories);
    }

    public IEnumerable<string> Template(int num)
    {
        var result = new string[num];
        var keys = Properties.Select(x => x.Key).ToArray();

        for( var i = 0; i < num; i++ )
        {
            result[i] = Random.ArrayElement(keys);
        }
        return result;
    }


    public string[] UniqueTemplate(int num)
    {
        var result = new string[num];
        if (Templates.Count < num)
            throw new ArgumentOutOfRangeException($"num must be between 0 and {Templates.Count}");

        var temp = new List<string>(Templates);
        
        for( var i = 0; i < num; i++ )
        {
            var selectedItem = Random.CollectionItem(temp);
            result[i] = selectedItem;
            temp.Remove(selectedItem);
        }
        return result;
    }
    
    public Dictionary<string, List<string>> Property(int num)
    {
        if (num < 0 || num > Templates.Count)
            throw new ArgumentOutOfRangeException($"num must be between 0 to {Templates.Count}");
            
        var result = new Dictionary<string, List<string>>();
        var r = new Random();

        var i = 0;
        while (i < num)
        {
            var element = Properties.ElementAt(r.Next(0, Properties.Count));
            var added = result.TryAdd(element.Key, element.Value);
            if (added)
                i++;
        }
        return result;
    }
    

    public (string Key, List<string> Value) Property(string template)
    {
        var filter = Properties.Where(x => x.Key == template).ToArray();
        return Random.ArrayElement(filter);
    }

}