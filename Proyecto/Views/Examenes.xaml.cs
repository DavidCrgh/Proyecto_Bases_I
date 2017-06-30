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
    public partial class Examenes : Page
    {
        ws.wsCentroMedicoClient servicio = new ws.wsCentroMedicoClient();
        List<ws.clExamen> mExamenes = null;
        List<ws.clItem> Items = null;
        List<decimal> IDExamenes = null;
        List<decimal> IDItems = null;



        public Examenes()
        {
            InitializeComponent();
            InitializeUI();
            servicio.getExamenesCompleted += new EventHandler<ws.getExamenesCompletedEventArgs>(cargarExamenes);
            servicio.getExamenesAsync();
            servicio.getItemsCompleted += new EventHandler<ws.getItemsCompletedEventArgs>(cargarItems);
            servicio.getItemsAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        //Evento que se ejecuta luego de que getExamenesAsync() obtenga los datos de examenes de la BD
        public void cargarExamenes(object sender, ws.getExamenesCompletedEventArgs e) {
            mExamenes = e.Result.ToList();
            DG_Examenes.ItemsSource = e.Result;
        }

        //Evento que se ejecuta luego de que getItemsAsync() obtenga los datos de items de la BD
        public void cargarItems(object sender, ws.getItemsCompletedEventArgs e)
        {
            Items = e.Result.ToList();
            DG_Items.ItemsSource = e.Result;
        }

        //Inicializa contextos de elementos gráficos
        private void InitializeUI()
        {
            //string[] Tipos = { "Programada", "Ausente", "Cancelada", "Realizada" };
            //DP_Fecha.DisplayDateStart = DateTime.Now;
            //DP_Fecha.DisplayDateEnd = DateTime.MaxValue;
            //DP_Fecha.SelectedDate = DateTime.Now;
            /*foreach (string tipo in Tipos)
            {
                //CB_Tipo.Items.Add(tipo);
            }*/
            //CB_Tipo.SelectedIndex = 0;
        }

    }
}
