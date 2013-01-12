using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.localhost;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

private void button1_Click(object sender, EventArgs e)
{
    MapGuideWebService mapguideWS = new MapGuideWebService();

    String address = textBox1.Text + ", Sheboygan, WI";
    String ParcelType = textBox2.Text;
    String bufferDistance = textBox3.Text;
    
    ParcelProperty[] props = mapguideWS.GetParcelList(address, ParcelType, Convert.ToDouble(bufferDistance));

    for (int i = 0; i < props.Length; i++)
    {
        parcelPropertyBindingSource.Add(props[i]);
    }
}




    }
}