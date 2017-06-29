using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clPacienteCNA
{

    [DataMember]
    public string Cedula { get; set; }
    [DataMember]
    public string Nombre { get; set; }
    [DataMember]
    public string Apellido { get; set; }
    [DataMember]
    public string forUI { get; set; }

    public clPacienteCNA(
        string Cedula,
        string Nombre,
        string Apellido) {
        this.Cedula = Cedula;
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        forUI = toString();
    }

    public string toString() {
        return Apellido + " " + Nombre + " (" + Cedula + ")";
    }

}
