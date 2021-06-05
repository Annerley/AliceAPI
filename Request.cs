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
        //если even = NULL, ЗНАЧИТ ЕСТЬ НА ОБОИХ НЕДЕЛЯХ
        //type 0 - лекция 1 - семинар
        //subgroup = NULL => для всей группы
        //с четными пока не работает, еще думаю как сделать



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


            MySqlCommand cmd = new MySqlCommand("SELECT `lesson_name`, `lesson_number` FROM schedule WHERE `group_id` = @group_id AND (`subgroup` = @subgroup OR `subgroup` IS NULL) AND `week_day` = @week", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@week", MySqlDbType.VarChar).Value = weekDay;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sb.Append(reader[1].ToString() + " " + reader[0].ToString()+" ");
            }

            reader.Close();
            db.closeConnection();

            return sb.ToString().Trim();

        }

        /// <summary>Schedules for one lesson by requesting with number of the lesson.</summary>
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
            MySqlCommand cmd = new MySqlCommand("SELECT `lesson_name` FROM schedule WHERE `group_id` = @group_id AND (`subgroup` = @subgroup OR `subgroup` IS NULL) AND `lesson_number` = @number AND `week_day` = @week", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@number", MySqlDbType.VarChar).Value = numberOfLesson;
            cmd.Parameters.Add("@week", MySqlDbType.VarChar).Value = weekDay;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sb.Append(reader[0].ToString() + " ");
            }

            reader.Close();
            db.closeConnection();

            return sb.ToString().Trim();

        }

        /// <summary>Request room for one lesson by its number</summary>
        /// <param name="weekDay">The week day in Russian.</param>
        /// <param name="numberOfLesson">The number of lesson.</param>
        /// <param name="student">Name of student on Russian</param>
        /// <returns>return number of lesson (type str)</returns>
        public string roomForLesson(string weekDay, string numberOfLesson, string student)
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

            MySqlCommand cmd = new MySqlCommand("SELECT `room` FROM schedule WHERE `group_id` = @group_id AND (`subgroup` = @subgroup OR `subroup` IS NULL) AND `lesson_number` = @number AND `week_day` = @week", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@number", MySqlDbType.VarChar).Value = numberOfLesson;
            cmd.Parameters.Add("@week", MySqlDbType.VarChar).Value = weekDay;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            reader.Read();
            sb.Append(reader[0].ToString());

            reader.Close();
            db.closeConnection();

            return sb.ToString().Trim();

        }

        /// <summary>Request quantity of lessons on one day of the week</summary>
        /// <param name="weekDay">The week day on Russian</param>
        /// <param name="student">The student name on Russian.</param>
        /// <returns>Return quantity in str format.</returns>
        public string quantityOfLessonsDay(string weekDay, string student)
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

            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM schedule WHERE `group_id` = @group_id AND (`subgroup` = @subgroup OR `subgroup` IS NULL) AND `week_day` = @week", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@week", MySqlDbType.VarChar).Value = weekDay;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            reader.Read();
            sb.Append(reader[0].ToString());

            reader.Close();
            db.closeConnection();

            return sb.ToString().Trim();
        }

        public string firstLesson(string weekDay, string student)
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

            MySqlCommand cmd = new MySqlCommand("SELECT MIN(lesson_number) FROM schedule WHERE `group_id` = @group_id AND (`subgroup` = @subgroup OR `subgroup` IS NULL) AND `week_day` = @week", db.getConnection());

            cmd.Parameters.Add("@group_id", MySqlDbType.VarChar).Value = group_id;
            cmd.Parameters.Add("@subgroup", MySqlDbType.VarChar).Value = sub_id;
            cmd.Parameters.Add("@week", MySqlDbType.VarChar).Value = weekDay;

            StringBuilder sb = new StringBuilder();
            reader = cmd.ExecuteReader();
            reader.Read();
            sb.Append(reader[0].ToString());

            reader.Close();
            db.closeConnection();

            return sb.ToString().Trim();
        }

        public string breakStart(string lessonNumber)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT `start_time` FROM break_schedule WHERE `number` = @number", db.getConnection());
            searchGroup.Parameters.Add("@name", MySqlDbType.Int32).Value = lessonNumber;
            MySqlDataReader reader = searchGroup.ExecuteReader();
            reader.Read();
            return reader[0].ToString().Trim();
            
           
        }
        public string breakStartAndFinish(string lessonNumber)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT `start_time`, `end_time` FROM break_schedule WHERE `number` = @number", db.getConnection());
            searchGroup.Parameters.Add("@name", MySqlDbType.Int32).Value = lessonNumber;
            MySqlDataReader reader = searchGroup.ExecuteReader();
            reader.Read();
            return reader[0].ToString().Trim()+" "+reader[1].ToString().Trim();
        }

        public string breakFinish(string lessonNumber)
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT`end_time` FROM break_schedule WHERE `number` = @number", db.getConnection());
            searchGroup.Parameters.Add("@name", MySqlDbType.Int32).Value = lessonNumber;
            MySqlDataReader reader = searchGroup.ExecuteReader();
            reader.Read();
            return reader[0].ToString().Trim();
        }

        public string schedule()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand searchGroup = new MySqlCommand("SELECT * FROM break_schedule", db.getConnection());
            
            MySqlDataReader reader = searchGroup.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append(reader[0].ToString() + " пара начинается в ");
                sb.Append(reader[1].ToString() + " и заканчивается в");
                sb.Append(reader[2].ToString() + " ");

            }
            return sb.ToString();
        }

        public string windowScheduleWeek(string weekDay, string student)
        {
            //в процессе
            return "";
        }
    }
}
