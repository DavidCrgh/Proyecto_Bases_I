using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]

public class clEmpleadoNI
{

    [DataMember]
    public string Apellidos { get; set; }
    [DataMember]
    public string Nombre { get; set; }
    [DataMember]
    public decimal IDEmpleado { get; set; }
    [DataMember]
    public string forUI { get; set; }

    public clEmpleadoNI(
        string Apellidos,
        string Nombre,
        decimal IDEmpleado) {
        this.Apellidos = Apellidos;
        this.Nombre = Nombre;
        this.IDEmpleado = IDEmpleado;
        forUI = toString();
    }

    public string toString() {
        return Apellidos + " " + Nombre + " ID: " + IDEmpleado;
    }
}