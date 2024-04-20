using UnityEngine;

public class UIMailCompose : MonoBehaviour
{
	public UILabel lblSubject;

	public UILabel lblSender;

	public UILabel lblMessage;

	public UISprite Icon;

	public UIScrollView DescriptionScroll;

	public GameObject ComposeObject;

	public void OnBackBtnClicked()
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnSendMailClicked()
	{
		MailMessage mail = new MailMessage(Session.MyPlayerData.ID, lblSubject.text, Entities.Instance.me.name, lblMessage.text);
		Game.Instance.SendSendMailRequest(mail, lblSender.text);
		base.gameObject.SetActive(value: false);
	}
}
