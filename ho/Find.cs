using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ho
{
    public partial class Find : Form
    {
        private NpgsqlConnection connect;
        private NpgsqlCommand sqlCommand;
        private string login = "";
        private string password = "";
        private string connectionstring = "";
        private string sql = "";
        private DataTable dt;
        private string table_name;
        List<string> parametrs = new List<string>();

        public Find()
        {
            InitializeComponent();
        }
        public Find(string login, string password, string table_name)
        {
            InitializeComponent();
            this.login = login;
            this.password = password;
            this.table_name = table_name;
            connectionstring = $"Server = localhost;" + "Port = 5432;" + "Database = Storage;" + "User Id = '" + login + "';" + "Password = '" + password + "';";
            dt = new DataTable();
        }

        private List<string> Get_Parametrs_Of_Table()
        {
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            List<string> parameters = new List<string>();
            try
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                // Создание команды для вызова хранимой функции
                sqlCommand = new NpgsqlCommand($"SELECT * FROM get_table_columns(\'{table_name}\')", connect);
                // Выполнение команды и получение результата
                NpgsqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    string columnName = reader.GetString(0);
                    parameters.Add(columnName);
                }
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
            return parameters;
        }
        private List<string> Get_Parametrs_Of_Table(string table)
        {
            if (connect != null && connect.State != ConnectionState.Closed)
            {
                connect.Close();
            }
            List<string> parameters = new List<string>();
            try
            {
                connect = new NpgsqlConnection(connectionstring);
                connect.Open();
                // Создание команды для вызова хранимой функции
                sqlCommand = new NpgsqlCommand($"SELECT * FROM get_table_columns(\'{table}\')", connect);
                // Выполнение команды и получение результата
                NpgsqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    string columnName = reader.GetString(0);
                    parameters.Add(columnName);
                }
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
            return parameters;
        }

        private List<string> GetItemsForComboBox()
        {
            List<string> namesOfRows = new List<string>();
            switch (table_name)
            {
                
                case "Country":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Название страны");
                    break;
                case "Company":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Название компании");
                    break;
                case "Customer":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Имя");
                    namesOfRows.Add("Фамилия");
                    namesOfRows.Add("Отчество");
                    namesOfRows.Add("Номер телефона");
                    break;
                case "Provider":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("ID Страны");
                    namesOfRows.Add("ID Компании");
                    namesOfRows.Add("Имя");
                    namesOfRows.Add("Фамилия");
                    namesOfRows.Add("Отчество");
                    break;
                case "Item":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Цена");
                    namesOfRows.Add("Категория");
                    namesOfRows.Add("Провайдер");
                    namesOfRows.Add("Название");
                    namesOfRows.Add("Описание");
                    break;
                case "Category":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Название категории");
                    break;
                case "Category.Attribute":
                    //sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE \"Category\".\"id_category\" = @index ORDER BY \"Category\".\"id_category\" ;";
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Название категории");
                    namesOfRows.Add("ID атрибута");
                    namesOfRows.Add("Название атрибутов");
                    break;
                case "Contract":
                    namesOfRows.Add("ID");
                    namesOfRows.Add("ID клиента");
                    namesOfRows.Add("ID товара");
                    namesOfRows.Add("Цена контракта");
                    namesOfRows.Add("Описание");
                    break;
                case "Item.Category.Attribute.Attribute_value":
                    //sql = $"SELECT \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE \"Category\".\"id_category\" = @index ORDER BY \"Category\".\"id_category\" ;";
                    namesOfRows.Add("ID");
                    namesOfRows.Add("Название товара");
                    namesOfRows.Add("Категория");
                    namesOfRows.Add("Название атрибутов");
                    namesOfRows.Add("Значение атрибута");
                    break;
            }
            return namesOfRows;
        }
        private void FillComboBox()
        {
            List<string> comboBoxItems = new List<string>();
            comboBoxItems = GetItemsForComboBox();
            foreach (var item in comboBoxItems)
            {
                comboBox1.Items.Add(item);
            }
        }
        private string NamesOfFuilds()
        {
            string names = "";
            switch (table_name)
            {
                case "Company":
                    names = $" id_company AS ID , company_name AS Название  ";
                    break;
                case "Country":
                    names = $" id_country AS ID , country_name AS Название  ";
                    break;
                case "Provider":
                    names = $" id_provider AS ID , first_name AS Имя , second_name AS Фамилия, last_name AS Отчество, id_country AS ID_Страны, id_company AS ID_Компании ";
                    break;
                case "Item":
                    names = $" id_item AS ID , item_name AS Название , item_cost AS Цена, id_category AS ID_Категории, id_provider AS ID_Поставщика, item_discription AS Описание ";
                    break;
                case "Category":
                    names = $" id_category AS ID , category_name AS Название ";
                    break;
                case "Customer":
                    names = $" id_customer AS ID , first_name AS Имя, second_name AS Фамили, last_name AS Отчество, phone_number AS \"Номер телефона\" ";
                    break;
                case "Contract":
                    names = $" id_contract AS ID , id_customer AS ID_клиента, contract_cost AS \"Стоимость контракта\", discription AS Описание, id_item AS ID_Товара ";
                    break;
                case "Item.Category.Attribute.Attribute_value":
                    names = $" i.\"id_item\" AS ID, i.\"item_name\" AS Название, c.\"category_name\" AS Категория, a.\"attribute_name\" AS \"Название Атрибута\", av.\"attribute_value\" AS \"Значение атрибута\" ";
                    break;
                case "Category.Attribute":
                    names = $" \"Category\".\"id_category\" AS \"Идентификатор категории\", \"Category\".\"category_name\" AS \"Название категории\", \"Attribute\".attribute_name AS \"Название атрибута\", \"Attribute\".\"id_attribute\" AS \"Идентификатор атрибута\" ";
                    break;

            }
            return names;
        }
        private bool IsPhoneNumber(string text)
        {
            // Шаблон регулярного выражения для номера телефона
            string pattern = @"^\+\d+$";// Предполагаем, что номер состоит из 10 цифр

            // Проверка соответствия значения шаблону
            return Regex.IsMatch(text, pattern);
        }

        private string SQL_Find()
        {
            string selectedParameter = parametrs[comboBox1.SelectedIndex];
            string parameterValue = textBox1.Text;
            string sql = "";
            decimal item = 0;
            //Таблица Категорий с атрибутами
            if (table_name == "Category.Attribute")
            {
                string table = "";
                if(comboBox1.SelectedIndex > 0)
                {
                    table = $" \"Attribute\".";
                }
                else
                {
                    table = $" \"Category\".";
                }
                if (int.TryParse(parameterValue, out _) )
                {
                    sql = $"SELECT {NamesOfFuilds()} FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE {table}\"{selectedParameter}\" = {parameterValue};";
                }
                else
                {
                    sql = $"SELECT {NamesOfFuilds()} FROM \"Category\" JOIN \"Attribute\" ON \"Category\".\"id_category\" = \"Attribute\".\"id_category\" WHERE lower({table}\"{selectedParameter}\") = lower('{parameterValue}');";
                }
            }
            if (table_name == "Item.Category.Attribute.Attribute.value")
            {
                string table = "";
                switch (comboBox1.SelectedIndex)
                {
                    case int n when (n >= 0 && n <= 1):
                        table = "i";
                        break;
                    case 2:
                        table = "c";
                        break;
                    case 3:
                        table = "a";
                        break;
                    case 4:
                        table = "av";
                        break;
                }

                if (int.TryParse(parameterValue, out _))
                {
                    sql = $"SELECT {NamesOfFuilds()} " +
                             $"FROM \"Item\" i " +
                             $"JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                             $"LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                             $"LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" WHERE {table}.\"{selectedParameter}\" = {parameterValue}";
                }
                else
                {
                    sql = $"SELECT {NamesOfFuilds()} " +
                          $"FROM \"Item\" i " +
                          $"JOIN \"Category\" c ON i.\"id_category\" = c.\"id_category\" " +
                          $"LEFT JOIN \"Attribute_value\" av ON i.\"id_item\" = av.\"id_item\" " +
                          $"LEFT JOIN \"Attribute\" a ON av.\"id_attribute\" = a.\"id_attribute\" WHERE lower({table}.\"{selectedParameter}\") = lower('{parameterValue}') ";
                }
            }

            else
            {
                if (decimal.TryParse(parameterValue, out item) && !IsPhoneNumber(parameterValue) )
                {
                    if(!(parametrs[comboBox1.SelectedIndex].Contains("id")))
                        sql = $"SELECT {NamesOfFuilds()} FROM \"{table_name}\" WHERE \"{selectedParameter}\" = CAST({item} AS money);";
                    else
                        sql = $"SELECT {NamesOfFuilds()} FROM \"{table_name}\" WHERE \"{selectedParameter}\" = CAST({item} AS integer);";
                }
                else
                {
                    sql = $"SELECT {NamesOfFuilds()} FROM \"{table_name}\" WHERE lower(\"{selectedParameter}\") = lower('{parameterValue}');";
                }
            }
            return sql;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = SQL_Find();
            try
            {

                using (connect = new NpgsqlConnection(connectionstring))
                {
                    connect.Open();

                    using (sqlCommand = new NpgsqlCommand(sql, connect))
                    {
                        using (var reader = sqlCommand.ExecuteReader())
                        {
                            dt.Load(reader);
                            MessageBox.Show($"Количество полей - {dt.Rows.Count}", "Информация");
                        }
                    }
                }
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
                this.Close();
            }

            
        }

        public DataTable GetResultTable()
        {
            return dt;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Find_Load(object sender, EventArgs e)
        {
            if(table_name == "Category.Attribute")
            {
                parametrs = Get_Parametrs_Of_Table("Category");
                parametrs.AddRange(Get_Parametrs_Of_Table("Attribute"));
                parametrs = parametrs.Distinct().ToList();
            }
            else if(table_name == "Item.Category.Attribute.Attribute_value")
            {
                parametrs = Get_Parametrs_Of_Table("Item");
                parametrs.Remove("item_cost");
                parametrs.Remove("id_provider");
                parametrs.Remove("item_discription");
                parametrs.AddRange(Get_Parametrs_Of_Table("Category"));
                parametrs.AddRange(Get_Parametrs_Of_Table("Attribute"));
                parametrs.AddRange(Get_Parametrs_Of_Table("Attribute_value"));
                parametrs = parametrs.Distinct().ToList();
                parametrs.Remove("id_category");
                parametrs.Remove("id_attribute");
                parametrs.Remove("id_attribute_value");
            }
            else
            {
                parametrs = Get_Parametrs_Of_Table();
            }
            
            FillComboBox();
        }
    }
}
