namespace Orc.Snapshots
{
    using System;
    using System.Threading.Tasks;

    public interface ISnapshot 
    {
        string Title { get; set; }
        string Category { get; set; }

        string[] Keys { get; }
        DateTime Created { get; set; }

        Task<string> GetContentHashAsync();
        Task InitializeFromBytesAsync(byte[] bytes);
        Task<byte[]> GetAllBytesAsync();
        byte[] GetData(string key);
        void SetData(string key, byte[] data);
        void ClearData(string key);
    }
}
