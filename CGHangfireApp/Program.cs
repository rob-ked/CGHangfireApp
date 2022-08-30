using System;
using Hangfire;
using CGHangfireApp.Helper;
using Microsoft.Owin.Hosting;
using System.Data.SqlClient;
using System.Linq;
using CGHangfireApp.Job;
using System.Collections.Generic;
using CGHangfireAppJob = CGHangfireApp.Model.Settings.Job;

namespace CGHangfireApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitializeHangfire();
        }

        /// <summary>
        /// Uruchamia Hangfire, stosując wcześniej konfigurację,
        /// inicjujac bazę danych jeśli trzeba oraz tworząc witrynę pulpitu aplikacji
        /// </summary>
        private static void InitializeHangfire()
        {
            try
            {
                // lista skonfigurowanych zadań
                List<CGHangfireAppJob> jobs = Settings.Params.Jobs;

                if (jobs.Count == 0 || jobs.Any(j => j.IsActive == true) == false)
                {
                    throw new Exception("Nie określono listy zadań lub wszystkie zadania są wyłączone");
                }

                // ciąg połączenia z bazą danych hangfire
                string hangfireSQLServerConnectionString = Settings.Params.Hangfire.SQLConnectionString;
                VerifyDBConfiguration(hangfireSQLServerConnectionString);

                // ciąg połaczenia z bazą danych aplikacji
                string appSQLServerConnectionString = Settings.Params.App.SQLConnectionString;
                VerifyDBConfiguration(appSQLServerConnectionString);

                // włączamy logowanie do konsoli
                GlobalConfiguration.Configuration.UseColouredConsoleLogProvider(Settings.Params.Hangfire.LogLevel);
                                
                // 
                GlobalConfiguration.Configuration.UseSqlServerStorage(hangfireSQLServerConnectionString);
                
                // uruchamiamy pulpit
                StartOptions hangfireStartupOptions = new StartOptions();
                hangfireStartupOptions.Urls.Add(Settings.Params.Hangfire.HangfireURL);

                using (WebApp.Start<HangfireStartup>(hangfireStartupOptions))
                {
                    Console.WriteLine
                    (
                        $"*************************************\n\n" +
                        $"Hangfire został uruchomiony. \n\n" +
                        $"Otwórz {Settings.Params.Hangfire.HangfireURL}/dashboard " +
                        $"w swojej ulubionej przeglądarce, aby pozyskać więcej danych.\n\n" +
                        $"*************************************\n\n"
                    );

                    // Przetwarzamy zadania do wykonania
                    ProcessJobList(jobs);                 

                    //
                    Console.ReadKey();
                }
                
            }
            catch (Exception e)
            {
                DisplayError($"{e.Message} {e.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Sprawdza połączenie z bazą danych
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="Exception"></exception>
        private static void VerifyDBConfiguration(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Nie określono ciągu połączenia z bazą danych. Sprawdź plik Settings.json.");
            }

            // próba połączenia pozwoli nam zweryfikować działanie
            // serwera i ewentualne błędy konfiguracji
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        $"Wystapił błąd podczas próby połączenia z bazą danych {connection.Database}. \n" +
                        $"*** Czy Twój serwer działa? \n" +
                        $"*** Czy użytkownik posiada właściwy zestaw uprawnień? \n" +
                        $"*** Czy baza danych istnieje? \n" +
                        $"*** Szczegóły błędu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Przetwarza listę zadań
        /// </summary>
        /// <param name="jobs"></param>
        /// <exception cref="Exception"></exception>
        private static void ProcessJobList(List<CGHangfireAppJob> jobs)
        {
            var jobClasses = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IJob).IsAssignableFrom(p) && p.Name.Equals("IJob") == false);

            var configuredJobCounter = 0;

            foreach (CGHangfireAppJob job in jobs)
            {
                var jobClass = jobClasses.FirstOrDefault(j => j.Name == $"Job{job.Name}");
                if (jobClass != null)
                {
                    var jobInstance = (IJob)Activator.CreateInstance(jobClass);
                    jobInstance.SetSchedule(job.Schedule);

                    if (job.IsActive)
                    {
                        configuredJobCounter++;
                        ConfigureJob(jobInstance);
                    }
                    else
                    {
                        RemoveJob(jobInstance);
                    }
                }
            }

            if (configuredJobCounter == 0)
            {                
                throw new Exception("Nie skonfigurowano żadnych zadań do wykonania. " +
                    "Sprawdź swoją konfigurację. Dostępne zadania to: \n" +
                    String.Join(",\n", jobClasses.Select(j => j.Name.Remove(0, 3)).ToArray())
                );
            }
        }

        /// <summary>
        /// Konfiguruje zadanie
        /// </summary>
        /// <param name="job"></param>
        private static void ConfigureJob(IJob job)
        {
            if (job.GetSchedule() != null)
            {
                Console.WriteLine($"Konfiguruje zadanie rekurencyjne {job.GetName()} - {job.GetDescription()}");                
                RecurringJob.AddOrUpdate
                (
                    job.GetName(),
                    () => job.Run(),
                    job.GetSchedule(),
                    TimeZoneInfo.Local
                );
            }
            else
            {
                Console.WriteLine($"Konfiguruje zadanie jednorazowe {job.GetName()} - {job.GetDescription()}");
                BackgroundJob.Enqueue
                (
                    () => job.Run()
                );
            }
        }

        /// <summary>
        /// Usuwa zadanie z harmonogramu
        /// </summary>
        /// <param name="job"></param>
        private static void RemoveJob(IJob job)
        {
            if (job.GetSchedule() != null)
            {
                Console.WriteLine($"Usuwam zadanie rekurencyjne z harmonogramu {job.GetName()} - {job.GetDescription()}");
                RecurringJob.RemoveIfExists
                (
                    job.GetName()
                );
            }
        }

        /// <summary>
        /// Wyświetla błąd w konsoli aplikacji
        /// </summary>
        /// <param name="message"></param>
        private static void DisplayError(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Naciśnij dowolny przycisk, aby zamknąć aplikację.");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
