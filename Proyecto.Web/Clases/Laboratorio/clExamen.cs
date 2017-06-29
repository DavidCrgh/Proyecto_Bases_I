using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clExamen
{

	[DataMember]
	public decimal IDExamen { get; set; }
	[DataMember]
	public string Nombre { get; set; }
	[DataMember]
	public string Descripcion { get; set; }

	public clExamen(
		decimal IDExamen,
		string Nombre,
		string Descripcion)
	{
		this.IDExamen = IDExamen;
		this.Nombre = Nombre;
		this.Descripcion = Descripcion;
	}

}
