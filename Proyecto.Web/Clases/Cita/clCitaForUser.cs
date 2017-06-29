using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clCitaForUser
{

    [DataMember]
    public decimal Identificador { get; set; }
    [DataMember]
    public decimal Sede { get; set; }
    [DataMember]
    public string Médico { get; set; }
    [DataMember]
    public string Especialidad { get; set; }
    [DataMember]
    public string Cédula { get; set; }
    [DataMember]
    public string Nombre { get; set; }
    [DataMember]
    public DateTime Fecha { get; set; }
    [DataMember]
    public string Observaciones { get; set; }
    [DataMember]
    public string Estado { get; set; }
    [DataMember]
    public decimal? Factura { get; set; }

    public clCitaForUser(
        decimal IDCita,
        decimal Sede,
        string Medico,
        string Especialidad,
        string CedulaPaciente,
        string NombrePaciente,
        string DiaCita,
        DateTime FechaCita,
        string Observaciones,
        string Estado,
        decimal? IDFactura) {
        this.Identificador = IDCita;
        this.Sede = Sede;
        this.Médico = Medico;
        this.Especialidad = Especialidad;
        this.Cédula = CedulaPaciente;
        this.Nombre = NombrePaciente;
        this.Fecha = FechaCita;
        this.Observaciones = Observaciones;
        this.Estado = Estado;
        this.Factura = IDFactura;
    }

}
