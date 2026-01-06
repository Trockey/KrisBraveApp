using System.Net.Http.Json;
using System.Text.Json;
using DeveloperGoals.Configuration;
using DeveloperGoals.DTOs;
using DeveloperGoals.Exceptions;
using DeveloperGoals.Models;
using Microsoft.Extensions.Options;

namespace DeveloperGoals.Services;

/// <summary>
/// Implementacja serwisu komunikacji z OpenRouter AI API
/// </summary>
public class OpenRouterService : IOpenRouterService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterOptions _options;
    private readonly ILogger<OpenRouterService> _logger;

    public OpenRouterService(
        HttpClient httpClient,
        IOptions<OpenRouterOptions> options,
        ILogger<OpenRouterService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<AIRecommendationResult>> GetRecommendationsAsync(
        UserProfile profile,
        TechnologyDto sourceTechnology,
        List<TechnologyDto> contextTechnologies,
        HashSet<int> existingTechDefinitionIds,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = BuildPrompt(profile, sourceTechnology, contextTechnologies);
            var requestBody = new
            {
                model = _options.Model,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = GetSystemPrompt()
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                temperature = _options.Temperature,
                max_tokens = _options.MaxTokens
            };

            _logger.LogInformation(
                "Sending request to OpenRouter API for user profile with role {Role}",
                profile.Role);

            var response = await _httpClient.PostAsJsonAsync(
                "/chat/completions",
                requestBody,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(
                    "OpenRouter API returned error {StatusCode}: {Error}",
                    response.StatusCode,
                    errorContent);

                throw new OpenRouterApiException(
                    "Failed to generate recommendations. Please try again later.",
                    $"OpenRouter API returned error {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var recommendations = ParseAIResponse(responseContent);

            _logger.LogInformation(
                "Successfully received {Count} recommendations from OpenRouter API",
                recommendations.Count);

            return recommendations;
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
        {
            throw;
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("OpenRouter API request timed out after {Timeout}s", _options.Timeout);
            throw new AIServiceTimeoutException(
                "AI service did not respond within 20 seconds. Please try again.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while calling OpenRouter API");
            throw new OpenRouterApiException(
                "Failed to connect to AI service. Please try again later.",
                ex.Message);
        }
        catch (OpenRouterApiException)
        {
            throw;
        }
        catch (AIServiceTimeoutException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while calling OpenRouter API");
            throw new OpenRouterApiException(
                "An unexpected error occurred while generating recommendations.",
                ex.Message);
        }
    }

    private string GetSystemPrompt()
    {
        return @"You are a technology learning advisor for software developers. 
Your task is to recommend the next technologies to learn based on the user's profile and current technology.

IMPORTANT RULES:
1. Return EXACTLY 10 recommendations
2. Recommendations should be logical next steps in the learning path
3. Consider the user's role, development area, and main technologies
4. Each recommendation must include: name, prefix, tag, systemDescription, and aiReasoning
5. Return ONLY valid JSON array, no additional text or markdown
6. Use these exact tag values: Technologia, Framework, BazaDanych, Metodologia, Narzedzie
7. Name format: ""Prefix - Technology Name"" (e.g., ""DotNet - LINQ Advanced"")
8. Prefix should be a category like: DotNet, JavaScript, Python, Database, DevOps, Cloud, etc.";
    }

    private string BuildPrompt(
        UserProfile profile,
        TechnologyDto sourceTechnology,
        List<TechnologyDto> contextTechnologies)
    {
        var mainTechList = profile.MainTechnologies;
        var mainTechString = string.Join(", ", mainTechList);

        var contextTechString = contextTechnologies.Any()
            ? string.Join("\n", contextTechnologies.Select(t =>
                $"- {t.Name} ({t.Prefix}): {t.SystemDescription}"))
            : "None";

        var prompt1 = $@"Generate 10 technology recommendations for the following context:

USER PROFILE:
- Role: {profile.Role}
- Development Area: {profile.DevelopmentArea}
- Main Technologies: {mainTechString}

";

        var prompt2 = $@"CURRENT TECHNOLOGY (learning from):
Name: {sourceTechnology.Name}
Prefix: {sourceTechnology.Prefix}
Description: {sourceTechnology.SystemDescription}

";


        var prompt3 = $@"CONTEXT TECHNOLOGIES (already in user's graph):
{contextTechString}

";

        var prompt4 = $@"Return a JSON array with exactly 10 recommendations in this format:
[
  {{
    ""name"": ""Prefix - Technology Name"",
    ""prefix"": ""Category"",
    ""tag"": ""Technologia|Framework|BazaDanych|Metodologia|Narzedzie"",
    ""systemDescription"": ""Brief description of the technology"",
    ""aiReasoning"": ""Why this is a good next step for the user""
  }}
]

IMPORTANT: Return ONLY the JSON array, no markdown formatting or additional text.";


        string prompt = string.Empty;
        if(sourceTechnology.Name == "Start")
            prompt = prompt1 + prompt4;
        else
            prompt = prompt1 + prompt2 + prompt3 + prompt4;

        return prompt;
    }

    private List<AIRecommendationResult> ParseAIResponse(string responseContent)
    {
        try
        {
            using var document = JsonDocument.Parse(responseContent);
            var root = document.RootElement;

            if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                throw new InvalidAIResponseFormatException(
                    "AI service returned invalid response format: missing choices");
            }

            var firstChoice = choices[0];
            if (!firstChoice.TryGetProperty("message", out var message) ||
                !message.TryGetProperty("content", out var content))
            {
                throw new InvalidAIResponseFormatException(
                    "AI service returned invalid response format: missing message content");
            }

            var aiContent = content.GetString() ?? string.Empty;

            // Usuń markdown code blocks jeśli są
            aiContent = aiContent.Trim();
            if (aiContent.StartsWith("```json"))
            {
                aiContent = aiContent[7..]; // Usuń ```json
            }
            if (aiContent.StartsWith("```"))
            {
                aiContent = aiContent[3..]; // Usuń ```
            }
            if (aiContent.EndsWith("```"))
            {
                aiContent = aiContent[..^3]; // Usuń końcowe ```
            }
            aiContent = aiContent.Trim();

            var recommendations = JsonSerializer.Deserialize<List<AIRecommendationResult>>(
                aiContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (recommendations == null || recommendations.Count == 0)
            {
                throw new InvalidAIResponseFormatException(
                    "AI service returned empty recommendations list");
            }

            // Walidacja każdej rekomendacji
            foreach (var rec in recommendations)
            {
                if (string.IsNullOrWhiteSpace(rec.Name) ||
                    string.IsNullOrWhiteSpace(rec.Prefix) ||
                    string.IsNullOrWhiteSpace(rec.Tag) ||
                    string.IsNullOrWhiteSpace(rec.SystemDescription) ||
                    string.IsNullOrWhiteSpace(rec.AiReasoning))
                {
                    throw new InvalidAIResponseFormatException(
                        "AI service returned recommendation with missing required fields");
                }
            }

            return recommendations;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response as JSON");
            throw new AIResponseParsingException(
                "Failed to parse AI response",
                ex);
        }
    }
}
