using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace AliceAPI
{
    public class Request
    {

        /// <summary>Schedule for the day by requesting with name of the day and student name</summary>
        /// <param name="weekDay">Name of the day of the week in Russian</param>
        /// <param name="student">Name od the student in Russian. Ex."Лапутина Дарья Кирилловна"</param>
        /// <returns>Return string with numbers of lesson and its names. "1 Мат. анализ 2 ТСПП 3 Физ-ра" </returns>
        public string scheduleForDay(string weekDay, string student)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT `group_id`, `id` FROM students WHERE `name` = @name", db.getConnection());
            searchGroup.Parameters.Add("@name", MySqlDbType.VarChar).Value = student;
            MySqlDataReader reader = searchGroup.ExecuteReader();
            reader.Read();
            string group_id = reader[0].ToString();
            string student_id = reader[1].ToString();
            reader.Close();


            MySqlCommand searchSubgroup = new MySqlCommand("SELECT `subgroup_id` FROM `group_to_student` WHERE `student_id` = @st_id", db.getConnection());
            searchSubgroup.Parameters.Add("@st_id", MySqlDbType.VarChar).Value = student_id;
            reader = searchSubgroup.ExecuteReader();
            reader.Read();
            string sub_id = reader[0].ToString();
            reader.Close();


            MySqlCommand cmd = new MySqlCommand("SELECT `lesson_name`, `lesson_number` FROM schedule WHERE `group_id` = @group_id AND `subgroup` = @subgroup", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sb.Append(reader[1].ToString() + " " + reader[0].ToString()+" ");
            }
            return sb.ToString().Trim();

        }

        /// <summary>Schedules for one lesson.</summary>
        /// <param name="weekDay">The week day in Russian.</param>
        /// <param name="numberOfLesson">The number of lesson.</param>
        /// <param name="student">The student full-name.</param>
        /// <returns>String with lessons</returns>
        public string scheduleForOneLesson(string weekDay, string numberOfLesson, string student)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT `group_id`, `id` FROM students WHERE `name` = @name", db.getConnection());
            searchGroup.Parameters.Add("@name", MySqlDbType.VarChar).Value = student;
            MySqlDataReader reader = searchGroup.ExecuteReader();
            reader.Read();
            string group_id = reader[0].ToString();
            string student_id = reader[1].ToString();
            reader.Close();


            MySqlCommand searchSubgroup = new MySqlCommand("SELECT `subgroup_id` FROM `group_to_student` WHERE `student_id` = @st_id", db.getConnection());
            searchSubgroup.Parameters.Add("@st_id", MySqlDbType.VarChar).Value = student_id;
            reader = searchSubgroup.ExecuteReader();
            reader.Read();
            string sub_id = reader[0].ToString();
            reader.Close();

            //пофиксить для подгрупп = NULL
            MySqlCommand cmd = new MySqlCommand("SELECT `lesson_name` FROM schedule WHERE `group_id` = @group_id AND `subgroup` = @subgroup AND `lesson_number` = @number", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@number", MySqlDbType.VarChar).Value = numberOfLesson;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sb.Append(reader[0].ToString() + " ");
            }
            return sb.ToString().Trim();

        }


    }
}
