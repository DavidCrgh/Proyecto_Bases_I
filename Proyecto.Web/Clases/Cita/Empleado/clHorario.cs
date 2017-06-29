using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clHorario
{

	[DataMember]
	public string Dia { get; set; }
	[DataMember]
	public decimal Hora { get; set; }
    [DataMember]
    public string forUI { get; set; }

	public clHorario(
		string Dia,
		decimal Hora)
	{
		this.Dia = Dia;
		this.Hora = Hora;
        forUI = toString();
	}

    public string toString() {
        return Hora + ":00";
    }
}
