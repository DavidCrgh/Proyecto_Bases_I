using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;

[DataContract]
public class clSede
{

    [DataMember]
    public decimal IDSede { get; set; }
    [DataMember]
    public string RazonSocial { get; set; }
    [DataMember]
    public string forUI { get; set; }

    public clSede(
        decimal IDSede,
        string RazonSocial)
    {
        this.IDSede = IDSede;
        this.RazonSocial = RazonSocial;
        forUI = toString();
    }

    public string toString() {
        return IDSede + " - " + RazonSocial;
    }
}