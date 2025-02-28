namespace ConsoleApp_DD_Scruping;

public class Threads : BaseEntity
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string? MainTitle { get; set; }
    public string? Type { get; set; }
    
    public ICollection<LinkEd2k> LinkEd2Ks { get; } = new List<LinkEd2k>(); // Collection navigation containing dependents
    
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Threads>()
    //         .HasMany(e => e.LinkEd2k)
    //         .WithOne(e => e.Threads)
    //         .HasForeignKey(e => e.ThreadsId)
    //         .IsRequired();
    // }
    
}