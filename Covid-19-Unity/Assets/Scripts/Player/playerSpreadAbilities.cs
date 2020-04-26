using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpreadAbilities : MonoBehaviour
{
    public GameObject germPrefab;
    private Transform germParent;
    public SpritePool germSpritePool;

    // Bools:
    private bool inAction = false;

    // Refrences to other scripts:
    private PlayerController player;

    void Start() 
    {
        player = GetComponent<PlayerController>();     
        germParent = GameObject.Find("GermPool").GetComponent<Transform>();
    }

    void Update() 
    {
        // return if game is paused
        if (GameData.instance.isPaused || !player.isControlable)
        {
            return;
        }

        if (!inAction)
        {
            // Cough mechanic (left-click or XBox 'A' button)
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 0"))
            {
                if (AbilityIcons.instance.canUseCough && !FluidCapsule.instance.isEmpty)
                {
                    inAction = true;
                    AbilityIcons.instance.UseCoughAbility();
                    FluidCapsule.instance.RemoveFluid(GameData.instance.coughFluidCost);
                    CoughAbility();
                }
            }

            // Sneeze mechanic (right-click or XBox 'B' button)
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("joystick button 1"))
            {
                if (AbilityIcons.instance.canUseSneeze && !FluidCapsule.instance.isEmpty)
                {
                    inAction = true;
                    AbilityIcons.instance.UseSneezeAbility();
                    FluidCapsule.instance.RemoveFluid(GameData.instance.sneezeFluidCost);
                    SneezeAbility();
                }
            }
        }
    }

    public void CoughAbility()
    {
        //print ("cough-cough!");
        AudioManager.inst.PlaySound(Sound.cough);

        for (int i = 0; i < GameData.instance.coughProjectileCount ; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, Quaternion.identity, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // add users's velocity
            germ.GetComponent<Rigidbody2D>().velocity += player.getPlayerVelocity();
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * GameData.instance.coughGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * GameData.instance.coughGermSpeed);
        }

        inAction = false;
    }

    public void SneezeAbility()
    {
        //print("achooo!");
        AudioManager.inst.PlaySound(Sound.sneeze);
        Vector2 userDirection = userDirection = player.GetPlayerDirection();

        for (int i = 0; i < GameData.instance.sneezeProjectileCount; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, transform.rotation, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // add users's velocity
            germ.GetComponent<Rigidbody2D>().velocity += player.getPlayerVelocity();
            // randomize distance
            Vector2 randomDistance = userDirection * Random.Range(0.75f, 2f);
            // randomize spread
            randomDistance.x = randomDistance.x + Random.Range(-GameData.instance.sneezeSpread, GameData.instance.sneezeSpread);
            randomDistance.y = randomDistance.y + Random.Range(-GameData.instance.sneezeSpread, GameData.instance.sneezeSpread);
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(randomDistance * GameData.instance.sneezeGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * GameData.instance.sneezeGermSpeed);
  
        }

        inAction = false;
    }

    private Sprite GetRandomGermSprite()
    {
        int num = Random.Range(0, 8);
        return germSpritePool.sprites[num];
    }

    public void UpgradeSneezeProjectile()
    {
        GameData.instance.sneezeProjectileCount += 10;
        if (GameData.instance.sneezeProjectileCount > 50)
        {
            GameData.instance.sneezeProjectileCount = 50;
        }

        GameData.instance.sneezeCooldown -= 0.5f;
        if (GameData.instance.sneezeCooldown <= 1f)
        {
            GameData.instance.sneezeCooldown = 1f;
        }
    }

    public void UpgradeCoughProjectile()
    {
        GameData.instance.coughProjectileCount += 10;
        if (GameData.instance.coughProjectileCount > 50)
        {
            GameData.instance.coughProjectileCount = 50;
        }

        GameData.instance.coughCooldown -= 0.5f;
        if (GameData.instance.coughCooldown  <= 1f)
        {
            GameData.instance.coughCooldown  = 1f;
        }
    }

    public void UpgradeSneezeSpeed()
    {
        GameData.instance.sneezeGermSpeed += 20;
        GameData.instance.sneezeCooldown -= 0.5f;
        if (GameData.instance.sneezeCooldown <= 1f)
        {
            GameData.instance.sneezeCooldown = 1f;
        }
    }

    public void UpgradeCoughSpeed()
    {
        GameData.instance.coughGermSpeed += 20;
        GameData.instance.coughCooldown  -= 0.5f;
        if (GameData.instance.coughCooldown  <= 1f)
        {
            GameData.instance.coughCooldown  = 1f;
        }
    }
}
