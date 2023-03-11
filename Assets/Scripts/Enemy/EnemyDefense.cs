using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefense : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private EnemyManager _enemyManager;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private bool _canDefence;
    private bool _haveDefence;
    private float _defencePeriod;
    [SerializeField] private float _defenceDuration;

    private void Awake()
    {
        _enemyManager = _enemy.EnemyManager;
    }

    public void Initialize(bool canDefence, float defencePeriod)
    {
        _canDefence = canDefence;
        _defencePeriod = defencePeriod;

        if (_canDefence)
        {
            StartCoroutine(DefencePeriodicly());
        }
    }

    public bool GetDefenceState()
    {
        return _haveDefence;
    }

    private IEnumerator DefencePeriodicly()
    {
        WaitForSeconds wait = new WaitForSeconds(_defencePeriod + _defenceDuration);
        while (true)
        {
            yield return wait;
            StartCoroutine(StayInvul());
        }
    }

    private IEnumerator StayInvul()
    {
        _haveDefence = true;
        _renderer.material.color = Color.white;

        yield return new WaitForSeconds(_defenceDuration);

        _haveDefence = false;
        _renderer.material.color = Color.red;
    }
}
