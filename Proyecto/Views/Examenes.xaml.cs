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
            servicio.registrarItemCompleted += new EventHandler<ws.registrarItemCompletedEventArgs>(registrarItem);
            servicio.getExamenCompleted += new EventHandler<ws.getExamenCompletedEventArgs>(cargarExamen);
            servicio.getItemCompleted += new EventHandler<ws.getItemCompletedEventArgs>(cargarItem);
            servicio.actualizarExamenCompleted += new EventHandler<ws.actualizarExamenCompletedEventArgs>(examenActualizado);
            servicio.actualizarItemCompleted += new EventHandler<ws.actualizarItemCompletedEventArgs>(itemActualizado);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        public void examenActualizado(object sender, ws.actualizarExamenCompletedEventArgs e)
        {
            servicio.getExamenesAsync();
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        public void itemActualizado(object sender, ws.actualizarItemCompletedEventArgs e)
        {
            servicio.getExamenesAsync();
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        //Evento lanzado al finalizar registrarExamenItemAsync()
        public void registrarExamen(object sender, ws.registrarExamenItemsCompletedEventArgs e)
        {
            servicio.getExamenesAsync();
            servicio.getItemsAsync();
            servicio.getIDExamenesAsync();
        }

        //Evento lanzado al finalizar registrarItemAsync()
        public void registrarItem(object sender, ws.registrarItemCompletedEventArgs e)
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

        //Se acciona cuando el usuario cambia el elemento seleccionado del ComboBox de examenes
        public void CB_ID_UpdateItem_ItemChanged(object sender, EventArgs e)
        {
            if(CB_ID_UpdateItem.SelectedIndex != -1 && CB_ID_UpdateExamen.SelectedIndex != -1)
            {
                decimal IDItem = IDItems[CB_ID_UpdateItem.SelectedIndex];
                decimal IDExamen = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
                servicio.getItemAsync(IDExamen, IDItem);
            }
        }

        public void cargarExamen(object sender, ws.getExamenCompletedEventArgs e)
        {
            ws.clExamen tmpExamen = e.Result;
            TB_NombreExamen.Text = tmpExamen.Nombre;
            TB_Descripcion.Text = tmpExamen.Descripcion;
        }

        //Se acciona cuando el usuario cambia el elemento seleccionado del ComboBox de items
        public void CB_ID_UpdateExamen_ItemChanged(object sender, EventArgs e)
        {
            if (IDExamenes.Count > 0)
            {
                try
                {
                    decimal idActual = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
                    servicio.getIDItemsAsync(Convert.ToInt32(idActual));
                    servicio.getExamenAsync(idActual);
                    TB_NombreItem.Text = "";
                    TB_ExpresionRegular.Text = "";
                }
                catch
                {

                }
            }
        }

        public void cargarItem(object sender, ws.getItemCompletedEventArgs e)
        {
            ws.clItem tmpItem = e.Result;
            TB_NombreItem.Text = tmpItem.Nombre;
            TB_ExpresionRegular.Text = tmpItem.ExpresionRegular;
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
                BN_ActualizarExamen.IsEnabled = true;
                BN_BorrarExamen.IsEnabled = true;
                BN_ActualizarItem.IsEnabled = true;
                BN_BorrarItem.IsEnabled = true;
                CB_ID_UpdateExamen.IsEnabled = true;
                CB_ID_UpdateItem.IsEnabled = true;
                TB_NombreExamen.IsEnabled = true;
                TB_Descripcion.IsEnabled = true;


                return;
            }
            //Lo que sucede cuando se desea iniciar el proceso de insercion de un examen con sus items
            modoCreacion = true;
            BN_Cancelar_Insercion.Visibility = Visibility.Visible;
            BN_InsertarExamen.Content = "Aceptar";
            

            BN_ActualizarExamen.IsEnabled = false;
            BN_BorrarExamen.IsEnabled = false;
            BN_ActualizarItem.IsEnabled = false;
            BN_BorrarItem.IsEnabled = false;
            CB_ID_UpdateExamen.IsEnabled = false;
            CB_ID_UpdateItem.IsEnabled = false;
            TB_NombreExamen.IsEnabled = false;
            TB_Descripcion.IsEnabled = false;

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
            BN_ActualizarExamen.IsEnabled = true;
            BN_BorrarExamen.IsEnabled = true;
            BN_ActualizarItem.IsEnabled = true;
            BN_BorrarItem.IsEnabled = true;
            CB_ID_UpdateExamen.IsEnabled = true;
            CB_ID_UpdateItem.IsEnabled = true;
            TB_NombreExamen.IsEnabled = true;
            TB_Descripcion.IsEnabled = true;
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
            ws.clItem tmpItem = new ws.clItem();
            decimal IDExamen = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
            tmpItem.IDExamen = IDExamen;
            if(IDItems.Count > 0)
            {
                tmpItem.IDItem = IDItems.Last() + 1;
            }
            else
            {
                tmpItem.IDItem = 1;
            }
            tmpItem.Nombre = TB_NombreItem.Text + " ";
            tmpItem.ExpresionRegular = TB_ExpresionRegular.Text + " ";
            servicio.registrarItemAsync(tmpItem);
        }

        private void BN_ActualizarExamen_Click(object sender, RoutedEventArgs e)
        {
            ws.clExamen tmpExamen = new ws.clExamen();
            tmpExamen.IDExamen = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
            tmpExamen.Nombre = TB_NombreExamen.Text;
            tmpExamen.Descripcion = TB_Descripcion.Text;
            try
            {
                servicio.actualizarExamenAsync(tmpExamen);
            }
            catch
            {

            }
        }

        private void BN_ActualizarItem_Click(object sender, RoutedEventArgs e)
        {
            ws.clItem tmpItem = new ws.clItem();
            tmpItem.IDExamen = IDExamenes[CB_ID_UpdateExamen.SelectedIndex];
            tmpItem.IDItem = IDItems[CB_ID_UpdateItem.SelectedIndex];
            tmpItem.Nombre = TB_NombreItem.Text;
            tmpItem.ExpresionRegular = TB_ExpresionRegular.Text;
            try
            {
                servicio.actualizarItemAsync(tmpItem);
            } catch
            {

            }
        }
    }
}
