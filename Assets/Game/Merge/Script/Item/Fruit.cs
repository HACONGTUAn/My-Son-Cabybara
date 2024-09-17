using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
public class Fruit : MonoBehaviour
{
    public static Action<Fruit, Fruit> onCollisionWithFruit;
    public SpriteRenderer spriteRenderer;
    public bool inIce;
    [SerializeField] private FruitType fruitType;
    public FruitItemObjectSO fruitItem;
    public GameObject highlightObj;
    // public AnimationHandle neutralState;
    // public AnimationHandle fallingState;
    // public AnimationHandle hotState;
    public FruitCollisionHandle[] fruitCollisions;
    private static HashSet<Collider2D> damagedColliders = new HashSet<Collider2D>();
    public Vector3 position => GetAnchor().position;
    public bool hasCollided;
    public bool hasContacted;

    private void Start()
    {

    }
    public void Initialize()
    {
        SetSkin(GameManager.Instance.currentTheme);
        hasContacted = false;
        hasCollided = false;
        fruitCollisions = GetComponentsInChildren<FruitCollisionHandle>(true);
        for (int i = 0; i < fruitCollisions.Length; i++)
        {
            fruitCollisions[i].Initialize(this);
        }
        // if (fallingState)
        //     fallingState.Deactive();
        // if (fallingState)
        //     neutralState.Active();
        DisablePhysic();
        if (highlightObj)
            highlightObj.SetActive(false);
    }
    public void OnSpawn()
    {
        Vector3 orScl = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(orScl, 0.15f).SetEase(Ease.OutQuad).SetTarget(this);
    }

    public void SetSkin(int skinIndex)
    {
        if (skinIndex >= 0 && skinIndex < fruitItem.fruitSkins.Length)
        {
            FruitSkinData skinData = fruitItem.fruitSkins[skinIndex];
            spriteRenderer.sprite = skinData.fruitSkin;
            spriteRenderer.transform.localScale = Vector3.one * skinData.spriteScale;
        }
        else
        {
            Debug.LogWarning("Skin index is out of range!");
        }
    }

    public void StartDrop()
    {
        // if (fallingState)
        //     fallingState.Active();
        // if (neutralState)
        //     neutralState.Deactive();
    }
    public void EnablePhysic()
    {
        Rigidbody2D[] rd = GetComponentsInChildren<Rigidbody2D>();
        foreach (var item in rd)
        {
            item.bodyType = RigidbodyType2D.Dynamic;
        }
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D coll in colliders)
        {
            coll.enabled = true;
        }
        Joint2D[] joints = GetComponentsInChildren<Joint2D>();
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].enabled = true;
        }
    }
    public void DisablePhysic()
    {
        // SpriteSkin spriteSkin = GetComponentInChildren<SpriteSkin>();
        // if (spriteSkin) spriteSkin.enabled = false;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D coll in colliders)
        {
            coll.enabled = false;
        }
        Joint2D[] joints = GetComponentsInChildren<Joint2D>();
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].enabled = false;
        }
        Rigidbody2D[] rd = GetComponentsInChildren<Rigidbody2D>();
        foreach (var item in rd)
        {
            item.velocity = Vector2.zero;
            item.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    public virtual void OnCollision(Collision2D other)
    {
        if (hasCollided) return;
        FruitCollisionHandle otherColl = other.transform.GetComponent<FruitCollisionHandle>();
        if (otherColl)
        {
            var otherFruit = otherColl.owner;
            if (otherFruit == null || otherFruit == this) return;
            if (otherColl.owner.GetFruitType() == fruitType)
            {
              onCollisionWithFruit?.Invoke(this, otherColl.owner);               
            }
        }
        if (hasContacted == false)
        {
            // if (fallingState)
            //     fallingState.Deactive();
            // if (neutralState)
            //     neutralState.Active();
            AudioManager.Instance.PlayOneShot("DropImpact", 1f);
        }
        hasContacted = true;
    }

    public void CheckBoxOver()
    {
        if (damagedColliders.Count == 0)
        {
            // This assumes CheckBoxOver() is called once per frame per object.
            // If called multiple times per frame, you may need another method to determine the start of a new frame.
            damagedColliders.Clear();
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, transform.GetComponent<CircleCollider2D>().radius + 0.1f);
        for (int i = 0; i < cols.Length; i++)
        {
            Collider2D col = cols[i];
            if (!damagedColliders.Contains(col))
            {
                damagedColliders.Add(col);
                IDamage damage = col.GetComponent<IDamage>();
                if (damage != null)
                {
                    damage.TakeDamage(1);
                }
            }
        }
    }

    public static void ClearDamagedColliders()
    {
        damagedColliders.Clear();
    }
    public FruitType GetFruitType()
    {
        return fruitType;
    }
    public bool HasCollided()
    {
        return hasCollided;
    }
    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
    public Bounds GetBounds()
    {
        return spriteRenderer.bounds;
    }
    public Transform GetAnchor()
    {
        if(transform != null)
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(transform.childCount - 1);
            }
            return transform;
        }
        return null;
    }
    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
