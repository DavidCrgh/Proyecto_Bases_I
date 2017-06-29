using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clItem
{

	[DataMember]
	public decimal IDExamen { get; set; }
	[DataMember]
	public decimal IDItem { get; set; }
	[DataMember]
	public string Nombre { get; set; }
	[DataMember]
	public string ExpresionRegular { get; set; }

	public clItem(
		decimal IDExamen,
		decimal IDItem,
		string Nombre,
		string ExpresionRegular)
	{
		this.IDExamen = IDExamen;
		this.IDItem = IDItem;
		this.Nombre = Nombre;
		this.ExpresionRegular = ExpresionRegular;
	}

}
