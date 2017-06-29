using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clCita
{

	[DataMember]
	public decimal IDCita { get; set; }
	[DataMember]
	public decimal Sede { get; set; }
	[DataMember]
	public decimal Medico { get; set; }
	[DataMember]
	public decimal Especialidad { get; set; }
	[DataMember]
	public string CedulaPaciente { get; set; }
	[DataMember]
	public string DiaCita { get; set; }
	[DataMember]
	public decimal HoraCita { get; set; }
	[DataMember]
	public DateTime FechaCita { get; set; }
	[DataMember]
	public string Observaciones { get; set; }
	[DataMember]
	public string Estado { get; set; }
	[DataMember]
	public decimal? IDFactura { get; set; }

	public clCita(
		decimal IDCita,
		decimal Sede,
		decimal Medico,
		decimal Especialidad,
		string CedulaPaciente,
		string DiaCita,
		decimal HoraCita,
		DateTime FechaCita,
		string Observaciones,
		string Estado,
		decimal? IDFactura)
	{
		this.IDCita = IDCita;
		this.Sede = Sede;
		this.Medico = Medico;
		this.Especialidad = Especialidad;
		this.CedulaPaciente = CedulaPaciente;
		this.DiaCita = DiaCita;
		this.HoraCita = HoraCita;
		this.FechaCita = FechaCita;
		this.Observaciones = Observaciones;
		this.Estado = Estado;
		this.IDFactura = IDFactura;
	}

}
