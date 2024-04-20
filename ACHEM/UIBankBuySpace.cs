using UnityEngine;

public class UIBankBuySpace : MonoBehaviour
{
	public UILabel PriceLabel;

	private void Start()
	{
		PriceLabel.text = UIBankManager.Instance.BankCost.ToString();
	}

	private void Update()
	{
	}
}
