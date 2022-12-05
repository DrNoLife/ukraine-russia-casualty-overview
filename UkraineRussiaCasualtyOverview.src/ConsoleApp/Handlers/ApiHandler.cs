namespace ConsoleApp.Handlers;

internal class ApiHandler
{
    private string _apiUrl;
	private HttpClient _httpClient;

	public ApiHandler(string apiUrl)
	{
		_apiUrl= apiUrl;
		_httpClient = new HttpClient()
		{
			BaseAddress = new Uri(apiUrl)
		};
	}

	public async Task<string> GetAllDataAsync()
	{
		HttpRequestMessage request = new(HttpMethod.Get, _apiUrl);
		HttpResponseMessage response = await _httpClient.SendAsync(request);

		if(!response.IsSuccessStatusCode)
		{
			throw new Exception("Failure @ API.");
		}

		return await response.Content.ReadAsStringAsync();
	}
}
