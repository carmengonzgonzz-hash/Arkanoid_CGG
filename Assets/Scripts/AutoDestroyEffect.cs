using UnityEngine;
public class AutoDestroyEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.3f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}