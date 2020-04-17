using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FluidCapsule : MonoBehaviour
{
    public static FluidCapsule instance { get; private set; }

    public SpritePool capsuleSpritePool;
    private Image capsuleImage;

    [Range(0f, 1f)] private float percentFilled;
    private const int NUM_SPRITES = 24;

    // capsule data:
    private float fluidCapsuleMaxCapacity;
    private float fluidCapsuleCurrAmount;

    // capsule states:
    private bool isFull;
    public bool isEmpty;

    // refill animation:
    public float refillSpeed;
    private bool isRefilling = false;

    // empty animation:
    private float timer = 0f;
    private bool redOutline = false;

    void Start()
    {
        instance = this;
        capsuleImage = GetComponent<Image>();

        fluidCapsuleMaxCapacity = GameData.instance.fluidCapsuleMaxCapacity;
        fluidCapsuleCurrAmount = fluidCapsuleMaxCapacity;
        UpdateFluidCapsule();
    }


    void Update()
    {
        // play refill animation
        if (isRefilling)
        {
            fluidCapsuleCurrAmount += refillSpeed;
            UpdateFluidCapsule();

            if (isFull)
            {
                fluidCapsuleCurrAmount = fluidCapsuleMaxCapacity;
                isRefilling = false;
                UpdateFluidCapsule();
            }
            
        }
        // play empty animation
        if (isEmpty)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 0.5f)
            {
                timer = 0f;
                if (redOutline)
                {
                    redOutline = false;
                    capsuleImage.sprite = capsuleSpritePool.sprites[23];
                }
                else
                {
                    redOutline = true;
                    capsuleImage.sprite = capsuleSpritePool.sprites[24];
                }
            }
        }
    }

    private void UpdateFluidCapsule()
    {
        percentFilled = fluidCapsuleCurrAmount / fluidCapsuleMaxCapacity;

        isFull = false;
        isEmpty = false;
        if (percentFilled == 1f)
        {
            fluidCapsuleCurrAmount = fluidCapsuleMaxCapacity;
            isFull = true;
        }
        else if (percentFilled == 0f)
        {
            fluidCapsuleCurrAmount = 0f;
            isEmpty = true;
            redOutline = false;
            return;
        }

        int index = 24 - (int)(percentFilled * NUM_SPRITES);
        capsuleImage.sprite = capsuleSpritePool.sprites[index];
    }


    public void RemoveFluid(float amount)
    {
        fluidCapsuleCurrAmount -= amount;

        if (fluidCapsuleCurrAmount <= 0f)
        {
            fluidCapsuleCurrAmount = 0f;
        }
        UpdateFluidCapsule();
    }

    public void RefillFluid()
    {
        isRefilling = true;
    }
}
