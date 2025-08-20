using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Money : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI Moneytext;

    private float MoneyNum = 0f;
    private float MoneyValue = 1000f;

    // 돈이 바뀔 때 알림 (현재 금액 전달)
    public System.Action<float> OnMoneyChanged;

    void Start()
    {
        UpdateText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AddMoney(MoneyValue);
    }

    public float GetMoney() => MoneyNum;

    public void AddMoney(float amount)
    {
        MoneyNum += amount;
        UpdateText();
        OnMoneyChanged?.Invoke(MoneyNum);
    }

    // amount 소비, 성공 시 true
    public bool Spend(float amount)
    {
        if (MoneyNum + 1e-6f < amount) return false;
        MoneyNum -= amount;
        UpdateText();
        OnMoneyChanged?.Invoke(MoneyNum);
        return true;
    }

    // 필요 시 외부에서 클릭당 돈 변경
    public void SetMoneyValue(float v)
    {
        MoneyValue = Mathf.Max(0f, v);
    }

    private void UpdateText()
    {
        if (Moneytext != null)
            Moneytext.text = "돈: " + Mathf.FloorToInt(MoneyNum) + "₩";
    }
}
