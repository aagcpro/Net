using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prOperadores
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {




        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            //Dclearcion de Variables
            double num1, num2;
            double suma;
            double resta, mult, div, potencia, raiz;
            //entrada de datos 

            num1 = double.Parse(txtNumero1.Text);
            num2 = Convert.ToDouble(txtNumero2.Text);
            //Proceso 

            suma = num1 + num2;
            resta = num1 - num2;
            mult = num1 * num2;
            div = num1 / num2;
            potencia = Math.Pow(num1, num2);
            raiz = Math.Pow(num1, (1 / num2));
            //Salida de Informacion
            txtSuma.Text = Convert.ToString(suma);
            txtResta.Text = Convert.ToString(resta);
            txtDivicion.Text = Convert.ToString(div);
            txtMultiplicacion.Text = Convert.ToString(mult);
            txtPotencia.Text = Convert.ToString(potencia);
            txtRaiz.Text = Convert.ToString(raiz);

        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtDivicion.Clear();
            txtMultiplicacion.Clear();
            txtNumero1.Clear();
            txtNumero2.Clear();
            txtPotencia.Clear();
            txtRaiz.Clear();
            txtResta.Clear();
            txtSuma.Clear();
           

        }
    }
}
