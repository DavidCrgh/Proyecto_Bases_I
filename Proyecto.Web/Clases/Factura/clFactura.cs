using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clFactura
{

	[DataMember]
	public decimal IDFactura { get; set; }
	[DataMember]
	public DateTime FechaFactura { get; set; }
	[DataMember]
	public string CedulaPaciente { get; set; }

	public clFactura(
		decimal IDFactura,
		DateTime FechaFactura,
		string CedulaPaciente)
	{
		this.IDFactura = IDFactura;
		this.FechaFactura = FechaFactura;
		this.CedulaPaciente = CedulaPaciente;
	}

}
