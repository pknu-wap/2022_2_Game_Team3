using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    private float range;  // 아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false;  // 아이템 습득 가능할시 True 

    private RaycastHit hitInfo;  // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask;  // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.

    [SerializeField]
    private Text actionText;  // 행동을 보여 줄 텍스트

    private bool floorState = false;

    private Transform floor;
    private void Awake()
    {
        floor = GameObject.Find("FlameParent").GetComponent<Transform>();
    }

    void Update()
    {
        CheckItem();
        TryAction();
        if (floorState == true)
        {
            floor.localScale += new Vector3(0, 0.01f, 0);
        }
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }
    
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range))
        {
            Debug.Log("테스트");
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else
            ItemInfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        //actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log("획득 했습니다.");  // 인벤토리 넣기
                Destroy(hitInfo.transform.gameObject);
                floorState = true;
                ItemInfoDisappear();
            }
        }
    }
}