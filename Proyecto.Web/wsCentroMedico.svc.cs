using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.ObjectModel;
using cnCentroMedico;

namespace Proyecto.Web
{
    [ServiceContract(Namespace = "http://tempuri.org")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class wsCentroMedico
    {

        //Actualizar un examen
        [OperationContract]
        public bool actualizarExamen(clExamen examen)
        {
            var dataBase = new dcCentroMedico();
            dataBase.actualizarExamen(examen.IDExamen, examen.Nombre, examen.Descripcion);
            return true;
        }

        //Actualizar un item
        [OperationContract]
        public bool actualizarItem(clItem item)
        {
            var dataBase = new dcCentroMedico();
            dataBase.actualizarItem(item.IDExamen, item.IDItem, item.Nombre, item.ExpresionRegular);
            return true;
        }

        //Eliminar una cita :(
        [OperationContract]
        public bool eliminarCita(decimal IDCita)
        {
            var dataBase = new dcCentroMedico();
            dataBase.eliminarCitaPorID(IDCita);
            return true;
        }

        //Eliminar un examen
        [OperationContract]
        public bool eliminarExamen(decimal IDExamen)
        {
            var dataBase = new dcCentroMedico();
            dataBase.eliminarExamenPorID(IDExamen);
            return true;
        }

        //Eliminar un item
        [OperationContract]
        public bool eliminarItem(decimal IDItem, decimal IDExamen)
        {
            var dataBase = new dcCentroMedico();
            dataBase.eliminarItemPorID(IDItem, IDExamen);
            return true;
        }

        //Ingresar nueva vita
        [OperationContract]
        public bool registrarCita(clCita cita)
        {
            enCentroMedico.CITA tempCita = new enCentroMedico.CITA();
            tempCita.IDCITA = cita.IDCita;
            tempCita.SEDE = cita.Sede;
            tempCita.MEDICO = cita.Medico;
            tempCita.ESPECIALIDAD = cita.Especialidad;
            tempCita.CEDULAPACIENTE = cita.CedulaPaciente;
            tempCita.DIACITA = cita.DiaCita[0];
            tempCita.HORACITA = cita.HoraCita;
            tempCita.FECHACITA = cita.FechaCita;
            tempCita.OBSERVACIONES = cita.Observaciones;
            tempCita.ESTADO = cita.Estado[0];
            tempCita.IDFACTURA = cita.IDFactura;
            var dataBase = new dcCentroMedico();
            //dataBase.Connection.BeginTransaction()
            dataBase.CITAs.InsertOnSubmit(tempCita);
            dataBase.SubmitChanges();
            return true;
        }

        //Registrar examen con sus items
        [OperationContract]
        public bool registrarExamenItems(clExamen examen, List<clItem> items)
        {
            var dataBase = new dcCentroMedico();
            dataBase.Connection.Open();

            using (dataBase.Transaction = dataBase.Connection.BeginTransaction())
            {
                try
                {
                    enCentroMedico.EXAMEN tempExamen = new enCentroMedico.EXAMEN();
                    tempExamen.IDEXAMEN = examen.IDExamen;
                    tempExamen.NOMBRE = examen.Nombre;
                    tempExamen.DESCRIPCION = examen.Descripcion;
                    dataBase.EXAMENs.InsertOnSubmit(tempExamen);
                    dataBase.SubmitChanges();

                    foreach (var item in items)
                    {
                        enCentroMedico.ITEM tempItem = new enCentroMedico.ITEM();
                        tempItem.IDEXAMEN = examen.IDExamen;
                        tempItem.IDITEM = item.IDItem;
                        tempItem.NOMBRE = item.Nombre;
                        tempItem.EXPRESIONREGULAR = item.ExpresionRegular;
                        dataBase.ITEMs.InsertOnSubmit(tempItem);
                    }

                    dataBase.SubmitChanges();
                    dataBase.Transaction.Commit();
                    dataBase.Connection.Close();
                    Console.WriteLine("Exitoso");
                    return true;
                }
                catch
                {
                    dataBase.Transaction.Rollback();
                    dataBase.Connection.Close();
                    Console.WriteLine("Fallido");
                    return false;
                }
            }
        }

        //Registrar un item solo
        [OperationContract]
        public bool registrarItem(clItem item)
        {
            enCentroMedico.ITEM tempItem = new enCentroMedico.ITEM();
            tempItem.IDEXAMEN = item.IDExamen;
            tempItem.IDITEM = item.IDItem;
            tempItem.NOMBRE = item.Nombre;
            tempItem.EXPRESIONREGULAR = item.ExpresionRegular;
            var dataBase = new dcCentroMedico();
            dataBase.ITEMs.InsertOnSubmit(tempItem);
            dataBase.SubmitChanges();
            return true;
        }

        //Obtengo los IDs de citas
        [OperationContract]
        public ObservableCollection<decimal> getIDCitas()
        {
            var lista = new ObservableCollection<decimal>();
            var dataBase = new dcCentroMedico();
            var vID = dataBase.obtenerIDCitas();

            foreach (var ID in vID)
            {
                lista.Add(ID.IDCITA);
            }

            return lista;
        }

        //Obtengo los IDs de Examenes
        [OperationContract]
        public ObservableCollection<decimal> getIDExamenes()
        {
            var lista = new ObservableCollection<decimal>();
            var dataBase = new dcCentroMedico();
            var vID = dataBase.obtenerIDExamenes();

            foreach (var ID in vID)
            {
                lista.Add(ID.IDEXAMEN);
            }

            return lista;
        }

        //Obtengo los IDs de Items para un Examen
        [OperationContract]
        public ObservableCollection<decimal> getIDItems(decimal IDExamen)
        {
            var lista = new ObservableCollection<decimal>();
            var dataBase = new dcCentroMedico();
            var vID = dataBase.obtenerIDItems(IDExamen);

            foreach (var ID in vID)
            {
                lista.Add(ID.IDITEM);
            }

            return lista;
        }

        //Obtengo los Examenes
        [OperationContract]
        public ObservableCollection<clExamen> getExamenes()
        {
            var lista = new ObservableCollection<clExamen>();
            var dataBase = new dcCentroMedico();
            var vExamen = dataBase.obtenerExamenes();
            clExamen tempExamen;

            foreach (var examen in vExamen)
            {
                tempExamen = new clExamen(examen.IDExamen, examen.Nombre, examen.Descripcion);
                lista.Add(tempExamen);
            }
            return lista;
        }

        //Se obtiene un solo examen dado
        [OperationContract]
        public clExamen getExamen(decimal idExamen)
        {
            var dataBase = new dcCentroMedico();
            var vExamen = dataBase.obtenerExamen(idExamen).First();
            clExamen tempExamen = new clExamen(vExamen.IDExamen, vExamen.Nombre, vExamen.Descripcion);
            return tempExamen;
        }

        //Se obtiene un solo item dado
        [OperationContract]
        public clItem getItem(decimal idExamen, decimal idItem)
        {
            var dataBase = new dcCentroMedico();
            var vItem = dataBase.obtenerItem(idExamen, idItem).First();
            clItem tempItem = new clItem(vItem.IDExamen, vItem.IDItem, vItem.Nombre, vItem.ExpresionRegular);
            return tempItem;
        }

        //Se obtiene una sola cita dad
        [OperationContract]
        public clCita getCita(decimal idCita)
        {
            var dataBase = new dcCentroMedico();
            var vExamen = dataBase.obtenerCita(idCita).First();
            clCita tempCita = new clCita(
                vExamen.IDCITA, 
                vExamen.SEDE, 
                vExamen.MEDICO, 
                vExamen.ESPECIALIDAD, 
                vExamen.CEDULAPACIENTE, 
                vExamen.DIACITA + "", 
                vExamen.HORACITA, 
                vExamen.FECHACITA, 
                vExamen.OBSERVACIONES, 
                vExamen.ESTADO + "", 
                vExamen.IDFACTURA
                );
            return tempCita;
        }

        //Obtengo los items
        [OperationContract]
        public ObservableCollection<clItem> getItems()
        {
            var lista = new ObservableCollection<clItem>();
            var dataBase = new dcCentroMedico();
            var vItem = dataBase.obtenerItems();
            clItem tempItem;

            foreach (var item in vItem)
            {
                tempItem = new clItem(item.IDExamen, item.IDItem, item.Nombre, item.ExpresionRegular);
                lista.Add(tempItem);
            }
            return lista;
        }

        //Obtengo las citas
        [OperationContract]
        public ObservableCollection<clCitaForUser> getCitas()
        {
            var lista = new ObservableCollection<clCitaForUser>();
            var dataBase = new dcCentroMedico();
            var vCita = dataBase.obtenerCitas();
            clCitaForUser tempCita;

            foreach (var cita in vCita)
            {
                tempCita = new clCitaForUser(
                    cita.Identificador,
                    cita.Sede,
                    cita.MedicoA + " " + cita.MedicoN + " - " + cita.IDMedico,
                    cita.Especialidad + " - " + cita.IDEspecialidad,
                    cita.Cedula,
                    cita.Apellido + " " + cita.Nombre,
                    getDia((int)cita.Fecha.DayOfWeek),
                    cita.Fecha.AddHours(-1 * cita.Fecha.Hour).AddHours((double)cita.Hora),
                    cita.Observaciones,
                    getEstado(cita.Estado),
                    cita.Factura
                );
                lista.Add(tempCita);
            }

            return lista;
        }

        //Obtengo los pacientes
        [OperationContract]
        public ObservableCollection<clPacienteCNA> getPacientes()
        {
            var lista = new ObservableCollection<clPacienteCNA>();
            var dataBase = new dcCentroMedico();
            var vPaciente = dataBase.obtenerCedulaNombrePacientes();
            clPacienteCNA tempPaciente;

            foreach (var fila in vPaciente)
            {
                tempPaciente = new clPacienteCNA(
                    fila.CEDULA,
                    fila.NOMBRE,
                    fila.APELLIDO
                );
                lista.Add(tempPaciente);
            }
            return lista;
        }

        //Obtengo los médicos
        [OperationContract]
        public ObservableCollection<clEmpleadoANNII> getMedicos(int sede)
        {
            var lista = new ObservableCollection<clEmpleadoANNII>();
            var dataBase = new dcCentroMedico();
            var vMedico = dataBase.obtenerMedicosEspecialiadPorSede(sede);
            clEmpleadoANNII tempMedico;

            foreach (var fila in vMedico)
            {
                tempMedico = new clEmpleadoANNII(
                    fila.APELLIDOS,
                    fila.NOMBRE,
                    fila.ESPECIALIDAD,
                    fila.IDEMPLEADO,
                    fila.IDESPECIALIDAD
                );
                lista.Add(tempMedico);
            }
            return lista;
        }

        //Obtengo médicos (Solo médico)
        [OperationContract]
        public ObservableCollection<clEmpleadoNI> getMedicosSolos()
        {
            var lista = new ObservableCollection<clEmpleadoNI>();
            var dataBase = new dcCentroMedico();
            var vMedicoO = dataBase.obtenerMedicos();
            clEmpleadoNI tempMedicoO;

            foreach (var fila in vMedicoO)
            {
                tempMedicoO = new clEmpleadoNI(
                    fila.APELLIDOS,
                    fila.NOMBRE,
                    fila.IDEMPLEADO
                );
                lista.Add(tempMedicoO);
            }
            return lista;
        }

        //Obtengo la consulta sobre citas por periodo de un médico
        [OperationContract]
        public ObservableCollection<clCitaForUser> getCitasPorPeriodoMedico(decimal medico, DateTime inicio, DateTime fin)
        {
            var lista = new ObservableCollection<clCitaForUser>();
            var dataBase = new dcCentroMedico();
            var vCita = dataBase.obtenerCitasParaMedicoPorPeriodo(medico, inicio, fin);
            clCitaForUser tempCita;

            foreach (var cita in vCita)
            {
                tempCita = new clCitaForUser(
                    cita.Identificador,
                    cita.Sede,
                    cita.MedicoA + " " + cita.MedicoN + " - " + cita.IDMedico,
                    cita.Especialidad + " - " + cita.IDEspecialidad,
                    cita.Cedula,
                    cita.Apellido + " " + cita.Nombre,
                    getDia((int)cita.Fecha.DayOfWeek),
                    cita.Fecha.AddHours(-1 * cita.Fecha.Hour).AddHours((double)cita.Hora),
                    cita.Observaciones,
                    getEstado(cita.Estado),
                    cita.Factura
                );
                lista.Add(tempCita);
            }

            return lista;
        }

        //Obtengo las sedes
        [OperationContract]
        public ObservableCollection<clSede> getSedes()
        {
            var lista = new ObservableCollection<clSede>();
            var dataBase = new dcCentroMedico();
            var vSede = dataBase.obtenerSedes();
            clSede tempSede;

            foreach (var fila in vSede)
            {
                tempSede = new clSede(
                    fila.IDSEDE,
                    fila.RAZONSOCIAL
                );
                lista.Add(tempSede);
            }
            return lista;
        }

        //Obtengo horarios
        [OperationContract]
        public ObservableCollection<clHorario> getHorarios(decimal Medico, decimal Sede, string Dia, DateTime Fecha)
        {
            var lista = new ObservableCollection<clHorario>();
            var dataBase = new dcCentroMedico();
            var vHorario = dataBase.obtenerDisponibilidadMedicoPorDia(Medico, Sede, Dia, Fecha);
            clHorario tempHorario;

            foreach (var fila in vHorario)
            {
                tempHorario = new clHorario(
                    fila.DIA.ToString(),
                    fila.HORA
                );
                lista.Add(tempHorario);
            }
            return lista;
        }

        //Dado un día [0, 6] retorna la letra correspondiente 0 -> D | 2 -> K | 6 -> S 
        private string getDia(int dia)
        {
            string[] Dias = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            return Dias[dia];
        }

        //Dada la inicial de un estado, retorna el estado completo
        private string getEstado(char estado)
        {
            switch (estado)
            {
                case 'C':
                    return "Cancelada";
                case 'A':
                    return "Ausente";
                case 'R':
                    return "Realizada";
                default:
                    return "Programada";
            }
        }

    }
}
