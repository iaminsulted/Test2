using System;

[Serializable]
public class FinalizeTxn
{
	public long orderid;

	public int appid;

	public string Postdata => "";

	public FinalizeTxn(long _orderid, int _appid)
	{
		orderid = _orderid;
		appid = _appid;
	}
}
