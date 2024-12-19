using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    enum HintState{
        PreHint,
        Hinting,
        Hinted
    }

    public bool LTUsed;
    HintState LTStatus = HintState.PreHint;
    public float LTtimer = 0.0f;
    private GameObject LT;
    

    public bool TTUsed;
    HintState TTStatus = HintState.PreHint;
    public float TTtimer = 0.0f;
    private GameObject TT;

    public bool RGUsed;
    HintState RGStatus = HintState.PreHint;
    public float RGtimer = 0.0f;
    private GameObject RG;

    HintState SpellStatus = HintState.PreHint;
    public float SpellTimer = 0.0f;
    [SerializeField] protected float placedWait = 2.0f; 
    [SerializeField] protected float hintTime = 10.0f;

    public bool IsTowerDamaged;
    HintState DamagedHint = HintState.PreHint;
    public float DamTimer = 0.0f;
    private GameObject DamT;

    public bool IsTowerDowned;
    HintState DownedHint = HintState.PreHint;
    public float DowTimer = 0.0f;
    private GameObject DowT;

    private static HintManager _instance;
    public static HintManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("HintManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Hint Logic for Light Tower
        if (LTUsed){
            switch (LTStatus)
            {
                case HintState.PreHint:
                    if(LTtimer > placedWait){
                        HintLightHouse();
                        LTStatus = HintState.Hinting;
                        LTtimer = 0;
                    }
                    LTtimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(LTtimer > hintTime){
                        HideHintLightHouse();
                        LTStatus = HintState.Hinted;
                    }
                    LTtimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }

        // Hint Logic for Trigger Tower
        if (TTUsed){
            switch (TTStatus)
            {
                case HintState.PreHint:
                    if(TTtimer > placedWait){
                        HintTriggerTower();
                        TTStatus = HintState.Hinting;
                        TTtimer = 0;
                    }
                    TTtimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(TTtimer > hintTime){
                        HideHintTriggerTower();
                        TTStatus = HintState.Hinted;
                    }
                    TTtimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }

        // Hint Logic for Railgun Tower
        if (RGUsed){
            switch (RGStatus)
            {
                case HintState.PreHint:
                    if(RGtimer > placedWait){
                        HintRailGun();
                        RGStatus = HintState.Hinting;
                        RGtimer = 0;
                    }
                    RGtimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(RGtimer > hintTime){
                        HideHintRailGun();
                        RGStatus = HintState.Hinted;
                    }
                    RGtimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }

        if(LTUsed || TTUsed || RGUsed){
            switch (SpellStatus)
            {
                case HintState.PreHint:
                    if(SpellTimer > 2 * placedWait + hintTime){
                        GameObject hint = gameObject.transform.GetChild(3).gameObject;
                        hint.SetActive(true);
                        SpellStatus = HintState.Hinting;
                        SpellTimer = 0;
                    }
                    SpellTimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(SpellTimer > hintTime){
                        GameObject hint = gameObject.transform.GetChild(3).gameObject;
                        hint.SetActive(false);
                        SpellStatus = HintState.Hinted;
                    }
                    SpellTimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }

        // Hint Logic for Damaged Tower
        if (IsTowerDamaged){
            switch (DamagedHint)
            {
                case HintState.PreHint:
                    if(DamTimer > placedWait){
                        HintDamagedTower();
                        DamagedHint = HintState.Hinting;
                        DamTimer = 0;
                    }
                    DamTimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(DamTimer > hintTime){
                        HideHintDamagedTower();
                        DamagedHint = HintState.Hinted;
                    }
                    DamTimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }

        // Hint Logic for Downed Tower
        if (IsTowerDowned){
            switch (DownedHint)
            {
                case HintState.PreHint:
                    if(DowTimer > placedWait){
                        HintDownedTower();
                        DownedHint = HintState.Hinting;
                        DowTimer = 0;
                    }
                    DowTimer += UnityEngine.Time.deltaTime;
                    break;

                case HintState.Hinting:
                    if(DowTimer > hintTime){
                        HideHintDownedTower();
                        DownedHint = HintState.Hinted;
                    }
                    DowTimer += UnityEngine.Time.deltaTime;
                    break;

                default:
                    break;
            }
        }
    }
    public void TowerPlace(GameObject tower){
        print("Got tower " + tower);
        switch (tower.tag)
        {
            case "LightHouseTower":
                print("It's a light tower");
                if(!LTUsed){
                    LT = tower;
                    LTUsed = true;
                }
                break;

            case "RailgunTower":
                print("It's a railgun tower");
                if(!RGUsed){
                    RG = tower;
                    RGUsed = true;
                }
                break;

            case "Tower":
                print("It's a trigger tower");
                if(!TTUsed){
                    TT = tower;
                    TTUsed = true;
                }
                break;

            default:
                break;
        }
    }

    public void TowerDamaged(GameObject tower){
        if(!IsTowerDamaged){
            DamT = tower;
            IsTowerDamaged = true;
        }
    }

    public void TowerDowned(GameObject tower){
        if(!IsTowerDowned){
            DowT = tower;
            IsTowerDowned = true;
        }
    }

    void HintLightHouse(){
        GameObject hint = gameObject.transform.GetChild(0).gameObject;
        hint.transform.position = LT.transform.position;

        hint.SetActive(true);
    }
    void HideHintLightHouse(){
        GameObject hint = gameObject.transform.GetChild(0).gameObject;
        hint.SetActive(false);
    }

    void HintTriggerTower(){
        GameObject hint = gameObject.transform.GetChild(1).gameObject;
        hint.transform.position = TT.transform.position;

        GameObject arrows = hint.transform.GetChild(1).gameObject;
        arrows.transform.rotation = TT.transform.rotation;

        hint.SetActive(true);
    }
    void HideHintTriggerTower(){
        GameObject hint = gameObject.transform.GetChild(1).gameObject;
        hint.SetActive(false);
    }

    void HintRailGun(){
        GameObject hint = gameObject.transform.GetChild(2).gameObject;
        hint.transform.position = RG.transform.position;

        GameObject arrows = hint.transform.GetChild(1).gameObject;
        arrows.transform.rotation = RG.transform.rotation;

        hint.SetActive(true);
    }
    void HideHintRailGun(){
        GameObject hint = gameObject.transform.GetChild(2).gameObject;
        hint.SetActive(false);
    }

    void HintDamagedTower(){
        GameObject hint = gameObject.transform.GetChild(4).gameObject;
        hint.transform.position = DamT.transform.position;

        hint.SetActive(true);
    }
    void HideHintDamagedTower(){
        GameObject hint = gameObject.transform.GetChild(4).gameObject;
        hint.SetActive(false);
    }

    void HintDownedTower(){
        GameObject hint = gameObject.transform.GetChild(5).gameObject;
        hint.transform.position = DowT.transform.position;

        hint.SetActive(true);
    }
    void HideHintDownedTower(){
        GameObject hint = gameObject.transform.GetChild(5).gameObject;
        hint.SetActive(false);
    }
}
