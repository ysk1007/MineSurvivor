using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    public bool newDay;
    public int attendance;
    public Animator[] RewardAnims; // ������ �ִϸ��̼�
    public GameObject[] TodayRewardFocus; // ������ ���� ��Ŀ��
    [SerializeField] private Color[] color;

    [SerializeField] private GameObject[] RewardPrefabs;
    [SerializeField] private Transform RewardParent;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform[] endPositions;
    [SerializeField] private float duration;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int coinAmount;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    List<GameObject> Rewards = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

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

    public async void CollectReward(int type, int value)
    {
        GuiManager.instance.PrintLog(value.ToString(), color[type]);
        // ����
        for (int i = 0; i < Rewards.Count; i++)
        {
            Destroy(Rewards[i]);
        }
        Rewards.Clear();

        // ���� ����
        for (int i = 0; i < coinAmount; i++)
        {
            List<UniTask> moveCoinTask = new List<UniTask>();
            GameObject RewardInstance = Instantiate(RewardPrefabs[type], RewardParent);

            float xPos = spawnLocation.position.x + Random.Range(minX, maxX);
            float yPos = spawnLocation.position.y + Random.Range(minY, maxY);

            //coinInstance.transform.position = new Vector3(xPos, yPos);
            Vector3 newPos = new Vector3(xPos, yPos);

            Rewards.Add(RewardInstance);
            //await MoveObject(coins[i].transform, newPos);
            moveCoinTask.Add(MoveObject(Rewards[i].transform, newPos));
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        // ������
        await MoveCoinsTask(type,value);
    }

    private async UniTask MoveCoinsTask(int type, int value)
    {
        List<UniTask> moveCoinTask = new List<UniTask>();
        for (int i = 0; i < Rewards.Count; i++)
        {
            moveCoinTask.Add(MoveCoinTask(type,i));
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }
    }

    private async UniTask MoveCoinTask(int type, int i)
    {
        await MoveObject(Rewards[i].transform, endPositions[type].position);
        Destroy(Rewards[i]);
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
