namespace OrderTester.Utility
{
    public static class ProductUtility
    {
        public static string GetRandomProduct()
        {
            int adjectiveIndex = RandomNumberGenerator.Next(0, Adjectives.Length);
            int productIndex = RandomNumberGenerator.Next(0, Products.Length);
            return $"{Adjectives[adjectiveIndex]} {Products[productIndex]}";
        }

        public static int GetRandomPrice()
        {
            return RandomNumberGenerator.Next(1, 1000);
        }
        private static readonly Random RandomNumberGenerator = new Random(DateTime.Now.Second);

        private static string[] Adjectives = new string[]
        {
            "Small",
            "Medium",
            "Large",
            "Extra-large",
            "Light",
            "Heavy",
            "Strong",
            "Shiny",
            "Rusty",
            "New",
            "Used",
            "Broken"
        };

        private static string[] Products = new string[]
        {
            "Sword",
            "Shield",
            "Mace",
            "Axe",
            "Lance",
            "Bow",
            "Knife",
            "Club",
            "Armor",
            "Boots",
            "Gloves",
            "Helmet",
            "Gauntlets",
            "Crossbow"
        };
    }
}