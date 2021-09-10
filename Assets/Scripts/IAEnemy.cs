using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IAEnemy : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent agent;
    private float playerDistance;
    private  float pointDistance;
    
    [Header("Enemy Settings")]
    public float enemyField = 30;
    public float followDistance = 20;
    public float attackDistance = 3;
    public float speed = 3;
    public float chaseSpeed = 6;
    public float attackRate = 1.5f;
    public int enemyDamage = 10;

    private bool seeingPlayer;

    public Transform[] randomDestination;

    private int currentAiPoint;
    private bool chasing;
    private bool counterChasing;
    private bool attacking;

    private float timerChasing;
    private float timerAttacking;

    private Animator anim;

    private bool isDead = false;
    private bool hit = false;

    [Header("Health")]
    public int health = 100;
    public Transform healthBar;
    public GameObject healthBarObject;

    private Vector3 healthBarScale;
    private float healthPercent;

    [Header("Objects")]
    public GameObject[] spawnObjects;

    void Awake()
    {
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / health;
        healthBarObject.SetActive(false);
    }

    void Start()
    {
        // Draw a point for the enemy to walk.
        currentAiPoint = Random.Range(0, randomDestination.Length);
        agent = transform.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        pointDistance = Vector3.Distance(randomDestination[currentAiPoint].transform.position, transform.position);
        // Raycast.
        RaycastHit hit;
        Vector3 fromWhere = transform.position;
        Vector3 whither = player.transform.position;
        Vector3 direction = whither - fromWhere;
        if(Physics.Raycast(transform.position, direction, out hit, 1000) && playerDistance < enemyField) {
            if(hit.collider.gameObject.CompareTag("Player") && Player.player.isDead == false) {
                seeingPlayer = true;
            }else {
                seeingPlayer = false;
            }
        }
        // Enemy checks and decisions.
        if(playerDistance > enemyField && Player.player.isDead == false) {
            Walk();
        }
        if(playerDistance <= enemyField && playerDistance > followDistance && Player.player.isDead == false) {
            if(seeingPlayer) {
                Look();
            }else {
                Walk();
            }
        }
        if(playerDistance <= followDistance && playerDistance > attackDistance) {
            if(seeingPlayer) {
                Chase();
                chasing = true;
            }else {
                Walk();
            }
        }
        //Attack
        if(playerDistance <= attackDistance) {
            Attack();
        }
        // Walk.
        if(pointDistance <= 2) {
            currentAiPoint = Random.Range(0, randomDestination.Length);
            Walk();
        }
        // Chase counters.
        if(counterChasing) {
            timerChasing += Time.deltaTime;
        }
        if(timerChasing >= 5 && !seeingPlayer) {
            counterChasing = false;
            timerChasing = 0;
            chasing = false;
        }
        // Attacking counters.
        if(attacking) {
            attackRate += Time.deltaTime;
        }
        if(attackRate >= timerAttacking && playerDistance <= attackDistance && Player.player.isDead == false) {
            attacking = true;
            attackRate = 0;
        }else if(attackRate >= timerAttacking && playerDistance > attackDistance) {
            attacking = false;
            attackRate = 0;
        }
    }

    void Walk()
    {
        if(!chasing) {
            anim.SetBool("walk", true);
            agent.acceleration = 5;
            agent.speed = speed;
            agent.destination = randomDestination[currentAiPoint].position;
        } else if(chasing) {
            counterChasing = true;
            anim.SetBool("walk", false);
        }
    }

    void Look()
    {
        anim.SetBool("walk", false);
        anim.SetBool("run", false);
        agent.speed = 0;
        transform.LookAt(player);
    }

    void Chase()
    {
        anim.SetBool("run", true);
        agent.acceleration = 8;
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void Attack() 
    {
        attacking = true;
        anim.SetTrigger("attack");
    }

    public void UpdateHealthBar()
    {
        healthBarScale.x = healthPercent * health;
        healthBar.localScale = healthBarScale;
    }

    public void TookDamage(int damage)
    {
        healthBarObject.SetActive(true);

        health -= damage;
        UpdateHealthBar();

        if(health <= 0) {
            isDead = true;
            StartCoroutine(EnemyDead());
        }else {
            StartCoroutine(TookDamageCoRoutine());
        }
    }

    IEnumerator TookDamageCoRoutine()
    {
        hit = true;
        if(hit) {
            anim.SetBool("hit", true);
        }
        yield return new WaitForSeconds(0.1f);

        hit = false;
        if(!hit) {
            anim.SetBool("hit", false);
        }
        yield return new WaitForSeconds(1f);

        healthBarObject.SetActive(false);
    }

    IEnumerator EnemyDead() 
    {
        if(isDead) {
            agent.isStopped = true;
            anim.SetTrigger("death");
            yield return new WaitForSecondsRealtime(5f);
            Instantiate(spawnObjects[Random.Range(0, spawnObjects.Length)], transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)     
    {
        if(collider.CompareTag("Player") && Player.player.tookDamage == false && !isDead) {
            StartCoroutine(Player.player.TookDamage(enemyDamage));
        }
    }
}
