using Cooking;
using Cooking.Recipe;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Save
{
    public class DataPersistenceManager : IDataPersistence
    {
        private const string KEY = "w5QEGQhgTHRNb+hy6JR+gyEMGhXqbXzHjCX/5ZQOeUM=";
        private const string IV = "d1zi3fFoHPuCu5/OyrjQrw==";

        public bool WriteData<T>(string relativePath, T data, bool isEncrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            try
            {
                if (File.Exists(path))
                {
                    Debug.Log("Data exists. Replacing old file...");
                    File.Delete(path);
                }

                using FileStream stream = File.Create(path);

                // Settings
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new DictionaryAsArrayResolver();

                if (isEncrypted)
                {
                    WriteEncryptedData(data, stream, settings);
                    return true;
                }
                
                stream.Close();

                File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented, settings));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to save data: {e.Message} {e.StackTrace} ");
                return false;
            }
        }

        private void WriteEncryptedData<T>(T data, FileStream stream, JsonSerializerSettings settings)
        {
            using Aes aesProvider = Aes.Create();
            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);
            using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
            using CryptoStream cryptoStream = new CryptoStream(
                stream,
                cryptoTransform,
                CryptoStreamMode.Write
            );

            cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data, Formatting.Indented, settings)));
        }

        public T ReadData<T>(string relativePath, bool isEncrypted)
        {
            string path = Application.persistentDataPath + relativePath;

            if(!File.Exists(path))
            {
                Debug.Log($"Cannot load file at {path}. File does not exist!");
                throw new FileNotFoundException($"{path} does not exist.");
            }

            try
            {
                // Settings
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new DictionaryAsArrayResolver();
                settings.Converters.Add(new SOConverter<Recipe>());
                settings.Converters.Add(new SOConverter<Ingredient>());

                T data;
                if (isEncrypted)
                {
                    data = ReadEncryptedData<T>(path, settings);
                    return data;
                }

                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), settings);

                return data;
            }
            catch(Exception e)
            {
                Debug.LogError($"Failed to load data: {e.Message} {e.StackTrace} ");
                throw e;
            }
        }

        private T ReadEncryptedData<T>(string path, JsonSerializerSettings settings)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            using Aes aesProvider = Aes.Create();

            aesProvider.Key = Convert.FromBase64String(KEY);
            aesProvider.IV = Convert.FromBase64String(IV);

            using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
                aesProvider.Key,
                aesProvider.IV
            );
            using MemoryStream decryptionStream = new MemoryStream(fileBytes);
            using CryptoStream cryptoStream = new CryptoStream(
                decryptionStream,
                cryptoTransform,
                CryptoStreamMode.Read
            );
            using StreamReader reader = new StreamReader(cryptoStream);

            string result = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(result, settings);
        }
    }
}