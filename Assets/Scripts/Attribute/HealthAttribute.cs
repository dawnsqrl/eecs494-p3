using UnityEngine;

public class HealthAttribute : MonoBehaviour
{
    [SerializeField] private float healthCapacity;

    private float health;
    private bool isAlive;

    private void Start()
    {
        health = healthCapacity;
    }

    private void Update()
    {
        isAlive = health > 0;
    }

    public void TakeDamage(float value)
    {
        health = value > health ? 0 : health - value;
    }

    public bool GetLivingState()
    {
        return isAlive;
    }
}