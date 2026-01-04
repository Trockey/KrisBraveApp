namespace DeveloperGoals.Exceptions;

/// <summary>
/// Wyjątek rzucany gdy profil użytkownika jest niekompletny
/// </summary>
public class ProfileIncompleteException : Exception
{
    public ProfileIncompleteException(string message) : base(message) { }
}

/// <summary>
/// Wyjątek rzucany gdy technologia nie należy do użytkownika
/// </summary>
public class TechnologyNotOwnedException : Exception
{
    public TechnologyNotOwnedException(string message) : base(message) { }
}

/// <summary>
/// Wyjątek rzucany gdy technologia nie została znaleziona
/// </summary>
public class TechnologyNotFoundException : Exception
{
    public TechnologyNotFoundException(string message) : base(message) { }
}

/// <summary>
/// Wyjątek rzucany gdy status technologii jest nieprawidłowy
/// </summary>
public class InvalidTechnologyStatusException : Exception
{
    public InvalidTechnologyStatusException(string message) : base(message) { }
}

/// <summary>
/// Wyjątek rzucany gdy AI service przekroczył timeout
/// </summary>
public class AIServiceTimeoutException : Exception
{
    public AIServiceTimeoutException(string message) : base(message) { }
}

/// <summary>
/// Wyjątek rzucany gdy OpenRouter API zwrócił błąd
/// </summary>
public class OpenRouterApiException : Exception
{
    public string Details { get; }

    public OpenRouterApiException(string message, string details) : base(message)
    {
        Details = details;
    }
}

/// <summary>
/// Wyjątek rzucany gdy nie udało się sparsować odpowiedzi AI
/// </summary>
public class AIResponseParsingException : Exception
{
    public AIResponseParsingException(string message, Exception? innerException = null) 
        : base(message, innerException) { }
}

/// <summary>
/// Wyjątek rzucany gdy format odpowiedzi AI jest nieprawidłowy
/// </summary>
public class InvalidAIResponseFormatException : Exception
{
    public InvalidAIResponseFormatException(string message) : base(message) { }
}
