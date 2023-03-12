using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private int _numberOfMurdersToNewAbility;
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private int _countOfStartAbilities;
    [SerializeField] private int _multiplierForNumberOfMurdersToNewAbility = 2;
    private int _counterOfKills;
    void Awake()
    {
        int index = 0;
        AbilityGroup abilityParent = FindAnyObjectByType<AbilityGroup>();
        ChargeIcon[] icons = abilityParent.GetComponentsInChildren<ChargeIcon>(true);

        foreach (var icon in icons)
        {
            _abilities[index++].ChargeIcon = icon;
        }

        GameplayEventManager.OnEnemyKilled.AddListener(KilledEnemy);
    }
    private void Start()
    {
        int temp = _numberOfMurdersToNewAbility; 
        for (int i = 0; i < _countOfStartAbilities; i++)
        {
            RandomActivateAbility();
        }
        _numberOfMurdersToNewAbility = temp;
    }
    private void Update()
    {
        if (_counterOfKills >= _numberOfMurdersToNewAbility && _abilities.Count > 0)
        {
            Debug.Log("_abilities!=null");
            RandomActivateAbility();
            _counterOfKills -= _numberOfMurdersToNewAbility;
        }
    }

    private void KilledEnemy()
    {
        _counterOfKills++;
    }
    
    private void RandomActivateAbility()
    {
        int randomActivateIndex = Random.Range(0, _abilities.Count);
        _abilities[randomActivateIndex].enabled = true;
        _abilities[randomActivateIndex].ChargeIcon.gameObject.SetActive(true);
        _abilities.RemoveAt(randomActivateIndex);
        _numberOfMurdersToNewAbility *= _multiplierForNumberOfMurdersToNewAbility;
    }
    
    public int NumberOfMurdersToNewAbility
    {
        get { return _numberOfMurdersToNewAbility; }
        private set { }
    }
}
