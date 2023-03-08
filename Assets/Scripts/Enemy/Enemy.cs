using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static System.Action<float> OnBorn;

    public EnemyManager EnemyManager;

    [SerializeField] private float _hazzard;
    [SerializeField] private float _health = 1;
    [SerializeField] private float _reproductionPeriod = 1;
    private float _reproductionTimer;
    [SerializeField] private bool _canReproduse;

    [SerializeField] private bool _canShowRays;

    public static float RayRange = 0.2f;

    [SerializeField] private List<Vector3> _rayDirections = new List<Vector3>();
    private List<Ray> _rays = new List<Ray>();

    private void Awake()
    {
        OnBorn?.Invoke(_hazzard);
        // add randomness for reproduction period for greater unevenness of the appearance new cells
        _reproductionPeriod += Random.Range(-0.5f, 1f);
        // defence of mistake: negative value in reproductionPeriod;
        _reproductionPeriod = Mathf.Clamp(_reproductionPeriod, 1, 100);

        CreateRays();

        //Debug.Log("Я родился");
    }

    private void CreateRays()
    {
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            _rays.Add(new Ray(transform.position, _rayDirections[i] * RayRange));
        }
    }

    private void ShowRays()
    {
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            Debug.DrawRay(transform.position, _rayDirections[i] * RayRange);
        }
    }

    private void Update()
    {
        if (_canReproduse)
        {
            _reproductionTimer += Time.deltaTime;
            if (_reproductionTimer >= _reproductionPeriod)
            {
                Vector3 newCellPosition = GetPositionForNewCells();
                if (newCellPosition != Vector3.zero)
                {
                    EnemyManager.Reproduce(newCellPosition);
                    //Debug.Log($"{gameObject.name} reproduce");
                }
                _reproductionTimer = 0;
            }
        }

        if (_canShowRays)
        {
            ShowRays();
        }
    }

    private IEnumerator ReproduceCoroutine(bool canReproduce)
    {
        WaitForSeconds wait = new WaitForSeconds(_reproductionPeriod);

        while (canReproduce)
        {
            Vector3 newCellPosition = GetPositionForNewCells();
            if (newCellPosition == Vector3.zero)
            {
                EnemyManager.Reproduce(newCellPosition);
            }

            yield return wait;
        }
    }


    private Vector3 GetPositionForNewCells()
    {
        Vector3 vectorToNewCells = Vector3.zero;
        for (int i = 0; i < _rayDirections.Count; i++)
        {
            Ray ray = new Ray(transform.position, transform.TransformDirection(_rayDirections[i])); 

            if (Physics.Raycast(ray, out RaycastHit hit, RayRange*Mathf.Sqrt(2)))
            {
                if (hit.collider)
                {
                    //Debug.Log($"{gameObject.name} hit {hit.transform.gameObject.name} in {_rayDirections[i]} direction");
                    continue;
                }
            }
            else
            {
                //Debug.Log($"Direction: {_rayDirections[i] * RayRange} empty! We can reproduce on this position!");
                vectorToNewCells = _rayDirections[i] * RayRange;
                return transform.position + vectorToNewCells;
            }
        }

        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Ball>() is Ball ball)
        {
            _health -= ball.DamageValue;

            if (CheckDeath())
            {
                Destroy(gameObject);
                //Debug.Log($"{gameObject.name} dead");
            }
        }
    }

    private bool CheckDeath()
    {
        return _health <= 0 ? true : false;
    }
}
