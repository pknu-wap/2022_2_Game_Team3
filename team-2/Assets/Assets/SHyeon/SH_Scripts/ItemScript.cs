using UnityEngine;

public class ItemScript : MonoBehaviour
{
    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        RotateGameObject(new Vector3(0f, (Mathf.Cos(timer) * 0.5f + 0.5f) * 360f, 0f));
    }

    public void RotateGameObject(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}