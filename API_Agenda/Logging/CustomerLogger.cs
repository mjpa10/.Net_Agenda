using System.Xml.Linq;

namespace API_Agenda.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }    

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        EscreverTextoNoArquivo(mensagem);
    }

    public void EscreverTextoNoArquivo(string mensagem)
    {
        // Defina o caminho completo para o arquivo de log no diretório atual
        string caminhoLog = "log/Log.txt";

        // Escreve a mensagem no arquivo de log
        try
        {
            using (StreamWriter writer = new StreamWriter(caminhoLog, true))
            {
                writer.WriteLine(mensagem);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao escrever no arquivo: {ex.Message}");
        }
    }
    }

