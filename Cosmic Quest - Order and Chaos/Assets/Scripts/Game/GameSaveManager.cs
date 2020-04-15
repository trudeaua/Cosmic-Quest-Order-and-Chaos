using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    #region Singleton
    public static GameSaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Only one game save manager should be in the scene!");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public ChaosVoid[] chaosVoids;
    public int CurrentSlot { get; private set; }

    private void Start()
    {
        CurrentSlot = -1;
    }

    /// <summary>
    /// Save a game in a specific slot
    /// </summary>
    /// <param name="slot">Slot to save the game to</param>
    public void SaveGame(int slot)
    {
        if (!IsSaveFile(slot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + slot);
        }
        if (!Directory.Exists(Application.persistentDataPath + "/saves/" + slot + "/levels"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + slot + "/levels");
        }
        CurrentSlot = slot;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        for (int i = 0; i < chaosVoids.Length; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath + "/saves/" + slot + "/levels/level" + i + "_save");
            var json = JsonUtility.ToJson(chaosVoids[i]);
            binaryFormatter.Serialize(file, json);
            file.Close();
        }
    }

    /// <summary>
    /// Save a game to the current slot
    /// </summary>
    /// <param name="slot">Slot to save the game to</param>
    public void SaveGame()
    {
        if (!IsSaveFile(CurrentSlot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + CurrentSlot);
        }
        if (!Directory.Exists(Application.persistentDataPath + "/saves/" + CurrentSlot + "/levels"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + CurrentSlot + "/levels");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        for (int i = 0; i < chaosVoids.Length; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath + "/saves/" + CurrentSlot + "/levels/level" + i + "_save");
            var json = JsonUtility.ToJson(chaosVoids[i]);
            binaryFormatter.Serialize(file, json);
            file.Close();
        }
    }

    /// <summary>
    /// Checks if the save file exists
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool IsSaveFile(int slot)
    {
        return Directory.Exists(Application.persistentDataPath + "/saves/" + slot);
    }

    /// <summary>
    /// Load a save game file
    /// </summary>
    /// <param name="slot">Slot to load</param>
    public void LoadGame(int slot)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/" + slot + "/levels"))
        {
            return;
        }
        CurrentSlot = slot;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        for (int i = 0; i < chaosVoids.Length; i++)
        {
            FileStream file = File.Open(Application.persistentDataPath + "/saves/" + slot + "/levels/level" + i + "_save", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)binaryFormatter.Deserialize(file), chaosVoids[i]);
            file.Close();
        }
    }

    /// <summary>
    /// Start a new game in a specific slot
    /// </summary>
    /// <param name="slot">Slot to start a new game in</param>
    public void NewGame(int slot)
    {
        if (!IsSaveFile(slot))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + slot);
        }
        if (!Directory.Exists(Application.persistentDataPath + "/saves/" + slot + "/levels"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + slot + "/levels");
        }
        for (int i = 0; i < chaosVoids.Length; i++)
        {
            File.Delete(Application.persistentDataPath + "/saves/" + slot + "/levels/level" + i + "_save");
            chaosVoids[i].Reset();
        }
        SaveGame(slot);
    }

    /// <summary>
    /// Return a number representing how complete the game is
    /// </summary>
    /// <param name="slot">Slot to check</param>
    public float GetCompletion(int slot)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/" + slot + "/levels"))
        {
            return 0;
        }
        float percentComplete = 0;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        for (int i = 0; i < chaosVoids.Length; i++)
        {
            FileStream file = File.Open(Application.persistentDataPath + "/saves/" + slot + "/levels/level" + i + "_save", FileMode.Open);
            ChaosVoid chaosVoid = ScriptableObject.CreateInstance<ChaosVoid>();
            JsonUtility.FromJsonOverwrite((string)binaryFormatter.Deserialize(file), chaosVoid);
            if (chaosVoid.cleared)
                percentComplete += 1f / chaosVoids.Length;
            file.Close();
        }
        return percentComplete;
    }
}
