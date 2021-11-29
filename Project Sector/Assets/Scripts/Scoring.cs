using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    [SerializeField]
    Text text;

    private uint score = 0;

    public void Reset() {
        score = 0;
        adjustScreen();
    }

    public void increase() {
        score++;
        adjustScreen();
    }

    void adjustScreen() {
        text.text = $"Score: {score}";
    }

}
