using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose(IEnumerable<Item> items)
{
    public void UpdateQuality()
    {
        foreach (var t in items)
        {
            if (t.Name == "Sulfuras, Hand of Ragnaros")
                continue;

            AlterQuality(t, DoesIncreaseInQualityOverTime(t) ? 1 : -1);
            t.SellIn -= 1;
        }
    }

    private static bool DoesIncreaseInQualityOverTime(Item item) => IsBackstagePass(item) || item.Name == "Aged Brie";
    private static bool IsBackstagePass(Item item) => item.Name.StartsWith("Backstage passes", StringComparison.OrdinalIgnoreCase);
    
    private static void AlterQuality(Item item, int value)
    {
        try
        {
            if (IsBackstagePass(item))
            {
                item.Quality += item.SellIn switch
                {
                    > 10 => 1,
                    < 11 and > 5 => 2,
                    <= 5 => 3
                };

                if (item.SellIn < 1) 
                    item.Quality = 0;
                
                return;
            }

            if (item.SellIn < 1)
                value *= 2;

            if (item.Name.StartsWith("Conjured", StringComparison.OrdinalIgnoreCase))
                value *= 2;
            
            item.Quality += value;
        }
        finally
        {
            EnforceQualityBoundaries(item);
        }
    }

    private static void EnforceQualityBoundaries(Item item)
    {
        if (item.Quality > 50)
            item.Quality = 50;
        
        if (item.Quality < 0)
            item.Quality = 0;
    }
}