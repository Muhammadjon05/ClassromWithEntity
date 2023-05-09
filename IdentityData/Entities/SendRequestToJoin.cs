namespace IdentityData.Entities;

public class SendRequestToJoin
{
    public Guid Id { get; set; }

    public Guid FromWhomId { get; set; }
    public User FromWhom { get; set; }
    
    public Guid ScienceId { get; set; }
    public Science Science { get; set; }
    
    public Guid ToUserId  { get; set; }
    
    public bool IsJoined { get; set; }
}