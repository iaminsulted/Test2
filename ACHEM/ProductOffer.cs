using System;

public class ProductOffer
{
	public ProductID ProductID;

	public DateTime ExpireUTC;

	public ProductOffer()
	{
	}

	public ProductOffer(ProductID productID, DateTime expireUTC)
	{
		ProductID = productID;
		ExpireUTC = expireUTC;
	}
}
