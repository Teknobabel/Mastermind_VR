public class Condition
{
    public enum Operation 
    {
        None,
        EqualTo,
        GreaterThan,
        LessThan,
        Boolean,
        GainItem,
        LoadEvent,
    }
    public Operation operation { get; set; }
    public int value { get; set; }

}
