# ============================================================================
# PODSTAWOWE ZMIENNE - DOSTOSUJ DO SWOJEGO ŚRODOWISKA
# ============================================================================
BASE_URL="http://localhost:5057"
# Dla HTTPS użyj: BASE_URL="https://localhost:7069"
# UWAGA: W Postmanie możesz użyć zmiennych środowiskowych zamiast ręcznej zmiany

# Cookie z sesji - po zalogowaniu przez przeglądarkę, skopiuj wartość cookie
# Format: "Cookie: .AspNetCore.Cookies=twoja_wartosc_cookie"
# 
# JAK UZYSKAĆ COOKIE:
# 1. Uruchom aplikację: dotnet run --project DeveloperGoals/DeveloperGoals/DeveloperGoals.csproj
# 2. Otwórz przeglądarkę: http://localhost:5057
# 3. Zaloguj się przez Google OAuth
# 4. Otwórz DevTools (F12) -> Application (Chrome) / Storage (Edge/Firefox)
# 5. Przejdź do: Cookies -> http://localhost:5057
# 6. Znajdź cookie: .AspNetCore.Cookies (lub .AspNetCore.Identity.Application)
# 7. Skopiuj wartość z kolumny "Value"
# 8. Wklej wartość poniżej zamiast "TWÓJ_COOKIE_TUTAJ"
#
# Szczegółowa instrukcja: zobacz plik INSTRUKCJA_COOKIE.md
COOKIE_HEADER="Cookie: .AspNetCore.Cookies=TWÓJ_COOKIE_TUTAJ"

# ============================================================================
# 1. PODSTAWOWY REQUEST - Generowanie rekomendacji bez kontekstu
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 2. REQUEST Z KONTEKSTEM - Generowanie rekomendacji z technologiami kontekstowymi
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [2, 3, 4]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 3. REQUEST Z DUŻĄ ILOŚCIĄ KONTEKSTU - Test maksymalnej liczby (50)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 4. TEST CACHE - Wywołaj ten sam request dwa razy (drugi powinien być z cache)
# ============================================================================
# Pierwsze wywołanie (cache miss)
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [2, 3]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -s | jq '.fromCache'

# Drugie wywołanie (cache hit - powinno zwrócić fromCache: true)
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [2, 3]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -s | jq '.fromCache'

# ============================================================================
# 5. TEST DEDUPLIKACJI - Duplikaty w contextTechnologyIds
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [2, 2, 3, 3, 3, 4, 4]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 6. TEST WALIDACJI - Brak fromTechnologyId (400 Bad Request)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "contextTechnologyIds": [1, 2]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 7. TEST WALIDACJI - Nieprawidłowy ID (0 lub ujemny) (400 Bad Request)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 0
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 8. TEST WALIDACJI - Za dużo technologii kontekstowych (>50) (400 Bad Request)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51]
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 9. TEST AUTORYZACJI - Brak cookie (401 Unauthorized)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -d '{
    "fromTechnologyId": 1
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 10. TEST Z PUSTĄ LISTĄ KONTEKSTU
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": []
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 11. TEST Z NULL KONTEKSTEM
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": null
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v

# ============================================================================
# 12. TEST Z NIEISTNIEJĄCĄ TECHNOLOGIĄ (404 Not Found)
# ============================================================================
curl -X POST "${BASE_URL}/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -H "${COOKIE_HEADER}" \
  -d '{
    "fromTechnologyId": 99999
  }' \
  -w "\nHTTP Status: %{http_code}\n" \
  -v