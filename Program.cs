using System.Collections;
using System.Data;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

ArrayList PoolMutations = new ArrayList();
ArrayList CommonItems = new ArrayList();
ArrayList UncommonItems = new ArrayList();
ArrayList RareItems = new ArrayList();
ArrayList VeryRareItems = new ArrayList();
ArrayList LegendaryItems = new ArrayList();
ArrayList ArtifactItems = new ArrayList();

List<int> PlayerLevel = new List<int> { 1, 1, 1};
int MonsterCount = 1;
int CurrentRoom = 1;
Difficulty difficulty = Difficulty.Medium;

string TrapDmgValue = "";

bool generateStaircase = false;

Dictionary<int, String> MagicItemCapstones = new Dictionary<int, String>()
{
    {1,"Common" },
    {3,"Uncommon" },
    {5,"Rare" },
    {11,"VeryRare" },
    {17,"Legendary" },
    {20, "Artifact" }
};

ArrayList skills = new ArrayList()
{
    "Acrobatics",
    "Animal Handling",
    "Arcana",
    "Athletics",
    "Deception",
    "History",
    "Insight",
    "Intimidation",
    "Investigation",
    "Medicine",
    "Nature",
    "Perception",
    "Performance",
    "Persuasion",
    "Religion",
    "Sleight of Hand",
    "Stealth",
    "Survival"
};

Dictionary<int, List<String>> MonsterManual = new Dictionary<int, List<String>>()
{
    {5, new List<String>{ "Awakened shrub", "Baboon", "Badger", "Bat", "Cat", "Commoner", "Crab", "Crawling claw", "Deer", "Eagle", "Frog", "Giant fire beetle", "Goat", "Hawk", "Homunculus", "Hyena", "Jackal", "Lemure", "Lizard", "Myconid sprout", "Octopus", "Owl", "Quipper", "Rat", "Raven", "Scorpion", "Sea horse", "Shrieker", "Spider", "Vulture", "Weasel" }},
    {25, new List<String>{"Bandit", "Blood hawk", "Camel", "Cultist", "Flumph", "Flying snake", "Giant crab", "Giant rat", "Giant weasel", "Guard", "Kobold", "Manes", "Mastiff", "Merfolk", "Monodrone", "Mule", "Noble", "Poisonous snake", "Pony", "Slaad tadpole", "Stirge", "Tribal warrior", "Twig blight"}},
    {50, new List<String>{"Aarakocra", "Acolyte", "Axe beak", "Blink dog", "Boar", "Bullywug", "Constrictor snake", "Draft horse", "Dretch", "Drow", "Duodrone", "Elk", "Flying sword", "Giant badger", "Giant bat", "Giant centipede", "Giant frog", "Giant lizard", "Giant owl", "Giant poisonous snake", "Giant wolf spider", "Goblin", "Grimlock", "Kenku", "Kuo-toa", "Mud mephit", "Needle blight", "Panther", "Pixie", "Pseudodragon", "Pteranodon", "Riding horse", "Skeleton", "Smoke mephit", "Sprite", "Steam mephit", "Swarm of bats", "Swarm of rats", "Swarm of ravens", "Troglodyte", "Violet fungus", "Winged kobold", "Wolf", "Zombie"}},
    {100, new List<String>{"Ape", "Black bear", "Cockatrice", "Crocodile", "Darkmantle", "Deep gnome", "Dust mephit", "Gas spore", "Giant goat", "Giant sea horse", "Giant wasp", "Gnoll", "Gray ooze", "Hobgoblin", "Ice mephit", "Jackalwere", "Lizardfolk", "Magma mephit", "Magmin", "Myconid adult", "Orc", "Piercer", "Reef shark", "Rust monster", "Sahuagin", "Satyr", "Scout", "Shadow", "Swarm of insects", "Thug", "Tridrone", "Vine blight", "Warhorse", "Warhorse skeleton", "Worg"}},
    {200, new List<String>{"Animated armor", "Brass dragon wyrmling", "Brown bear", "Bugbear", "Copper dragon wyrmling", "Death dog", "Dire wolf", "Dryad", "Duergar", "Faerie dragon (young)", "Fire snake", "Ghoul", "Giant eagle", "Giant hyena", "Giant octopus", "Giant spider", "Giant toad", "Giant vulture", "Goblin boss", "Half-ogre", "Harpy", "Hippogriff", "Imp", "Kuo-toa whip", "Lion", "Quadrone", "Quaggoth spore servant", "Quasit", "Scarecrow", "Specter", "Spy", "Swarm of quippers", "Thri-kreen", "Tiger", "Yuan-ti pureblood"}},
    {450, new List<String>{"Allosaurus", "Ankheg", "Awakened tree", "Azer", "Bandit captain", "Berserker", "Black dragon wyrmling", "Bronze dragon wyrmling", "Carrion crawler", "Centaur", "Cult fanatic", "Druid", "Ettercap", "Faerie dragon (old)", "Gargoyle", "Gelatinous cube", "Ghast", "Giant boar", "Giant constrictor snake", "Giant elk", "Gibbering mouther", "Githzerai monk", "Gnoll pack lord", "Green dragon wyrmling", "Grick", "Griffon", "Hunter shark", "Intellect devourer", "Lizardfolk shaman", "Merrow", "Mimic", "Minotaur skeleton", "Myconid sovereign", "Nothic", "Ochre jelly", "Ogre", "Ogre zombie", "Orc Eye of Gruumsh", "Orog", "Pegasus", "Pentadrone", "Peryton", "Plesiosaurus", "Polar bear", "Poltergeist (specter)", "Priest", "Quaggoth", "Rhinoceros", "Rug of smothering", "Saber-toothed tiger", "Sahuagin priestess", "Sea hag", "Silver dragon wyrmling", "Spined devil", "Swarm of poisonous snakes", "Wererat", "White dragon wyrmling", "Will-o’-wisp"}},
    {700, new List<String>{"Ankylosaurus", "Basilisk", "Bearded devil", "Blue dragon wyrmling", "Bugbear chief", "Displacer beast", "Doppelganger", "Giant scorpion", "Githyanki warrior", "Gold dragon wyrmling", "Green hag", "Grell", "Hell hound", "Hobgoblin captain", "Hook horror", "Killer whale", "Knight", "Kuo-toa monitor", "Manticore", "Minotaur", "Mummy", "Nightmare", "Owlbear", "Phase spider", "Quaggoth thonot", "Spectator", "Veteran", "Water weird", "Werewolf", "Wight", "Winter wolf", "Yeti", "Yuan-ti malison"}},
    {1100, new List<String>{"Banshee", "Black pudding", "Bone naga", "Chuul", "Couatl", "Elephant", "Ettin", "Flameskull", "Ghost", "Gnoll fang of Yeenoghu", "Helmed horror", "Incubus", "Lamia", "Lizard king/queen", "Orc war chief", "Red dragon wyrmling", "Sea hag (in coven)", "Shadow demon", "Succubus", "Wereboar", "Weretiger"}},
    {1800, new List<String>{"Air elemental", "Barbed devil", "Barlgura", "Beholder zombie", "Bulette", "Cambion", "Drow elite warrior", "Earth elemental", "Fire elemental", "Flesh golem", "Giant crocodile", "Giant shark", "Gladiator", "Gorgon", "Green hag (in coven)", "Half-red dragon veteran", "Hill giant", "Mezzoloth", "Night hag", "Otyugh", "Red slaad", "Revenant", "Roper", "Sahuagin baron", "Salamander", "Shambling mound", "Triceratops", "Troll", "Umber hulk", "Unicorn", "Vampire spawn", "Water elemental", "Werebear", "Wraith", "Xorn", "Young remorhaz"}},
    {2300, new List<String>{"Chasme", "Chimera", "Cyclops", "Drider", "Galeb duhr", "Githzerai zerth", "Hobgoblin warlord", "Invisible stalker", "Kuo-toa archpriest", "Mage", "Mammoth", "Medusa", "Vrock", "Wyvern", "Young brass dragon", "Young white dragon"}},
    {2900, new List<String>{"Blue slaad", "Drow mage", "Giant ape", "Grick alpha", "Mind flayer", "Night hag (in coven)", "Oni", "Shield guardian", "Stone giant", "Young black dragon", "Young copper dragon", "Yuan-ti abomination"}},
    {3900, new List<String>{"Assassin", "Chain devil", "Cloaker", "Drow priestess of Lolth", "Fomorian", "Frost giant", "Githyanki knight", "Green slaad", "Hezrou", "Hydra", "Mind flayer arcanist", "Spirit naga", "Tyrannosaurus rex", "Young bronze dragon", "Young green dragon"}},
    {5000, new List<String>{"Abominable yeti", "Bone devil", "Clay golem", "Cloud giant", "Fire giant", "Glabrezu", "Gray slaad", "Nycaloth", "Treant", "Young blue dragon", "Young silver dragon"}},
    {5900, new List<String>{"Aboleth", "Death slaad", "Deva", "Guardian naga", "Stone golem", "Yochlol", "Young gold dragon", "Young red dragon"}},
    {7200, new List<String>{"Behir", "Dao", "Djinni", "Efreeti", "Gynosphinx", "Horned devil", "Marid", "Remorhaz", "Roc", "Baba Lysaga", "Baba Lysaga Creeping Hut"}},
    {8400, new List<String>{"Arcanaloth", "Archmage", "Erinyes"}},
    {10000, new List<String>{"Adult brass dragon", "Adult white dragon", "Beholder (not in lair)", "Nalfeshnee", "Rakshasa", "Storm giant", "Ultroloth", "Vampire", "Young red shadow dragon"}},
    {11500, new List<String>{"Adult black dragon", "Adult copper dragon", "Beholder (in lair)", "Death tyrant (not in lair)", "Ice devil"}},
    {13000, new List<String>{"Adult bronze dragon", "Adult green dragon", "Death tyrant (in lair)", "Mummy lord (not in lair)", "Purple worm", "Vampire (spellcaster)", "Vampire (warrior)"}},
    {15000, new List<String>{"Adult blue dragon", "Adult silver dragon", "Iron golem", "Marilith", "Mummy lord (in lair)", "Planetar"}},
    {18000, new List<String>{"Adult blue dracolich", "Adult gold dragon", "Adult red dragon", "Androsphinx", "Death knight", "Dragon turtle", "Goristro"}},
    {20000, new List<String>{"Demilich (not in lair)"}},
    {22000, new List<String>{"Balor"}},
    {25000, new List<String>{"Ancient brass dragon", "Ancient white dragon", "Demilich (in lair)", "Pit fiend"}},
    {33000, new List<String>{"Ancient black dragon", "Ancient copper dragon", "Lich (not in lair)", "Solar"}},
    {41000, new List<String>{"Ancient bronze dragon", "Ancient green dragon", "Lich (in lair)"}},
    {50000, new List<String>{"Ancient blue dragon", "Ancient silver dragon", "Empyrean", "Kraken"}},
    {62000, new List<String>{"Ancient gold dragon", "Ancient red dragon"}},
    {155000, new List<String>{"Tarrasque"}},
};

int[] Challange =
{
    5,
    25,
    50,
    100,
    200,
    450,
    700,
    1100,
    1800,
    2300,
    2900,
    3900,
    5000,
    5900,
    7200,
    8400,
    10000,
    11500,
    13000,
    15000,
    18000,
    20000,
    22000,
    25000,
    33000,
    41000,
    50000,
    62000,
    155000
};

Console.WriteLine("p for generating a Pool");
Console.WriteLine("c for generating combat according to difficulty");
Console.WriteLine("l for generating loot");
Console.WriteLine("r for generating a room");

LoadPools();
LoadCommon();
LoadUncommon();
LoadRare();
LoadVeryRare();
LoadLegendary();
LoadArtifact();
while (true)
{
    string input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        GeneratorManager(input, PoolMutations);
    }
    else
    {
        Console.WriteLine("Invalid Command");
    }
}

void LoadPools()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\PoolMutations.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            PoolMutations.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
void LoadCommon()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\Common.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            CommonItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
void LoadUncommon()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\Uncommon.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            UncommonItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
void LoadRare()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\Rare.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            RareItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
void LoadVeryRare()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\VeryRare.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            VeryRareItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
void LoadLegendary()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\Legendary.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            LegendaryItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

}
void LoadArtifact()
{
    String line;
    try
    {
        StreamReader sr = new StreamReader("C:\\Users\\Eike\\source\\repos\\DungeonLandGenerator\\Artifact.txt");
        line = sr.ReadLine();
        while (line != null)
        {
            ArtifactItems.Add(line);
            line = sr.ReadLine();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

void GeneratorManager(String input, ArrayList poolmutations)
{
    if (input == "p")
    {
        GeneratePool(poolmutations);
    }
    else if (input == "c")
    {
        GenerateCombat(PlayerLevel, difficulty, MonsterCount);
    }
    else if (input == "l")
    {
        GenerateLoot();
    }
    else if (input == "r")
    {
        GenerateRoom();
    }
    else
    {
        Console.WriteLine("Invalid Command");
    }
}

void GenerateRoom()
{
    int prob = 0;
    if (generateStaircase)
    {
        prob = RandomNumberGenerator.GetInt32(126);
    }
    else
    {
        prob = RandomNumberGenerator.GetInt32(101);
    }
  

    if (CurrentRoom == 15)
    {
        Console.WriteLine("Why do i hear Boss Music ? If you dont want to Fight you can bail out for 10 % of your Gold or free if you slay this Boss");
        generateStaircase = true;
        GenerateCombat(PlayerLevel, (Difficulty)((int)difficulty + 1), 1);
        return;
    }
    if (prob < 25)
    {
        Console.WriteLine("A room with a pool of glowing Water appears before you");
        GeneratePool(PoolMutations);
    }
    if (prob >= 25 && prob < 50)
    {
        Console.WriteLine("Enemies Abound role Initiative");
        GenerateCombat(PlayerLevel, difficulty, MonsterCount);
    }
    if (prob >= 50 && prob < 90)
    {
        Console.WriteLine("It seems this room bears a difficult puzzle");
        GeneratePuzzle();
    }
    if (prob >= 90 && prob < 100)
    {
        Console.WriteLine("A room with nothing in it ? Suspicious Roll Perception x,y,z");
        GenerateTrap();
    }
    if (prob >= 100 && CurrentRoom >= 15)
    {
        Console.WriteLine("A Staircase appears before you leading deeper into the Dungeon");
    }
    CurrentRoom++;
}





void GenerateTrap()
{
    var PercSaveDC = generateTrapSave();
    while (true)
    {
        var input = Console.ReadLine();
        Console.WriteLine(PercSaveDC);
        if (!string.IsNullOrEmpty(input))
        {
            var rolls = input.Split(',');
            for (int i = 0; i < rolls.Length; i++)
            {
                var roll = rolls[i];
                if (BigInteger.Parse(roll) < PercSaveDC)
                {
                    var playernumber = i;
                    playernumber++;
                    Console.WriteLine("Player " + playernumber + " found Nothing");
                }
                else
                {
                    var playernumber = i;
                    playernumber++;
                    Console.WriteLine("Player " + playernumber + " found the Trap you have Advantage");
                }
            }
            break;
        }
    }

    GenerateTrapTriggered();

}

int generateTrapSave()
{
    var Trapsave = RandomNumberGenerator.GetInt32(2);
    var TrapDifficulty = new Random();
    if (Trapsave == 0)
    {
        generateTrapDamage(0);
        return TrapDifficulty.Next(10, 11);

    }
    else if (Trapsave == 1)
    {
        generateTrapDamage(1);
        return TrapDifficulty.Next(12, 15);
    }
    else
    {
        generateTrapDamage(2);
        return TrapDifficulty.Next(16, 20);
    }
}

void generateTrapDamage(int i)
{
    if (i == 0)
    {
        if (PlayerLevel.Contains(1) || PlayerLevel.Contains(2) || PlayerLevel.Contains(3) || PlayerLevel.Contains(4))
        {
            TrapDmgValue = "1d10";
        }
        if (PlayerLevel.Contains(5) || PlayerLevel.Contains(6) || PlayerLevel.Contains(7) || PlayerLevel.Contains(8) || PlayerLevel.Contains(9) || PlayerLevel.Contains(10))
        {
            TrapDmgValue = "2d10";
        }

        if (PlayerLevel.Contains(11) || PlayerLevel.Contains(12) || PlayerLevel.Contains(13) || PlayerLevel.Contains(14) || PlayerLevel.Contains(15) || PlayerLevel.Contains(16))
        {
            TrapDmgValue = "4d10";
        }

        if (PlayerLevel.Contains(17) || PlayerLevel.Contains(18) || PlayerLevel.Contains(19) || PlayerLevel.Contains(20))
        {
            TrapDmgValue = "10d10";
        }
    }
    else if (i == 1)
    {
        if (PlayerLevel.Contains(1) || PlayerLevel.Contains(2) || PlayerLevel.Contains(3) || PlayerLevel.Contains(4))
        {
            TrapDmgValue = "2d10";
        }
        if (PlayerLevel.Contains(5) || PlayerLevel.Contains(6) || PlayerLevel.Contains(7) || PlayerLevel.Contains(8) || PlayerLevel.Contains(9) || PlayerLevel.Contains(10))
        {
            TrapDmgValue = "4d10";
        }

        if (PlayerLevel.Contains(11) || PlayerLevel.Contains(12) || PlayerLevel.Contains(13) || PlayerLevel.Contains(14) || PlayerLevel.Contains(15) || PlayerLevel.Contains(16))
        {
            TrapDmgValue = "10d10";
        }

        if (PlayerLevel.Contains(17) || PlayerLevel.Contains(18) || PlayerLevel.Contains(19) || PlayerLevel.Contains(20))
        {
            TrapDmgValue = "18d10";
        }
    }
    else if (i == 0)
    {
        if (PlayerLevel.Contains(1) || PlayerLevel.Contains(2) || PlayerLevel.Contains(3) || PlayerLevel.Contains(4))
        {
            TrapDmgValue = "4d10";
        }
        if (PlayerLevel.Contains(5) || PlayerLevel.Contains(6) || PlayerLevel.Contains(7) || PlayerLevel.Contains(8) || PlayerLevel.Contains(9) || PlayerLevel.Contains(10))
        {
            TrapDmgValue = "10d10";
        }

        if (PlayerLevel.Contains(11) || PlayerLevel.Contains(12) || PlayerLevel.Contains(13) || PlayerLevel.Contains(14) || PlayerLevel.Contains(15) || PlayerLevel.Contains(16))
        {
            TrapDmgValue = "18d10";
        }

        if (PlayerLevel.Contains(17) || PlayerLevel.Contains(18) || PlayerLevel.Contains(19) || PlayerLevel.Contains(20))
        {
            TrapDmgValue = "24d10";
        }
    }
}

void GenerateTrapTriggered()
{
    var trapsave = generateTrapSave();
    ArrayList TrapDmgTypes = new ArrayList() { "Acid", "Bludgeoning", "Cold", "Fire", "Force", "Lightning", "Necrotic", "Piercing", "Poison", "Psychic", "Radiant", "Slashing", "Thunder" };
    Console.WriteLine("The Save DC for this Trap is " + trapsave);
    Console.WriteLine("It deals " + TrapDmgValue + " " + TrapDmgTypes[RandomNumberGenerator.GetInt32(12)] + " Damage to all Players");

}

void GeneratePuzzle()
{
    var SaveDC = generateTrapSave();
    Console.WriteLine(" A Situation is at Hand which needs a Skillcheck to be made, pls make a " + skills[RandomNumberGenerator.GetInt32(17)] + " " + SaveDC + " Saving throw or face the consequences");
    int prob = RandomNumberGenerator.GetInt32(76);
    while (true)
    {
        Console.WriteLine("Did any of you succed? y/n");
        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            if (input == "y")
            {
                Console.WriteLine("Congratulations maybe something nice will happen");

                if (prob < 25)
                {
                    Console.WriteLine("A room with a pool of glowing Water appears before you");
                    GeneratePool(PoolMutations);
                }
                break;
            }
            else
            {
                Console.WriteLine("Oh no face the Consequences or get lucky");
                if (prob >= 25 && prob < 50)
                {
                    Console.WriteLine("Enemies Abound role Initiative");
                    GenerateCombat(PlayerLevel, difficulty, MonsterCount);

                }
                if (prob >= 50 && prob < 75)
                {
                    Console.WriteLine("A room with nothing in it ? Suspicious Roll Perception x,y,z");
                    GenerateTrap();
                }
                break;
            }

        }
    }
}

void GenerateLoot()
{
    var loottable = new LootCalculator();
    int j = 1;

    IEnumerator<KeyValuePair<int[], LootProbability>> iterator = loottable.lootTable.GetEnumerator();

    //7,7,7,9
    for (int i = 0; i < PlayerLevel.Count; i++)
    {
        Console.WriteLine("The " + j + ". Player recieves:");
        j++;
        do
        {
            if (iterator.Current.Key.Contains(PlayerLevel[i]))
            {
                var probability = iterator.Current.Value;

                int prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.Common)
                {
                    Console.WriteLine("A Common Treasure appears before you: " + CommonItems[RandomNumberGenerator.GetInt32(CommonItems.Count)]);
                }

                prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.Uncommon)
                {
                    Console.WriteLine("An Uncommon Treasure appears before you: " + UncommonItems[RandomNumberGenerator.GetInt32(UncommonItems.Count)]);
                }

                prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.Rare)
                {
                    Console.WriteLine("A Rare Treasure appears before you: " + RareItems[RandomNumberGenerator.GetInt32(RareItems.Count)]);
                }

                prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.VeryRare)
                {
                    Console.WriteLine("A Very Rare Treasure appears before you: " + VeryRareItems[RandomNumberGenerator.GetInt32(VeryRareItems.Count)]);
                }

                prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.Legendary)
                {
                    Console.WriteLine("A Legendary Treasure appears before you: " + LegendaryItems[RandomNumberGenerator.GetInt32(LegendaryItems.Count)]);
                }

                prob = RandomNumberGenerator.GetInt32(101);

                if (prob <= probability.Artifact)
                {
                    Console.WriteLine("An Artifact Treasure appears before you: " + ArtifactItems[RandomNumberGenerator.GetInt32(ArtifactItems.Count)]);
                }

            }
        }
        while (iterator.MoveNext());
        iterator.Reset();
        iterator.MoveNext();
    }
}

void GenerateCombat(List<int> PlayerLevel, Difficulty diff, int MonsterCount)
{
    var calculator = new EncounterCalculator();

    int xp = calculator.GetEncounterXP(PlayerLevel, diff);

    int MonsterXP = Challange[RandomNumberGenerator.GetInt32(Challange.Length - 1)];

    while (true)
    {
        if (MonsterXP > xp)
        {
            MonsterXP = Challange[RandomNumberGenerator.GetInt32(Challange.Length - 1)];
        }
        if (MonsterXP * MonsterCount > xp)
        {
            MonsterXP = Challange[RandomNumberGenerator.GetInt32(Challange.Length - 1)];
        }
        else
        {
            break;
        }
    }

    int MonsterNumbers = xp / MonsterXP;

    if(MonsterNumbers > MonsterCount)
    {
        MonsterNumbers = MonsterCount;
    }

    List<string> MonsterList = MonsterManual[MonsterXP];

    string MonsterToCombat = MonsterList[RandomNumberGenerator.GetInt32(MonsterList.Count)];

    Console.WriteLine("Player XP Threshold:" + xp + " MonsterXp Values:" + MonsterXP * MonsterNumbers);

    Console.WriteLine(MonsterNumbers + " " + MonsterToCombat + "(s) appear ready to fight you !");

}

void GeneratePool(ArrayList poolmutations)
{
    Console.WriteLine(poolmutations[RandomNumberGenerator.GetInt32(poolmutations.Count)]);
}
