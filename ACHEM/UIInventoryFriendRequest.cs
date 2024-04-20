using UnityEngine;

public class UIInventoryFriendRequest : UIItem
{
	private IMessage friendRequest;

	public void Init(IMessage request)
	{
		friendRequest = request;
		NameLabel.text = friendRequest.GetName();
		InfoLabel.text = friendRequest.GetMessage();
	}

	protected override void OnClick()
	{
		if (friendRequest is ResponseFriend)
		{
			RequestClick();
		}
		else if (friendRequest is ResponseSummon)
		{
			GotoClick();
		}
	}

	private void RequestClick()
	{
		Confirmation.Show("Friend Request", "Add " + friendRequest.GetName() + " as a friend?", delegate(bool b)
		{
			RequestFriendAdd r = new RequestFriendAdd
			{
				FriendID = friendRequest.GetID(),
				Confirm = b
			};
			Session.MyPlayerData.friendRequests.Remove(friendRequest);
			Game.Instance.aec.sendRequest(r);
			Object.Destroy(base.gameObject);
		});
	}

	private void GotoClick()
	{
		ResponseSummon response = (ResponseSummon)friendRequest;
		Confirmation.Show("Go To Friend", "Go to " + friendRequest.GetName() + " now?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendGotoRequest(response.FriendID, "");
			}
			Session.MyPlayerData.friendRequests.Remove(friendRequest);
			Object.Destroy(base.gameObject);
		});
	}
}
