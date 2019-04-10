using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ADOPractice
{
    public partial class Form1 : Form
    {
        OleDbConnection dbConnection;
        OleDbCommand dbCommand;
        OleDbDataAdapter adapter;
        DataSet ds;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=AMDatabase.mdb");
            dbCommand = new OleDbCommand("select * from VehicleStock;", dbConnection);
            adapter = new OleDbDataAdapter(dbCommand);
            ds = new DataSet();

           
            dbConnection.Open();
            adapter.Fill(ds, "VehicleStock");

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = ds.Tables["VehicleStock"];


            dgvVehicleStock.DataSource = bindingSource;
            lblStockNumber.DataBindings.Add("Text", bindingSource, "stocknumber");
            cboTest.DataSource = bindingSource;
            cboTest.DisplayMember = "ID";
            cboTest.ValueMember = "basePrice";

            dbConnection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            updateData();

        }

        private void updateData()
        {
            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
            commandBuilder.GetUpdateCommand();
            commandBuilder.GetDeleteCommand();
            commandBuilder.GetInsertCommand();

            dbConnection.Open();

            adapter.Update(ds, "VehicleStock");

            dbConnection.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if(dgvVehicleStock.SelectedRows.Count > 0 && dgvVehicleStock.SelectedRows[0].Index != dgvVehicleStock.RowCount -1 )
            {
                foreach ( DataGridViewRow row in dgvVehicleStock.SelectedRows)
                dgvVehicleStock.Rows.Remove(row);
            }

            updateData();
        }
    }
}
