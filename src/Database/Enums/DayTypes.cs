namespace TaskManager.Database.Models; 

public enum DayTypes
{
    //
    // Сводка:
    //     Indicates Sunday.
    Sunday = 0,
    //
    // Сводка:
    //     Indicates Monday.
    Monday = 1,
    //
    // Сводка:
    //     Indicates Tuesday.
    Tuesday = 2,
    //
    // Сводка:
    //     Indicates Wednesday.
    Wednesday = 3,
    //
    // Сводка:
    //     Indicates Thursday.
    Thursday = 4,
    //
    // Сводка:
    //     Indicates Friday.
    Friday = 5,
    //
    // Сводка:
    //     Indicates Saturday.
    Saturday = 6, 
    All = 7, 
}
public class DayTypesService
{
    static public DayTypes FromSTDWeekDay(DayOfWeek day)
    {
        foreach (string name in Enum.GetNames(typeof(DayTypes)))
        {
            if (name == day.ToString()) {
                return (DayTypes)Enum.Parse(typeof(DayTypes), name);    
            }
        }
        throw new Exception("Не правильный тип данных"); 
    }
}
