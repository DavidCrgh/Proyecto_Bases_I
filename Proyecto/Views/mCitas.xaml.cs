using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Proyecto.Views
{
    public partial class mCitas : Page
    {
        ws.wsCentroMedicoClient servicio = new ws.wsCentroMedicoClient();
        List<ws.clSede> Sedes = null;
        List<ws.clEmpleadoANNII> Medicos = null;
        List<ws.clPacienteCNA> Pacientes = null;
        List<ws.clHorario> Horarios = null;
        List<ws.clCitaForUser> Citas = null;
        List<decimal> IDCitas = null;

        public mCitas() {
            InitializeComponent();
            InitializeUI();
            servicio.getCitasCompleted += new EventHandler<ws.getCitasCompletedEventArgs>(cargaCitas);
            servicio.getCitasAsync();
            servicio.getSedesCompleted += new EventHandler<ws.getSedesCompletedEventArgs>(cargaSedes);
            servicio.getSedesAsync();
            servicio.getPacientesCompleted += new EventHandler<ws.getPacientesCompletedEventArgs>(cargaPacientes);
            servicio.getPacientesAsync();
            servicio.getMedicosCompleted += new EventHandler<ws.getMedicosCompletedEventArgs>(cargaMedicos);
            servicio.getHorariosCompleted += new EventHandler<ws.getHorariosCompletedEventArgs>(cargaHorarios);
            servicio.getIDCitasCompleted += new EventHandler<ws.getIDCitasCompletedEventArgs>(cargaIDCitas);
            servicio.getIDCitasAsync();
            servicio.registrarCitaCompleted += new EventHandler<ws.registrarCitaCompletedEventArgs>(registraCita);
            servicio.eliminarCitaCompleted += new EventHandler<ws.eliminarCitaCompletedEventArgs>(eliminaCita);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e) {

        }
        
        //Insertar una nueva cita
        public void BN_Insertar_Click (object sender, EventArgs e) {
            try {
                ws.clCita tmp = new ws.clCita();
                tmp.IDCita = IDCitas.Last() + 1;
                tmp.Sede = Sedes[CB_Sede.SelectedIndex].IDSede;
                tmp.Medico = Medicos[CB_Medicos.SelectedIndex].IDEmpleado;
                tmp.Especialidad = Medicos[CB_Medicos.SelectedIndex].IDEspecialidad;
                tmp.CedulaPaciente = Pacientes[CB_Pacientes.SelectedIndex].Cedula;
                DateTime Fecha = DP_Fecha.SelectedDate ?? DateTime.Now;
                tmp.DiaCita = getDia((int)Fecha.DayOfWeek);
                tmp.HoraCita = Horarios[CB_Horarios.SelectedIndex].Hora;
                tmp.FechaCita = Fecha;
                tmp.Observaciones = TB_Observaciones.Text + " ";
                tmp.Estado = CB_Tipo.SelectedItem.ToString();
                servicio.registrarCitaAsync(tmp);
            } catch {

            }
        }

        //Evento lanzado luego de que RegistrarCitaAsync() haya finalizado
        public void registraCita(object sender, ws.registrarCitaCompletedEventArgs e) {
            servicio.getCitasAsync();
            servicio.getIDCitasAsync();
            servicio.getSedesAsync();
        }

        public void BN_Actualizar_Click (object sender, EventArgs e) {

        }

        //Eliminar una cita, poor cita
        public void BN_Borrar_Click (object sender, EventArgs e) {
            try {
                servicio.eliminarCitaAsync(IDCitas[CB_ID_Delete.SelectedIndex]);
            } catch {

            }
        }

        //Evento lanzado al finalizar eliminarCitaAsync()
        public void eliminaCita(object sender, ws.eliminarCitaCompletedEventArgs e) {
            servicio.getCitasAsync();
            servicio.getIDCitasAsync();
            servicio.getSedesAsync();
        }

        //Evento que se ejecuta luego de que getIDCitasAsync() obtiene los datos de ID citas de la BD
        public void cargaIDCitas(object sender, ws.getIDCitasCompletedEventArgs e) {
            IDCitas = e.Result.ToList();
            CB_ID_Delete.Items.Clear();
            CB_ID_Update.Items.Clear();
            foreach (var ID in IDCitas) {
                CB_ID_Delete.Items.Add(ID);
                CB_ID_Update.Items.Add(ID);
            }
        }

        //Evento que se ejecuta luego de que getCitasAsync() obtengA los datos de citas de la BD
        public void cargaCitas(object sender, ws.getCitasCompletedEventArgs e) {
            Citas = e.Result.ToList();
            DG_Citas.ItemsSource = e.Result;
        }

        //Evento que se ejecuta luego de que getPacientesAsync() obtenga los datos de pacientes de la BD
        public void cargaPacientes(object sender, ws.getPacientesCompletedEventArgs e) {
            Pacientes = e.Result.ToList();
            CB_Pacientes.Items.Clear();
            foreach (var paciente in Pacientes) {
               CB_Pacientes.Items.Add(paciente.forUI);
            }
        }

        //Evento que se ejecuta luego de que getSedesAsync() obtenga los datos de sedes de la BD
        public void cargaSedes(object sender, ws.getSedesCompletedEventArgs e) {
            Sedes = e.Result.ToList();
            CB_Sede.Items.Clear();
            foreach (var sede in Sedes) {
                CB_Sede.Items.Add(sede.forUI);
            }
            if (Sedes.Count > 0) {
                actualizarMedicos(Sedes.First().IDSede);
            }
        }

        //Actualiza los médicos disponibles en base a una sede
        public void actualizarMedicos(decimal sede) {
            try {
                servicio.getMedicosAsync(Convert.ToInt32(sede));
            } catch {

            }
        }

        //Evento que se ejecuta luego de que getMedicosAsync() obtenga los datos de medicos de la BD
        public void cargaMedicos(object sender, ws.getMedicosCompletedEventArgs e) {
            Medicos = e.Result.ToList();
            CB_Medicos.Items.Clear();
            foreach (var medico in Medicos) {
                CB_Medicos.Items.Add(medico.forUI);
            }
        }

        //Se acciona cuando el usuario cambia el elemento seleccionado del ComboBox de sedes
        public void CB_Sede_ItemChanged(object sender, EventArgs e) {
            try {
                decimal sede = Sedes[CB_Sede.SelectedIndex].IDSede;
                actualizarMedicos(sede);
            } catch {
                decimal sede = Sedes.First().IDSede;
                actualizarMedicos(sede);
            }
        }

        //Evento que se ejecuta luego de que getHorariosAsync() obtenga los datos de horarios de la BD
        public void cargaHorarios(object sender, ws.getHorariosCompletedEventArgs e) {
            Horarios = e.Result.ToList();
            CB_Horarios.Items.Clear();
            foreach (var horario in Horarios) {
                CB_Horarios.Items.Add(horario.forUI);
            }
        }

        //Actualiza los horarios disponibles en base a un médico para una sede, en un día, para una fecha
        public void actualizarHorarios(decimal Medico, decimal Sede, string Dia, DateTime Fecha) {
            servicio.getHorariosAsync(Medico, Sede, Dia, Fecha);
        }

        //Evento que se ejecuta luego de que el usuario seleccione un médico en el ComboBox de médicos
        public void CB_Medicos_ItemChanged(object sender, EventArgs e) {
            try {
                decimal Medico = Medicos[CB_Medicos.SelectedIndex].IDEmpleado;
                decimal Sede = Sedes[CB_Sede.SelectedIndex].IDSede;
                DateTime Fecha = DP_Fecha.SelectedDate ?? DateTime.Now;
                string Dia = getDia((int)Fecha.DayOfWeek);
                actualizarHorarios(Medico, Sede, Dia, Fecha);
            } catch {

            }
        }

        //Evento que se acciona luego de que el usuario seleccione una fecha en el DatePicker
        private void DP_Fecha_ItemChanged(object sender, EventArgs e) {
            try {
                decimal Medico = Medicos[CB_Medicos.SelectedIndex].IDEmpleado;
                decimal Sede = Sedes[CB_Sede.SelectedIndex].IDSede;
                DateTime Fecha = DP_Fecha.SelectedDate ?? DateTime.Now;
                string Dia = getDia((int)Fecha.DayOfWeek);
                actualizarHorarios(Medico, Sede, Dia, Fecha);
            } catch {

            }
        }

        //Dado un día [0, 6] retorna la letra correspondiente 0 -> D | 2 -> K | 6 -> S 
        private string getDia(int dia) {
            return "DLKMJVS"[dia].ToString();
        }

        //Evento que se ejecuta al cambiar el item de tipos
        public void CB_Tipo_ItemChanged(object sender, EventArgs e) {
            if (CB_Tipo.SelectedItem.ToString().Equals("Programada")) {
                TB_IDFactura.IsEnabled = false;
            } else {
                TB_IDFactura.IsEnabled = true;
            }
        }

        //Inicializa contextos de elementos gráficos
        private void InitializeUI() {
            string[] Tipos = { "Programada", "Ausente", "Cancelada", "Realizada" };
            DP_Fecha.DisplayDateStart = DateTime.Now;
            DP_Fecha.DisplayDateEnd = DateTime.MaxValue;
            DP_Fecha.SelectedDate = DateTime.Now;
            foreach (string tipo in Tipos) {
                CB_Tipo.Items.Add(tipo);
            }
            CB_Tipo.SelectedIndex = 0;
        }

    }
}
