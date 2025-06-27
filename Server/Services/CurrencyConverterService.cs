using System.Text.Json;

namespace BudgetBuddy.Services;

public class CurrencyConverterService {
  private readonly HttpClient _httpClient;
  private readonly Dictionary<string, decimal> _rates = new();

  public CurrencyConverterService(HttpClient httpClient) {
    _httpClient = httpClient;
  }

  public async Task<decimal> GetExchangeRateAsync(string currencyCode) {
    if (currencyCode == "PLN") return 1m;
    if (_rates.TryGetValue(currencyCode, out var rate)) return rate;

    var url = $"https://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json";
    var response = await _httpClient.GetAsync(url);
    response.EnsureSuccessStatusCode();
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<NbpRateResponse>(json);
    rate = data.rates[0].mid;
    _rates[currencyCode] = rate;
    return rate;
  }

  public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency) {
    var fromRate = await GetExchangeRateAsync(fromCurrency);
    var toRate = await GetExchangeRateAsync(toCurrency);
    var amountInPln = amount / fromRate;
    return amountInPln * toRate;
  }
}

public class NbpRateResponse {
  public Rate[] rates { get; set; }
}

public class Rate {
  public decimal mid { get; set; }
}