using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PhoneKit.Framework.Storage
{
    /// <summary>
    /// Helper class for accessing the isolated storage more easy.
    /// </summary>
    public static class StorageHelper
    {
        #region Members

        /// <summary>
        /// The isolated storage scheme.
        /// </summary>
        public const string ISTORAGE_SCHEME = "isostore://";

        /// <summary>
        /// The local resource scheme.
        /// </summary>
        public const string APPX_SCHEME = "ms-appx://";

        /// <summary>
        /// The local appdata scheme.
        /// </summary>
        public const string APPDATA_LOCAL_SCHEME = "ms-appdata:///local";

        #endregion

        #region Isolated Storage Settings

        /// <summary>
        /// Saves a value in the isolated storage settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to store.</param>
        /// <returns>Returns whether the value has been saved.</returns>
        public static bool SaveValue(string key, object value)
        {
            // validate data
            if (value == null)
                return false;

            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(key))
                store[key] = value;
            else
                store.Add(key, value);

            store.Save();

            return true;
        }

        /// <summary>
        /// Loads a value from isolated storage settings.
        /// </summary>
        /// <typeparam name="T">The type to load.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The loaded value or the types default value.</returns>
        public static T LoadValue<T>(string key)
        {
            return LoadValue<T>(key, default(T));
        }

        /// <summary>
        /// Loads a value from isolated storage settings.
        /// </summary>
        /// <typeparam name="T">The type to load.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value, if no value was stored.</param>
        /// <returns>Returns the loaded value or the default value.</returns>
        public static T LoadValue<T>(string key, T defaultValue)
        {
            var store = IsolatedStorageSettings.ApplicationSettings;
            if (!store.Contains(key))
                return defaultValue;

            return (T)store[key];
        }

        /// <summary>
        /// Verifies whether the an value in the isolated storage settings.
        /// for the given key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns true if the key exists, else false.</returns>
        public static bool ValueExists(string key)
        {
            var store = IsolatedStorageSettings.ApplicationSettings;
            return store.Contains(key);
        }

        /// <summary>
        /// Deletes a value from isolated storage settings.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void DeleteValue(string key)
        {
            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(key))
                store.Remove(key);
            store.Save();
        }

        #endregion

        #region Isolated Storage File

        /// <summary>
        /// Saves an object as an XML serialized value into isolated storage.
        /// </summary>
        /// <typeparam name="T">The data type to store.</typeparam>
        /// <param name="path">The file path.</param>
        /// <param name="data">The data to store.</param>
        /// <returns>Returns true, if the value has been saved, else false.</returns>
        public static bool SaveFile<T>(string path, T data)
        {
            // validate data
            if (data == null)
                return false;

            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // verify directory exists
                    string directory = Path.GetDirectoryName(path);
                    if (!store.DirectoryExists(directory))
                    {
                        store.CreateDirectory(directory);
                    }

                    using (IsolatedStorageFileStream fileStream = store.OpenFile(path, FileMode.Create, FileAccess.Write))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(fileStream, data);
                        return true;
                    }
                }
            }
            catch (Exception) { }

            return false;
        }

        /// <summary>
        /// Saves a file from stream.
        /// </summary>
        /// <param name="path">The local path to store the file.</param>
        /// <param name="stream">The source file stream.</param>
        public static bool SaveFileFromStream(string path, Stream stream)
        {
            using (var responseStream = stream)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        // verify directory exists
                        string directory = Path.GetDirectoryName(path);
                        if (!store.DirectoryExists(directory))
                        {
                            store.CreateDirectory(directory);
                        }

                        using (var isoStoreFile = store.OpenFile(path,
                            FileMode.Create,
                            FileAccess.ReadWrite))
                        {
                            // store loaded data in isolated storage
                            var dataBuffer = new byte[1024];
                            while (responseStream.Read(dataBuffer, 0, dataBuffer.Length) > 0)
                            {
                                isoStoreFile.Write(dataBuffer, 0, dataBuffer.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Saving the downloaded file failed with error: " + ex.Message);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Saves an JPEG image to isolated storage.
        /// </summary>
        /// <param name="path">The full image path of the JPEG image.</param>
        /// <param name="image">The image to save.</param>
        /// <returns>Returns the image URI in isolated storage when successful, else null.</returns>
        public static Uri SaveJpeg(string path, WriteableBitmap image)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(path, FileMode.Create, store))
            {
                try
                {
                    image.SaveJpeg(fileStream, image.PixelWidth, image.PixelHeight, 0, 100);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Saving jpeg image failed with error: " + ex.Message);
                    return null;
                }
            }

            return new Uri(ISTORAGE_SCHEME + path, UriKind.Absolute);
        }

        /// <summary>
        /// Loads the file data from isolated storage.
        /// </summary>
        /// <typeparam name="T">The data type to store.</typeparam>
        /// <param name="path">The files path.</param>
        /// <returns>The loaded data or its types default value.</returns>
        public static T LoadFile<T>(string path)
        {
            return LoadFile<T>(path, default(T));
        }

        /// <summary>
        /// Loads the file data from isolated storage.
        /// </summary>
        /// <typeparam name="T">The data type to store.</typeparam>
        /// <param name="path">The files path.</param>
        /// <param name="defaultValue">The default value, if no value was stored.</param>
        /// <returns>The loaded data or the default value.</returns>
        public static T LoadFile<T>(string path, T defaultValue)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(path))
                    return defaultValue;

                using (IsolatedStorageFileStream fileStream = store.OpenFile(path, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(fileStream);
                }
            }
        }

        /// <summary>
        /// Verifies whether the file exists.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>Returns true if the file exists, else false.</returns>
        public static bool FileExists(string path)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return store.FileExists(path);
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The file path.</param>
        public static void DeleteFile(string path)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(path))
                    store.DeleteFile(path);
            }
        }

        /// <summary>
        /// Verifies whether the directory exists.
        /// </summary>
        /// <param name="path">The file or directory path.</param>
        /// <returns>Returns true if the directory exists, else false.</returns>
        public static bool DirectoryExists(string path)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string directory = Path.GetDirectoryName(path);
                return store.DirectoryExists(directory);
            }
        }

        /// <summary>
        /// Deletes a directory and its files.
        /// </summary>
        /// <param name="path">The directory or file path.</param>
        public static void DeleteDirectory(string path)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string directory = Path.GetDirectoryName(path);
                if (store.DirectoryExists(directory))
                {
                    store.DeleteDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Deletes the foldes containing files.
        /// </summary>
        /// <param name="path">The file or directory path.</param>
        public static void TryDeleteAllDirectoryFiles(string path)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string directory = Path.GetDirectoryName(path);
                if (store.DirectoryExists(directory))
                {
                    try
                    {
                        string[] fileNames = store.GetFileNames(path);

                        foreach (var fileName in fileNames)
                        {
                            string fullFilePath = path + fileName;
                            if (FileExists(fullFilePath))
                            {
                                store.DeleteFile(fullFilePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Deleting all files failed. One file might be in use. Error: " + ex.Message);
                    }
                }
            }
        }

        #endregion
    }
}
