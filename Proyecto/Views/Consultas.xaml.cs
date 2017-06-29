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
    public partial class Consultas : Page
    {

        ws.wsCentroMedicoClient servicio = new ws.wsCentroMedicoClient();
        List<ws.clEmpleadoNI> Empleados = null;

        public Consultas() {
            InitializeComponent();
            InitializeUI();
            servicio.getCitasPorPeriodoMedicoCompleted += new EventHandler<ws.getCitasPorPeriodoMedicoCompletedEventArgs>(cargaCitas);
            servicio.getMedicosSolosCompleted += new EventHandler<ws.getMedicosSolosCompletedEventArgs>(cargaMedicos);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e) {

        }
        //CONSULTA MEDICOS
        public void BN_MedicosFacturados_Click (object sender, EventArgs e) {
            SP_Citas.Visibility = Visibility.Collapsed;
            SP_Medicos.Visibility = Visibility.Visible;
            SP_Pacientes.Visibility = Visibility.Collapsed;
            DG_ConsultasData.ItemsSource = null;
        }

        //CONSULTA CITAS
        //Desplegar gráfica consulta citas
        public void BN_CitasProgramadas_Click(object sender, EventArgs e) {
            DG_ConsultasData.ItemsSource = null;
            SP_Citas.Visibility = Visibility.Visible;
            SP_Medicos.Visibility = Visibility.Collapsed;
            SP_Pacientes.Visibility = Visibility.Collapsed;
            servicio.getMedicosSolosAsync();
        }

        //Realizar Consulta citas
        public void BN_Consulta_Citas_Click(object sender, EventArgs e) {
            try {
                servicio.getCitasPorPeriodoMedicoAsync(
                    Empleados[Citas_CB_Medicos.SelectedIndex].IDEmpleado,
                    Citas_DP_Inicio.SelectedDate ?? DateTime.Now,
                    Citas_DP_Fin.SelectedDate ?? DateTime.Now
                );
            } catch {

            }
        }

        //Obtengo las citas por consulta
        public void cargaCitas (object sender, ws.getCitasPorPeriodoMedicoCompletedEventArgs e) {
            DG_ConsultasData.ItemsSource = e.Result;
        }

        //Obtengo los médicos para consulta
        public void cargaMedicos(object sender, ws.getMedicosSolosCompletedEventArgs e) {
            Empleados = e.Result.ToList();
            Citas_CB_Medicos.Items.Clear();
            foreach (var empleado in Empleados) {
                Citas_CB_Medicos.Items.Add(empleado.forUI);
            }
        }

        //CONSULTA PACIENTES
        public void BN_FacturaPaciente_Click(object sender, EventArgs e) {
            SP_Citas.Visibility = Visibility.Collapsed;
            SP_Medicos.Visibility = Visibility.Collapsed;
            SP_Pacientes.Visibility = Visibility.Visible;
            DG_ConsultasData.ItemsSource = null;
        }

        private void InitializeUI() {
            Citas_DP_Inicio.SelectedDate = DateTime.Now;
            Citas_DP_Fin.SelectedDate = DateTime.Now;
        }
    }
}
