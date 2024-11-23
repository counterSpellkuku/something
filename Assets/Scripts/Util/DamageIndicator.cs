using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Show(Vector2 pos) {
        GameObject obj = Resources.Load<GameObject>("Prefabs/DamageInfo");

        Instantiate(obj, pos, Quaternion.identity);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
