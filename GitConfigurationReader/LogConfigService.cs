using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class LogConfigService
{
    private readonly ILogger<LogConfigService> logger;
    private readonly IConfiguration configuration;

    public LogConfigService(ILogger<LogConfigService> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        logger.LogInformation("user name: {0}", configuration["user:name"]);
        logger.LogInformation("user email: {0}", configuration["user:email"]);
        logger.LogInformation("remote origin url: {0}", configuration.GetValue<string>("remote:origin:url"));
        logger.LogInformation("remote origin fetch: {0}", configuration.GetValue<string>("remote:origin:fetch"));
        logger.LogInformation("branch main remote: {0}", configuration.GetValue<string>("branch:main:remote"));
        logger.LogInformation("branch main merge: {0}", configuration.GetValue<string>("branch:main:merge"));

        foreach (var alias in configuration.GetSection("alias")?.GetChildren())
        {
            logger.LogInformation("alias: git {0} `{1}`", alias.Key, alias.Value);
        }
    }
}
