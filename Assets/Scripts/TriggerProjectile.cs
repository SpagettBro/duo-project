using UnityEngine;

public class TriggerProjectile : MonoBehaviour
{
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectile4;
    [SerializeField] GameObject projectile5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + " Collided");
        if(other.gameObject.tag == "Player")
        {
            projectile1.SetActive(true);
            projectile2.SetActive(true);
            projectile3.SetActive(true);
            projectile4.SetActive(true);
            projectile5.SetActive(true);
            Destroy(gameObject);
        }
    }
}
