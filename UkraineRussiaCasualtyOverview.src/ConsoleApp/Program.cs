﻿using ConsoleApp.Handlers;

// Call the API.
var apiHandler = new ApiHandler("https://russian-casualties.in.ua/api/v1/data/json/daily");
var jsonData = await apiHandler.GetAllDataAsync();

// Format the data properly.
var dataHandler = new DataHandler(jsonData);
var casualtyWrapper = dataHandler.GetCasualtyWrapper();

#region 5 most deadly days
Console.WriteLine("The 5 days with the highest amount of Russian deaths.");
var highestCasualties = casualtyWrapper.Data
    .OrderByDescending(x => x.Value.Personnel)
    .Take(5)
    .ToList();
highestCasualties.ForEach(x =>
{
    Console.WriteLine($"\t[{x.Key.ToString()}] {x.Value.Personnel}");
});
#endregion

#region Average of last 7 days
Console.WriteLine("\nThe average amount of dead Russian personel over the last 7 days.");
int lastSevenDayTotalDeaths = 0;
var lastSevenDays = casualtyWrapper.Data
    .OrderByDescending(x => x.Key)
    .Take(7)
    .ToList();
foreach (var day in lastSevenDays)
{
    lastSevenDayTotalDeaths += day.Value.Personnel;
}
int lastSevenDaysAverage = lastSevenDayTotalDeaths / 7;
Console.WriteLine($"\t{lastSevenDaysAverage}");
#endregion

#region Total Russian deaths
Console.WriteLine("\nTotal amount of Russian personnel killed.");
int totalDeaths = 0;
foreach(var day in casualtyWrapper.Data)
{
    totalDeaths += day.Value.Personnel;
}
Console.WriteLine($"\t{totalDeaths}");
#endregion

#region Days till 100.000
Console.WriteLine("\nDays till 100.000 deaths");
int killsNeeded = 100_000 - totalDeaths;
double daysNeeded = killsNeeded / lastSevenDaysAverage;
Console.WriteLine($"\t{daysNeeded}");
#endregion

#region Casualties in 1 year of war

DateOnly warStartDate = DateOnly.Parse("24.02.2022");
DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
int daysWarActive = currentDate.DayNumber - warStartDate.DayNumber;
int daysInYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
int daysLeftTill1Year = daysInYear - daysWarActive;
int totalAverageDeaths = totalDeaths / daysWarActive;

int totalDeathsAllAverage = totalDeaths + (daysLeftTill1Year * totalAverageDeaths);
int totalDeathsSevenDayAverage = totalDeaths + (daysLeftTill1Year * lastSevenDaysAverage);

Console.WriteLine("\nTotal projected casualties for 1 year of war: ");
Console.WriteLine($"\tUsing total average deaths: {totalDeathsAllAverage}");
Console.WriteLine($"\tUsing last 7 day average deaths: {totalDeathsSevenDayAverage}");

#endregion