using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCrystal : MonoBehaviour
{
    [SerializeField] private TowerAim towerAim;
    [SerializeField] private Vector2 floatBound;

    void Start()
    {
        towerAim = GetComponentInParent<TowerAim>();

        if(towerAim != null)
            InvokeRepeating("Floating", 1f, 1f);
    }

    private void Floating()
    {
        if(towerAim.targetedEnemyHealth != null) { LeanTween.cancel(gameObject); return; }

        // LeanTween.value( gameObject, floatBound.x, floatBound.y, 1f).setOnUpdate( (float val)=>{
        //     this.transform.position = new Vector3(this.transform.position.x, val, this.transform.position.z);
        // } ).setOnComplete(FloatBack);

        LeanTween.moveY(gameObject, floatBound.y, 1f).setLoopPingPong();
    }

    private void FloatBack()
    {
        // LeanTween.value( gameObject, floatBound.y, floatBound.x, 1f).setOnUpdate( (float val)=>{
        //     this.transform.position = new Vector3(this.transform.position.x, val, this.transform.position.z);
        // } ).setOnComplete(Floating);
    }
}
