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
using System.IO;
using BCrypt.Net;


namespace ho
{
    public partial class Create_user : Form
    {
        private NpgsqlConnection connect;
        //private NpgsqlCommand sqlCommand;
        private string sql = "";
        private string login;
        private string password;
        public Create_user()
        {
            InitializeComponent();
        }
        public Create_user(string name, string password)
        {
            this.login = name;
            this.password = password;
            sql = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            InitializeComponent();

        }
        private string Get_RoleFromCombobox()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                return "Admin";
            }
            else
            {
                return "Employee";
            }
        }
        private bool CheckIsExistUser()
        {
            bool exist = false;
            string line = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            using (NpgsqlConnection conn = new NpgsqlConnection(line))
            {
                conn.Open();
                string check = $"SELECT CASE WHEN EXISTS(SELECT username FROM \"users\" WHERE username = '{textBox_loggin.Text}') THEN 1 ELSE 0 END;";
                using (NpgsqlCommand checkCommand = new NpgsqlCommand(check, conn))
                {
                    int result = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (result == 1)
                    {
                        exist = true;
                    }
                }
                
            }
            return exist;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox_loggin.Text) || string.IsNullOrEmpty(textBox_password.Text))
            {
                MessageBox.Show("Error: ","Логин или пароль не могут быть пустыми", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string role = Get_RoleFromCombobox();
            sql = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            try
            {
                if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }
                if (CheckIsExistUser())
                {
                    MessageBox.Show("Такой пользователь уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                };

                using (connect = new NpgsqlConnection(sql))
                {
                    connect.Open();
                    using (var transaction = connect.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = "INSERT INTO users (username, user_rol) VALUES (@username, @role)";
                            using (NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connect))
                        {
                            insertCommand.Parameters.AddWithValue("@username", textBox_loggin.Text);
                            insertCommand.Parameters.AddWithValue("@role", role);

                            insertCommand.ExecuteNonQuery();
                        }
                            string createUserQuery = $"CREATE USER \"{textBox_loggin.Text}\" WITH PASSWORD '{textBox_password.Text}';";
                            using (NpgsqlCommand createCommand = new NpgsqlCommand(createUserQuery, connect))
                        {
                            //createCommand.Parameters.AddWithValue("@user", textBox_loggin.Text);
                            //createCommand.Parameters.AddWithValue("@password", textBox_password.Text);

                            createCommand.ExecuteNonQuery();
                        }
                            string grantuserQuery = $"GRANT \"{role}\" TO \"{textBox_loggin.Text}\";";
                            using (NpgsqlCommand createCommand = new NpgsqlCommand(grantuserQuery, connect))
                        {
                            createCommand.Parameters.AddWithValue("@user", textBox_loggin.Text);
                            createCommand.Parameters.AddWithValue("@permission", role);

                            createCommand.ExecuteNonQuery();
                        }

                            MessageBox.Show("Пользователь создан", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            transaction.Commit();
                            this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Ошибка при обработке запроса" , "Ошибка создания пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Ошибка создания пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        

        

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Вызов Application.Exit() для завершения программы
            Application.Exit();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
        //Выход
        private void button3_Click(object sender, EventArgs e)
        {
            new Login();
            this.Close();
            
        }
    }
}
