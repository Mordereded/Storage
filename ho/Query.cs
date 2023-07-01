using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ho
{
    public partial class Query : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string login = "";
        private string password = "";
        private string connectionstring = "";
        private string sql = "";
        private DataTable dt;
        private string table_name;
        public Query()
        {
            InitializeComponent();
        }
        public Query(string login,string password,string table_name)
        {
            InitializeComponent();
            this.login = login;
            this.password = password;
            this.table_name = table_name;
        }

        private void FormChangeByComboBox()
        {

        }

        private void SQL_Quary()
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:

                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
