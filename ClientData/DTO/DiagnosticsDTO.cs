namespace ClientData.DTO;

public class DiagnosticsDTO
{
    public int serverTimeOnline { get; set; }
    public int numberOfAllBoats { get; set; }
    public int numberOfActiveClients { get; set; }

    public DiagnosticsDTO(int serverTimeOnline, int numberOfAllBoats, int numberOfActiveClients)
    {
        this.serverTimeOnline = serverTimeOnline;
        this.numberOfAllBoats = numberOfAllBoats;
        this.numberOfActiveClients = numberOfActiveClients;
    }
}