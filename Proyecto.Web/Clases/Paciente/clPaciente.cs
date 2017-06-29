using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clPaciente
{

    [DataMember]
    public string Cedula { get; set; }
    [DataMember]
    public string Nombre { get; set; }
    [DataMember]
    public string Apellido { get; set; }
    [DataMember]
    public DateTime FechaNacimiento { get; set; }
    [DataMember]
    public string EstadoCivil { get; set; }
    [DataMember]
    public string Ocupacion { get; set; }
    [DataMember]
    public string Foto { get; set; }
    [DataMember]
    public string Direccion { get; set; }
    [DataMember]
    public string Tipo { get; set; }

    public clPaciente(
        string Cedula,
        string Nombre,
        string Apellido,
        DateTime FechaNacimiento,
        string EstadoCivil,
        string Ocupacion,
        string Foto,
        string Direccion,
        string Tipo)
    {
        this.Cedula = Cedula;
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.FechaNacimiento = FechaNacimiento;
        this.EstadoCivil = EstadoCivil;
        this.Ocupacion = Ocupacion;
        this.Foto = Foto;
        this.Direccion = Direccion;
        this.Tipo = Tipo;
    }

}