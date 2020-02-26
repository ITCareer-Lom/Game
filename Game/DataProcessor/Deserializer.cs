namespace Game.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportHero
            = "Successfully imported hero!";
        private const string SuccessfulImportEnemy
            = "Successfully imported enemy!";
        private const string SuccessfulImportGood 
            = "Successfully imported good!";
        private const string SuccessfulImportItem
            = "Successfully imported item!";

        public static string ImportHeroes(GameContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportEnemies(GameContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportGood(GameContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportItems(GameContext context, string xmlString)
        {
            throw new NotImplementedException();
        }
    }
}