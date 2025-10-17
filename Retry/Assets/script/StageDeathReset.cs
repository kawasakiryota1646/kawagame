using UnityEngine;
using UnityEngine.SceneManagement;

public class StageDeathReset : MonoBehaviour
{
    void Start()
    {
        string key = "HasReset_" + SceneManager.GetActiveScene().name;

        // ‚Ü‚¾ƒŠƒZƒbƒg‚µ‚Ä‚È‚¯‚ê‚Î‰Šú‰»
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt("DeathCount", 0);
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            Debug.Log("Death count reset for this stage (first time only).");
        }
    }
}
