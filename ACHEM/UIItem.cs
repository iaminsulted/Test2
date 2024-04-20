using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIItem : MonoBehaviour
{
	public static Color DefaultColor = new Color(0f, 0.41960785f, 1f);

	public static Color SelectedColor = new Color(1f, 1f, 1f);

	public static Color ColorLabel2Default = new Color(1f, 8f / 15f, 12f / 85f);

	public static Color ColorLabel2Selected = new Color(0f, 0f, 0f);

	public const string DefaultSprite = "TransparentSprite";

	public const string SelectedSprite = "Highlight";

	[FormerlySerializedAs("item")]
	public Item Item;

	public UISprite Background;

	public UISprite Highlight;

	public UISprite Icon;

	[FormerlySerializedAs("IconFg")]
	public UISprite IconForeground;

	public UISprite CheckIcon;

	[FormerlySerializedAs("lblName")]
	public UILabel NameLabel;

	[FormerlySerializedAs("lblInfo")]
	public UILabel InfoLabel;

	public UISprite ModifierSprite;

	public UILabel ModifierLabel;

	private bool isSelected;

	private bool isEnabled;

	public int CategoryNumber;

	public bool hasModifier;

	public int modifierRarity;

	public ItemModifier modifier;

	public bool Selected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (value == isSelected)
			{
				return;
			}
			isSelected = value;
			if (isSelected)
			{
				if (Highlight != null)
				{
					Highlight.enabled = true;
				}
				if (Background != null)
				{
					Background.color = SelectedColor;
					Background.spriteName = "Highlight";
				}
			}
			else
			{
				if (Highlight != null)
				{
					Highlight.enabled = false;
				}
				if (Background != null)
				{
					Background.color = DefaultColor;
					Background.spriteName = "TransparentSprite";
				}
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
		}
	}

	public event Action<UIItem> Clicked;

	public virtual void Init(Item item)
	{
		Item = item;
		NameLabel.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		if (item is InventoryItem inventoryItem)
		{
			modifier = inventoryItem.modifier;
		}
		Icon.spriteName = item.Icon;
		Icon.color = ((item.IsEquipType && !item.CanBeEquipped) ? new Color(0.5f, 0.5f, 0.5f, 0.8f) : Color.white);
		if (IconForeground != null)
		{
			IconForeground.gameObject.SetActive(!string.IsNullOrEmpty(item.IconFg));
			IconForeground.spriteName = item.IconFg;
		}
		if (item.MaxStack > 1 && item.Qty > 1)
		{
			UILabel nameLabel = NameLabel;
			nameLabel.text = nameLabel.text + " [000000]x" + item.Qty + "[-]";
		}
		if ((item.HasStats && modifier != null) || modifierRarity != 0)
		{
			if (modifier != null)
			{
				modifierRarity = (int)modifier.rarity;
			}
			if (modifierRarity >= 2)
			{
				hasModifier = true;
			}
			else
			{
				hasModifier = false;
			}
		}
		if (!hasModifier)
		{
			ModifierSprite.gameObject.SetActive(value: false);
			ModifierLabel.gameObject.SetActive(value: false);
			InfoLabel.text = item.GetTagline();
		}
		else
		{
			ModifierLabel.text = item.GetTagline();
			ModifierSprite.spriteName = setModifierIcon(modifierRarity);
			ModifierSprite.gameObject.SetActive(value: true);
			ModifierLabel.gameObject.SetActive(value: true);
			InfoLabel.gameObject.SetActive(value: false);
		}
	}

	public static string setModifierIcon(int modifierRarity)
	{
		return modifierRarity switch
		{
			2 => "Modifier_Rare_Icon_green", 
			3 => "Modifier_Rare_Icon_blue", 
			4 => "Modifier_Rare_Icon_purple", 
			5 => "Modifier_Rare_Icon_red", 
			6 => "Modifier_Rare_Icon_diamond", 
			_ => "", 
		};
	}

	protected virtual void OnClick()
	{
		AudioManager.Play2DSFX("sfx_engine_btnpress");
		OnClicked();
	}

	private void OnEnable()
	{
	}

	protected void OnClicked()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this);
		}
	}

	protected void OnHover(bool isOver)
	{
		if (!isOver)
		{
			_ = Selected;
		}
	}

	public virtual void OnTooltip(bool show)
	{
		if (Item != null)
		{
			if (show)
			{
				Tooltip.ShowAtPosition(Item.GetToolTip(), UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}
}
