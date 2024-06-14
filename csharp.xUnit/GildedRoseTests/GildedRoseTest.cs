using Xunit;
using System.Collections.Generic;
using GildedRoseKata;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Theory]
    [InlineData(10, 5, 9, 4)] // At the end of each day our system lowers both values for every item
    [InlineData(0, 5, -1, 3)] // Once the sell by date has passed, Quality degrades twice as fast
    [InlineData(10, 0, 9, 0)] // The Quality of an item is never negative
    public void Update_quality_works_as_expected(int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        // Arrange
        var items = new List<Item> { new() { Name = nameof(Update_quality_works_as_expected), SellIn = sellIn, Quality = quality } };
        var sut = new GildedRose(items);
        
        // Act
        sut.UpdateQuality();
        
        // Assert
        Assert.Equal(expectedSellIn, items[0].SellIn);
        Assert.Equal(expectedQuality, items[0].Quality);
    }

    [Theory]
    // "Aged Brie" actually increases in Quality the older it gets
    [InlineData("Aged Brie",10, 5, 9, 6)] 
    [InlineData("Aged Brie",10, 50, 9, 50)] // The Quality of an item is never more than 50
    // "Sulfuras, Hand of Ragnaros", being a legendary item, never has to be sold or decreases in Quality
    [InlineData("Sulfuras, Hand of Ragnaros",10, 80, 10, 80)] 
    // "Backstage passes", like aged brie, increases in Quality as its SellIn value approaches
    // "Conjured" items degrade in Quality twice as fast as normal items
    [InlineData("Conjured Mana Cake",10, 5, 9, 3)] 
    [InlineData("Conjured Wizard's Sleeve",10, 5, 9, 3)] 
    public void Certain_items_have_special_use_cases(string item, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        // Arrange
        var items = new List<Item> { new() { Name = item, SellIn = sellIn, Quality = quality } };
        var sut = new GildedRose(items);
        
        // Act
        sut.UpdateQuality();
        
        // Assert
        Assert.Equal(item, items[0].Name);
        Assert.Equal(expectedSellIn, items[0].SellIn);
        Assert.Equal(expectedQuality, items[0].Quality);
    }
    
    [Theory]
    [InlineData("Backstage passes to a TAFKAL80ETC concert",15, 5, 14, 6)] 
    [InlineData("Backstage passes to a Busted concert",15, 5, 14, 6)] 
    // - Quality increases by 2 when there are 10 days or less
    [InlineData("Backstage passes to a TAFKAL80ETC concert",10, 5, 9, 7)] 
    // - Quality increases by 3 when there are 5 days or less 
    [InlineData("Backstage passes to a TAFKAL80ETC concert",5, 5, 4, 8)] 
    [InlineData("Backstage passes to a TAFKAL80ETC concert",1, 5, 0, 8)] 
    [InlineData("Backstage passes to a TAFKAL80ETC concert",0, 5, -1, 0)] 
    // - Quality drops to 0 after the concert
    [InlineData("Backstage passes to a TAFKAL80ETC concert",-1, 5, -2, 0)] 
    public void Backstage_passes_have_special_use_cases(string item, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        // Arrange
        var items = new List<Item> { new() { Name = item, SellIn = sellIn, Quality = quality } };
        var sut = new GildedRose(items);
        
        // Act
        sut.UpdateQuality();
        
        // Assert
        Assert.Equal(item, items[0].Name);
        Assert.Equal(expectedSellIn, items[0].SellIn);
        Assert.Equal(expectedQuality, items[0].Quality);
    }
}