using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// block & parry mechanics
public class BlockParryController : MonoBehaviour
{
    public float parryModeTime = 0.4f;
    bool parryWindow = false;
    bool blockPressed = false;
    public float parryVelocity = 50.0f;
    public GameObject parryClass;
    Parry parry;
    ParryBlockKnockback knockback;
    [SerializeField] GameObject parryBlockClass;
    [SerializeField] AttackPlayer enemyAttack;
    bool meleeParrySuccess = false;
    bool meleeBlockSuccess = false;


    void Start()
    {
        parry = parryClass.GetComponent<Parry>();
        knockback = parryBlockClass.GetComponent<ParryBlockKnockback>();

    }
    void FixedUpdate()
    {
        // detecting block and parry button press
        // the parry window is a fixed amount of time where
        // the player is locked in the parry state
        if (Input.GetKeyDown("space")){
            parryWindow = true;
            blockPressed = true;
            meleeBlockSuccess = false;
            meleeParrySuccess = false;
            StartCoroutine(ParryWindow());
        }
       
        if (Input.GetKeyUp("space")){ 
            blockPressed = false;
        }

        // checks if player is in the block or parry state when hit
        // enemyAttack.attacking is a bool from the AttackPlayer class
        if ((blockPressed && enemyAttack.attacking) || (parryWindow && enemyAttack.attacking))
        {
            Debug.Log("block or parry sucessful");

            if (!parryWindow)
            {
                Debug.Log("Melee attack blocked");
                meleeBlockSuccess = true;
                // player blocks incoming damage?
                // player takes reduced damage 
            }
            else if (parryWindow)
            {
                Debug.Log("Melee attack parried");
                meleeParrySuccess = true;
                // player blocks incoming damage
                // enemy is stunned
            }
        }
    }


    // projectile block & parry mechanic.
    // checks if parasol is in block or parry state
    // when projectile collides with it.
    // Destroys the projectile when blocked or parried.
    // the projectile is created again by the parasol and
    // shot toward the mouse.
    // player is knocked in the opposite direction they are 
    // facing on a sucessful block or parry. 
    void OnTriggerStay(Collider other){
        if ((other.tag == "EnemyProjectile" && blockPressed) || 
            (other.tag == "EnemyProjectile" && parryWindow))
        {
            if(!parryWindow){
                Debug.Log("blocked");     
                Destroy(other.gameObject);
                knockback.BlockParryKnockback();
            }
            else if(parryWindow){
                Debug.Log("parried");
                Destroy(other.gameObject);
                parry.PlayerShoot();
                knockback.BlockParryKnockback();
            }
        }
    }

    IEnumerator ParryWindow()
    {
        yield return new WaitForSeconds(parryModeTime);
        parryWindow = false;
    }
}
