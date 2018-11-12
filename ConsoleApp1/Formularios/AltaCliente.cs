﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;
using System.Text.RegularExpressions;
using Formularios.Interfaces;
using System.Globalization;
namespace Formularios
{
    public partial class AltaCliente : Form
    {
        Cliente cliente;
        IGrillaCliente ownerGrilla;
        IMenuPrincipal ownerMenu;
        public bool Modifica { get; set; }

        public AltaCliente()
        {

        }
        public AltaCliente(Cliente cl)
        {
            cliente = cl;
            Modifica = false;

            InitializeComponent();
        }
        public AltaCliente(Cliente pcliente, bool pModificacion)
        {
            cliente = pcliente;
            InitializeComponent();

            Modifica = true;
        }

        private void AltaCliente_Load(object sender, EventArgs e)
        {
            ownerGrilla = this.Owner as IGrillaCliente;
            ownerMenu = this.Owner as IMenuPrincipal;

            foreach (var item in Enum.GetValues(typeof(TipoDocumento)))
            {
                cbTipoDocumento.Items.Add(item);
            }
            cbTipoDocumento.SelectedIndex = 0;
            cbTipodeCliente.DataSource = new List<string> {"Regular", "VIP"};

            if (Modifica)
            {
                cbTipoDocumento.SelectedItem = cliente.TipoDoc;

                txtDNI.Text = cliente.Documento.ToString();

                txtNombreCompleto.Text = cliente.NombreCompleto;
                txtCorreo.Text = cliente.Email;
                txtCelular.Text = cliente.Celular;
                txtDomicilio.Text = cliente.Domicilio;
                txtCodigoPostal.Text = cliente.CodPostal.ToString();
                txtLocalidad.Text = cliente.Localidad;
                mkTxtFechaNacimiento.Text = cliente.FechaNacimiento.ToShortDateString();
                txtMontoMaximoaAutorizar.Text = cliente.MontoMaximoAutorizar.ToString();

                cbTipodeCliente.SelectedItem = cliente.EsVip ? "VIP" : "Regular";
                if(cliente.Sexo == Sexo.MASCULINO)
                {
                    rbHombre.Checked = true;
                    rbMujer.Checked = false;
                }
                else {
                    rbHombre.Checked = false;
                    rbMujer.Checked = true;
                }
            }

        }

        private void bGuardar_Click(object sender, EventArgs e)
        {
            Resultado resultado;

            if (String.IsNullOrWhiteSpace(cbTipoDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(cbTipodeCliente.Text) ||
                string.IsNullOrWhiteSpace(txtMontoMaximoaAutorizar.Text))
            {
                MessageBox.Show("Complete todos los campos obligatorios");
            }
            else
            {
                cliente.Documento = Convert.ToInt32(txtDNI.Text);
                cliente.NombreCompleto = txtNombreCompleto.Text;
                cliente.Email = txtCorreo.Text;
                cliente.MontoMaximoAutorizar = string.IsNullOrWhiteSpace(txtMontoMaximoaAutorizar.Text) ? 0 : float.Parse(txtMontoMaximoaAutorizar.Text);
                cliente.EsVip = cbTipodeCliente.SelectedItem.ToString() == "VIP" ? true : false;

                cliente.Celular = txtCelular.Text;
                cliente.Sexo = rbHombre.Checked ? Sexo.MASCULINO : Sexo.FEMENINO;
                cliente.Domicilio = txtDomicilio.Text;
                cliente.CodPostal = string.IsNullOrWhiteSpace(txtCodigoPostal.Text) ? 0 : int.Parse(txtCodigoPostal.Text);
                cliente.Localidad = txtLocalidad.Text;

                DateTime nacimiento;
                DateTime.TryParse(this.mkTxtFechaNacimiento.Text, out nacimiento);
                cliente.FechaNacimiento = nacimiento != null ? nacimiento : new DateTime();
            

                if (ownerGrilla != null)
                {
                    if(Modifica)
                    {
                        resultado = ownerGrilla.ModificacionCliente(cliente, true);
                    }
                    else
                    {
                        resultado = ownerGrilla.NuevoCliente(cliente);
                    }
                }
                else
                {
                    if (ownerMenu != null)
                    {
                        resultado = ownerMenu.NuevoCliente(cliente);
                    }
                    else
                    {
                        resultado = new Resultado();
                        resultado.FueCorrecto = false;
                        resultado.listaMsjs.Add("error inesperado");
                    }

                }

                if (resultado.FueCorrecto )
                {
                    MessageBox.Show("La Operacion se realizo con exito");
                    this.Close();
                }
                else
                {

                }
            }

        }

        private int VerificarCamposObligatorios()
        {
            //0 No hay Campos vacios
            //1 Tipo Documento vacio
            //2 Número DNI Vacio
            //3 Email Vacio
            //4 Tipo de cliente vacio
            //5 Monto Maximo Vacio
            //6 Controlar formato Correo
            //7 Controlar Tipo y Nro DNI ya cargado
            int Codigo;
            Codigo = 0;
                if (String.IsNullOrWhiteSpace(cbTipoDocumento.Text) || 
                string.IsNullOrWhiteSpace(txtDNI.Text) || 
                string.IsNullOrWhiteSpace(txtCorreo.Text) || 
                string.IsNullOrWhiteSpace(cbTipodeCliente.Text) ||
                string.IsNullOrWhiteSpace(txtMontoMaximoaAutorizar.Text))
                {
                    if (String.IsNullOrWhiteSpace(cbTipoDocumento.Text))
                    {
                        Codigo = 1;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtDNI.Text))
                        {
                            Codigo = 2;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
                            {
                                Codigo = 3;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(cbTipodeCliente.Text))
                                {
                                    Codigo = 4;
                                }
                                else
                                {
                                    Codigo = 5;

                                }
                            }
                        }
                    }

                }
                String Caracteres;
                Caracteres = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (!Regex.IsMatch(txtCorreo.Text, Caracteres))
                {
                    if (!(Regex.Replace(txtCorreo.Text, Caracteres, String.Empty).Length == 0))
                    {
                        Codigo = 6;
                    }               
                }         
            return Codigo;
        }

        private void txt_NombreC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Space))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtLocalidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Space))
            {
                e.Handled = true;
                return;
            }
        }

        private void txt_Celular_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
           if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {                
                e.Handled = true;
            }
        }

        private void txt_CodPostalKeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtMonto_Maximo(object sender, KeyPressEventArgs e)
        {
             CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;

             if (char.IsNumber(e.KeyChar)
                    || (e.KeyChar.ToString() == cc.NumberFormat.NumberDecimalSeparator)
                    || (Char.IsControl(e.KeyChar)))
             {
                e.Handled = false;
             }
             else
             {
                 e.Handled = true;
             }

        }

        private void txtDNI_Keypressed(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
