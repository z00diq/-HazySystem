using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderForNewAbility : MonoBehaviour
{
    private int _counterOfKills;
    private Slider _sliderForNewAbility;
    private AbilityController _abilityController;
    private void Awake()
    {
        GameplayEventManager.OnEnemyKilled.AddListener(ChangeSliderValue);
    }
    private void Start()
    {
        _sliderForNewAbility = GetComponent<Slider>();
        _sliderForNewAbility.value = 0;
        _abilityController = FindObjectOfType<AbilityController>();
    }
    private void Update()
    {
        _sliderForNewAbility.value = (float)_counterOfKills / (float)_abilityController.NumberOfMurdersToNewAbility ;
        if (_counterOfKills >= _abilityController.NumberOfMurdersToNewAbility)
        {
            _counterOfKills -= _abilityController.NumberOfMurdersToNewAbility;
        }
    }
    private void ChangeSliderValue()
    {
        _counterOfKills++;
    }
}
