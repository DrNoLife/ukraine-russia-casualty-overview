using ConsoleApp.Handlers;

// Call the API.
var apiHandler = new ApiHandler("https://russian-casualties.in.ua/api/v1/data/json/daily");
var jsonData = await apiHandler.GetAllDataAsync();

// Format the data properly.
var dataHandler = new DataHandler(jsonData);
var casualtyWrapper = dataHandler.GetCasualtyWrapper();

#region Death count today
int casualtyCountToday;
DateOnly today;

try
{
    today = DateOnly.FromDateTime(DateTime.Now);
    casualtyCountToday  = casualtyWrapper.Data[today].Personnel;
}
catch(Exception ex)
{
    today = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
    casualtyCountToday = casualtyWrapper.Data[today].Personnel;
}

Console.WriteLine($"Russian casualty count today ({today})");
Console.WriteLine($"\tSoldiers: {casualtyCountToday}");
#endregion

#region 5 most deadly days
Console.WriteLine("\nThe 5 days with the highest amount of Russian deaths.");
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
int daysNumber = 7;
Console.WriteLine($"\nThe average amount of dead Russian personel over the last {daysNumber} days.");
int lastSevenDayTotalDeaths = 0;
var lastSevenDays = casualtyWrapper.Data
    .OrderByDescending(x => x.Key)
    .Take(daysNumber)
    .ToList();
foreach (var day in lastSevenDays)
{
    lastSevenDayTotalDeaths += day.Value.Personnel;
}
int lastSevenDaysAverage = lastSevenDayTotalDeaths / daysNumber;
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
// Get the dates we need.
DateOnly warStartDate = DateOnly.Parse("24.02.2022");
DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

// Find out how many more days the war needs to continue till we reach 1 full year.
int daysWarActive = currentDate.DayNumber - warStartDate.DayNumber;
int daysInYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
int daysLeftTill1Year = daysInYear - daysWarActive;

// Handle getting the average for total deaths.
int totalAverageDeaths = totalDeaths / daysWarActive;

// Extrapolating.
int totalDeathsAllAverage = totalDeaths + (daysLeftTill1Year * totalAverageDeaths);
int totalDeathsSevenDayAverage = totalDeaths + (daysLeftTill1Year * lastSevenDaysAverage);

Console.WriteLine("\nTotal projected casualties for 1 year of war: ");
Console.WriteLine($"\tUsing total average deaths: {totalDeathsAllAverage}");
Console.WriteLine($"\tUsing last 7 day average deaths: {totalDeathsSevenDayAverage}");

#endregion

#region Deadliest month for Russia

// Create array of months, and fill it with deaths.
int[] monthlyCasualties = new int[12];
foreach(var item in casualtyWrapper.Data)
{
    int currentMonth = item.Key.Month - 1;
    monthlyCasualties[currentMonth] += item.Value.Personnel; 
}

// Now get some data for the best month.
int deadliestMonthIndex = monthlyCasualties.ToList().IndexOf(monthlyCasualties.Max());
int deadliestMonthBodycount = monthlyCasualties.Max();
string monthName = deadliestMonthIndex switch
{
    0 => "January",
    1 => "Febuary",
    2 => "March",
    3 => "April",
    4 => "May",
    5 => "June",
    6 => "July",
    7 => "August",
    8 => "September",
    9 => "October",
    10 => "November",
    11 => "December",
    _ => "Unknown month"
};

int daysInDeadliestMonth = DateTime.DaysInMonth(2022, deadliestMonthIndex + 1);
int deadliestMonthAverage = deadliestMonthBodycount / daysInDeadliestMonth;

Console.WriteLine("\nThe deadliest month for Russia was: ");
Console.WriteLine($"\t{monthName} with a total of {deadliestMonthBodycount} deaths.");
Console.WriteLine($"\t{monthName} had a daily average of {deadliestMonthAverage} dead Russian soldiers.");

#endregion

#region Daily average required for 100.000 at christmas

DateOnly christmasDate = DateOnly.Parse("25.12.2022");
int daysTillChristmas = christmasDate.DayNumber - currentDate.DayNumber;
int deathsTill100k = 100_000 - totalDeaths;

Console.WriteLine("\nIn order to reach 100.000 dead Russian soldiers before christmas (december 25th) we need: ");

if(deathsTill100k > 0)
{
    int averageRequired = deathsTill100k / daysTillChristmas;
    Console.WriteLine($"\tAn average of {averageRequired} dead Russian soldiers each day, for the next {daysTillChristmas} days.");
} else
{
    Console.WriteLine("We've hit over 100.000 deaths.");
}

#endregion