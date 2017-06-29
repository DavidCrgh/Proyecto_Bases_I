using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]

public class clEmpleadoANNII
{

    [DataMember]
    public string Apellidos { get; set; }
    [DataMember]
    public string Nombre { get; set; }
    [DataMember]
    public string Especialidad { get; set; }
    [DataMember]
    public decimal IDEmpleado { get; set; }
    [DataMember]
    public decimal IDEspecialidad { get; set; }
    [DataMember]
    public string forUI { get; set; }

    public clEmpleadoANNII(
        string Apellidos,
        string Nombre,
        string Especialidad,
        decimal IDEmpleado,
        decimal IDEspecialidad) {
        this.Apellidos = Apellidos;
        this.Nombre = Nombre;
        this.Especialidad = Especialidad;
        this.IDEmpleado = IDEmpleado;
        this.IDEspecialidad = IDEspecialidad;
        forUI = toString();
    }

    public string toString() {
        return Apellidos + " " + Nombre + " (" + Especialidad + ")" + " ID: " + IDEmpleado;
    }
}