using TMPro;
using UnityEngine;

public class EnemiesCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private EnemyFactory _enemyFactory;

    public void SetEnemyFactory(EnemyFactory enemyFactory) =>
        _enemyFactory = enemyFactory;

    private void Start()
    {
        UpdateEnemy();
        _enemyFactory.ChangeEnemiesCount += UpdateEnemy;
    }

    private void UpdateEnemy() =>
        _text.text = _enemyFactory.GetEnemiesCount().ToString();

    private void OnDestroy() =>
        _enemyFactory.ChangeEnemiesCount -= UpdateEnemy;
}
