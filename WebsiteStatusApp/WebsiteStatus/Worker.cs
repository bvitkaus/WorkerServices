using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;


namespace WebsiteStatus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private System.Net.Http.HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

// initialise the client:
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }

//here to call the client
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var result = await client.GetAsync("https://www.google.com");
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // add an if statement 

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status code {StatusCode}", result.StatusCode);

                }

                else
                {
                    _logger.LogError("the website is down, Status code {StatusCode}", result.StatusCode);
                }
                
                

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

// worker service when executed will allow us to know if the website is up every 5 seconds
// otherwise will write an error if website is down. 