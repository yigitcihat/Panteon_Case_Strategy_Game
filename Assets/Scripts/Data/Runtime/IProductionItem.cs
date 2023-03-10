using UnityEngine;

public interface IProductionItem
{
    /// <summary>
    /// The sprite of this item
    /// </summary>
    Sprite sprite { get; }

    /// <summary>
    /// Returns this items position within an inventory
    /// </summary>
    Vector2Int position { get; set; }

    /// <summary>
    /// The width of this item
    /// </summary>
    int width { get;}

    /// <summary>
    /// The height of this item
    /// </summary>
    int height { get;}

    /// <summary>
    /// Returns true if given local position is part 
    /// of this items shape
    /// </summary>
    bool IsPartOfShape(Vector2Int localPosition);
    public GameObject Object { get; set; }
    /// <summary>
    /// Returns true if this item can be dropped on the ground
    /// </summary>
    public bool canDrop { get; set; }

    public ItemDefinition itemDefination { get; set; }
}

internal static class ProductionItemExtensions
{
    /// <summary>
    /// Returns the lower left corner position of an item 
    /// within its inventory
    /// </summary>
    internal static Vector2Int GetMinPoint(this IProductionItem item)
    {
        return item.position;
    }

    /// <summary>
    /// Returns the top right corner position of an item 
    /// within its inventory
    /// </summary>
    internal static Vector2Int GetMaxPoint(this IProductionItem item)
    {
        return item.position + new Vector2Int(item.width, item.height);
    }

    /// <summary>
    /// Returns true if this item overlaps the given point within an inventory
    /// </summary>
    internal static bool Contains(this IProductionItem item, Vector2Int gridPoint)
    {
        for (var iX = 0; iX < item.width; iX++)
        {
            for (var iY = 0; iY < item.height; iY++)
            {
                var iPoint = item.position + new Vector2Int(iX, iY);
                if (iPoint == gridPoint) { return true; }
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true of this item overlaps a given item
    /// </summary>
    internal static bool Overlaps(this IProductionItem item, IProductionItem otherItem)
    {
        for (var iX = 0; iX < item.width; iX++)
        {
            for (var iY = 0; iY < item.height; iY++)
            {
                if (item.IsPartOfShape(new Vector2Int(iX, iY)))
                {
                    var iPoint = item.position + new Vector2Int(iX, iY);
                    for (var oX = 0; oX < otherItem.width; oX++)
                    {
                        for (var oY = 0; oY < otherItem.height; oY++)
                        {
                            if (otherItem.IsPartOfShape(new Vector2Int(oX, oY)))
                            {
                                var oPoint = otherItem.position + new Vector2Int(oX, oY);
                                if (oPoint == iPoint) { return true; } // Hit! Items overlap
                            }
                        }
                    }
                }
            }
        }
        return false; // Items does not overlap
    }
}
