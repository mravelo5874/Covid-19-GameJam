using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    sneezeProjectileUpgrade, coughProjectileUpgrade, sneezeSpeedUpgrade, coughSpeedUpgrade, capsuleUpgrade, speedUpgrade
}

public class ItemScript : MonoBehaviour
{
    public bool randomizeItem;
    public ItemType itemType;
    public SpritePool itemIconPool;
    public SpritePool germSpritePool;
    public int projectileAmount;
    public int projectileSpeed;
    public GameObject germPrefab;

    private Transform germParent;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        germParent = GameObject.Find("GermPool").GetComponent<Transform>();

        // become random item
        int index = (int)itemType;
        if (randomizeItem)
        {
            index = Random.Range(0, 6);
            itemType = (ItemType)index;
        }
        
        sprite.sprite = itemIconPool.sprites[index];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        playerSpreadAbilities player = other.GetComponent<playerSpreadAbilities>();
        if (player != null)
        {
            if (itemType == ItemType.sneezeProjectileUpgrade)
            {
                player.UpgradeSneezeProjectile();
            }
            else if (itemType == ItemType.coughProjectileUpgrade)
            {
                player.UpgradeCoughProjectile();
            }
            else if (itemType == ItemType.sneezeSpeedUpgrade)
            {
                player.UpgradeSneezeSpeed();
            }
            else if (itemType == ItemType.coughSpeedUpgrade)
            {
                player.UpgradeCoughSpeed();
            }
            else if (itemType == ItemType.capsuleUpgrade)
            {
                FluidCapsule.instance.UpgradeCapsuleMax();
            }
            else if (itemType == ItemType.speedUpgrade)
            {
                other.GetComponent<PlayerController>().UpgradeSpeed();
            }

            AudioManager.inst.PlaySound(Sound.powerUp);
            StartCoroutine(DeleteItem());
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
