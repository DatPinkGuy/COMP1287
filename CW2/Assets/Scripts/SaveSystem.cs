using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   public static void SaveSettings(SettingsMain settings)
   {
      var formatter = new BinaryFormatter();
      var path = Application.persistentDataPath + "/settings";
      var fileStream = new FileStream(path, FileMode.Create);
      var data = new SaveData(settings);
      formatter.Serialize(fileStream, data);
      fileStream.Close();
   }

   public static SaveData LoadSettings()
   {
      var path = Application.persistentDataPath + "/settings";
      if (File.Exists(path))
      {
         var formatter = new BinaryFormatter();
         var fileStream = new FileStream(path, FileMode.Open);
         if (fileStream.Length == 0)
         {
            fileStream.Dispose();
            return null;
         }
         var data = (SaveData) formatter.Deserialize(fileStream);
         fileStream.Close();
         return data;
      }
      else
      {
         Debug.Log("Save file for settings not found");
         return null;
      }
   }

   public static void SaveCurrency(Watch watch)
   {
      var formatter = new BinaryFormatter();
      var path = Application.persistentDataPath + "/currency";
      var fileStream = new FileStream(path, FileMode.Create);
      var data = new SaveData(watch);
      formatter.Serialize(fileStream, data);
      fileStream.Close();
   }
   
   public static SaveData LoadCurrency()
   {
      var path = Application.persistentDataPath + "/currency";
      if (File.Exists(path))
      {
         var formatter = new BinaryFormatter();
         var fileStream = new FileStream(path, FileMode.Open);
         if (fileStream.Length == 0)
         {
            fileStream.Dispose();
            return null;
         }
         var data = (SaveData) formatter.Deserialize(fileStream);
         fileStream.Close();
         return data;
      }
      else
      {
         Debug.Log("Save file for currency not found");
         return null;
      }
   }

   public static void ClearSaveData()
   {
      if (File.Exists(Application.persistentDataPath + "/settings"))
      {
         var fileStream = File.Open(Application.persistentDataPath + "/settings", FileMode.Open);
         fileStream.SetLength(0);
         fileStream.Close();
      }
      if (File.Exists(Application.persistentDataPath + "/currency"))
      {
         var fileStream = File.Open(Application.persistentDataPath + "/currency", FileMode.Open);
         fileStream.SetLength(0);
         fileStream.Close();
      }
      
   }
}
