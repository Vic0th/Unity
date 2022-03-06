using UnityEngine;

public class RopeDirection : MonoBehaviour
{
    [SerializeField]
    public bool toRight;

    public bool Direction{
        get => toRight;
    }
}
