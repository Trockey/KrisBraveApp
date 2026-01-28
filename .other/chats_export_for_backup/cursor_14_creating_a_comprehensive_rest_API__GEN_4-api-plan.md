# Creating a comprehensive REST API plan
_Exported on 12/22/2025 at 01:19:51 GMT+1 from Cursor (2.2.43)_

---

**User**

<db-plan>
@.ai/db-plan.md 
</db-plan>

<prd>
@.ai/prd.md 
</prd>

<tech-stack>
@.ai/tech-stack.md
</tech-stack>

Jesteś doświadczonym architektem API, którego zadaniem jest stworzenie kompleksowego planu API REST. Twój plan będzie oparty na podanym schemacie bazy danych, dokumencie wymagań produktu (PRD) i stacku technologicznym podanym powyżej. Uważnie przejrzyj dane wejściowe i wykonaj następujące kroki:

1. Przeanalizuj schemat bazy danych:
   - Zidentyfikuj główne encje (tabele)
   - Zanotuj relacje między jednostkami
   - Rozważ wszelkie indeksy, które mogą mieć wpływ na projekt API
   - Zwróć uwagę na warunki walidacji określone w schemacie.

2. Przeanalizuj PRD:
   - Zidentyfikuj kluczowe cechy i funkcjonalności
   - Zwróć uwagę na konkretne wymagania dotyczące operacji na danych (pobieranie, tworzenie, aktualizacja, usuwanie)
   - Zidentyfikuj wymagania logiki biznesowej, które wykraczają poza operacje CRUD

3. Rozważ stack technologiczny:
   - Upewnij się, że plan API jest zgodny z określonymi technologiami.
   - Rozważ, w jaki sposób te technologie mogą wpłynąć na projekt API

4. Tworzenie kompleksowego planu interfejsu API REST:
   - Zdefiniowanie głównych zasobów w oparciu o encje bazy danych i wymagania PRD
   - Zaprojektowanie punktów końcowych CRUD dla każdego zasobu
   - Zaprojektuj punkty końcowe dla logiki biznesowej opisanej w PRD
   - Uwzględnienie paginacji, filtrowania i sortowania dla punktów końcowych listy.
   - Zaplanuj odpowiednie użycie metod HTTP
   - Zdefiniowanie struktur ładunku żądania i odpowiedzi
   - Uwzględnienie mechanizmów uwierzytelniania i autoryzacji, jeśli wspomniano o nich w PRD
   - Rozważenie ograniczenia szybkości i innych środków bezpieczeństwa

Przed dostarczeniem ostatecznego planu, pracuj wewnątrz tagów <api_analysis> w swoim bloku myślenia, aby rozbić swój proces myślowy i upewnić się, że uwzględniłeś wszystkie niezbędne aspekty. W tej sekcji:

1. Wymień główne encje ze schematu bazy danych. Ponumeruj każdą encję i zacytuj odpowiednią część schematu.
2. Wymień kluczowe funkcje logiki biznesowej z PRD. Ponumeruj każdą funkcję i zacytuj odpowiednią część PRD.
3. Zmapuj funkcje z PRD do potencjalnych punktów końcowych API. Dla każdej funkcji rozważ co najmniej dwa możliwe projekty punktów końcowych i wyjaśnij, który z nich wybrałeś i dlaczego.
4. Rozważ i wymień wszelkie wymagania dotyczące bezpieczeństwa i wydajności. Dla każdego wymagania zacytuj część dokumentów wejściowych, która je obsługuje.
5. Wyraźnie mapuj logikę biznesową z PRD na punkty końcowe API.
6. Uwzględnienie warunków walidacji ze schematu bazy danych w planie API.

Ta sekcja może być dość długa.

Ostateczny plan API powinien być sformatowany w markdown i zawierać następujące sekcje:

```markdown
# REST API Plan

## 1. Zasoby
- Wymień każdy główny zasób i odpowiadającą mu tabelę bazy danych

## 2. Punkty końcowe
Dla każdego zasobu podaj:
- Metoda HTTP
- Ścieżka URL
- Krótki opis
- Parametry zapytania (jeśli dotyczy)
- Struktura ładunku żądania JSON (jeśli dotyczy)
- Struktura ładunku odpowiedzi JSON
- Kody i komunikaty powodzenia
- Kody i komunikaty błędów

## 3. Uwierzytelnianie i autoryzacja
- Opisz wybrany mechanizm uwierzytelniania i szczegóły implementacji

## 4. Walidacja i logika biznesowa
- Lista warunków walidacji dla każdego zasobu
- Opisz, w jaki sposób logika biznesowa jest zaimplementowana w API
```

Upewnij się, że Twój plan jest kompleksowy, dobrze skonstruowany i odnosi się do wszystkich aspektów materiałów wejściowych. Jeśli musisz przyjąć jakieś założenia z powodu niejasnych informacji wejściowych, określ je wyraźnie w swojej analizie.

Końcowy wynik powinien składać się wyłącznie z planu API w formacie markdown w języku angielskim, który zapiszesz w .ai/api-plan.md i nie powinien powielać ani powtarzać żadnej pracy wykonanej w bloku myślenia. @4-api-plan.md (16) 

---

**Cursor**

Przeanalizuję dostarczone dokumenty i stworzę kompleksowy plan API REST dla aplikacji DeveloperGoals.

```plaintext
- Id: int (PK)
- GoogleId: varchar(255) UNIQUE, NOT NULL, indexed
- Email: varchar(255), NOT NULL, indexed
- Name: varchar(255), NOT NULL (DisplayName z Google)
- CreatedAt: timestamp, NOT NULL
- LastLoginAt: timestamp, NULL
```

```plaintext
- Id: int (PK)
- UserId: int (FK → Users, UNIQUE, CASCADE)
- MainTechnologies: varchar(1000), NOT NULL (CSV lub lista oddzielona przecinkami)
- Role: int (enum → Programista, Tester, Analityk, DataScienceSpecialist)
- DevelopmentArea: int (enum → UserInterface, Backend, FullStack, Testing, DataScience, DevOps)
- CreatedAt: timestamp, NOT NULL
- UpdatedAt: timestamp, NULL
```

```plaintext
- Id: int (PK)
- Name: varchar(255), NOT NULL
- Prefix: varchar(100), NOT NULL
- Tag: int (enum → Technologia, Framework, BazaDanych, Metodologia, Narzedzie)
- SystemDescription: varchar(1000), NOT NULL
- CreatedAt: timestamp, NOT NULL
- UNIQUE na (Name, Prefix, Tag)
- Specjalny rekord: Id=1, Name="Start" dla węzła startowego
```

Przeanalizuję dokumenty i stworzę kompleksowy plan API REST dla aplikacji DeveloperGoals.

```markdown
# REST API Plan for DeveloperGoals

Version: 1.0  
Date: December 1, 2025  
Status: DRAFT - MVP  
Technology Stack: ASP.NET Core Blazor Server (.NET 8), PostgreSQL, Entity Framework Core

---

## 1. Resources

The API is structured around the following core resources, mapped to database entities:

| Resource | Database Table | Description |
|----------|----------------|-------------|
| **Auth** | Users | Google OAuth authentication and session management |
| **Profile** | UserProfiles | User professional profile (technologies, role, area) |
| **Technologies** | UserTechnologies, TechnologyDefinitions | User's technology graph nodes |
| **Dependencies** | TechnologyDependencies | Graph edges between technologies |
| **AI Recommendations** | In-memory cache | AI-generated technology suggestions |
| **Ignored Technologies** | IgnoredTechnologies | Technologies user has explicitly ignored |
| **Admin** | Multiple tables | Administrative metrics and statistics |

---

## 2. Endpoints

### 2.1 Authentication

#### POST /api/auth/login
**Description:** Initiates Google OAuth login flow  
**Authentication:** None (public)

**Response 200:**
```json
{
  "redirectUrl": "https://accounts.google.com/o/oauth2/v2/auth?client_id=..."
}
```

**Response 500:**
```json
{
  "error": "InternalServerError",
  "message": "Failed to initiate OAuth flow"
}
```

---

#### GET /api/auth/callback
**Description:** Handles Google OAuth callback, creates or retrieves user  
**Authentication:** None (public)  
**Query Parameters:**
- `code` (required): OAuth authorization code
- `state` (optional): CSRF protection state

**Response 200:**
```json
{
  "user": {
    "id": 123,
    "email": "user@example.com",
    "name": "John Doe",
    "hasProfile": true
  },
  "redirectTo": "/dashboard" // or "/profile" if hasProfile=false
}
```

**Response 401:**
```json
{
  "error": "Unauthorized",
  "message": "Invalid OAuth code"
}
```

---

#### POST /api/auth/logout
**Description:** Logs out current user and terminates session  
**Authentication:** Required

**Response 200:**
```json
{
  "message": "Successfully logged out"
}
```

---

#### GET /api/auth/me
**Description:** Returns current authenticated user information  
**Authentication:** Required

**Response 200:**
```json
{
  "id": 123,
  "email": "user@example.com",
  "name": "John Doe",
  "googleId": "1234567890",
  "createdAt": "2025-11-15T10:30:00Z",
  "lastLoginAt": "2025-12-01T08:15:00Z",
  "hasProfile": true
}
```

**Response 401:**
```json
{
  "error": "Unauthorized",
  "message": "User not authenticated"
}
```

---

### 2.2 User Profile

#### GET /api/profile
**Description:** Retrieves current user's profile  
**Authentication:** Required

**Response 200:**
```json
{
  "id": 456,
  "userId": 123,
  "mainTechnologies": [
    "C#",
    ".NET Core",
    "PostgreSQL",
    "JavaScript",
    "Docker"
  ],
  "role": "Programista",
  "developmentArea": "Backend",
  "createdAt": "2025-11-15T10:35:00Z",
  "updatedAt": "2025-11-20T14:22:00Z"
}
```

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Profile not found. Please complete onboarding."
}
```

---

#### POST /api/profile
**Description:** Creates user profile (onboarding). Automatically creates "Start" node in user's technology graph.  
**Authentication:** Required

**Request Body:**
```json
{
  "mainTechnologies": [
    "C#",
    ".NET Core",
    "PostgreSQL",
    "JavaScript",
    "Docker"
  ],
  "role": "Programista",
  "developmentArea": "Backend"
}
```

**Validation Rules:**
- `mainTechnologies`: Array of strings, minimum 5 items, maximum 50 items
- `role`: Enum - "Programista" | "Tester" | "Analityk" | "DataScienceSpecialist"
- `developmentArea`: Enum - "UserInterface" | "Backend" | "FullStack" | "Testing" | "DataScience" | "DevOps"

**Response 201:**
```json
{
  "id": 456,
  "userId": 123,
  "mainTechnologies": ["C#", ".NET Core", "PostgreSQL", "JavaScript", "Docker"],
  "role": "Programista",
  "developmentArea": "Backend",
  "createdAt": "2025-11-15T10:35:00Z",
  "startNodeCreated": true
}
```

**Response 400:**
```json
{
  "error": "ValidationError",
  "message": "Validation failed",
  "errors": [
    {
      "field": "mainTechnologies",
      "message": "Profile must contain at least 5 technologies"
    }
  ]
}
```

**Response 409:**
```json
{
  "error": "Conflict",
  "message": "Profile already exists. Use PUT to update."
}
```

---

#### PUT /api/profile
**Description:** Updates user profile. Does NOT regenerate technology graph.  
**Authentication:** Required

**Request Body:**
```json
{
  "mainTechnologies": [
    "C#",
    ".NET Core",
    "PostgreSQL",
    "React",
    "Docker",
    "Kubernetes"
  ],
  "role": "Programista",
  "developmentArea": "FullStack"
}
```

**Response 200:**
```json
{
  "id": 456,
  "userId": 123,
  "mainTechnologies": ["C#", ".NET Core", "PostgreSQL", "React", "Docker", "Kubernetes"],
  "role": "Programista",
  "developmentArea": "FullStack",
  "createdAt": "2025-11-15T10:35:00Z",
  "updatedAt": "2025-12-01T09:20:00Z"
}
```

**Response 400:** (Same validation as POST)

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Profile not found. Use POST to create."
}
```

---

### 2.3 Technologies (Graph Nodes)

#### GET /api/technologies
**Description:** Retrieves all active technologies for current user  
**Authentication:** Required  
**Query Parameters:**
- `status` (optional): "Active" | "All" (default: "Active")
- `includeStart` (optional): boolean (default: true)

**Response 200:**
```json
{
  "technologies": [
    {
      "id": 1001,
      "userId": 123,
      "technologyDefinitionId": 1,
      "name": "Start",
      "prefix": "System",
      "tag": "Metodologia",
      "systemDescription": "Starting point for your technology journey",
      "privateDescription": null,
      "progress": 100,
      "status": "Active",
      "isCustom": false,
      "isStart": true,
      "aiReasoning": null,
      "createdAt": "2025-11-15T10:35:00Z",
      "updatedAt": null
    },
    {
      "id": 1002,
      "userId": 123,
      "technologyDefinitionId": 15,
      "name": "DotNet - Entity Framework",
      "prefix": "DotNet",
      "tag": "Framework",
      "systemDescription": "Object-relational mapper for .NET",
      "privateDescription": "Learning basics, completed tutorials",
      "progress": 45,
      "status": "Active",
      "isCustom": false,
      "isStart": false,
      "aiReasoning": "Essential ORM for .NET backend development",
      "createdAt": "2025-11-16T14:20:00Z",
      "updatedAt": "2025-11-28T16:45:00Z"
    }
  ],
  "count": 2
}
```

**Response 401:** (Standard unauthorized response)

---

#### GET /api/technologies/{id}
**Description:** Retrieves single technology by ID  
**Authentication:** Required

**Response 200:**
```json
{
  "id": 1002,
  "userId": 123,
  "technologyDefinitionId": 15,
  "name": "DotNet - Entity Framework",
  "prefix": "DotNet",
  "tag": "Framework",
  "systemDescription": "Object-relational mapper for .NET",
  "privateDescription": "Learning basics, completed tutorials",
  "progress": 45,
  "status": "Active",
  "isCustom": false,
  "isStart": false,
  "aiReasoning": "Essential ORM for .NET backend development",
  "createdAt": "2025-11-16T14:20:00Z",
  "updatedAt": "2025-11-28T16:45:00Z"
}
```

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Technology not found"
}
```

**Response 403:**
```json
{
  "error": "Forbidden",
  "message": "This technology belongs to another user"
}
```

---

#### POST /api/technologies
**Description:** Adds single technology to user's graph  
**Authentication:** Required

**Request Body:**
```json
{
  "technologyDefinitionId": 15,
  "fromTechnologyId": 1001,
  "privateDescription": "Want to learn this for my project",
  "isCustom": false
}
```

**Validation Rules:**
- `technologyDefinitionId`: Required, must exist in TechnologyDefinitions
- `fromTechnologyId`: Required, must be user's existing technology
- `privateDescription`: Optional, max 2000 characters
- `isCustom`: Optional, boolean (default: false)

**Response 201:**
```json
{
  "technology": {
    "id": 1002,
    "userId": 123,
    "technologyDefinitionId": 15,
    "name": "DotNet - Entity Framework",
    "prefix": "DotNet",
    "tag": "Framework",
    "systemDescription": "Object-relational mapper for .NET",
    "privateDescription": "Want to learn this for my project",
    "progress": 0,
    "status": "Active",
    "isCustom": false,
    "isStart": false,
    "aiReasoning": null,
    "createdAt": "2025-12-01T10:15:00Z",
    "updatedAt": null
  },
  "dependency": {
    "id": 5001,
    "userId": 123,
    "fromTechnologyId": 1001,
    "toTechnologyId": 1002,
    "createdAt": "2025-12-01T10:15:00Z"
  }
}
```

**Response 400:**
```json
{
  "error": "ValidationError",
  "message": "Invalid request",
  "errors": [
    {
      "field": "technologyDefinitionId",
      "message": "Technology definition does not exist"
    }
  ]
}
```

**Response 409:**
```json
{
  "error": "Conflict",
  "message": "Dependency already exists in graph"
}
```

---

#### POST /api/technologies/batch
**Description:** Adds multiple technologies to user's graph at once (for selecting multiple AI recommendations)  
**Authentication:** Required

**Request Body:**
```json
{
  "fromTechnologyId": 1001,
  "technologies": [
    {
      "technologyDefinitionId": 15,
      "privateDescription": "Essential for my project"
    },
    {
      "technologyDefinitionId": 23,
      "privateDescription": null
    }
  ]
}
```

**Response 201:**
```json
{
  "added": [
    {
      "technology": {
        "id": 1002,
        "name": "DotNet - Entity Framework",
        "progress": 0
      },
      "dependency": {
        "id": 5001,
        "fromTechnologyId": 1001,
        "toTechnologyId": 1002
      }
    },
    {
      "technology": {
        "id": 1003,
        "name": "PostgreSQL - Advanced Queries",
        "progress": 0
      },
      "dependency": {
        "id": 5002,
        "fromTechnologyId": 1001,
        "toTechnologyId": 1003
      }
    }
  ],
  "count": 2
}
```

**Response 207 (Multi-Status):**
```json
{
  "results": [
    {
      "technologyDefinitionId": 15,
      "status": "success",
      "technologyId": 1002
    },
    {
      "technologyDefinitionId": 23,
      "status": "error",
      "error": "Dependency already exists"
    }
  ],
  "successCount": 1,
  "errorCount": 1
}
```

---

#### POST /api/technologies/custom
**Description:** Creates custom technology (user-defined, not from AI)  
**Authentication:** Required

**Request Body:**
```json
{
  "name": "My Custom Framework",
  "prefix": "JavaScript",
  "tag": "Framework",
  "systemDescription": "A framework I'm building",
  "fromTechnologyId": 1001,
  "privateDescription": "My own project"
}
```

**Validation Rules:**
- `name`: Required, max 255 characters
- `prefix`: Required, max 100 characters
- `tag`: Required, Enum - "Technologia" | "Framework" | "BazaDanych" | "Metodologia" | "Narzedzie"
- `systemDescription`: Required, max 1000 characters
- `fromTechnologyId`: Required
- `privateDescription`: Optional, max 2000 characters

**Response 201:**
```json
{
  "technologyDefinition": {
    "id": 789,
    "name": "My Custom Framework",
    "prefix": "JavaScript",
    "tag": "Framework",
    "systemDescription": "A framework I'm building",
    "createdAt": "2025-12-01T11:30:00Z"
  },
  "technology": {
    "id": 1005,
    "userId": 123,
    "technologyDefinitionId": 789,
    "name": "My Custom Framework",
    "progress": 0,
    "isCustom": true,
    "isStart": false,
    "createdAt": "2025-12-01T11:30:00Z"
  },
  "dependency": {
    "id": 5005,
    "fromTechnologyId": 1001,
    "toTechnologyId": 1005
  }
}
```

---

#### PATCH /api/technologies/{id}
**Description:** Updates technology progress or private description  
**Authentication:** Required

**Request Body:**
```json
{
  "progress": 75,
  "privateDescription": "Completed advanced course, working on project"
}
```

**Validation Rules:**
- `progress`: Optional, integer 0-100
- `privateDescription`: Optional, max 2000 characters, can be null

**Response 200:**
```json
{
  "id": 1002,
  "progress": 75,
  "privateDescription": "Completed advanced course, working on project",
  "updatedAt": "2025-12-01T12:45:00Z"
}
```

**Response 400:**
```json
{
  "error": "ValidationError",
  "message": "Progress must be between 0 and 100"
}
```

**Response 404:** (Standard not found)

---

#### DELETE /api/technologies/{id}
**Description:** Deletes technology from user's graph. Cannot delete Start node. Deletes all related dependencies.  
**Authentication:** Required

**Response 200:**
```json
{
  "message": "Technology deleted successfully",
  "deletedDependencies": 3
}
```

**Response 403:**
```json
{
  "error": "Forbidden",
  "message": "Cannot delete Start node"
}
```

**Response 404:** (Standard not found)

---

#### GET /api/technologies/graph
**Description:** Retrieves complete technology graph for current user (nodes + edges)  
**Authentication:** Required

**Response 200:**
```json
{
  "nodes": [
    {
      "id": 1001,
      "name": "Start",
      "prefix": "System",
      "tag": "Metodologia",
      "progress": 100,
      "isStart": true,
      "systemDescription": "Starting point for your technology journey"
    },
    {
      "id": 1002,
      "name": "DotNet - Entity Framework",
      "prefix": "DotNet",
      "tag": "Framework",
      "progress": 45,
      "isStart": false,
      "systemDescription": "Object-relational mapper for .NET"
    },
    {
      "id": 1003,
      "name": "PostgreSQL - Advanced Queries",
      "prefix": "PostgreSQL",
      "tag": "BazaDanych",
      "progress": 20,
      "isStart": false,
      "systemDescription": "Advanced SQL querying techniques"
    }
  ],
  "edges": [
    {
      "id": 5001,
      "from": 1001,
      "to": 1002,
      "createdAt": "2025-11-16T14:20:00Z"
    },
    {
      "id": 5002,
      "from": 1001,
      "to": 1003,
      "createdAt": "2025-11-17T09:15:00Z"
    },
    {
      "id": 5003,
      "from": 1002,
      "to": 1004,
      "createdAt": "2025-11-20T16:30:00Z"
    }
  ],
  "stats": {
    "totalNodes": 3,
    "totalEdges": 3,
    "averageProgress": 55,
    "completedCount": 1
  }
}
```

---

### 2.4 Dependencies (Graph Edges)

#### POST /api/technologies/{id}/dependencies
**Description:** Creates new dependency (edge) from specified technology to another  
**Authentication:** Required

**Request Body:**
```json
{
  "toTechnologyId": 1005
}
```

**Response 201:**
```json
{
  "id": 5010,
  "userId": 123,
  "fromTechnologyId": 1002,
  "toTechnologyId": 1005,
  "createdAt": "2025-12-01T13:20:00Z"
}
```

**Response 400:**
```json
{
  "error": "ValidationError",
  "message": "Target technology does not exist"
}
```

**Response 409:**
```json
{
  "error": "Conflict",
  "message": "Dependency already exists"
}
```

---

#### DELETE /api/technologies/{id}/dependencies/{dependencyId}
**Description:** Removes dependency (edge) from graph  
**Authentication:** Required

**Response 200:**
```json
{
  "message": "Dependency deleted successfully"
}
```

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Dependency not found"
}
```

---

### 2.5 AI Recommendations

#### POST /api/ai/recommendations
**Description:** Generates AI recommendations for next technologies to learn. Uses in-memory cache (24h TTL). No rate limiting.  
**Authentication:** Required

**Request Body:**
```json
{
  "fromTechnologyId": 1002,
  "contextTechnologyIds": [1001, 1002, 1003]
}
```

**Validation Rules:**
- `fromTechnologyId`: Required, technology must belong to user
- `contextTechnologyIds`: Optional, array of user's technology IDs for additional context
- User must have completed profile

**Response 200:**
```json
{
  "recommendations": [
    {
      "technologyDefinitionId": 45,
      "name": "DotNet - LINQ Advanced",
      "prefix": "DotNet",
      "tag": "Technologia",
      "systemDescription": "Advanced Language Integrated Query techniques for data manipulation",
      "aiReasoning": "Natural progression after Entity Framework, helps optimize database queries",
      "isAlreadyInGraph": false
    },
    {
      "technologyDefinitionId": 67,
      "name": "PostgreSQL - Indexing Strategies",
      "prefix": "PostgreSQL",
      "tag": "BazaDanych",
      "systemDescription": "Performance optimization through proper database indexing",
      "aiReasoning": "Complements Entity Framework knowledge, highly relevant for backend developers",
      "isAlreadyInGraph": false
    }
  ],
  "count": 10,
  "fromCache": false,
  "cacheExpiresAt": "2025-12-02T14:30:00Z",
  "generatedAt": "2025-12-01T14:30:00Z"
}
```

**Response 400:**
```json
{
  "error": "ValidationError",
  "message": "Profile is incomplete. Please complete your profile first."
}
```

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Source technology not found"
}
```

**Response 408:**
```json
{
  "error": "Timeout",
  "message": "AI service did not respond within 20 seconds. Please try again."
}
```

**Response 500:**
```json
{
  "error": "AIServiceError",
  "message": "Failed to generate recommendations. Please try again later.",
  "details": "OpenRouter API returned error 503"
}
```

**Response 502:**
```json
{
  "error": "BadGateway",
  "message": "AI service returned invalid response format"
}
```

---

### 2.6 Ignored Technologies

#### GET /api/ignored-technologies
**Description:** Retrieves list of all technologies ignored by current user  
**Authentication:** Required  
**Query Parameters:**
- `limit` (optional): integer, max 100 (default: 50)
- `offset` (optional): integer (default: 0)

**Response 200:**
```json
{
  "ignoredTechnologies": [
    {
      "id": 3001,
      "userId": 123,
      "name": "Java - Spring Boot",
      "category": "Java",
      "tag": "Framework",
      "systemDescription": "Popular Java framework for building enterprise applications",
      "aiReasoning": "Commonly used with Java backend development",
      "contextTechnologyId": 1002,
      "contextTechnologyName": "DotNet - Entity Framework",
      "ignoredAt": "2025-11-18T10:20:00Z"
    },
    {
      "id": 3002,
      "userId": 123,
      "name": "Ruby on Rails",
      "category": "Ruby",
      "tag": "Framework",
      "systemDescription": "Web application framework written in Ruby",
      "aiReasoning": "Alternative backend framework",
      "contextTechnologyId": 1001,
      "contextTechnologyName": "Start",
      "ignoredAt": "2025-11-16T15:45:00Z"
    }
  ],
  "count": 2,
  "total": 2,
  "limit": 50,
  "offset": 0
}
```

---

#### POST /api/ignored-technologies
**Description:** Adds technology to ignore list (single or batch)  
**Authentication:** Required

**Request Body:**
```json
{
  "technologies": [
    {
      "name": "Java - Spring Boot",
      "category": "Java",
      "tag": "Framework",
      "systemDescription": "Popular Java framework for building enterprise applications",
      "aiReasoning": "Commonly used with Java backend development",
      "contextTechnologyId": 1002
    }
  ]
}
```

**Validation Rules:**
- `technologies`: Array, minimum 1 item, maximum 20 items
- `name`: Required, max 255 characters
- `category`: Required, max 100 characters
- `tag`: Required, Enum
- `systemDescription`: Required, max 1000 characters
- `aiReasoning`: Optional, max 1000 characters
- `contextTechnologyId`: Optional, must be user's technology

**Response 201:**
```json
{
  "added": [
    {
      "id": 3001,
      "name": "Java - Spring Boot",
      "category": "Java",
      "ignoredAt": "2025-12-01T15:10:00Z"
    }
  ],
  "count": 1
}
```

**Response 409:**
```json
{
  "error": "Conflict",
  "message": "Technology 'Java - Spring Boot' is already ignored"
}
```

---

#### DELETE /api/ignored-technologies/{id}
**Description:** Removes technology from ignore list (restores it for future recommendations)  
**Authentication:** Required

**Response 200:**
```json
{
  "message": "Technology restored successfully",
  "id": 3001,
  "name": "Java - Spring Boot"
}
```

**Response 404:**
```json
{
  "error": "NotFound",
  "message": "Ignored technology not found"
}
```

---

#### DELETE /api/ignored-technologies/batch
**Description:** Removes multiple technologies from ignore list at once  
**Authentication:** Required

**Request Body:**
```json
{
  "ids": [3001, 3002, 3005]
}
```

**Response 200:**
```json
{
  "message": "Technologies restored successfully",
  "restoredCount": 3,
  "restored": [
    {
      "id": 3001,
      "name": "Java - Spring Boot"
    },
    {
      "id": 3002,
      "name": "Ruby on Rails"
    },
    {
      "id": 3005,
      "name": "PHP - Laravel"
    }
  ]
}
```

**Response 207:**
```json
{
  "results": [
    {
      "id": 3001,
      "status": "success"
    },
    {
      "id": 3002,
      "status": "success"
    },
    {
      "id": 3005,
      "status": "error",
      "error": "Not found"
    }
  ],
  "restoredCount": 2
}
```

---

### 2.7 Admin Endpoints

All admin endpoints require authenticated user with GoogleId in admin whitelist (configured in appsettings.json).

#### GET /api/admin/metrics
**Description:** Retrieves dashboard metrics for MVP KPIs  
**Authentication:** Required (Admin only)

**Response 200:**
```json
{
  "kpis": {
    "profileCompletionRate": 92.5,
    "profileCompletionTarget": 90,
    "profileCompletionMet": true,
    "technologyGenerationRate": 78.3,
    "technologyGenerationTarget": 75,
    "technologyGenerationMet": true
  },
  "users": {
    "total": 1247,
    "withProfile": 1154,
    "withoutProfile": 93,
    "activeLastWeek": 856,
    "activeLastMonth": 1089
  },
  "technologies": {
    "totalGenerated": 15678,
    "totalInGraphs": 12345,
    "totalIgnored": 3333,
    "averagePerUser": 9.9,
    "averageProgress": 42.7,
    "completedCount": 3456
  },
  "ai": {
    "totalRequests": 8934,
    "cacheHits": 2145,
    "cacheHitRate": 24.0,
    "averageResponseTime": 4.2,
    "timeouts": 23,
    "errors": 12
  },
  "generatedAt": "2025-12-01T16:00:00Z"
}
```

**Response 403:**
```json
{
  "error": "Forbidden",
  "message": "Admin access required"
}
```

---

#### GET /api/admin/users
**Description:** Retrieves paginated list of users with basic stats  
**Authentication:** Required (Admin only)  
**Query Parameters:**
- `limit` (optional): integer, max 100 (default: 50)
- `offset` (optional): integer (default: 0)
- `sortBy` (optional): "createdAt" | "lastLoginAt" | "email" (default: "createdAt")
- `order` (optional): "asc" | "desc" (default: "desc")

**Response 200:**
```json
{
  "users": [
    {
      "id": 123,
      "email": "user@example.com",
      "name": "John Doe",
      "hasProfile": true,
      "technologiesCount": 12,
      "averageProgress": 56,
      "createdAt": "2025-11-15T10:30:00Z",
      "lastLoginAt": "2025-12-01T08:15:00Z"
    }
  ],
  "count": 50,
  "total": 1247,
  "limit": 50,
  "offset": 0
}
```

---

#### GET /api/admin/technologies/popular
**Description:** Retrieves most popular technology prefixes/categories  
**Authentication:** Required (Admin only)  
**Query Parameters:**
- `limit` (optional): integer, max 50 (default: 20)
- `groupBy` (optional): "prefix" | "tag" (default: "prefix")

**Response 200:**
```json
{
  "popularPrefixes": [
    {
      "prefix": "DotNet",
      "count": 3456,
      "percentage": 28.0,
      "uniqueUsers": 892
    },
    {
      "prefix": "JavaScript",
      "count": 2987,
      "percentage": 24.2,
      "uniqueUsers": 845
    },
    {
      "prefix": "PostgreSQL",
      "count": 1876,
      "percentage": 15.2,
      "uniqueUsers": 678
    }
  ],
  "total": 12345,
  "generatedAt": "2025-12-01T16:15:00Z"
}
```

---

#### GET /api/admin/statistics/profiles
**Description:** Retrieves profile statistics breakdown  
**Authentication:** Required (Admin only)

**Response 200:**
```json
{
  "roles": {
    "Programista": {
      "count": 890,
      "percentage": 71.4
    },
    "Tester": {
      "count": 234,
      "percentage": 18.8
    },
    "Analityk": {
      "count": 89,
      "percentage": 7.1
    },
    "DataScienceSpecialist": {
      "count": 34,
      "percentage": 2.7
    }
  },
  "developmentAreas": {
    "Backend": {
      "count": 567,
      "percentage": 45.5
    },
    "FullStack": {
      "count": 345,
      "percentage": 27.7
    },
    "UserInterface": {
      "count": 234,
      "percentage": 18.8
    },
    "Testing": {
      "count": 67,
      "percentage": 5.4
    },
    "DataScience": {
      "count": 23,
      "percentage": 1.8
    },
    "DevOps": {
      "count": 11,
      "percentage": 0.9
    }
  },
  "totalProfiles": 1247,
  "generatedAt": "2025-12-01T16:20:00Z"
}
```

---

## 3. Authentication and Authorization

### 3.1 Authentication Mechanism

**OAuth 2.0 with Google**

The API uses Google OAuth 2.0 for user authentication. Implementation details:

1. **Flow Type:** Authorization Code Flow
2. **Library:** `Microsoft.AspNetCore.Authentication.Google` (ASP.NET Core)
3. **Session Management:** Server-side sessions (Blazor Server)
4. **Token Storage:** Tokens are NOT stored in database; managed by authentication middleware

**Authentication Flow:**

```
1. User clicks "Login with Google"
2. Frontend calls POST /api/auth/login
3. API returns Google OAuth URL
4. User is redirected to Google login
5. After authentication, Google redirects to GET /api/auth/callback?code=...
6. API exchanges code for tokens with Google
7. API creates/retrieves User record from database
8. API creates server-side session
9. API returns user data and redirect URL
10. All subsequent requests include session cookie
```

### 3.2 Session Management

- **Session Storage:** Server-side (in-memory or distributed cache)
- **Session Lifetime:** 7 days (sliding expiration)
- **Cookie Name:** `.AspNetCore.Blazor.Session` (default)
- **Cookie Attributes:** HttpOnly, Secure (HTTPS only), SameSite=Lax

### 3.3 Authorization Levels

**Public Endpoints (No Authentication):**
- `POST /api/auth/login`
- `GET /api/auth/callback`

**Authenticated Endpoints (Requires Valid Session):**
- All `/api/profile/*` endpoints
- All `/api/technologies/*` endpoints
- All `/api/ai/*` endpoints
- All `/api/ignored-technologies/*` endpoints
- `GET /api/auth/me`
- `POST /api/auth/logout`

**Admin Endpoints (Requires Admin GoogleId):**
- All `/api/admin/*` endpoints

**Admin Whitelist Configuration (appsettings.json):**
```json
{
  "AdminSettings": {
    "GoogleIds": [
      "1234567890",
      "0987654321"
    ]
  }
}
```

### 3.4 Authorization Implementation

**Middleware Check Pattern:**

```csharp
// Authenticated user context
var userId = HttpContext.User.FindFirst("UserId")?.Value;
if (userId == null) return Unauthorized();

// Admin check
var googleId = HttpContext.User.FindFirst("GoogleId")?.Value;
var adminGoogleIds = _configuration.GetSection("AdminSettings:GoogleIds").Get<string[]>();
if (!adminGoogleIds.Contains(googleId)) return Forbid();
```

**Data Isolation Pattern:**

All user-scoped queries automatically filter by `UserId`:

```csharp
// Extension method applied to all queries
.ForCurrentUser(userId)
// Translates to: .Where(x => x.UserId == userId)
```

### 3.5 Security Headers

All API responses include:
```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

### 3.6 CORS Policy

For Blazor Server (same-origin):
- CORS is NOT required (API and UI on same domain)
- If API is separate: Configure AllowedOrigins in appsettings.json

---

## 4. Validation and Business Logic

### 4.1 Validation Rules by Resource

#### User Profile Validation

**POST/PUT /api/profile:**
- `mainTechnologies`:
  - Required
  - Array of strings
  - Minimum 5 items (PRD requirement, line 93)
  - Maximum 50 items
  - Each item max 100 characters
  - No duplicates
- `role`:
  - Required
  - Enum: "Programista" | "Tester" | "Analityk" | "DataScienceSpecialist"
- `developmentArea`:
  - Required
  - Enum: "UserInterface" | "Backend" | "FullStack" | "Testing" | "DataScience" | "DevOps"

**Error Response Format:**
```json
{
  "error": "ValidationError",
  "message": "Validation failed",
  "errors": [
    {
      "field": "mainTechnologies",
      "message": "Profile must contain at least 5 technologies"
    }
  ]
}
```

---

#### Technology Validation

**POST /api/technologies:**
- `technologyDefinitionId`:
  - Required
  - Must exist in TechnologyDefinitions table
- `fromTechnologyId`:
  - Required
  - Must be user's existing technology (userId check)
  - Cannot be the same as target technology (no self-loops)
- `privateDescription`:
  - Optional
  - Max 2000 characters (DB schema, line 166)

**PATCH /api/technologies/{id}:**
- `progress`:
  - Optional
  - Integer
  - Range: 0-100 (DB constraint, line 175)
  - Error: "Progress must be between 0 and 100"
- `privateDescription`:
  - Optional
  - Max 2000 characters
  - Can be null (clears description)

**DELETE /api/technologies/{id}:**
- Cannot delete if `IsStart = true` (DB schema, line 176)
- Error: 403 Forbidden - "Cannot delete Start node"
- Deletes all related dependencies (CASCADE)

---

#### Custom Technology Validation

**POST /api/technologies/custom:**
- `name`:
  - Required
  - Max 255 characters
  - No duplicate check (user can create duplicate names)
- `prefix`:
  - Required
  - Max 100 characters
- `tag`:
  - Required
  - Enum: "Technologia" | "Framework" | "BazaDanych" | "Metodologia" | "Narzedzie"
- `systemDescription`:
  - Required
  - Max 1000 characters
- Creates TechnologyDefinition with `(Name, Prefix, Tag)` uniqueness
- If definition exists, reuses it; otherwise creates new

---

#### Dependency Validation

**POST /api/technologies/{id}/dependencies:**
- `toTechnologyId`:
  - Required
  - Must be user's existing technology
  - Cannot be the same as `fromTechnologyId` (no self-loops)
- Uniqueness constraint: `(UserId, FromTechnologyId, ToTechnologyId)` (DB schema, line 188)
- Error on duplicate: 409 Conflict - "Dependency already exists"

---

#### Ignored Technologies Validation

**POST /api/ignored-technologies:**
- `name`:
  - Required
  - Max 255 characters
- `category`:
  - Required
  - Max 100 characters
- `tag`:
  - Required
  - Enum (same as Technology tag)
- `systemDescription`:
  - Required
  - Max 1000 characters
- `aiReasoning`:
  - Optional
  - Max 1000 characters
- Uniqueness constraint: `(UserId, Name, Category)` (DB schema, line 204)
- Error on duplicate: 409 Conflict - "Technology already ignored"

---

### 4.2 Business Logic Implementation

#### BL-1: Profile Creation with Start Node

**Trigger:** POST /api/profile

**Logic:**
1. Validate profile data (minimum 5 technologies)
2. Check if profile already exists → 409 Conflict
3. Create UserProfile record
4. **Automatically create "Start" node:**
   - Retrieve TechnologyDefinition with Id=1 (seed data)
   - Create UserTechnology:
     - TechnologyDefinitionId = 1
     - IsStart = true
     - Progress = 100
     - Status = Active
5. Return profile + startNodeCreated flag

**References:** DB plan line 272-273, PRD line 342

---

#### BL-2: AI Recommendations with Caching

**Trigger:** POST /api/ai/recommendations

**Logic:**
1. Validate user has complete profile → 400 if not (PRD line 88)
2. Validate `fromTechnologyId` belongs to user → 404 if not
3. **Check cache:**
   - Generate cache key: `ai-recommendations:{userId}:{fromTechnologyId}:{profileHash}`
   - ProfileHash = MD5(MainTechnologies + Role + DevelopmentArea) (DB plan line 72)
   - Check IMemoryCache
4. **If cache hit:**
   - Return cached recommendations
   - Set `fromCache: true`
5. **If cache miss:**
   - Prepare AI request payload:
     - User profile (all fields)
     - All user's active technologies (from graph)
     - Context: fromTechnology
     - List of ignored technologies (exclude from results)
   - Call OpenRouter.ai API (20 second timeout)
   - Parse JSON response (~10 technologies)
6. **Filter results:**
   - Remove technologies already in user's graph (set `isAlreadyInGraph: true` if found)
   - Remove technologies in IgnoredTechnologies
7. **Store in cache:**
   - TTL: 24 hours (DB plan line 74)
   - Store as JSON
8. Return recommendations with metadata

**Error Handling:**
- Timeout (>20s): 408 Request Timeout (PRD line 148)
- AI API error: 500 Internal Server Error with generic message (PRD line 680)
- Invalid JSON: 502 Bad Gateway

**References:** DB plan lines 69-75, PRD lines 119-158

---

#### BL-3: Respecting User Specialization

**Trigger:** POST /api/ai/recommendations

**Logic:**
- AI prompt includes full profile:
  - MainTechnologies (e.g., ".NET, C#, PostgreSQL")
  - Role (e.g., "Programista")
  - DevelopmentArea (e.g., "Backend")
- AI is instructed to:
  - NOT recommend technologies outside user's MainTechnologies ecosystems
  - Example: .NET developer → NO Java recommendations (PRD line 827)
  - Example: Frontend developer → NO C# backend frameworks (PRD line 828)
- Filter is applied via AI prompt, not post-processing

**References:** PRD lines 154-158, 822-831

---

#### BL-4: Graph Integrity on Delete

**Trigger:** DELETE /api/technologies/{id}

**Logic:**
1. Check if `IsStart = true` → 403 Forbidden (DB plan line 176)
2. Find all dependencies:
   - Where `FromTechnologyId = {id}` (outgoing edges)
   - Where `ToTechnologyId = {id}` (incoming edges)
3. **Cascade delete all dependencies** (DB FK CASCADE)
4. **Orphaned technologies:**
   - Technologies with no incoming edges after delete remain in graph
   - User decision: can be left as-is or deleted manually
   - No automatic cleanup (PRD does not specify)
5. Delete UserTechnology record
6. Return count of deleted dependencies

**Open Question (DB plan line 351-354):**
- Should orphaned technologies be auto-deleted?
- **MVP Decision:** NO - keep orphaned technologies, user can delete manually

**References:** DB plan lines 351-355, PRD line 563

---

#### BL-5: Profile Update Does Not Regenerate Graph

**Trigger:** PUT /api/profile

**Logic:**
1. Update UserProfile record
2. **NO graph operations:**
   - Do not regenerate recommendations
   - Do not modify existing technologies
   - Do not invalidate cache (cache uses profileHash, will miss naturally)
3. User must manually generate new recommendations if desired

**References:** PRD lines 88-89, 361-362

---

#### BL-6: Batch Technology Addition

**Trigger:** POST /api/technologies/batch

**Logic:**
1. Validate `fromTechnologyId` belongs to user
2. For each technology in array:
   - Check if TechnologyDefinition exists
   - Check if dependency already exists (UNIQUE constraint)
   - If error: log but continue (partial success allowed)
3. **Transaction handling:**
   - Use database transaction
   - Commit all or rollback all on critical error
   - For constraint violations: skip item, continue
4. Return:
   - 201 Created if all succeeded
   - 207 Multi-Status if partial success
   - List of successes and errors per item

**Use Case:** User selects multiple checkboxes in AI recommendations popup and clicks "Add to graph" (PRD line 415)

**References:** PRD lines 414-424

---

#### BL-7: Ignoring Technologies with Full Archive

**Trigger:** POST /api/ignored-technologies

**Logic:**
1. For each technology to ignore:
   - Store **full definition** in IgnoredTechnologies table (not just ID reference)
   - Fields: Name, Category, Tag, SystemDescription, AiReasoning
   - Store `contextTechnologyId` (optional) - the node from which it was recommended
2. **Do NOT create TechnologyDefinition record** (DB plan line 61)
3. **Uniqueness:** Check (UserId, Name, Category) before insert
4. **Future filtering:** When generating AI recommendations:
   - Query IgnoredTechnologies for user
   - Exclude matching names from AI results

**Rationale:** Avoid polluting TechnologyDefinitions with rejected technologies (DB plan line 61)

**References:** DB plan lines 54-61, PRD lines 437-443

---

#### BL-8: Restoring Ignored Technologies

**Trigger:** DELETE /api/ignored-technologies/{id}

**Logic:**
1. Delete record from IgnoredTechnologies
2. Technology becomes eligible for future AI recommendations again
3. **Does NOT automatically add to graph** - user must re-add manually
4. If user adds restored technology:
   - Creates new TechnologyDefinition (if not exists)
   - Creates new UserTechnology
   - Creates new dependency

**References:** PRD lines 453-461

---

#### BL-9: Admin Access Control

**Trigger:** Any /api/admin/* endpoint

**Logic:**
1. Check if user is authenticated → 401 if not
2. Extract GoogleId from user claims
3. Load admin whitelist from `appsettings.json`:
   ```json
   "AdminSettings": {
     "GoogleIds": ["1234567890", "0987654321"]
   }
   ```
4. Check if GoogleId is in whitelist → 403 Forbidden if not
5. Proceed with admin operation

**Implementation:** Custom authorization policy or middleware

**References:** DB plan lines 368-370, PRD lines 240-249

---

#### BL-10: Data Isolation (Security)

**Applied to:** ALL authenticated endpoints

**Logic:**
1. Extract UserId from authenticated session/claims
2. Apply filter to ALL queries:
   ```csharp
   .Where(x => x.UserId == userId)
   ```
3. **Never trust client-provided UserId** - always from auth context
4. Extension method pattern:
   ```csharp
   _context.UserTechnologies.ForCurrentUser(userId)
   ```
5. **Validation on write operations:**
   - POST: Set UserId from auth context (ignore client value)
   - PUT/PATCH/DELETE: Verify record belongs to user → 403 if not

**References:** DB plan lines 224-229, PRD lines 770-779

---

### 4.3 Error Handling Standards

**Error Response Format (RFC 7807 Problem Details):**

```json
{
  "error": "ErrorType",
  "message": "Human-readable message",
  "details": "Optional technical details",
  "errors": [
    {
      "field": "fieldName",
      "message": "Field-specific error"
    }
  ]
}
```

**Standard HTTP Status Codes:**

| Code | Usage |
|------|-------|
| 200 OK | Successful GET, PATCH, DELETE |
| 201 Created | Successful POST (resource created) |
| 207 Multi-Status | Partial success in batch operations |
| 400 Bad Request | Validation errors, malformed requests |
| 401 Unauthorized | Missing or invalid authentication |
| 403 Forbidden | Authenticated but insufficient permissions |
| 404 Not Found | Resource does not exist |
| 408 Request Timeout | AI timeout (>20 seconds) |
| 409 Conflict | Uniqueness constraint violation |
| 500 Internal Server Error | Unhandled server errors |
| 502 Bad Gateway | AI service returned invalid response |

---

### 4.4 Performance Considerations

#### Pagination Standards

**Endpoints with pagination:**
- GET /api/ignored-technologies
- GET /api/admin/users

**Default parameters:**
- `limit`: 50 (max 100)
- `offset`: 0

**Response includes:**
- `count`: Items in current response
- `total`: Total items available
- `limit`: Applied limit
- `offset`: Applied offset

#### Caching Strategy

**In-Memory Cache (IMemoryCache):**
- AI recommendations: 24 hour TTL
- Key format: `ai-recommendations:{userId}:{technologyId}:{profileHash}`
- ProfileHash: MD5 of (MainTechnologies + Role + DevelopmentArea)

**Database Query Optimization:**
- Use `AsNoTracking()` for all GET endpoints (read-only)
- Use `Include()` for graph endpoint (eager loading)
- Indexes utilized (from DB plan):
  - Users.GoogleId (login)
  - UserTechnologies (UserId, Status) - partial index
  - TechnologyDependencies (UserId, FromTechnologyId)

---

## 5. API Versioning

**Current Version:** v1 (implicit)

**Future Versioning Strategy:**
- URL path versioning: `/api/v2/...`
- No versioning in MVP (single version)

---

## 6. Rate Limiting

**MVP Decision:** NO rate limiting (PRD line 151, 791)

**Post-MVP Consideration:**
- AI recommendations: 100 requests/hour per user
- Admin endpoints: 1000 requests/hour per admin

---

## 7. Testing Requirements

**Endpoint Testing Priorities:**

1. **Critical Path (Must Test):**
   - POST /api/auth/callback (OAuth flow)
   - POST /api/profile (onboarding + Start node creation)
   - POST /api/ai/recommendations (AI integration + caching)
   - POST /api/technologies (graph building)
   - GET /api/technologies/graph (visualization data)

2. **High Priority:**
   - PATCH /api/technologies/{id} (progress tracking)
   - DELETE /api/technologies/{id} (Start node protection)
   - POST /api/ignored-technologies (ignoring workflow)
   - PUT /api/profile (profile update without graph regeneration)

3. **Medium Priority:**
   - All other CRUD endpoints
   - Admin endpoints
   - Batch operations

**Test Scenarios:**
- **Security:** Test data isolation (User A cannot access User B's data)
- **Validation:** Test all constraint violations (progress 0-100, min 5 techs, etc.)
- **Business Logic:** Test Start node protection, AI caching, profile hash changes
- **Error Handling:** Test AI timeout, invalid responses, 404s

---

## 8. Documentation

**API Documentation Format:** OpenAPI 3.0 (Swagger)

**Generated Documentation:**
- Swagger UI endpoint: `/swagger`
- OpenAPI JSON: `/swagger/v1/swagger.json`

**Additional Documentation:**
- This API Plan (design reference)
- Inline code comments (XML documentation)
- Example requests/responses (Swagger annotations)

---

## 9. Deployment Considerations

**Environment Variables:**

```bash
# Authentication
GOOGLE_CLIENT_ID=<your-google-client-id>
GOOGLE_CLIENT_SECRET=<your-google-client-secret>
GOOGLE_REDIRECT_URI=https://yourdomain.com/api/auth/callback

# Database
DATABASE_CONNECTION_STRING=<postgresql-connection-string>

# AI Service
OPENROUTER_API_KEY=<your-openrouter-api-key>
OPENROUTER_API_URL=https://openrouter.ai/api/v1

# Admin
ADMIN_GOOGLE_IDS=1234567890,0987654321

# Application
ASPNETCORE_ENVIRONMENT=Production
SESSION_TIMEOUT_MINUTES=10080
```

**HTTPS Requirement:**
- All endpoints MUST use HTTPS in production
- Google OAuth requires HTTPS redirect URIs

**Session Storage:**
- MVP: In-memory (single server)
- Production: Distributed cache (Redis) for multi-server

---

## 10. Monitoring and Logging

**Metrics to Track:**
- Request latency (p50, p95, p99)
- AI recommendations success rate
- AI response time average
- Cache hit rate
- Error rate by endpoint
- 4xx vs 5xx errors

**Logging Requirements:**
- All errors (500, 502) with stack traces
- AI timeouts (408) with context
- Admin access attempts (all users)
- OAuth failures

**Structured Logging Format (JSON):**
```json
{
  "timestamp": "2025-12-01T16:45:00Z",
  "level": "Error",
  "endpoint": "POST /api/ai/recommendations",
  "userId": 123,
  "error": "AI timeout",
  "duration": 20000
}
```

---

## Appendix A: Enum Definitions

### UserRole
```csharp
public enum UserRole
{
    Programista = 1,
    Tester = 2,
    Analityk = 3,
    DataScienceSpecialist = 4
}
```

### DevelopmentArea
```csharp
public enum DevelopmentArea
{
    UserInterface = 1,
    Backend = 2,
    FullStack = 3,
    Testing = 4,
    DataScience = 5,
    DevOps = 6
}
```

### TechnologyTag
```csharp
public enum TechnologyTag
{
    Technologia = 1,
    Framework = 2,
    BazaDanych = 3,
    Metodologia = 4,
    Narzedzie = 5
}
```

### TechnologyStatus
```csharp
public enum TechnologyStatus
{
    Active = 1
    // Note: "Ignored" removed - ignored technologies are in separate table
}
```

---

## Appendix B: Database Entity Mapping

| API Resource | Primary Table | Related Tables |
|--------------|---------------|----------------|
| Auth | Users | - |
| Profile | UserProfiles | Users |
| Technologies | UserTechnologies | TechnologyDefinitions, Users |
| Dependencies | TechnologyDependencies | UserTechnologies, Users |
| Ignored | IgnoredTechnologies | UserTechnologies (optional FK) |
| AI Recommendations | (in-memory cache) | - |

---

## Appendix C: API Request Examples

### Example 1: Complete Onboarding Flow

```bash
# Step 1: Login
POST /api/auth/login
Response: { "redirectUrl": "https://accounts.google.com/..." }

# Step 2: OAuth Callback (automatic redirect)
GET /api/auth/callback?code=<auth-code>
Response: { "user": {...}, "redirectTo": "/profile" }

# Step 3: Create Profile
POST /api/profile
{
  "mainTechnologies": ["C#", ".NET Core", "PostgreSQL", "Blazor", "Docker"],
  "role": "Programista",
  "developmentArea": "Backend"
}
Response: { "id": 456, ..., "startNodeCreated": true }

# Step 4: Get Graph (now includes Start node)
GET /api/technologies/graph
Response: { "nodes": [{"id": 1001, "name": "Start", "isStart": true}], "edges": [] }
```

### Example 2: AI Recommendation + Adding Technologies

```bash
# Step 1: Generate Recommendations
POST /api/ai/recommendations
{
  "fromTechnologyId": 1001,
  "contextTechnologyIds": [1001]
}
Response: { "recommendations": [{...}, {...}], "count": 10, "fromCache": false }

# Step 2: Add Multiple Technologies
POST /api/technologies/batch
{
  "fromTechnologyId": 1001,
  "technologies": [
    { "technologyDefinitionId": 15, "privateDescription": "Essential ORM" },
    { "technologyDefinitionId": 23, "privateDescription": null }
  ]
}
Response: { "added": [{...}, {...}], "count": 2 }
```

### Example 3: Ignoring and Restoring

```bash
# Step 1: Ignore Technology
POST /api/ignored-technologies
{
  "technologies": [{
    "name": "Java - Spring Boot",
    "category": "Java",
    "tag": "Framework",
    "systemDescription": "Java framework...",
    "aiReasoning": "Popular in enterprise",
    "contextTechnologyId": 1001
  }]
}
Response: { "added": [{...}], "count": 1 }

# Step 2: List Ignored
GET /api/ignored-technologies
Response: { "ignoredTechnologies": [{...}], "count": 1 }

# Step 3: Restore
DELETE /api/ignored-technologies/3001
Response: { "message": "Technology restored successfully", "id": 3001 }
```

---

**End of REST API Plan**

*This document defines the complete API contract for DeveloperGoals MVP. All endpoints follow RESTful principles, support JSON payloads, and implement proper error handling. Security is enforced through Google OAuth with session-based authentication and data isolation by UserId.*

```

Stworzyłem kompleksowy plan API REST dla aplikacji DeveloperGoals i zapisałem go w pliku `.ai/api-plan.md`.

## Główne elementy planu:

### 1. **7 głównych grup zasobów:**
- **Auth** - autentykacja przez Google OAuth
- **Profile** - profil użytkownika z technologiami, rolą, obszarem
- **Technologies** - węzły grafu (CRUD, batch operations, custom)
- **Dependencies** - krawędzie grafu
- **AI Recommendations** - generowanie rekomendacji z cache'owaniem
- **Ignored Technologies** - zarządzanie ignorowanymi technologiami
- **Admin** - metryki i statystyki (MVP KPIs)

### 2. **32 endpointy** szczegółowo opisane z:
- Metodami HTTP
- Ścieżkami URL
- Strukturami request/response JSON
- Kodami błędów i komunikatami
- Parametrami zapytań
- Walidacją

### 3. **Logika biznesowa zaimplementowana:**
- ✅ Automatyczne tworzenie węzła "Start" przy tworzeniu profilu
- ✅ Cache AI (24h TTL, profileHash)
- ✅ Ochrona węzła Start przed usunięciem (403 Forbidden)
- ✅ Respektowanie specjalizacji użytkownika w AI
- ✅ Pełna archiwizacja ignorowanych technologii (bez zaśmiecania słownika)
- ✅ Izolacja danych między użytkownikami (UserId filtering)
- ✅ Admin whitelist (hardcoded GoogleIds)

### 4. **Bezpieczeństwo:**
- Google OAuth 2.0 (Authorization Code Flow)
- Sesje server-side (Blazor Server)
- Automatyczne filtrowanie po UserId
- Admin authorization policy
- HTTPS required, security headers

### 5. **Specjalne funkcje:**
- Batch operations dla technologii (checkbox multiple select)
- Multi-status responses (207) dla częściowych sukcesów
- Graph endpoint z nodes + edges dla vis.js
- AI timeout handling (20s limit)
- Zero rate limiting (zgodnie z PRD)

Plan jest gotowy do implementacji w ASP.NET Core z Entity Framework Core i obejmuje wszystkie wymagania z PRD oraz schemat bazy z db-plan.md.

