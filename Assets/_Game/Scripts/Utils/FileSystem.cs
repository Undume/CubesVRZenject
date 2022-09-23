using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SharedUtils
{
    public static class FileSystem
    {
        private const string k_Extension = ".shotboxes";

        //LoadData any file to a class, make sure the class is marked with [System.Serializable]
        public static T LoadFile<T>(string filename) where T : class
        {
            T data = null;
            var path = $"{Application.persistentDataPath}/{filename}{k_Extension}";
            if (IsValidFilename(filename) && File.Exists(path))
            {
                FileStream file = null;
                try
                {
                    var bf = new BinaryFormatter();
                    file = File.Open(path, FileMode.Open);
                    data = (T) bf.Deserialize(file);
                    file.Close();
                }
                catch
                {
                    file?.Close();
                }
            }

            return data;
        }

        //Save any class to a file, make sure the class is marked with [System.Serializable]
        public static void SaveFile<T>(string filename, T data) where T : class
        {
            if (IsValidFilename(filename))
            {
                FileStream file = null;
                try
                {
                    var bf = new BinaryFormatter();
                    var fullpath = $"{Application.persistentDataPath}/{filename}{k_Extension}";
                    file = File.Create(fullpath);
                    bf.Serialize(file, data);
                    file.Close();
                }
                catch
                {
                    if (file != null) file.Close();
                }
            }
        }

        public static void DeleteFile(string filename)
        {
            var fullpath = Application.persistentDataPath + "/" + filename + k_Extension;
            if (File.Exists(fullpath))
                File.Delete(fullpath);
        }

        //Return all save files
        public static List<string> GetAllSave()
        {
            List<string> saves = new List<string>();
            string[] files = Directory.GetFiles(Application.persistentDataPath);
            foreach (string file in files)
            {
                if (file.EndsWith(k_Extension))
                {
                    string filename = Path.GetFileName(file).Split('.')[0];
                    if (!saves.Contains(filename))
                        saves.Add(filename);
                }
            }

            return saves;
        }

        public static bool DoesFileExist(string filename)
        {
            string fullpath = Application.persistentDataPath + "/" + filename + k_Extension;
            return IsValidFilename(filename) && File.Exists(fullpath);
        }

        public static bool IsValidFilename(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return false; //Filename cant be blank

            if (filename.Contains("."))
                return false; //Dont allow dot as they are for extensions savefile

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (filename.Contains(c.ToString()))
                    return false; //Dont allow any special characters
            }

            return true;
        }
    }
}