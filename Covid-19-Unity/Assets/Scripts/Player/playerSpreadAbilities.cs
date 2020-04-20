﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpreadAbilities : MonoBehaviour
{
    public GameObject germPrefab;
    private Transform germParent;
    public SpritePool germSpritePool;

    // Bools:
    private bool inAction = false;

    // Cough Mechanic:
    private int coughProjectileCount;
    private float coughCoolDown;
    private float coughGermSpeed;
    
    // Sneeze Mechanic:
    private int sneezeProjectileCount;
    private float sneezeCoolDown;
    private float sneezeGermSpeed;
    private float sneezeSpread;

    // Refrences to other scripts:
    private PlayerController player;

    void Start() 
    {
        // load data from GameData
        coughProjectileCount = GameData.instance.coughProjectileCount;
        coughCoolDown = GameData.instance.coughCooldown;
        coughGermSpeed = GameData.instance.coughGermSpeed;

        sneezeProjectileCount = GameData.instance.sneezeProjectileCount;
        sneezeCoolDown = GameData.instance.sneezeCooldown;
        sneezeGermSpeed = GameData.instance.sneezeGermSpeed;
        sneezeSpread = GameData.instance.sneezeSpread;

        player = GetComponent<PlayerController>();     
        germParent = GameObject.Find("GermPool").GetComponent<Transform>();
    }

    void Update() {
        
        // return if game is paused
        if (GameManager.instance.isPaused)
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
        print ("cough-cough!");
        for (int i = 0; i < coughProjectileCount; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, Quaternion.identity, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // add users's velocity
            germ.GetComponent<Rigidbody2D>().velocity += player.getPlayerVelocity();
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * coughGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * coughGermSpeed);
        }

        inAction = false;
    }

    public void SneezeAbility()
    {
        print("achooo!");
        Vector2 userDirection = userDirection = player.GetPlayerDirection();

        for (int i = 0; i < sneezeProjectileCount; i++)
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
            randomDistance.x = randomDistance.x + Random.Range(-sneezeSpread, sneezeSpread);
            randomDistance.y = randomDistance.y + Random.Range(-sneezeSpread, sneezeSpread);
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(randomDistance * sneezeGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * sneezeGermSpeed);
  
        }

        inAction = false;
    }

    private Sprite GetRandomGermSprite()
    {
        int num = Random.Range(0, 8);
        return germSpritePool.sprites[num];
    }
}
