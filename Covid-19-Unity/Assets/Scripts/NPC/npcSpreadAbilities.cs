using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcSpreadAbilities : MonoBehaviour
{
    public GameObject germPrefab;
    private Transform germParent;
    public SpritePool germSpritePool;

    // Refrences to other scripts:
    private npcController npc;

    void Start() 
    {
        npc = GetComponent<npcController>();
        germParent = GameObject.Find("GermPool").GetComponent<Transform>();
    }

    public void CoughAbility()
    {
        //print ("cough-cough!");
        if (!GameData.instance.muteFX)
        {
            AudioManager.inst.PlaySound(Sound.cough);
        }

        for (int i = 0; i < GameData.instance.coughProjectileCount; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, Quaternion.identity, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // add users's velocity
            germ.GetComponent<Rigidbody2D>().velocity += npc.getNPCVelocity();
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * GameData.instance.coughGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * GameData.instance.coughGermSpeed);
        }
    }


    public void SneezeAbility()
    {
        //print("achooo!");
        if (!GameData.instance.muteFX)
        {
            AudioManager.inst.PlaySound(Sound.sneeze);
        }

        Vector2 userDirection = userDirection = npc.GetNPCDirection();

        for (int i = 0; i < GameData.instance.sneezeProjectileCount; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, transform.rotation, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // add users's velocity
            germ.GetComponent<Rigidbody2D>().velocity += npc.getNPCVelocity();
            // randomize distance
            Vector2 randomDistance = userDirection * Random.Range(0.75f, 2f);
            // randomize spread
            randomDistance.x = randomDistance.x + Random.Range(-GameData.instance.sneezeSpread, GameData.instance.sneezeSpread);
            randomDistance.y = randomDistance.y + Random.Range(-GameData.instance.sneezeSpread, GameData.instance.sneezeSpread);
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(randomDistance * GameData.instance.sneezeGermSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * GameData.instance.sneezeGermSpeed);
  
        }
    }

    private Sprite GetRandomGermSprite()
    {
        int num = Random.Range(0, 8);
        return germSpritePool.sprites[num];
    }
}
