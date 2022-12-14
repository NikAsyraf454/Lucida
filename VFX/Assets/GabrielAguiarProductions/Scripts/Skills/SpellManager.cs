using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private AudioClip clip_lightning, clip_froze, clip_moneymultiplier, clip_towerPowerUp;
    public static SpellManager Instance;
    private PlayerManager playerManager;
    [SerializeField] private GameObject previewProjection;
    [SerializeField] private List<EnemyMovement> enemyMovements;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float areaRadius = 5f;
    public Color slowedColor;
    [SerializeField] private GameObject lightning;
    private bool isHold = false;
    [SerializeField] private GameObject[] spellButton;
    [SerializeField] private bool[] isActivated = {false, false, false, false};
    private bool preview = false;
    [SerializeField] private Color normal, dimmed;
    [SerializeField] private GameObject coinEffect;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    void LateUpdate()
    {
        if(playerManager.currentCharge <= 0) { previewProjection.SetActive(false); preview = false;}
        if(!preview) { previewProjection.SetActive(false); return; }

        previewProjection.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            previewProjection.transform.position = hit.point;
        }

    }

    // private void OnClick()
    // {
    //     OnSpell1();
    // }

    public void OnSpell1(/* InputValue value */)     //Slowdown Enemy
    {
        int chargeCost = 1;
        if(!playerManager.CheckCharge(chargeCost)) { return; }
        // Debug.Log("Q or Spell1 is activated");
        if(!ActivatedSpell(0)) { return; }
        if(!isActivated[0]) { isActivated[0] = true; preview = true; return;} 
        playerManager.ReduceCharge(chargeCost);
        
        SoundManager.Instance.PlaySound(clip_froze);

        Collider[] colliders = AreaDetection();

        foreach(Collider co in colliders)
        {
            // Debug.Log(co.gameObject.name);
            if(co.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
            {
                enemyMovement.DoSlowDown(50f,2f);
            }
        }

        DeactivateSpell(0);
        preview = false;
        // float temp = value.Get<int>;
        // Debug.Log("Activated " + );
    }

    public void OnSpell2()     //Increase tower damage
    {
        int chargeCost = 2;
        if(!playerManager.CheckCharge(chargeCost)) { return; }
        if(!ActivatedSpell(1)) { return; }
        if(!isActivated[1]) { isActivated[1] = true; return;}
        playerManager.ReduceCharge(chargeCost);

        //decide on all tower or area selection
        //if all tower

        SoundManager.Instance.PlaySound(clip_towerPowerUp);
        
        List<TowerLevel> towerList = TowerManager.Instance.spawnedTowerList;

        foreach(TowerLevel towerLevel in towerList)
        {
            towerLevel.DoIncreaseDamage(50f, 10f);
        }

        DeactivateSpell(1);

    }

    public void OnSpell3()     //Increase Resource multiplier
    {
        int chargeCost = 1;
        if(!playerManager.CheckCharge(chargeCost)) { return; }
        if(!ActivatedSpell(2)) { return; }
        if(!isActivated[2]) { isActivated[2] = true; return;} 
        playerManager.ReduceCharge(chargeCost);

        SoundManager.Instance.PlaySound(clip_moneymultiplier);

        PlayerManager.Instance.DoIncreaseMultiplier(50f, 15f);

        DeactivateSpell(2);
    }

    public void OnSpell4()     //Damage enemy
    {
        int chargeCost = 2;
        if(!playerManager.CheckCharge(chargeCost)) { return; }
        if(!ActivatedSpell(3)) { return; }
        if(!isActivated[3]) { isActivated[3] = true; preview = true; return;} 
        playerManager.ReduceCharge(chargeCost);

        SoundManager.Instance.PlaySound(clip_lightning);
        
        Collider[] colliders = AreaDetection();

        foreach(Collider co in colliders)
        {
            if(co.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                if(co.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
                {
                    enemyMovement.DoSlowDown(100f,1f);
                }
                Vector3 pos = enemyHealth.transform.position;
                pos.y = 6f;
                Instantiate(lightning, pos, Quaternion.identity);
                enemyHealth.DoDelayedDealDamage(80, 0.55f);
            }
        }

        DeactivateSpell(3);
        preview = false;
    }

    public void ShowCoinIcon()
    {
        coinEffect.SetActive(true);
    }

    public void HideCoinIcon()
    {
        coinEffect.SetActive(false);
    }

    private void DamageEnemy()
    {

    }

    private Collider[] AreaDetection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            return(Physics.OverlapSphere(hit.point, areaRadius));
        }
        else
        {
            return null;
        }
        
    }

    private bool ActivatedSpell(int spell)
    {
        for(int i = 0; i < 4; i++)
        {
            if(isActivated[i] && i != spell)
            {
                
                return false;
            }
        }
        spellButton[spell].GetComponent<Image>().color = spellButton[spell].GetComponent<Button>().colors.pressedColor;
        return true;
    }

    private void DeactivateSpell(int spell)
    {
        isActivated[spell] = false;
        spellButton[spell].GetComponent<Image>().color = spellButton[spell].GetComponent<Button>().colors.normalColor;
        SpellIconChecker();
    }

    public void SpellIconChecker()
    {
        spellButton[0].GetComponent<Image>().color = dimmed;
        spellButton[1].GetComponent<Image>().color = dimmed;
        spellButton[2].GetComponent<Image>().color = dimmed;
        spellButton[3].GetComponent<Image>().color = dimmed;

        if(PlayerManager.Instance.currentCharge >= 1)
        {
            spellButton[0].GetComponent<Image>().color = normal;
            spellButton[2].GetComponent<Image>().color = normal;

            if(PlayerManager.Instance.currentCharge >= 2)
            {
                spellButton[1].GetComponent<Image>().color = normal;
                spellButton[3].GetComponent<Image>().color = normal;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // // Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        // if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        // {
        //     Gizmos.DrawWireSphere(hit.point, 5f);
        // }
    //     Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //     // Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
    //     if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
    //     {
    //         // Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //         Gizmos.DrawWireSphere(hit.point, 5f);
    //     }

    //     // Ray ray1 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //     // Debug.DrawRay(ray1.origin, ray1.direction * 20, Color.yellow);
    }
}
