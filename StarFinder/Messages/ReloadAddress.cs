namespace StarFinder.Messages
{
    public class ReloadAddress
    {
        private string Path { get; }

        public ReloadAddress(string path)
        {
            Path = path;
        }
    }
}