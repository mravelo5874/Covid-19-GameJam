using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidOrb : MonoBehaviour
{
    public SpritePool germSpritePool;
    public int projectileAmount;
    public int projectileSpeed;
    public GameObject germPrefab;

    private Transform germParent;

    void Start() 
    {
        germParent = GameObject.Find("GermPool").GetComponent<Transform>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // refill fluid capsule
            if (!FluidCapsule.instance.isFull)
            {
                FluidCapsule.instance.RefillFluid();
                AudioManager.inst.PlaySound(Sound.refill);
                StartCoroutine(DeleteItem());
            }
        }
    }

    private IEnumerator DeleteItem()
    {
        Explode();
        yield return new WaitForSeconds(0.01f);
        Destroy(this.gameObject);
    }

    private Sprite GetRandomGermSprite()
    {
        int num = Random.Range(0, 8);
        return germSpritePool.sprites[num];
    }

    private void Explode()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            // instantiate germs an player head
            Vector3 initPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, 0f);
            GameObject germ = Instantiate(germPrefab, initPos, Quaternion.identity, germParent);
            germ.GetComponent<SpriteRenderer>().sprite = GetRandomGermSprite();
            // apply force and torque
            germ.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * projectileSpeed);
            germ.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * projectileSpeed);
        }
    }
}
