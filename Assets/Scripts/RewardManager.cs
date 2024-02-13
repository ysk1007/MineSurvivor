using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class RewardManager : MonoBehaviour
{
    public bool newDay;
    public int attendance;
    public Animator[] RewardAnims; // ������ �ִϸ��̼�
    public GameObject[] TodayRewardFocus; // ������ ���� ��Ŀ��

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform endPosition;
    [SerializeField] private float duration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int coinAmount;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    List<GameObject> coins = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        attendance = UserInfoManager.Instance.userData.Attendance;
        TodayRewardFocus[attendance].SetActive(true);
        int Today = 0;
        if (UserInfoManager.Instance.userData.TodayStamp)
        {
            Today++;
        }

        for (int i = 0; i < attendance + Today; i++)
        {
            RewardAnims[i].SetTrigger("Skip");
        }

        if (newDay)
        {
            this.gameObject.GetComponent<Popup>().PopupOne();
        }
    }

    public async void CollectCoins(int value)
    {
        UserInfoManager.Instance.userData.GameMoney += value;
        // ����
        for (int i = 0; i < coins.Count; i++)
        {
            Destroy(coins[i]);
        }
        coins.Clear();

        // ���� ����
        for (int i = 0; i < coinAmount; i++)
        {
            List<UniTask> moveCoinTask = new List<UniTask>();
            GameObject coinInstance = Instantiate(coinPrefab, coinParent);

            float xPos = spawnLocation.position.x + Random.Range(minX, maxX);
            float yPos = spawnLocation.position.y + Random.Range(minY, maxY);

            //coinInstance.transform.position = new Vector3(xPos, yPos);
            Vector3 newPos = new Vector3(xPos, yPos);

            coins.Add(coinInstance);
            //await MoveObject(coins[i].transform, newPos);
            moveCoinTask.Add(MoveObject(coins[i].transform, newPos));
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        // ������
        await MoveCoinsTask(value);
    }

    private async UniTask MoveCoinsTask(int value)
    {
        List<UniTask> moveCoinTask = new List<UniTask>();
        for (int i = 0; i < coins.Count; i++)
        {
            moveCoinTask.Add(MoveCoinTask(i));
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    private async UniTask MoveCoinTask(int i)
    {
        await MoveObject(coins[i].transform, endPosition.position);
    }

    async UniTask MoveObject(Transform coin, Vector3 target)
    {
        float t = 0f;

        while (t < duration)
        {
            // t ���� �������Ѽ� ����
            t += Time.deltaTime * moveSpeed;
            coin.position = Vector3.Lerp(coin.position, target, t);
            await UniTask.Yield(); // ���� �����ӱ��� ���
        }

        // �̵� �Ϸ� �� ������ ��ġ ����
        coin.position = target;
    }

    public void Stamp(int index)
    {
        RewardAnims[index].SetTrigger("Stamp");
        UserInfoManager.Instance.Stamp();
    }

    public void Stamp()
    {
        RewardAnims[attendance].SetTrigger("Stamp");
        UserInfoManager.Instance.Stamp();
    }
}
