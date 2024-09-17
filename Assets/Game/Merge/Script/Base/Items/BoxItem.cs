using UnityEngine;
public class BoxItem : BaseItem
{
    [SerializeField] ParticleSystem particleSystem;


    public override void TakeDamage(int damage)
    {
        AudioManager.Instance.PlayOneShot("Box", 1);
        ParticleSystem boxDestroy = Instantiate(particleSystem, transform.position, Quaternion.identity);
        boxDestroy.transform.localScale = Vector3.one* 0.6f;
        boxDestroy.Play();
        base.TakeDamage(damage);
        if(id == 9)
        {
            if(healthPoint > 0)
            {

                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            }
            else
            {
                // classicMode.CheckMission(id, ItemType.Object, 1, transform.position);
            }
        }
        else
        {
            if (healthPoint <= 1)
            {
                // classicMode.CheckMission(id, ItemType.Object, 1, transform.position);
            }
        }
    }
}
