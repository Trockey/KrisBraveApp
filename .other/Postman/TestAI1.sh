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
COOKIE_HEADER="Cookie: .AspNetCore.Cookies=CfDJ8I_MNsn5DBtCmai4DLg7e06ftONBlEbgaTV8vzZYED7LL2_kgac7yY5bAOy2c1VX2ZbFWjShiYY2CaY1T-G-nuD2BUcykFVJ8wRC-U_oRXD8x7P8xMXFqVgtU_UhXTiFRjDLo7aTACSIz_Rw-EDZEjgI49R4z-eR4o6-5_DmakLjeRdPACgypwtSQf-fjMOWOOObYvCv3eoS6UOs5e0a2_EuQKU_eEoKZIxiIEEoHIo4AwuhZp9xi-I04mVPbIByFVPbgS6lED5H840uYnmdAlAwy6iIErwF4Bf763GUBFm0pXm_f767TcJ7kOnrpFJhPw-icYszIFF0B8JhgZLa4o4WsymtQZmjcZOvojDqvxjwoz_fnxFdfmMTDrlYWrepUHd95EYIrOrrOn5EF7GZOuTYT6FIS5S6WYzi6H-khXb0YYIgZM0-p6APDqrW1e_JnsNG1Qn69tKh1XNY_ZbCJPTUdkLm9QklRuLV_EL5oIiCir5ETmDij-Dy37miLUSOndF5Got1hEjZBmFWWHql00lBV1qrTag5eHRl6HM1i8RRan5wFye8jbY9cQTFytwBjeyQ8C8YY5TMRZdL7ywjTuZPoD_eU48-xUtSHdORtXnntMjZSHAeQTbiOW5rKmfzEgkkfhsj5s0uuBQrSC0drL8EYJV1U1EghsCp3pfJb4N2FTsYZmyZUPj6DsCYm8az5ZgPwyNABFwkQOAyp-K0dcwWO9y1wg31mTGdw6w4X9oFU5loS46MK2B--x6eS2jREalGeDNTYqOTMIBd0Ns5lYZmnB-_PsCWdSvyXBenqfD4TvCbESF6j0rCKwNhivzW206gSKZacgOGlQlz3GQwgSonJc2uty0IQzcotjOk22oFhyY3qa2bUX-zW6ac7s66TcRyYNJs_X329_lr3rUndAu4ZzxNAiDyv9squ4gunpy480pvGeTv3J4VOz4cTx0nij_gksTp3icmJvz0Wi4qyzb9P3gRlxLarYn7ztNXAv-TH3i-lGk3Ph2j_q5E6iJejQCVRe6p5y_yFb6ausc4eR-kZOXvZYPe-6DmHqMGylt_fxMgdyMXQCPc7dL-QHTzQpjuX0AoEQXNB7k8t_SkN16cVv_LOYndLzeq4L05cokboGuxMaiakpuYUQkh1mlXtYkmdI-GY9xXz7-TKarVk8yN8TqhfV6RftKypCe1n1Bix4PuiFdRRkAwD3uB199XZsD5NbfektwVufGFjfJHd8CzqSnnrLa3h8xgLpkeHgB2NciNT-ska_qEZZoldAX7Tfon2x8ptXDNdA_kz4BfatHvYsF0raLSuq6fhAytj-8RCun61Zw_V-BvLDDVMDYHPtOjkwYoAiCA7GYMBADp31rdI0EjFfN0o5G7gk6y2ExT"

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
