using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private int _numberOfMurdersToNewAbility;
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private int _countOfStartAbilities;
    private int _counterOfKill;
    void Awake()
    {
        GameplayEventManager.OnEnemyKilled.AddListener(KilledEnemy);
    }
    private void Start()
    {
        for (int i = 0; i < _countOfStartAbilities; i++)
        {
            RandomActivateAbility();
        }

    }
    private void Update()
    {
        if (_counterOfKill >= _numberOfMurdersToNewAbility && _abilities.Count > 0)
        {
            Debug.Log("_abilities!=null");
            RandomActivateAbility();
            _counterOfKill -= _numberOfMurdersToNewAbility;
        }
    }
    private void KilledEnemy()
    {
        _counterOfKill++;
    }
    
    private void RandomActivateAbility()
    {
        int randomActivateIndex = Random.Range(0, _abilities.Count);
        _abilities[randomActivateIndex].enabled = true;
        _abilities[randomActivateIndex].ChargeIcon.gameObject.SetActive(true);
        _abilities.RemoveAt(randomActivateIndex);
    }    
}
