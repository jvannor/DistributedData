namespace OrderTester.Utility
{
    public static class CustomerUtility
    {
        public static string GetRandomCustomer()
        {
            string firstName = GetRandomFirstName();
            string lastName = GetRandomLastName();

            return $"{firstName} {lastName}";
        }

        public static string GetRandomFirstName()
        {
            int i = RandomNumberGenerator.Next(0, FirstNames.Length);
            return FirstNames[i];
        }

        public static string GetRandomLastName()
        {
            int i = RandomNumberGenerator.Next(0, LastNames.Length);
            return LastNames[i];
        }

        private static readonly Random RandomNumberGenerator = new Random(DateTime.Now.Second);

        private static readonly string[] FirstNames = new string[] 
        {
            "Eddard",
            "Ned",
            "Robert",
            "Jaime",
            "Catelyn",
            "Cersei",
            "Daenerys",
            "Jorah",
            "Viserys",
            "Jon",
            "Robb",
            "Sansa",
            "Arya",
            "Theon",
            "Bran",
            "Joffrey",
            "Sandor",
            "Tyrion",
            "Khal"
        };

        private static readonly string[] LastNames = new string[]
        {
            "Stark",
            "Baratheon",
            "Lannister",
            "Targaryen",
            "Mormont",
            "Snow",
            "Greyjoy",
            "Baratheon",
            "Clegane",
            "Drogo"
        };
    }
}