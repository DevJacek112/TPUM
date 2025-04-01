namespace Logic;

public class Program
{
    static void Main(string[] args)
    {
        string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images/lodka.jpeg");
        Console.WriteLine($"Pełna ścieżka: {imagePath}");
        Console.WriteLine($"Czy plik istnieje? {System.IO.File.Exists(imagePath)}");
    }
}