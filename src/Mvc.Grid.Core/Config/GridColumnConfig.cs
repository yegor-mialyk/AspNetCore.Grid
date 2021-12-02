namespace NonFactors.Mvc.Grid;

public class GridColumnConfig
{
    public String Name { get; set; }
    public String Width { get; set; }
    public Boolean Hidden { get; set; }

    public GridColumnConfig()
    {
        Name = "";
        Width = "";
    }
}
