namespace TaskManager.Database.Models; 

public class DayTimeTableTypes
{
    public static readonly string work = "work";
    public static readonly string weekend = "weekend";
    public static readonly string general = "general";
}

public class DayTimeTableSubTypes
{
    public static readonly string breakTime = "break";
    public static readonly string work = "weekend";
    public static readonly string workPrep = "workPreperation";
    public static readonly string workEnd = "workEnd";
}
