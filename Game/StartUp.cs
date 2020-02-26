namespace Game
{
    using System;
    using System.IO;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new GameContext();

            Mapper.Initialize(config => config.AddProfile<GameProfile>());

            ResetDatabase(context, shouldDropDatabase: true);

            var projectDir = GetProjectDirectory();

           ImportEntities(context, projectDir + @"Datasets/", projectDir + @"ImportResults/");

            ExportEntities(context, projectDir + @"ExportResults/");

            using (var transaction = context.Database.BeginTransaction())
            {
                transaction.Rollback();
            }
        }

        private static void ImportEntities(GameContext context, string baseDir, string exportDir)
        {
            var heroes =
                DataProcessor.Deserializer.ImportMovies(context,
                    File.ReadAllText(baseDir + "heroes.json"));
            PrintAndExportEntityToFile(heroes, exportDir + "Actual Result - ImportHeroes.txt");

            var enemies =
                DataProcessor.Deserializer.ImportHallSeats(context,
                    File.ReadAllText(baseDir + "enemies.json"));
            PrintAndExportEntityToFile(enemies, exportDir + "Actual Result - ImportEnemies.txt");

            var good = DataProcessor.Deserializer.ImportProjections(context,
                File.ReadAllText(baseDir + "good.xml"));
            PrintAndExportEntityToFile(good, exportDir + "Actual Result - ImportGood.txt");

            var items =
                DataProcessor.Deserializer.ImportCustomerTickets(context,
                    File.ReadAllText(baseDir + "customers-items.xml"));
            PrintAndExportEntityToFile(items, exportDir + "Actual Result - ImportItems.txt");
        }

        private static void ExportEntities(GameContext context, string exportDir)
        {
            var exportTopHeroes = DataProcessor.Serializer.ExportTopMovies(context, 5);
            Console.WriteLine(exportTopHeroes);
            File.WriteAllText(exportDir + "Actual Result - ExportTopMovies.json", exportTopHeroes);

            var exportTopEnemies = DataProcessor.Serializer.ExportTopCustomers(context, 14);
            Console.WriteLine(exportTopEnemies);
            File.WriteAllText(exportDir + "Actual Result - ExportTopCustomers.xml", exportTopEnemies);
        }

        private static void ResetDatabase(GameContext context, bool shouldDropDatabase = false)
        {
            if (shouldDropDatabase)
            {
                context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            {
                return;
            }

            var disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            var deleteRowsQuery = "EXEC sp_MSforeachtable @command1='SET QUOTED_IDENTIFIER ON;DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            var enableIntegrityChecksQuery =
                "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            var reseedQuery =
                "EXEC sp_MSforeachtable @command1='IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS) DBCC CHECKIDENT(''?'', RESEED, 0)'";
            context.Database.ExecuteSqlCommand(reseedQuery);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static string GetProjectDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("netcoreapp") ? @"../../../" : string.Empty;

            return relativePath;
        }
    }
}