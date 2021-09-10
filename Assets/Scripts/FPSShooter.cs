using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30;
    private float fireRate = 2;
    // public float arcRange = 1;

    private Vector3 destination;
    private float timeToFire;

    void Start()
    {
        fireRate = GameManager.gameManager.fireRate;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= timeToFire && Player.player.isDead == false && Player.player.mana >= 5) {
            timeToFire = Time.time + 1 / fireRate;
            
            Player.player.mana -= 5;
            Player.player.UpdateManaUI();

            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            destination = hit.point;
        }else {
            destination = ray.GetPoint(1000);
        }

        InstantiateProjectile(firePoint);
    }

    void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;

        // Projectile Effects.
        // iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 2));
    }
}
