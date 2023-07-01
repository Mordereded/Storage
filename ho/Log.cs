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
    public partial class Login : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string sql = "";
        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Button_new_Login(object sender, EventArgs e)
        {
            string query;
            sql = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + textBox_loggin.Text + "';" + "Password = '" + textBox_password.Text + "';";
            try
            { 
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    connection.Open();
                    query = $"SELECT user_rol FROM users WHERE username = @username";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", textBox_loggin.Text);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // Выполнение запроса и получение результата
                                string role = reader.GetString(0);


                                if (role == "Admin")
                                {
                                    // Создание формы администратора
                                    View adminForm = new View(textBox_loggin.Text, textBox_password.Text);
                                    this.Hide();
                                    adminForm.Show();

                                }
                                else if (role == "Employee")
                                {
                                    // Создание формы пользователя
                                    View_user userForm = new View_user(textBox_loggin.Text, textBox_password.Text);
                                    this.Hide();
                                    userForm.Show();
                                }
                            }
                        }


                    }

                }
            

            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы ввели неверные данные." , "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Ошибка подключения ", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            sql = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + "postgres" + "';" + "Password = '" + "kill12345" + "';";
            try
            {
                if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }
                connect = new NpgsqlConnection(sql);
                connect.Open();
                connect.Close();
                Hide();
                new View("postgres", "kill12345").Show();
                //new View("postgres", "kill12345").Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Вызов Application.Exit() для завершения программы
            Application.Exit();
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_loggin.Text) || string.IsNullOrEmpty(textBox_password.Text))
            {
                MessageBox.Show("Для создания нового юзера вам необходимо войти под ролью администратор", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {

                sql = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + textBox_loggin.Text + "';" + "Password = '" + textBox_password.Text + "';";
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    connection.Open();

                    // Проверка, что текущий пользователь является администратором
                    string checkAdminQuery = "SELECT user_rol FROM users WHERE username = current_user";
                    using (NpgsqlCommand checkAdminCommand = new NpgsqlCommand(checkAdminQuery, connection))
                    {
                        string currentUserRole = (string)checkAdminCommand.ExecuteScalar();

                        if (currentUserRole == "Admin")
                        {
                            new Create_user(textBox_loggin.Text, textBox_password.Text).ShowDialog();

                        }
                        else
                        {
                            MessageBox.Show("Вы не администратор ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка" , "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
