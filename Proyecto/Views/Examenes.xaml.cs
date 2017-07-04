using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        bool modoCreacion = false;
        ws.clExamen ExamenPorInsertar = null;
        List<ws.clItem> ItemsPorInsertar = new List<ws.clItem>();



        public Examenes()
        {
            InitializeComponent();
            InitializeUI();
            servicio.getExamenesCompleted += new EventHandler<ws.getExamenesCompletedEventArgs>(cargarExamenes);
            servicio.getExamenesAsync();
            servicio.getItemsCompleted += new EventHandler<ws.getItemsCompletedEventArgs>(cargarItems);
            servicio.getItemsAsync();
            servicio.getIDExamenesCompleted += new EventHandler<ws.getIDExamenesCompletedEventArgs>(cargarIDExamenes);
            servicio.getIDExamenesAsync();
            servicio.getIDItemsCompleted += new EventHandler<ws.getIDItemsCompletedEventArgs>(cargarIDItems);
            servicio.eliminarExamenCompleted += new EventHandler<ws.eliminarExamenCompletedEventArgs>(eliminarExamen);
            servicio.eliminarItemCompleted += new EventHandler<ws.eliminarItemCompletedEventArgs>(eliminarItem);
            servicio.registrarExamenItemsCompleted += new EventHandler<ws.registrarExamenItemsCompletedEventArgs>(registrarExamen);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        //Evento lanzado al finalizar registrarExamenItemAsync()
        public void registrarExamen(object sender, ws.registrarExamenItemsCompletedEventArgs e)
        {
            servicio.getExamenesAsync();
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        //Evento lanzado al finalizar eliminarExamenAsync()
        public void eliminarExamen(object sender, ws.eliminarExamenCompletedEventArgs e)
        {
            servicio.getExamenesAsync();
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        //Evento lanzado al finalizar eliminarItemAsync()
        public void eliminarItem(object sender, ws.eliminarItemCompletedEventArgs e)
        {
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        //Evento que se ejecuta luego de que getIDExamenesAsync() obtenga los IDs de examenes de la BD
        public void cargarIDExamenes(object sender, ws.getIDExamenesCompletedEventArgs e)
        {
            IDExamenes = e.Result.ToList();
            CB_ID_UpdateExamen.Items.Clear();
            foreach (var ID in IDExamenes)
            {
                CB_ID_UpdateExamen.Items.Add(ID);
            }

            if(IDExamenes.Count > 0)
            {
                try
                {
                    servicio.getIDItemsAsync(Convert.ToInt32(IDExamenes.First()));
                }
                catch
                {

                }
            }
        }

        //Evento que se ejecuta luego de que getIDItemsAsync() obtenga los IDs de items de la BD
        public void cargarIDItems(object sender, ws.getIDItemsCompletedEventArgs e)
        {
            IDItems = e.Result.ToList();
            CB_ID_UpdateItem.Items.Clear();
            foreach (var ID in IDItems)
            {
                CB_ID_UpdateItem.Items.Add(ID);
            }
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

        //Se acciona cuando el usuario cambia el elemento seleccionado del ComboBox de sedes
        public void CB_ID_UpdateExamen_ItemChanged(object sender, EventArgs e)
        {
            if (IDExamenes.Count > 0)
            {
                try
                {
                    decimal idActual = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
                    servicio.getIDItemsAsync(Convert.ToInt32(idActual));
                }
                catch
                {

                }
            }
        }

        private void BN_BorrarExamen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                servicio.eliminarExamenAsync(IDExamenes[CB_ID_UpdateExamen.SelectedIndex]);
            }
            catch
            {

            }
        }

        private void BN_BorrarItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal IDItem = IDItems[CB_ID_UpdateItem.SelectedIndex];
                decimal IDExamen = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
                servicio.eliminarItemAsync(IDItem, IDExamen);
            }
            catch
            {

            }
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

        private void BN_InsertarExamen_Click(object sender, RoutedEventArgs e)
        {
            if (modoCreacion)
            {
                //Lo que sucede cuando se va a confirmar la insercion del examen con sus items
                ObservableCollection<ws.clItem> coleccion = new ObservableCollection<ws.clItem>(ItemsPorInsertar);
                servicio.registrarExamenItemsAsync(ExamenPorInsertar, coleccion);
                modoCreacion = false;
                BN_Cancelar_Insercion.Visibility = Visibility.Collapsed;
                BN_InsertarExamen.Content = "Insertar nuevo examen";


                return;
            }
            //Lo que sucede cuando se desea iniciar el proceso de insercion de un examen con sus items
            modoCreacion = true;
            BN_Cancelar_Insercion.Visibility = Visibility.Visible;
            BN_InsertarExamen.Content = "Aceptar";

            ExamenPorInsertar = new ws.clExamen();
            ExamenPorInsertar.IDExamen = IDExamenes.Last() + 1;
            ExamenPorInsertar.Nombre = TB_NombreExamen.Text + " ";
            ExamenPorInsertar.Descripcion = TB_Descripcion.Text + " ";
            ItemsPorInsertar.Clear();
        }

        private void BN_Cancelar_Insercion_Click(object sender, RoutedEventArgs e)
        {
            modoCreacion = false;
            BN_Cancelar_Insercion.Visibility = Visibility.Collapsed;
            BN_InsertarExamen.Content = "Insertar nuevo examen";
        }

        private void BN_InsertarItem_Click(object sender, RoutedEventArgs e)
        {
            if (modoCreacion)
            {
                //Para insertar un item de un examen todavia no existente
                ws.clItem tmp = new ws.clItem();
                tmp.IDExamen = ExamenPorInsertar.IDExamen;
                if(ItemsPorInsertar.Count > 0)
                {
                    tmp.IDItem = ItemsPorInsertar.Last().IDItem + 1;
                }else
                {
                    tmp.IDItem = 1;
                }
                tmp.Nombre = TB_NombreItem.Text + " ";
                tmp.ExpresionRegular = TB_ExpresionRegular.Text + " ";

                ItemsPorInsertar.Add(tmp);

                return;
            }
            //Para insertar un item de un examen ya existente

        }
    }
}
