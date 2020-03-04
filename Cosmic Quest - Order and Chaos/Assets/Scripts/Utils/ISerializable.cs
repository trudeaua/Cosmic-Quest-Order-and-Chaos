/// <summary>
/// Interface for serializable objects whose state should be included in save and load.
/// </summary>
public interface ISerializable
{
    /// <summary>
    /// Abstract function for serializing class data to a JSON string.
    /// </summary>
    /// <returns>Serialized JSON string of the class data</returns>
    string Serialize();

    /// <summary>
    /// Initializes a class from a serialized string of data.
    /// </summary>
    /// <param name="s">String of JSON encoded class data</param>
    void FromSerialized(string s);
}