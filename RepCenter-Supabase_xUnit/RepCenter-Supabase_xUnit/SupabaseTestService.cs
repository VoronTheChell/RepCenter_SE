using System.Reflection;

public class SupabaseTestService
{
    public Supabase.Client Client { get; private set; }

    public async Task InitializeAsync()
    {
        EmbeddedEnvLoader.LoadEmbeddedEnv();

        string url = Environment.GetEnvironmentVariable("SUPABASE_URL");
        string key = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_KEY");

        if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException("SUPABASE_URL или SUPABASE_SERVICE_KEY", "Проверь переменные окружения или .env");

        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = false
        };

        Client = new Supabase.Client(url, key, options);
        await Client.InitializeAsync();
    }

    public static class EmbeddedEnvLoader
    {
        public static void LoadEmbeddedEnv()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(".env"));

            if (resourceName == null)
            {
                throw new FileNotFoundException(".env-файл не найден среди Embedded ресурсов.");
            }

            using var stream = assembly.GetManifestResourceStream(resourceName)
                             ?? throw new Exception("Ошибка при загрузке ресурса .env");
            using var reader = new StreamReader(stream);

            Console.WriteLine("📦 Загрузка .env переменных:");

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    Environment.SetEnvironmentVariable(key, value);

                    Console.WriteLine($"  ✅ {key} загружен");
                }
                else
                {
                    Console.WriteLine($"  ⚠️  Некорректная строка в .env: {line}");
                }
            }

            Console.WriteLine("✅ Загрузка .env завершена.\n");
        }
    }
}
