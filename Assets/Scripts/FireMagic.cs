using UnityEngine;

public class FireMagic : MonoBehaviour
{
    public GameObject impactVFX;
    public int damage = 3;
    public float destroyTime = 3.5f;

    private bool colliderd;

    void Start() 
    {
        damage = GameManager.gameManager.damage;
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Player" && !colliderd) {
            if(collision.gameObject.tag == "Enemy") {
                IAEnemy enemy = collision.gameObject.GetComponent<IAEnemy>();

                if(enemy.health <= 0) {
                    // Ignores the collision of the projectile with the enemy's body.
                    Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), enemy.GetComponent<Collider>());
                }else {
                    if(enemy != null) {
                        enemy.TookDamage(damage);
                    }
                }
            }
            colliderd = true;
            
            var impact = Instantiate(impactVFX, collision.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }
}
