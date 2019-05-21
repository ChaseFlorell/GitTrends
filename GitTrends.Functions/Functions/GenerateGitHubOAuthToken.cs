﻿using System;
using System.Threading.Tasks;
using GitTrends.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace GitTrends.Functions
{
    public static class GenerateGitHubOAuthToken
    {
        readonly static string _clientSecret = Environment.GetEnvironmentVariable("GitTrendsClientSecret");
        readonly static string _clientId = Environment.GetEnvironmentVariable("GitTrendsClientId");

        [FunctionName(nameof(GenerateGitHubOAuthToken))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] [FromBody] GenerateTokenDTO generateTokenDTO, ILogger log)
        {
            log.LogInformation("Received request for OAuth Token");

            var token = await GitHubApiV3Service.GetGitHubToken(_clientId, _clientSecret, generateTokenDTO.LoginCode, generateTokenDTO.State).ConfigureAwait(false);

            log.LogInformation("Token Retrived");

            return new OkObjectResult(token);
        }
    }
}