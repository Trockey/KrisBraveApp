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
COOKIE_HEADER="Cookie: .AspNetCore.Cookies=CfDJ8I_MNsn5DBtCmai4DLg7e07QVpdd0CtQFlqnyE3tVCdJshRq8Yb-73po2P_1KbSt3I8x-TWTw4sFjpGBYNS8kVQX2WltEoWbs-St0YjW6X4csRiQdFCPdGLVTZVbKULU4UBWv9AE1kZzyjmkqItZEOmSK4a-ugzUkEGo66gzjCTY0lD7re1FFG2lXdF_RUMIhN4NHOXnQuxGzaKethvLyW8nfM0ziUU7mMT0SlVJMKXU63Er3T7lACZdXuNB_MyL2aUJTCXgUOWbHPNuD4WCmR1Yv7vISGFqaXkkq9kE9vYYFnuyFTr1EMdHBFadBpoUfZlnIcTEH1Kc_hhdm_w4Pn8xVGXH2aDHp45xaUZNcDjEh7_EKtRFNAv-WmTlRXFGRlDtdmSpaR3nsLt962j2ghQ8YYU7ePTM9KLM_pKBOIbW_xsXsxnHuOMhlYWGL1YW6ELdUPzPjCQ-YNNsetZk_vGDflBh4k0Oyqm8mKaR9Ge49Kt9p5T4j2ujhYZPLqOSBn1p7tTBPJ84fHQiBKO8k4PfUbJL5ClJl96H3YWJm3xUjOF8ts70-7UiYBFarT9ahFPIgNpe4ig4MdFd_mfszhn4k83b7tRYMcc7Yr3qhuxjrCnieiPyjl7Rjqoz4UTmTeZPN9pkp1qhcJ1fRk6Qh10XRxQoDddXh6DrGDnOaij-uHauVUnOBPPqyJL5YND7nolZQT4iWeMxqbXY-bRf_WOWY8pb19zM4-pGXAReXl_NAly4WhLEdxWkbchRhA1XX7GA3lYUBpXKkbg5YZuLxpR5SASw2GQtmroUvNY5FjvsiREUswO66ypjgcupfg_weohsZCWJ2LDgImDjFraSMlVDJyfj9hNssJGChD6Er_KFQ6_KtajnXQv1uVb_REZlxyKJXEwYNK-pG_Q2U2HewtQo5Jxps02QLbJUFqXZ8wYcywSyGsbW6IwfRjZ1kgVJ02ByWf2ujXdpe0oN9eN8zVoDQt8k5zajaBQIzGAMLc4I3BtjAe21IYwa9wP9PEfvLkQ6jLzVhbhCMJo_JjLTxF7b7Dzs2sn4JkKIy31TBZp7AL1lL_BRtT9FVVCF0hhRS985aQaUfcbUbyjK0HUq1EM-Uqa9s5Pe3DrOfAun6ywQfWQF1C57JhqNYYQYbQez5aRASMlUio1C3sq2lN_rt77W60xXOffVcRtzxOYc1wK1EBdMROpunBTVntv9eTtCtULrk4GfIbORYj-mnJN5_7QlGCOE0RugDZDI6ySF6G_PlR5z2BHGvR-1dOa8J4aLfYLlFWthuTsBHY4t9L4qL9EB_jCVGkWhhbA2A3Twso3ERZcaEMBhef0LoV8AD5vQKiLLPRWFou2s-FKUqn10vpQ_3upMH0ic1D69R-DyxdLwfzwhvgqvL2yjgE5H1K36seDA1QPygrGFxNIKyiiSXVe_sCnsmb_RpAA6BgGik_p5jb3HKFOaS0iJbr5rR7qMEBGviyJ_bXtxr8EC1WTdNXk"

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
