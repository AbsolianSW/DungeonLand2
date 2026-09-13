using System;
using System.Collections.ObjectModel;
public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Deadly,
    DeadlyII,
    DeadlyIII,
    DeadlyIV,
    DeadlyV,

}

public class EncounterThreshold
{
    public int Easy { get; set; }
    public int Medium { get; set; }
    public int Hard { get; set; }
    public int Deadly { get; set; }
    public int DeadlyII { get; set; }
    public int DeadlyIII { get; set; }
    public int DeadlyIV { get; set; }
    public int DeadlyV{ get; set; }

}
public class LootProbability
{
    public int Common { get; set; }
    public int Uncommon { get; set; }
    public int Rare { get; set; }
    public int VeryRare { get; set; }
    public int Legendary { get; set; }
    public int Artifact { get; set; }

}

public class LootCalculator
{
    public readonly Dictionary<int[], LootProbability> lootTable = new()
    {
        { [1,2], new LootProbability{Common=25, Uncommon=0, Rare=0, VeryRare=0, Legendary=0, Artifact=0 } },
        { [3,4], new LootProbability{Common=50, Uncommon=25, Rare=0, VeryRare=0, Legendary=0, Artifact=0 } },
        { [5,6,7,8,9,10], new LootProbability{Common=75, Uncommon=50, Rare=25, VeryRare=0, Legendary=0, Artifact=0 } },
        { [11,12,13,14,15,16], new LootProbability{Common=50, Uncommon=75, Rare=50, VeryRare=25, Legendary=0, Artifact=0 } },
        { [17,18,19], new LootProbability{Common=25, Uncommon=50, Rare=75, VeryRare=50, Legendary=25, Artifact=0 } },
        { [20], new LootProbability{Common=0, Uncommon=25, Rare=50, VeryRare=75, Legendary=50, Artifact=5 } },
    };
}
public class EncounterCalculator
{

        public readonly Dictionary<int, EncounterThreshold> encounterTable = new()
        {
            {1, new EncounterThreshold{Easy=25, Medium=50, Hard=75, Deadly=100, DeadlyII=200, DeadlyIII=300, DeadlyIV=400, DeadlyV=500}},
            {2, new EncounterThreshold{Easy=50, Medium=100, Hard=150, Deadly=200, DeadlyII=400, DeadlyIII=600, DeadlyIV=800, DeadlyV=1000}},
            {3, new EncounterThreshold{Easy=75, Medium=150, Hard=225, Deadly=400, DeadlyII=800, DeadlyIII=1200, DeadlyIV=1600, DeadlyV=2000}},
            {4, new EncounterThreshold{Easy=125, Medium=250, Hard=375, Deadly=500, DeadlyII=1000, DeadlyIII=1500, DeadlyIV=2000, DeadlyV=2500}},
            {5, new EncounterThreshold{Easy=250, Medium=500, Hard=750, Deadly=1100, DeadlyII=2200, DeadlyIII=3300, DeadlyIV=4400, DeadlyV=5500}},
            {6, new EncounterThreshold{Easy=300, Medium=600, Hard=900, Deadly=1400, DeadlyII=2800, DeadlyIII=4200, DeadlyIV=5600, DeadlyV=7000}},
            {7, new EncounterThreshold{Easy=350, Medium=750, Hard=1100, Deadly=1700, DeadlyII=3400, DeadlyIII=5100, DeadlyIV=6800, DeadlyV=8500}},
            {8, new EncounterThreshold{Easy=450, Medium=900, Hard=1400, Deadly=2100, DeadlyII=4200, DeadlyIII=6300, DeadlyIV=8400, DeadlyV=10500}},
            {9, new EncounterThreshold{Easy=550, Medium=1100, Hard=1600, Deadly=2400, DeadlyII=4800, DeadlyIII=7200, DeadlyIV=9600, DeadlyV=12000}},
            {10, new EncounterThreshold{Easy=600, Medium=1200, Hard=1900, Deadly=2800, DeadlyII=5600, DeadlyIII=8400, DeadlyIV=11200, DeadlyV=14000}},
            {11, new EncounterThreshold{Easy=800, Medium=1600, Hard=2400, Deadly=3600, DeadlyII=7200, DeadlyIII=10800, DeadlyIV=14400, DeadlyV=18000}},
            {12, new EncounterThreshold{Easy=1000, Medium=2000, Hard=3000, Deadly=4500, DeadlyII=9000, DeadlyIII=13500, DeadlyIV=18000, DeadlyV=22500}},
            {13, new EncounterThreshold{Easy=1100, Medium=2200, Hard=3400, Deadly=5100, DeadlyII=10200, DeadlyIII=15300, DeadlyIV=20400, DeadlyV=25500}},
            {14, new EncounterThreshold{Easy=1250, Medium=2500, Hard=3800, Deadly=5700, DeadlyII=11400, DeadlyIII=17100, DeadlyIV=22800, DeadlyV=28500}},
            {15, new EncounterThreshold{Easy=1400, Medium=2800, Hard=4300, Deadly=6400, DeadlyII=12800, DeadlyIII=19200, DeadlyIV=25600, DeadlyV=32000}},
            {16, new EncounterThreshold{Easy=1600, Medium=3200, Hard=4800, Deadly=7200, DeadlyII=14400, DeadlyIII=21600, DeadlyIV=28800, DeadlyV=36000}},
            {17, new EncounterThreshold{Easy=2000, Medium=3900, Hard=5900, Deadly=8800, DeadlyII=17600, DeadlyIII=26400, DeadlyIV=35200, DeadlyV=44000}},
            {18, new EncounterThreshold{Easy=2100, Medium=4200, Hard=6300, Deadly=9500, DeadlyII=19000, DeadlyIII=28500, DeadlyIV=38000, DeadlyV=47500}},
            {19, new EncounterThreshold{Easy=2400, Medium=4900, Hard=7300, Deadly=10900, DeadlyII=21800, DeadlyIII=32700, DeadlyIV=43600, DeadlyV=54500}},
            {20, new EncounterThreshold{Easy=2800, Medium=5700, Hard=8500, Deadly=12700, DeadlyII=25400, DeadlyIII=38100, DeadlyIV=50800, DeadlyV=63500}},
        };  
    


public int GetEncounterXP(List<int> playerLevels, Difficulty difficulty)
{
    int total = 0;

    foreach (int level in playerLevels)
    {
        var t = encounterTable[level];

        total += difficulty switch
        {
            Difficulty.Easy => t.Easy,
            Difficulty.Medium => t.Medium,
            Difficulty.Hard => t.Hard,
            Difficulty.Deadly => t.Deadly,
            Difficulty.DeadlyII => t.DeadlyII,
            Difficulty.DeadlyIII => t.DeadlyIII,
            Difficulty.DeadlyIV=> t.DeadlyIV,
            Difficulty.DeadlyV => t.DeadlyV,

            _ => 0
        };
    }

    return total;
}
}