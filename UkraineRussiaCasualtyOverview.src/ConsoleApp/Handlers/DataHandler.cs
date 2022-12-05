using ConsoleApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ConsoleApp.Handlers;

internal class DataHandler
{
    private string _jsonData;

	public DataHandler(string jsonData)
	{
		_jsonData = jsonData;
	}

	public CasualtyWrapper GetCasualtyWrapper() 
	{
		// Convert JSON to JObject.
		JObject jobject = JObject.Parse(_jsonData);

		// Get the legend data stored.
		var legendModel = GetLegendData(jobject["legend"]);

        // Go through the data list.
        var dataDictionary = GetCasualtyData(jobject["data"]);

		return new CasualtyWrapper 
        {
            Legend = legendModel,
            Data = dataDictionary
        };
    }

	private LegendModel GetLegendData(JToken legend)
	{
		// Format JSON.
        StringBuilder builder = new StringBuilder("{\n");
        foreach (JProperty item in legend)
        {
            string name = item.Name;
            string value = legend[item.Name].ToString();

            builder.Append($"\t\"{name}\": \"{value}\", \n");
        }
		builder.Length -= 3;
        builder.Append("}");

		// Parse JSON.
		string json = builder.ToString();
        var result = System.Text.Json.JsonSerializer.Deserialize<LegendModel>(json);

		return result;
    }

	private Dictionary<DateOnly, CasualtyModel> GetCasualtyData(JToken data)
	{
        Dictionary<DateOnly, CasualtyModel> result = new();

        foreach (JProperty item in data)
        {
            string name = item.Name;

            foreach (var child in item.Children())
            {
                string jsonData = child.ToString();
                var casualties = System.Text.Json.JsonSerializer.Deserialize<CasualtyModel>(jsonData);

                result.Add(DateOnly.Parse(name), casualties);
            }
        }

        return result;
    }
}
