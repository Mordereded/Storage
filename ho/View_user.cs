using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Npgsql;
using System.Reflection;
using Dapper;

namespace ho
{
    public partial class View_user : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string login = "";
        private string password = "";
        private string connectionstring = "";
        private string sql = "";
        private DataTable dt;
        private NpgsqlDataReader dataReader;
        private static int table_page;
        private string table_name;
        public View_user()
        {
            InitializeComponent();

        }
        public View_user(string login, string password)
        {
            InitializeComponent();
            this.login = login;
            this.password = password;
            connectionstring = "Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox1.Visible = false;
            table_name = "Country";
            label1.Visible = false;
            Load_dataGridView(page: 1,table : table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }
        //Вывод всей таблицы
        /*public void Load_dataGridView(string table = "Company")
        {
            try
            {
                if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }
                //sql = $"SELECT * FROM \"{table}\";";
                sql = SQL_SELECT();
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dataReader);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.AutoResizeColumns();

                connect.Close();
                dataGridView1.ReadOnly = true;
                //label4.Text = table_name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }*/

        //Вывод всей таблицы в другом окне
        public async Task Load_dataGridViewAsync(string table)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionstring))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(SQL_SELECT(table), connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Создаем и показываем новую форму для отображения таблицы
                            var form = new Form();
                            var dataGridView = new DataGridView();
                            dataGridView.DataSource = dataTable;
                            form.Size = new Size(500, 500);
                            
                            dataGridView.Dock = DockStyle.Fill;
                            dataGridView.Size = new Size(500, 500);
                            dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                            dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                            form.Controls.Add(dataGridView);
                            form.Text = "Вывод полной таблицы";

                            // Запускаем новую задачу для показа формы
                            Task.Run(() =>
                            {
                                Application.Run(form);
                            });

                            // Ждем, пока форма будет закрыта
                            while (!form.IsDisposed)
                            {
                                await Task.Delay(100);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*public void Load_dataGridViewAsync(string table = "Company")
        {
            try
            {
                if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }
                //sql = $"SELECT * FROM \"{table}\"";
                //sql = $"SELECT * FROM \"{table}\" ORDER BY id_{table.ToLower()} OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                sql = SQL_SELECT();
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dataReader);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.AutoResizeColumns();
                connect.Close();
                dataGridView1.ReadOnly = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
            *//*try
            {
                *//*if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }

                //sql = $"SELECT * FROM \"{table}\";";
                sql = SQL_SELECT();
                connect = new NpgsqlConnection(connectionstring);
                await connect.OpenAsync();

                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = await sqlCommand.ExecuteReaderAsync();

                dt = new DataTable();

                await Task.Run(() =>
                {
                    dt.Load(dataReader);
                });

                // Используем Invoke для изменения свойств DataGridView из основного потока
                dataGridView1.Invoke((MethodInvoker)delegate
                {
                    // Установка свойства DoubleBuffered для DataGridView
                    typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(dataGridView1, true, null);
                    dataGridView1.DataSource = dt;
                    
*//*                  dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;*//*
                    dataGridView1.ReadOnly = true;
                });

                connect.Close();
                //label4.Text = table_name;*//*
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }*//*
        }*/
        public void Load_dataGridView(int page, int pagesize = 20, string table = "Company")
        {
            try
            {
                if (connect != null && connect.State != ConnectionState.Closed)
                {
                    connect.Close();
                }
                //sql = $"SELECT * FROM \"{table}\"";
                //sql = $"SELECT * FROM \"{table}\" ORDER BY id_{table.ToLower()} OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                sql = SQL_SELECT_BY_PAGE(table_page, pagesize);
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sqlCommand = new NpgsqlCommand(sql, connect);
                dataReader = sqlCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dataReader);
                if (dt.Rows.Count == 0 && page > 1) // не менять страницу, если на новой странице нет записей.
                {
                    table_page -= 1;
                    connect.Close();
                    return;
                }
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.AutoResizeColumns();
                connect.Close();
                dataGridView1.ReadOnly = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }
        private void Get_Table_items_Count()
        {
            string tablename = table_name;
            if (tablename == "Item.Category.Attribute.Attribute_value")
            {
                tablename = "Item";
            }
            else if (tablename == "Category.Attribute")
            {
                tablename = "Category";
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionstring))
            {
                // Открыть подключение
                connection.Open();

                // Создать SQL-запрос для получения количества строк в таблице
                string sql = $"SELECT COUNT(*) FROM \"{tablename}\"";

                // Создать команду на основе SQL-запроса и подключения
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    // Выполнить запрос и получить результат (количество строк)
                    label7.Text = command.ExecuteScalar().ToString();

                    // Вывести результат
                }
            }
        }
        private void Load_dataGridView(int index)
        {
            try
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sql = SQL_SELECT_Single();
                sqlCommand = new NpgsqlCommand(sql, connect);
                sqlCommand.Parameters.AddWithValue($"@index", index);
                dataReader = sqlCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dataReader);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.AutoResizeColumns();
                dataGridView1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlCommand?.Dispose();
                connect?.Close();
                connect?.Dispose();
            }
        }
        public string SQL_SELECT(string table)
        {

            string sql = "";
            if (table == null) table = "Country";
            switch (table)
            {
                case "Company":
                    sql = $"SELECT id_company AS ID , company_name AS Название FROM \"Company\";";

                    break;
                case "Country":
                    sql = $"SELECT id_country AS ID , country_name AS Название FROM \"Country\";";

                    break;
                case "Provider":
                    sql = $"SELECT id_provider AS ID , first_name AS Имя , second_name AS Фамилия, last_name AS Отчество, id_country AS ID_Страны, id_company AS ID_Компании FROM\"Provider\";";
                    break;
                case "Item":
                    sql = $"SELECT id_item AS ID , item_name AS Название , item_cost AS Цена, id_category AS ID_Категории, id_provider AS ID_Поставщика, item_discription AS Описание FROM \"Item\";";
                    break;
                case "Category":
                    sql = $"SELECT id_category AS ID , category_name AS Название  FROM \"Category\";";
                    break;

                case "Customer":
                    sql = $"SELECT id_customer AS ID, first_name AS Имя, second_name AS Фамилия, last_name AS Отчество, phone_number AS \"Номер телефона\" FROM \"Customer\";";

                    break;
                case "Contract":
                    sql = $"SELECT id_contract AS ID , id_customer AS ID_клиента, contract_cost AS \"Стоимость контракта\", discription AS Описание, id_item AS ID_Товара FROM \"Contract\";";

                    break;
                case "Item.Category.Attribute.Attribute_value":
                    sql = $"SELECT i.\"id_item\" AS ID, i.\"item_name\" AS Название, c.\"category_name\" AS Категория, a.\"attribute_name\" AS \"Название Атрибута\", av.\"attribute_value\" AS \"Значение атрибута\" " +
                    "FROM \"Item\" i " +
                    "JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                    "LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                    "LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" " +
                    "ORDER BY i.\"id_item\"; ";

                    break;
                case "Category.Attribute":
                    sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" ORDER BY \"Category\".\"id_category\"";

                    break;


            }
            return sql;
        }
        public string SQL_SELECT()
        {
            
            string sql = "";
            if (table_name == null) table_name = "Country";
            switch (table_name)
            {
                case "Company":
                    sql = $"SELECT id_company AS ID , company_name AS Название FROM \"Company\";";
                    label4.Text = "Компании";
                    break;
                case "Country":
                    sql = $"SELECT id_country AS ID , country_name AS Название FROM \"Country\";";
                    label4.Text = "Страны";
                    break;
                case "Provider":
                    sql = $"SELECT id_provider AS ID , first_name AS Имя , second_name AS Фамилия, last_name AS Отчество, id_country AS ID_Страны, id_company AS ID_Компании FROM\"Provider\";";
                    label4.Text = "Поставщики";
                    break;
                case "Item":
                    sql = $"SELECT id_item AS ID , item_name AS Название , item_cost AS Цена, id_category AS ID_Категории, id_provider AS ID_Поставщика, item_discription AS Описание FROM \"Item\";";
                    label4.Text = "Товары";
                    break;
                case "Category":
                    sql = $"SELECT id_category AS ID , category_name AS Название  FROM \"Category\";";
                    label4.Text = "Категории";
                    break;
                case "Customer":
                    sql = $"SELECT id_customer AS ID, first_name AS Имя, second_name AS Фамилия, last_name AS Отчество, phone_number AS \"Номер телефона\" FROM \"Customer\";";
                    label4.Text = "Клиенты";
                    break;
                case "Contract":
                    sql = $"SELECT id_contract AS ID , id_customer AS ID_клиента, contract_cost AS \"Стоимость контракта\", discription AS Описание, id_item AS ID_Товара FROM \"Contract\";";
                    label4.Text = "Контракты";
                    break; 
                case "Item.Category.Attribute.Attribute_value":
                    sql = $"SELECT i.\"id_item\" AS ID, i.\"item_name\" AS Название, c.\"category_name\" AS Категория, a.\"attribute_name\" AS \"Название Атрибута\", av.\"attribute_value\" AS \"Значение атрибута\" " +
                    "FROM \"Item\" i " +
                    "JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                    "LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                    "LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" " +
                    "ORDER BY i.\"id_item\"; ";
                    label4.Text = "Товары с категориями и атрибутами";
                    break;
                case "Category.Attribute":
                    sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" ORDER BY \"Category\".\"id_category\"";
                    label4.Text = "Категории и названия атрибутов";
                    break;


            }
            return sql;
        }
        public string SQL_SELECT_BY_PAGE(int page, int pagesize = 20)
        {

            string sql = "";
            if (table_name == null) table_name = "Country";
            int offset = (page - 1) * pagesize;
            switch (table_name)
            {
                case "Company":
                    sql = $"SELECT id_company AS ID , company_name AS Название FROM \"Company\" ORDER BY id_company OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Компании";
                    break;
                case "Country":
                    sql = $"SELECT id_country AS ID , country_name AS Название FROM \"Country\" ORDER BY id_country OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Страны";
                    break;
                case "Provider":
                    sql = $"SELECT id_provider AS ID , first_name AS Имя , second_name AS Фамилия, last_name AS Отчество, id_country AS ID_Страны, id_company AS ID_Компании FROM \"Provider\" ORDER BY id_country OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Поставщики";
                    break;
                case "Item":
                    sql = $"SELECT id_item AS ID , item_name AS Название , item_cost AS Цена, id_category AS ID_Категории, id_provider AS ID_Поставщика, item_discription AS Описание FROM \"Item\" ORDER BY id_item OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Товары";
                    break;
                case "Category":
                    sql = $"SELECT id_category AS ID , category_name AS Название FROM \"Category\" ORDER BY id_category OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Категории";
                    break;
                case "Customer":
                    sql = $"SELECT \"id_customer\" AS ID , first_name AS Имя, second_name AS Фамиля, last_name AS Отчество, phone_number AS \"Номер телефона\" FROM \"Customer\" ORDER BY id_customer OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Клиенты";
                    break;
                case "Contract":
                    sql = $"SELECT id_contract AS ID , id_customer AS ID_клиента, contract_cost AS \"Стоимость контракта\", discription AS Описание, id_item AS ID_Товара FROM \"Contract\" ORDER BY id_contract OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY";
                    label4.Text = "Контракты";
                    break;
                case "Item.Category.Attribute.Attribute_value":
                    sql = $"SELECT i.\"id_item\" AS ID, i.\"item_name\" AS Название, c.\"category_name\" AS Категория, a.\"attribute_name\" AS \"Название Атрибута\", av.\"attribute_value\" AS \"Значение атрибута\" " +
                    "FROM \"Item\" i " +
                    "JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                    "LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                    "LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" " +
                    $"ORDER BY i.\"id_item\" OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY;";
                    label4.Text = "Товары с категориями и атрибутами";
                    break;
                case "Category.Attribute":
                    sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" ORDER BY \"Category\".\"id_category\" OFFSET {offset} ROWS FETCH NEXT {pagesize} ROWS ONLY;";
                    label4.Text = "Категории и названия атрибутов";
                    break;
            }
            return sql;
        }
        public string SQL_SELECT_Single()
        {

            string sql = "";
            if (table_name == null) table_name = "Country";
            switch (table_name)
            {
                case "Company":
                    sql = $"SELECT id_company AS ID , company_name AS Название FROM \"Company\"WHERE id_company = @index;";
                    label4.Text = "Компании";
                    break;
                case "Country":
                    sql = $"SELECT id_country AS ID , country_name AS Название FROM \"Country\" WHERE id_country = @index;";
                    label4.Text = "Страны";
                    break;
                case "Provider":
                    sql = $"SELECT id_provider AS ID , first_name AS Имя , second_name AS Фамилия, last_name AS Отчество, id_country AS ID_Страны, id_company AS ID_Компании FROM\"Provider\"WHERE id_provider = @index;";
                    label4.Text = "Поставщики";
                    break;
                case "Item":
                    sql = $"SELECT id_item AS ID , item_name AS Название , item_cost AS Цена, id_category AS ID_Категории, id_provider AS ID_Поставщика, item_discription AS Описание FROM \"Item\"WHERE id_item = @index;";
                    label4.Text = "Товары";
                    break;
                case "Category":
                    sql = $"SELECT id_category AS ID , category_name AS Название  FROM \"Category\"WHERE id_category = @index;";
                    label4.Text = "Категории";
                    break;
                case "Customer":
                    sql = $"SELECT id_customer AS ID , first_name AS Имя, second_name AS Фамили, last_name AS Отчество, phone_number AS \"Номер телефона\" FROM \"Customer\" WHERE id_customer = @index;";
                    label4.Text = "Клиенты";
                    break;
                case "Contract":
                    sql = $"SELECT id_contract AS ID , id_customer AS ID_клиента, contract_cost AS \"Стоимость контракта\", discription AS Описание, id_item AS ID_Товара FROM \"Contract\" WHERE id_contract = @index;";
                    label4.Text = "Контракты";
                    break;
                case "Item.Category.Attribute.Attribute_value":
                    sql = $"SELECT i.\"id_item\" AS ID, i.\"item_name\" AS Название, c.\"category_name\" AS Категория, a.\"attribute_name\" AS \"Название Атрибута\", av.\"attribute_value\" AS \"Значение атрибута\" " +
                    "FROM \"Item\" i " +
                    "JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                    "LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                    $"LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" WHERE i.id_item = @index " +
                    $"ORDER BY i.\"id_item\" ";
                    label4.Text = "Товары с категориями и атрибутами";
                    break;
                case "Category.Attribute":
                    sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE \"Category\".\"id_category\" = @index ORDER BY \"Category\".\"id_category\" ;";
                    label4.Text = "Категории и названия атрибутов";
                    break;

            }
            return sql;
        }

        private string Get_Parametrs_Of_Table(string table)
        {
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            string parametersString = "";
            try
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                // Создание команды для вызова хранимой функции
                sqlCommand = new NpgsqlCommand($"SELECT * FROM get_table_columns(\'{table_name}\')", connect);
                // Выполнение команды и получение результата
                NpgsqlDataReader reader = sqlCommand.ExecuteReader();
                List<string> parameters = new List<string>();
                while (reader.Read())
                {
                    // Чтение значения поля column_name
                    string columnName = reader.GetString(0);
                    parameters.Add($"{columnName}");
                }
                int index = parameters.FindIndex(item => item == $"id_{table_name.ToLower()}");
                //Убираем элемент с id из строки с параметрами
                if (index >= 0)
                {
                    parameters.RemoveAt(index);
                }
                parametersString = string.Join(", ", parameters);
                // Закрытие подключения и освобождение ресурсов

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрытие соединения и освобождение ресурсов
                sqlCommand.Dispose();
                connect.Close();
                connect.Dispose();
            }
            return parametersString;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            if (table_name == "Country" || table_name == "Company" || table_name == "Category")
            {
                try
                {

                    DialogResult dialogResult = MessageBox.Show("Вы уверен, что хотите добавить значение в таблицу?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No) return;
                    sql = $"INSERT INTO \"{table_name}\"({Get_Parametrs_Of_Table(table_name)}) VALUES(@item);";
                    connect = new NpgsqlConnection(connectionstring);
                    connect.Open();
                    sqlCommand = new NpgsqlCommand(sql, connect);
                    sqlCommand.Parameters.AddWithValue($"@item", textBox1.Text);
                    sqlCommand.ExecuteReader();
                    Load_dataGridView(table: table_name, page: 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
            }
            else if (table_name == "Provider")
            {
                //MessageBox.Show("Provider");
                Add_form form = new Add_form(login, password, table_name);
                form.ShowDialog();
                if (form.AddedItemId > 0)
                    Load_dataGridView(index: form.AddedItemId);
            }
            else if (table_name == "Item")
            {
                //MessageBox.Show("Item");
                Add_form form = new Add_form(login, password, table_name);
                form.ShowDialog();
                if (form.AddedItemId > 0)
                    Load_dataGridView(index: form.AddedItemId);
            }
            else if (table_name == "Contract")
            {
                //MessageBox.Show("Contract");
                Add_form form = new Add_form(login, password, table_name);
                form.ShowDialog();
                if (form.AddedItemId > 0)
                    Load_dataGridView(index: form.AddedItemId);
            }
            else if (table_name == "Customer")
            {
                //MessageBox.Show("Contract");
                Add_form form = new Add_form(login, password, table_name);
                form.ShowDialog();
                if (form.AddedItemId > 0)
                    Load_dataGridView(index: form.AddedItemId);
            }
            else if (table_name == "Category.Attribute")
            {
                //MessageBox.Show("Category.Attribute");
                Add_form form = new Add_form(login, password, table_name);
                form.ShowDialog();
                if (form.AddedItemId > 0)
                    Load_dataGridView(index: form.AddedItemId);
            }
            else if (table_name == "Item.Category.Attribute.Attribute_value")
            {
                //MessageBox.Show("Item.Category.Attribute.Attribute_value");
                Add_form form = new Add_form(login, password, table_name, Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
                form.ShowDialog();
                if (form.AddedItemId > 0 )
                    Load_dataGridView(index: form.AddedItemId);
            }
            
        }
        private static string GetParameterNames(List<int> values,string param = "param")
        {
            List<string> parameterNames = new List<string>();

            for (int i = 0; i < values.Count; i++)
            {
                parameterNames.Add($"@{param}{i}");
            }

            return string.Join(", ", parameterNames);
        }
        private static string GetParameterNames(List<string> values, string param = "param")
        {
            List<string> parameterNames = new List<string>();

            for (int i = 0; i < values.Count; i++)
            {
                parameterNames.Add($"@{param}{i}");
            }

            return string.Join(", ", parameterNames);
        }

        private async void  button2_Click(object sender, EventArgs e)
        {
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            DataGridViewSelectedRowCollection item = this.dataGridView1.SelectedRows;
            List<int> index_to_delete_ununic = new List<int>();
            //Определяем индексы удаляемых из таблицы элементов
            for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
            {
                index_to_delete_ununic.Add(Convert.ToInt32(item[i].Cells[0].Value.ToString()));
            }
            //Выбираем уникальные
            List<int> index_to_delete = index_to_delete_ununic.Distinct().ToList();
            DialogResult dialogResult = MessageBox.Show("Вы уверен, что хотите удалить все выделенный значения таблицы?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;


            if (table_name == "Item.Category.Attribute.Attribute_value")
            {
                List<string> selectedstr = new List<string>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Предполагая, что столбец с именем имеет индекс 1
                    string name = row.Cells[4].Value.ToString();
                    selectedstr.Add(name);
                }
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    await connect.OpenAsync(); // Асинхронное открытие соединения
                                               // Создание и выполнение асинхронного запроса DELETE с использованием оператора IN
                    sqlCommand = new NpgsqlCommand($" DELETE FROM \"Attribute_value\" WHERE id_item IN ({GetParameterNames(index_to_delete, "param")}) AND attribute_value IN ({GetParameterNames(selectedstr, "parametr")})", connect);
                    for (int i = 0; i < index_to_delete.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue($"@param{i}", index_to_delete[i]);
                        
                    }
                    for (int i = 0; i < selectedstr.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue($"@parametr{i}", selectedstr[i]);

                    }
                    

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync(); // Асинхронное выполнение операции DELETE
                    Load_dataGridView(table: table_name, page: 1);
                    MessageBox.Show($"Удалено - {rowsAffected} записей");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
            }
            else if (table_name == "Item")
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    await connect.OpenAsync(); // Асинхронное открытие соединения
                                               // Создание и выполнение асинхронного запроса DELETE с использованием оператора IN
                    sqlCommand = new NpgsqlCommand($" DELETE FROM \"Attribute_value\" WHERE id_item IN ({GetParameterNames(index_to_delete, "parametr")}) ;DELETE FROM \"Item\" WHERE \"Item\".id_item IN ({GetParameterNames(index_to_delete)}); ", connect);
                    for (int i = 0; i < index_to_delete.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue($"@param{i}", index_to_delete[i]);
                        sqlCommand.Parameters.AddWithValue($"@parametr{i}", index_to_delete[i]);
                    }

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync(); // Асинхронное выполнение операции DELETE
                    Load_dataGridView(table: table_name, page: 1);
                    MessageBox.Show($"Удалено - {rowsAffected} записей");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
            }
            else if (table_name == "Category.Attribute")
            {
                index_to_delete_ununic.Clear();
                index_to_delete.Clear();
                for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
                {
                    index_to_delete_ununic.Add(Convert.ToInt32(item[i].Cells[3].Value.ToString())); // добавляем
                }
                index_to_delete = index_to_delete_ununic.Distinct().ToList();
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    await connect.OpenAsync(); // Асинхронное открытие соединения
                                               // Создание и выполнение асинхронного запроса DELETE с использованием оператора IN
                    sqlCommand = new NpgsqlCommand($" DELETE FROM \"Attribute\" WHERE id_attribute IN ({GetParameterNames(index_to_delete, "parametr")});", connect);
                    for (int i = 0; i < index_to_delete.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue($"@param{i}", index_to_delete[i]);
                        sqlCommand.Parameters.AddWithValue($"@parametr{i}", index_to_delete[i]);
                    }

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync(); // Асинхронное выполнение операции DELETE
                    Load_dataGridView(table: table_name, page: 1);
                    MessageBox.Show($"Удалено - {rowsAffected} записей");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
            }
            else
            {
                try
                {
                    connect = new NpgsqlConnection(connectionstring);
                    await connect.OpenAsync(); // Асинхронное открытие соединения
                                               // Создание и выполнение асинхронного запроса DELETE с использованием оператора IN
                    sqlCommand = new NpgsqlCommand($"DELETE FROM \"{table_name}\" WHERE \"{table_name}\".id_{table_name.ToLower()} IN ({GetParameterNames(index_to_delete)})", connect);
                    for (int i = 0; i < index_to_delete.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue($"@param{i}", index_to_delete[i]);
                    }

                    int rowsAffected = await sqlCommand.ExecuteNonQueryAsync(); // Асинхронное выполнение операции DELETE
                    Load_dataGridView(table: table_name, page: 1);
                    MessageBox.Show($"Удалено - {rowsAffected} записей");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие соединения и освобождение ресурсов
                    sqlCommand?.Dispose();
                    connect?.Close();
                    connect?.Dispose();
                }
                //sql = $"DELETE FROM\"Company\"WHERE \"Company\".id_company >= {Convert.ToInt32(item[0].Cells["id_company"].Value.ToString())} AND \"Company\".id_company <= {Convert.ToInt32(item[(this.dataGridView1.SelectedRows.Count) - 1].Cells["id_company"].Value.ToString())}";
                //sqlCommand = new NpgsqlCommand(sql, connect);
                //sqlCommand.ExecuteReader();
            }

            //if (comboBox1.SelectedIndex == 0)
            //{
            //    table_name = "Country";
            //    values_name = "id_country";
            //}
            //else if (comboBox1.SelectedIndex == 1)
            //{
            //    table_name = "Company";
            //    values_name = "id_company";

            //}



        }
        private void dataGridView1_double_click_on_cell(object sender, EventArgs e)
        {

            if(table_name == "Item" && dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                MessageBox.Show(dataGridView1.CurrentCell.Value.ToString(), "Ваш текст", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dataGridView1.CurrentCell.RowIndex >= 0 && dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                DataGridViewCell clickedCell = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[dataGridView1.CurrentCell.ColumnIndex];
                if (clickedCell != null && clickedCell.Value != null)
                {
                    string cellValue = clickedCell.Value.ToString();
                    Clipboard.Clear();
                    Clipboard.SetText(cellValue);
                }
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            table_page = 1;
            label_page_count.Text = table_page.ToString();
            if (comboBox1.SelectedIndex == 1)
            {
                ///Load_dataGridView();
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                //Load_dataGridView("Country");
            }

        }
        public string ShowPositiveNumberInputDialog(string prompt)
        {
            Form inputForm = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            inputForm.Text = prompt;
            label.Text = prompt;
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            inputForm.ClientSize = new Size(396, 107);
            inputForm.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            inputForm.ClientSize = new Size(Math.Max(300, label.Right + 10), inputForm.ClientSize.Height);
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.StartPosition = FormStartPosition.CenterScreen;
            inputForm.MinimizeBox = false;
            inputForm.MaximizeBox = false;
            inputForm.AcceptButton = buttonOk;
            inputForm.CancelButton = buttonCancel;

            string userInput = string.Empty;
            bool isValidInput = false;

            while (!isValidInput)
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    userInput = textBox.Text;

                    if (decimal.TryParse(userInput, out decimal result) && result > 0)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        MessageBox.Show("Введите положительное числовое значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    break;
                }
            }

            return userInput;
        }
        //Удалить всё из таблицы
        /*private void button3_Click(object sender, EventArgs e)
        {
            //ChoseTableName(out table_name);
            try
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверен, что хотите удалить все значения таблицы?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No) return;
                if(table_name == "Item.Category.Attribute.Attribute_value")
                {
                    sql = $"DELETE FROM \"Item\";";
                }
                else if(table_name == "Category.Attribute")
                {
                    sql = $"DELETE FROM \"Attribute\";";
                }
                else
                    sql = $"DELETE FROM \"{table_name}\";";
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                sqlCommand = new NpgsqlCommand(sql, connect);
                sqlCommand.ExecuteReader();
                Load_dataGridView(table: table_name, page: 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрытие соединения и освобождение ресурсов
                sqlCommand?.Dispose();
                connect?.Close();
                connect?.Dispose();
            }

        }*/
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            new Login().Show();
        }
        // Создание Excel файла и его вывод
        private async void button4_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            var exceldt = new DataTable();
            NpgsqlConnection conn = new NpgsqlConnection();
            NpgsqlCommand Command = new NpgsqlCommand();
            string sql_command;
            DialogResult dialogRes = MessageBox.Show("Если вы хотите вывести все записи то нажмите 'Да', если 'Нет' то выберите количество записей. Вывод всех записей может занять значительное время", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogRes == DialogResult.Yes)
            {
                sql_command = SQL_SELECT();
            }
            else
            {
                string userInput = ShowPositiveNumberInputDialog("Введите значение :");
                sql_command = SQL_SELECT_BY_PAGE(page: 1, pagesize: Convert.ToInt32(userInput));
            }

            await Task.Run(() =>
            {
                try
                {
                    
                    conn = new NpgsqlConnection(connectionstring);
                    conn.Open();

                    Command = new NpgsqlCommand(sql_command, connect);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(Command);

                    adapter.Fill(exceldt);
                    // Добавление названий столбцов
                    for (int col = 0; col < exceldt.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1] = exceldt.Columns[col].ColumnName;
                    }

                    // Добавление данных из DataTable в лист Excel
                    object[,] data = new object[exceldt.Rows.Count, exceldt.Columns.Count];

                    for (int row = 0; row < exceldt.Rows.Count; row++)
                    {
                        for (int col = 0; col < exceldt.Columns.Count; col++)
                        {
                            data[row, col] = exceldt.Rows[row][col];
                        }
                    }

                    int startRow = 2; // Начальная строка для записи данных
                    int startColumn = 1; // Начальный столбец для записи данных

                    int endRow = startRow + exceldt.Rows.Count - 1; // Конечная строка для записи данных
                    int endColumn = startColumn + exceldt.Columns.Count - 1; // Конечный столбец для записи данных

                    Excel.Range dataRange = worksheet.Range[worksheet.Cells[startRow, startColumn], worksheet.Cells[endRow, endColumn]];
                    dataRange.Value = data;

                    // Регулировка размера ячеек на основе размера текста
                    dataRange.EntireColumn.AutoFit();

                    // Применение стилей для названий столбцов
                    Excel.Range headerRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, exceldt.Columns.Count]];
                    headerRange.Font.Bold = true;
                    headerRange.Interior.Color = System.Drawing.Color.LightGray;
                    // Применение стилей для данных
                    Excel.Range dataRange_data = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[exceldt.Rows.Count + 1, exceldt.Columns.Count]];
                    dataRange_data.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Command?.Dispose();
                    conn?.Close();
                    conn?.Dispose();
                }

            });
            // Сохранение файла Excel
            try
            {
                string filePath = $"C:\\Users\\One\\source\\repos\\ho\\{comboBox1.Text}_file.xlsx";
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();
                // Открытие файла Excel
                DialogResult dialogResult = MessageBox.Show("Хотите открыть файл Excel?", "Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes) Process.Start(filePath);
                var process = Process.Start(filePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Сдвинуть страницу влево 
        private void button_Shift_page_left_Click(object sender, EventArgs e)
        {
            //ChoseTableName(out table_name);
            table_page -= 1;
            if (table_page <= 0) table_page = 1;
            Load_dataGridView(table: table_name, page: table_page);
            label_page_count.Text = table_page.ToString();
        }
        //Сдвинуть страницу таблицы вправо
        private void button_Shift_page_right_Click(object sender, EventArgs e)
        {
            //ChoseTableName(out table_name);
            //if (comboBox1.SelectedIndex == 0)
            //{
            //    table_name = "Country";
            //}
            //else if (comboBox1.SelectedIndex == 1)
            //{
            //    table_name = "Company";
            //}
            table_page += 1;
            Load_dataGridView(table: table_name, page: table_page);
            label_page_count.Text = table_page.ToString();
        }
        //Вывод всей таблицы при нажатии на "Вся таблица"
        private void button_Show_Full_Table_Click(object sender, EventArgs e)
        {
            //ChoseTableName(out table_name);
            //if (iTEMToolStripMenuItem.Name == "Страны")
            //{
            //    table_name = "Country";
            //}
            //else if (iTEMToolStripMenuItem.Name == "Компании")
            //{
            //    table_name = "Company";
            //}
            table_page = 1;
            label_page_count.Text = table_page.ToString();
            Load_dataGridViewAsync(table : table_name);
        }
        //Выбор пункта компания
        private void компанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            label_page_count.Text = "1";
            table_name = "Company";
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
        }

        private void страныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            label_page_count.Text = "1";
            table_name = "Country";
            textBox1.Visible = true;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            table_name = "Provider";
            label_page_count.Text = "1";
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
        }
        private void категорииToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            table_name = "Category";
            label_page_count.Text = "1";
            label5.Visible = true;
            textBox1.Visible = true;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
        }

        private void товарыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            table_name = "Item";
            label_page_count.Text = "1";
            label5.Visible = false;
            textBox1.Visible = false;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            label5.Visible = false;
            button1_add.Visible = true;
            textBox1.Visible = false;
            button2.Visible = true;
            button_change_data.Visible = true;
        }
        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            table_name = "Customer";
            label_page_count.Text = "1";
            label5.Visible = false;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = false;
            label5.Visible = false;
            button1_add.Visible = true;
            textBox1.Visible = false;
            button2.Visible = false;
            button_change_data.Visible = true;
        }
        private void контрактыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_name = "Contract";
            label_page_count.Text = "1";
            label5.Visible = false;
            textBox1.Visible = false;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
            button_change_data.Visible = true;
            label5.Visible = false;
            button1_add.Visible = true;
            textBox1.Visible = false;
            button2.Visible = true;
        }
        private void товарыСКатегориямиИСвойстваимиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_name = "Item.Category.Attribute.Attribute_value";
            label_page_count.Text = "1";
            button_change_data.Visible = true;
            label5.Visible = false;
            button1_add.Visible = true;
            textBox1.Visible = false;
            button2.Visible = true;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();
        }
        private void категорииИИхСвойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_add.Visible = false;
            table_name = "Category.Attribute";
            label_page_count.Text = "1";
            label5.Visible = false;
            textBox1.Visible = false;
            Load_dataGridView(page: 1, table: table_name);
            table_page = 1;
            Get_Table_items_Count();    
            
        }


        private void button1_Click_1(object sender, EventArgs e)
        { 
            DialogResult dialogResult = MessageBox.Show("Вы хотите изменить запись?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No) return;
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            DataGridViewSelectedRowCollection item = this.dataGridView1.SelectedRows;
            List<int> index_to_update = new List<int>();
            for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
            {
                index_to_update.Add(Convert.ToInt32(item[i].Cells[0].Value.ToString()));
            }
            try
            {
                int id = index_to_update[0];
                Update_form update = new Update_form(login, password, table_name, id);
                update.Show();
                Load_dataGridView(table: table_name, page: 1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlCommand?.Dispose();
                connect?.Close();
                connect?.Dispose();
                Load_dataGridView(table: table_name, page: 1);
            }
        }
        
        public void Load_dataGridView(DataTable data)
        {
            try
            {
                dataGridView1.DataSource = data;
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.AutoResizeColumns();
                connect.Close();
                dataGridView1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }
        private void Find_button_click(object sender, EventArgs e)
        {
            Find form = new Find(login, password, table_name);
            form.ShowDialog();
            if (form.GetResultTable().Rows.Count > 0)
                Load_dataGridView(form.GetResultTable());
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
