using System;
using Hangfire;
using CGHangfireApp.Helper;
using Microsoft.Owin.Hosting;
using System.Data.SqlClient;
using System.Linq;

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
                string sqlServerConnectionString = Settings.Params.SQLConnectionString;

                // włączamy logowanie do konsoli
                GlobalConfiguration.Configuration.UseColouredConsoleLogProvider(Settings.Params.Hangfire.LogLevel);

                if (string.IsNullOrEmpty(sqlServerConnectionString))
                {
                    throw new Exception("Nie określono ciągu połączenia z bazą danych. Sprawdź plik Settings.json.");
                }

                // próba połączenia pozwoli nam zweryfikować działanie
                // serwera i ewentualne błędy konfiguracji
                using (var connection = new SqlConnection(sqlServerConnectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"Wystapił błąd podczas próby połączenia z bazą danych. \n" +
                            $"*** Czy Twój serwer działa? \n" +
                            $"*** Czy użytkownik posiada właściwy zestaw uprawnień? \n" +
                            $"*** Czy baza danych istnieje? \n" +
                            $"*** Szczegóły błędu: {ex.Message}");
                    }
                }
                
                // 
                GlobalConfiguration.Configuration.UseSqlServerStorage(sqlServerConnectionString);
                
                // uruchamiamy aplikację
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

                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                DisplayError($"{e.Message} {e.InnerException?.Message}");
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
