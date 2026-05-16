using TMPro;
using UnityEngine;

public class WaveCount : MonoBehaviour
{
    public TMP_Text numWave;

    public int currentWave = 0;

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        // TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddNumber(1);
        }
    }

    public void AddNumber(int amount)
    {
        currentWave += amount;

        UpdateText();
    }

    void UpdateText()
    {
        numWave.text = currentWave.ToString();
    }
}
