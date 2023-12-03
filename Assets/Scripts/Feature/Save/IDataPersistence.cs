namespace Save
{
    public interface IDataPersistence
    {
        bool WriteData<T>(string relativePath, T data, bool encrypted);
        T ReadData<T>(string relativePath, bool encrypted);
        bool CheckExists(string relativePath);
    }
}