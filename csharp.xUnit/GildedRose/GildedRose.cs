using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        foreach (var t in Items)
        {
            if (t.Name == SpecialItems.SulfurasHandOfRagnaros)
                continue;

            // Change quality
            switch (t.Name)
            {
                case SpecialItems.AgedBrie:
                case SpecialItems.BackstagePasses:
                    AlterQuality(t, 1);
                    break;
                default:
                    AlterQuality(t, -1);
                    break;
            }

            t.SellIn -= 1;
        }
    }

    private static void AlterQuality(Item item, int value)
    {
        if (item.Quality + value > 50)
        {
            item.Quality = 50;
            return;
        }

        if (item.Name == SpecialItems.BackstagePasses)
        {
            item.Quality += item.SellIn switch
            {
                > 10 => 1,
                < 11 and > 5 => 2,
                <= 5 => 3
            };

            if (item.SellIn < 0)
                item.Quality = 0;

            return;
        }

        if (item.SellIn < 1)
            value *= 2;
            
        if (string.Equals(item.Name, SpecialItems.ConjuredManaCake, StringComparison.OrdinalIgnoreCase))
            value *= 2;
        
        item.Quality += value;

        if (item.Quality < 0)
            item.Quality = 0;
    }
}