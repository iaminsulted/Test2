using System;
using UnityEngine;

public class ProductButton : MonoBehaviour
{
	public ProductID ProductID;

	public UILabel txtPrice;

	public UIButton btnBuy;

	public GameObject Offer;

	public string Title => Products.ProductPackages[ProductID].Title;

	public bool IsOwned => Session.MyPlayerData.OwnsProduct(Products.ProductPackages[ProductID]);

	public event Action<ProductButton> Clicked;

	public event Action<ProductButton> Removed;

	public void SetPrice(string price)
	{
		if (IsOwned)
		{
			txtPrice.text = "Owned";
			btnBuy.isEnabled = false;
		}
		else
		{
			txtPrice.text = price;
		}
	}

	public void Init()
	{
		if (Products.ProductPackages[ProductID].IsDCPackage)
		{
			if (Session.Account.IsDcPromotion)
			{
				Offer.SetActive(value: true);
				txtPrice.transform.localPosition = new Vector3(144f, 34f, 0f);
			}
			else
			{
				Offer.SetActive(value: false);
				txtPrice.transform.localPosition = new Vector3(144f, 46f, 0f);
			}
		}
		else
		{
			if (!Products.ProductPackages[ProductID].HasBadge)
			{
				return;
			}
			bool flag = Session.MyPlayerData.OwnsProduct(Products.ProductPackages[ProductID.GUARDIAN]) && !Session.MyPlayerData.OwnsProduct(Products.ProductPackages[ProductID.COLLECTORS_GUARDIAN]);
			if (ProductID == ProductID.COLLECTORS_GUARDIAN)
			{
				base.gameObject.SetActive(!flag);
			}
			else if (ProductID == ProductID.COLLECTOR_UPGRADE)
			{
				base.gameObject.SetActive(flag);
			}
			bool flag2 = Session.MyPlayerData.OwnsProduct(Products.ProductPackages[ProductID.SPECIAL_EVENT_COLLECTION]) && !Session.MyPlayerData.OwnsProduct(Products.ProductPackages[ProductID.VIP_SPECIAL_EVENT_COLLECTION]);
			if (ProductID == ProductID.VIP_SPECIAL_EVENT_COLLECTION)
			{
				base.gameObject.SetActive(!flag2);
			}
			else if (ProductID == ProductID.VIP_SPECIAL_EVENT_UPGRADE)
			{
				base.gameObject.SetActive(flag2);
			}
			if (IsOwned)
			{
				txtPrice.text = "Owned";
				btnBuy.isEnabled = false;
				if (Offer != null)
				{
					Offer.SetActive(value: false);
				}
				base.transform.SetAsLastSibling();
			}
			else if (ProductID == ProductID.ADVENTURERS_PACK)
			{
				ProductOffer productOffer = null;
				if (Session.MyPlayerData.ProductOffers.ContainsKey(ProductID.ADVENTURERS_PACK))
				{
					productOffer = Session.MyPlayerData.ProductOffers[ProductID.ADVENTURERS_PACK];
				}
				bool flag3 = productOffer != null && productOffer.ExpireUTC > GameTime.ServerTime;
				base.gameObject.SetActive(flag3);
				if (flag3)
				{
					float time = (float)(productOffer.ExpireUTC - GameTime.ServerTime).TotalSeconds;
					Invoke("Hide", time);
					GetComponentInChildren<CountdownTimer>().SetTime(time);
				}
			}
			if ((ProductID == ProductID.SPECIAL_EVENT_COLLECTION || ProductID == ProductID.VIP_SPECIAL_EVENT_COLLECTION || ProductID == ProductID.VIP_SPECIAL_EVENT_UPGRADE) && (Products.ProductPackages[ProductID].startDate > DateTime.UtcNow || Products.ProductPackages[ProductID].endDate < DateTime.UtcNow))
			{
				Hide();
			}
		}
	}

	public void Refresh()
	{
		CancelInvoke();
		Init();
	}

	private void Hide()
	{
		base.gameObject.SetActive(value: false);
		if (this.Removed != null)
		{
			this.Removed(this);
		}
	}

	public void OnClick()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this);
		}
	}

	public void OnDestroy()
	{
		CancelInvoke();
	}
}
