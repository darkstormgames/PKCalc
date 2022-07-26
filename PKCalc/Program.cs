namespace PKCalc
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            App app = new()
            {
                CLArgs = args
            };
            app.Run();
        }
    }
}