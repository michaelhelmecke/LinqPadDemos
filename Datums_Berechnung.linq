void Main()
{
	
	 
	int Kalenderwoche =  GetIso8601WeekOfYear(Convert.ToDateTime("31.12.2019"));
	Kalenderwoche.Dump("Nach ISO8601");
	
	int KalenderwocheKW = KW(Convert.ToDateTime("31.12.2019"));
	KalenderwocheKW.Dump("Nach .Net");

	DateTime FirstDayOfWeek = GetFirstDayOfWeek(Convert.ToDateTime("31.12.2019"));
	FirstDayOfWeek.Dump("FirstDayOfWeek");

	DateTime LastDayOfWeek = GetLastDayOfWeek(Convert.ToDateTime("31.12.2019"));
	LastDayOfWeek.Dump("LastDayOfWeek");
	
	DateTime FirstDayOfMonth = GetFirstDayOfMonth(Convert.ToDateTime("31.12.2019"));
	FirstDayOfMonth.Dump("FirstDayOfMonth");

	DateTime TheLastDayOfMonth = GetTheLastDayOfMonth(Convert.ToDateTime("01.02.2020"));
	TheLastDayOfMonth.Dump("TheLastDayOfMonth");
	String.Format("{0:D}", TheLastDayOfMonth).Dump();
}

public static int GetIso8601WeekOfYear(DateTime time)
{
	// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
	// be the same week# as whatever Thursday, Friday or Saturday are,
	// and we always get those right
	DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
	if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
	{
		time = time.AddDays(3);
	}

	// Return the week of our adjusted day
	return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
}

public static int KW(DateTime Datum)
{
	CultureInfo CUI = CultureInfo.CurrentCulture;
	return CUI.Calendar.GetWeekOfYear(Datum, CUI.DateTimeFormat.CalendarWeekRule, CUI.DateTimeFormat.FirstDayOfWeek);
}

public static DateTime GetFirstDayOfWeek(DateTime dateTime)
{
	while (dateTime.DayOfWeek != DayOfWeek.Monday)
		dateTime = dateTime.Subtract(new TimeSpan(1, 0, 0, 0));
	return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
}

public static DateTime GetLastDayOfWeek(DateTime dateTime)
{
	while (dateTime.DayOfWeek != DayOfWeek.Sunday)
		dateTime = dateTime.AddDays(1);
	return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
}

public static DateTime GetFirstDayOfMonth(DateTime givenDate)
{
	return new DateTime(givenDate.Year, givenDate.Month, 1);
}

public static DateTime GetTheLastDayOfMonth(DateTime givenDate)
{
	return GetFirstDayOfMonth(givenDate).AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0, 0));
}
