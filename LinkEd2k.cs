namespace ConsoleApp_DD_Scruping;

public class LinkEd2k : BaseEntity
{
    public int Id { get; set; }
    public string Ed2kLink { get; set; }
    public string Title { get; set; }

    public int ThreadsId { get; set; } // Required foreign key property
    public Threads Threads { get; set; }  // Required reference navigation to principal
}