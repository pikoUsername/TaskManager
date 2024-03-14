namespace TaskManager.Database.Models; 

public enum DayTypes
{
    //
    // ������:
    //     Indicates Sunday.
    Sunday = 0,
    //
    // ������:
    //     Indicates Monday.
    Monday = 1,
    //
    // ������:
    //     Indicates Tuesday.
    Tuesday = 2,
    //
    // ������:
    //     Indicates Wednesday.
    Wednesday = 3,
    //
    // ������:
    //     Indicates Thursday.
    Thursday = 4,
    //
    // ������:
    //     Indicates Friday.
    Friday = 5,
    //
    // ������:
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
        throw new Exception("Нельзя"); 
    }
}
