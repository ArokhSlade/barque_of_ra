using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;

public class SaveDataTest : MonoBehaviour
{
    public int value = 0;

    [SerializeField] TextMeshProUGUI valueLabel;

    public void OnChange()
    {
        value++;
        UpdateLabel();
    }

    void UpdateLabel()
    {
        valueLabel.text = $"{value}";
    }

    public void OnLoad()
    {
        SaveData data = PerformLoad();
        value = data.value;
        UpdateLabel();
    }

    public void OnSave()
    {
        SaveData data = new SaveData(this.value);
        PerformSave(data);
    }

    public SaveData PerformLoad()
    {
        string path = Application.persistentDataPath + "/test_data.dat";

        int value = -1;
        if (File.Exists(path))
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    value = reader.ReadInt32();
                }
            }
        }
        else
        {

            Debug.LogError("Save file not found in " + path);
        }
        SaveData data = new SaveData(value);
        return data;
    }

    public void PerformSave(SaveData data)
    {
        string path = Application.persistentDataPath + "/test_data.dat";

        using (var stream = File.Open(path, FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(data.value);
            }
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public int value;

        public SaveData(int value)
        {
            this.value = value;
        }
    }
}
