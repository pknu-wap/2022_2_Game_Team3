using UnityEngine;

public class GetItem : MonoBehaviour
{

    public float flameSpeed = 0.1f;
    [SerializeField]
    private float range;  // 아이템 습득이 가능한 최대 거리

    private bool pickupActivated = false;  // 아이템 습득 가능할시 True 

    private RaycastHit hitInfo;  // 충돌체 정보 저장
    
    private bool flameState = false;

    private Transform flame;
    private void Awake()
    {
        flame = GameObject.Find("FlameParent").GetComponent<Transform>();
    }

    void Update()
    {
        TryAction();
        if (flameState == true)
        {
            flame.localScale += new Vector3(0, flameSpeed, 0);
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
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log("유물 획득");
                Destroy(hitInfo.transform.gameObject);
                flameState = true;
                ItemInfoDisappear();
            }
        }
    }
}