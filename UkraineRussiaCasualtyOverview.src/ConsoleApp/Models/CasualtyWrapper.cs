namespace ConsoleApp.Models;

internal class CasualtyWrapper
{
    public LegendModel Legend { get; set; }
    public Dictionary<DateOnly, CasualtyModel> Data { get; set; }

    public CasualtyWrapper()
    {
        Data = new();
    }
}
