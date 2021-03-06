﻿using COMP___1640.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace COMP___1640.DAL
{
    public class DataAccess
    {
        public static SqlConnection Connect()
        {
            //return new SqlConnection("Data Source=.;Initial Catalog=IdeasCampaignManager_ver2;Integrated Security=False;User Id=sa;Password=abc@12345;MultipleActiveResultSets=True");
            //return new SqlConnection("workstation id=IdeasCampaignManager.mssql.somee.com;packet size=4096;user id=minhdn2_SQLLogin_1;pwd=anxh9wmukk;data source=IdeasCampaignManager.mssql.somee.com;persist security info=False;initial catalog=IdeasCampaignManager");
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString);
        }
        #region PersonalDetail
        public PersonalDetails CheckLogIn(string email, string pass)
        {
            var query = string.Format("SELECT * FROM PersonalDetail WHERE p_Email = '{0}' AND p_Pass = '{1}'", email, pass);
            var conn = Connect();
            try
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                var u = new PersonalDetails();

                while (reader.Read())
                {
                    u.Id = int.Parse(reader["p_ID"].ToString());
                    u.roleId = int.Parse(reader["r_ID"].ToString());
                    u.departmentId = int.Parse(reader["dp_ID"].ToString());
                    u.Name = reader["p_Name"].ToString();
                    u.Email = reader["p_Email"].ToString();
                    u.Pass = reader["p_Pass"].ToString();
                    u.Details = reader["p_Detail"].ToString();
                }

                conn.Close();
                return u;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public PersonalDetails GetUserById(int id)
        {
            var query = string.Format("SELECT * FROM PersonalDetail WHERE p_ID = {0}", id);
            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var u = new PersonalDetails();

                    while (reader.Read())
                    {
                        u.Id = int.Parse(reader["p_ID"].ToString());
                        u.roleId = int.Parse(reader["r_ID"].ToString());
                        u.departmentId = int.Parse(reader["dp_ID"].ToString());
                        u.Name = reader["p_Name"].ToString();
                        u.Email = reader["p_Email"].ToString();
                        u.Pass = reader["p_Pass"].ToString();
                        u.Details = reader["p_Detail"].ToString();
                    }

                    conn.Close();
                    return u;
                }

                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public List<PersonalDetails> GetAllUsers()
        {
            var lst = new List<PersonalDetails>();

            var query = string.Format("SELECT * FROM PersonalDetail");

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tp = new PersonalDetails();
                    tp.Id = int.Parse(reader["p_ID"].ToString());
                    tp.roleId = int.Parse(reader["r_ID"].ToString());
                    tp.departmentId = int.Parse(reader["dp_ID"].ToString());
                    tp.Name = reader["p_Name"].ToString();
                    tp.Email = reader["p_Email"].ToString();
                    tp.Pass = reader["p_Pass"].ToString();
                    tp.Details = reader["p_Detail"].ToString();

                    lst.Add(tp);
                }

                conn.Close();
                return lst;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public int AddUser(PersonalDetails u)
        {
            var id = -1;

            var query = string.Format(@"INSERT INTO PersonalDetail(r_ID, dp_ID, p_Name, p_Email, p_Pass, p_Detail)
output INSERTED.p_ID
VALUES ({0}, {1}, '{2}', '{3}', '{4}', '{5}')",
u.roleId, u.departmentId, u.Name, u.Email, u.Pass, u.Details);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                id = (int)cmd.ExecuteScalar();

                conn.Close();
                return id;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        public bool UpdateUser(PersonalDetails u)
        {
            var stt = false;
            var query = string.Format("UPDATE PersonalDetail SET r_ID = {0}, dp_ID = {1}, p_Name = '{2}', p_Email = '{3}', p_Pass = '{4}', p_Detail = '{5}' WHERE p_ID = {6}",
                u.roleId, u.departmentId, u.Name, u.Email, u.Pass, u.Details, u.Id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }
        #endregion

        #region Topic
        public int AddTopic(Topic tp)
        {
            //Check logic Closure Date and Final Closure Date
            if ((DateTime.Parse(tp.ClosureDate.ToString()) - DateTime.Parse(tp.FinalClosureDate.ToString())).TotalDays > 0)
            {
                return -1;
            }

            if ((DateTime.Parse(tp.PostedDate.ToString()) - DateTime.Parse(tp.ClosureDate.ToString())).TotalDays > 0)
            {
                return -1;
            }

            var id = -1;
            var query = string.Format(@"INSERT INTO Topic (t_Name, t_Details, t_PostedDate, t_ClosureDate, t_FinalClosureDate)
output INSERTED.t_ID
VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
tp.Name, tp.Details, tp.PostedDate, tp.ClosureDate, tp.FinalClosureDate);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                id = (int)cmd.ExecuteScalar();

                conn.Close();
                return id;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        public Topic GetTopicById(int id)
        {
            var tp = new Topic();
            var query = string.Format("SELECT * FROM Topic WHERE t_ID = {0}", id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tp.Id = id;
                    tp.Name = reader["t_Name"].ToString();
                    tp.Details = reader["t_Details"].ToString();

                    var posted = reader["t_PostedDate"].ToString().Split(' ');
                    var temp = reader["t_PostedDate"];
                    var closure = reader["t_ClosureDate"].ToString().Split(' ');
                    var final = reader["t_FinalClosureDate"].ToString().Split(' ');

                    var dt_posted = string.IsNullOrEmpty(posted[0]) ? "" : posted[0];
                    var dt_closure = string.IsNullOrEmpty(closure[0]) ? "" : closure[0];
                    var dt_final = string.IsNullOrEmpty(final[0]) ? "" : final[0];
                    var sysDtFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                    tp.PostedDate = DateTime.ParseExact(dt_posted, sysDtFormat, CultureInfo.InvariantCulture);
                    tp.ClosureDate = DateTime.ParseExact(dt_closure, sysDtFormat, CultureInfo.InvariantCulture);
                    tp.FinalClosureDate = DateTime.ParseExact(dt_final, sysDtFormat, CultureInfo.InvariantCulture);
                }

                conn.Close();
                return tp;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public List<Topic> GetAllTopics()
        {
            var lstTp = new List<Topic>();
            var query = string.Format("SELECT * FROM Topic");

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tp = new Topic();
                    tp.Id = int.Parse(reader["t_ID"].ToString());
                    tp.Name = reader["t_Name"].ToString();
                    tp.Details = reader["t_Details"].ToString();
                    tp.PostedDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["t_PostedDate"].ToString()) ? "" : reader["t_PostedDate"].ToString());
                    tp.ClosureDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["t_ClosureDate"].ToString()) ? "" : reader["t_ClosureDate"].ToString());
                    tp.FinalClosureDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["t_FinalClosureDate"].ToString()) ? "" : reader["t_FinalClosureDate"].ToString());

                    lstTp.Add(tp);
                }

                conn.Close();
                return lstTp;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public bool UpdateTopic(Topic tp)
        {
            var stt = false;
            var query = string.Format("UPDATE Topic SET t_ClosureDate = '{0}', t_FinalClosureDate = '{1}' WHERE t_ID = '{2}'", tp.ClosureDate, tp.FinalClosureDate, tp.Id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }
        #endregion

        #region Idea
        public int AddIdea(Idea idea)
        {
            var id = -1;
            var query = string.Format(@"INSERT INTO Idea (t_ID, c_ID, p_ID, i_Title, i_Details, DocumentLink, i_IsAnonymous, TotalViews, i_PostedDate) 
output INSERTED.i_ID 
VALUES ({8}, {0}, {1}, '{2}', '{3}', '{4}', {5}, {6}, '{7}')",
idea.CategoryId, idea.PersonalId, idea.Title, idea.Details, idea.DocumentLink, idea.isAnonymous, idea.TotalViews, idea.PostedDate, idea.topicId);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                id = (int)cmd.ExecuteScalar();

                conn.Close();
                return id;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        public Idea GetIdeaById(int id)
        {
            var idea = new Idea();
            var query = string.Format("SELECT * FROM Idea WHERE i_ID = {0}", id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idea.Id = id;
                    idea.CategoryId = int.Parse(reader["c_ID"].ToString());
                    idea.PersonalId = int.Parse(reader["p_ID"].ToString());
                    idea.Title = reader["i_Title"].ToString();
                    idea.Details = reader["i_Details"].ToString();
                    idea.DocumentLink = reader["DocumentLink"].ToString();
                    idea.isAnonymous = bool.Parse(reader["i_IsAnonymous"].ToString()) ? 1 : 0;
                    idea.TotalViews = int.Parse(reader["TotalViews"].ToString());
                    idea.PostedDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["i_PostedDate"].ToString()) ? "" : reader["i_PostedDate"].ToString());
                    //idea.ClosureDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["i_ClosureDate"].ToString()) ? "" : reader["i_ClosureDate"].ToString());
                }
                conn.Close();
                return idea;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public List<Idea> GetIdeasByTopic(int tpId)
        {
            var lstIdea = new List<Idea>();
            var query = string.Format("SELECT * FROM Idea WHERE t_ID = {0}", tpId);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var idea = new Idea();
                        idea.Id = int.Parse(reader["i_ID"].ToString());
                        idea.CategoryId = int.Parse(reader["c_ID"].ToString());
                        idea.PersonalId = int.Parse(reader["p_ID"].ToString());
                        idea.Title = reader["i_Title"].ToString();
                        idea.Details = reader["i_Details"].ToString();
                        idea.DocumentLink = reader["DocumentLink"].ToString();
                        idea.isAnonymous = bool.Parse(reader["i_IsAnonymous"].ToString()) ? 1 : 0;
                        idea.TotalViews = int.Parse(reader["TotalViews"].ToString());
                        idea.PostedDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["i_PostedDate"].ToString()) ? "" : reader["i_PostedDate"].ToString());

                        lstIdea.Add(idea);
                    }

                    conn.Close();
                    return lstIdea;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }
        #endregion

        #region Category
        public List<Category> GetAllCategory()
        {
            var query = string.Format("SELECT * FROM Category");
            var lstCat = new List<Category>();

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var cat = new Category
                        {
                            Id = int.Parse(reader["c_ID"].ToString()),
                            Name = reader["c_Name"].ToString(),
                            Description = reader["c_Description"].ToString()
                        };
                        lstCat.Add(cat);
                    }
                    conn.Close();
                    return lstCat;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public Category GetCategoryById(int id)
        {
            var c = new Category();
            var query = string.Format("SELECT * FROM Category WHERE c_ID = {0}", id);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        c.Id = int.Parse(reader["c_ID"].ToString());
                        c.Name = reader["c_Name"].ToString();
                        c.Description = reader["c_Description"].ToString();
                    }

                    conn.Close();
                    return c;
                }

                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public bool AddCategory(Category cat)
        {
            var query = string.Format("INSERT INTO Category(c_Name, c_Description) VALUES('{0}', '{1}')", cat.Name, cat.Description);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }

        public bool UpdateCategory(Category cat)
        {
            var stt = false;
            var query = string.Format("UPDATE Category SET c_Name = '{0}', c_Description = '{1}' WHERE c_ID = {2}", cat.Name, cat.Description, cat.Id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            var stt = false;
            var query = string.Format("DELETE Category WHERE c_ID = {0}", id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }
        #endregion

        #region Role
        public Role GetRoleById(int id)
        {
            var r = new Role();
            var query = string.Format("SELECT * FROM Role WHERE r_ID = {0}", id);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        r.Id = int.Parse(reader["r_ID"].ToString());
                        r.Name = reader["r_Name"].ToString();
                        r.Description = reader["r_Description"].ToString();
                    }
                    conn.Close();
                    return r;
                }

                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public List<Role> GetAllRoles()
        {
            var lstRoles = new List<Role>();
            var query = string.Format("SELECT * FROM Role");

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var r = new Role();
                    r.Id = int.Parse(reader["r_ID"].ToString());
                    r.Name = reader["r_Name"].ToString();
                    r.Description = reader["r_Description"].ToString();

                    lstRoles.Add(r);
                }
                conn.Close();
                return lstRoles;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public bool AddRole(Role role)
        {
            var query = string.Format("INSERT INTO Role(r_Name, r_Description) VALUES('{0}', '{1}')", role.Name, role.Description);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }

        public bool UpdateRole(Role r)
        {
            var stt = false;
            var query = string.Format("UPDATE Role SET r_Name = '{0}', r_Description = '{1}' WHERE r_ID = {2}", r.Name, r.Description, r.Id);

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Comment
        public bool AddComment(Comment cmt)
        {


            var stt = false;
            var query = string.Format("INSERT INTO Comment (I_ID, p_ID, cmt_Detail, cmt_IsAnonymous, cmt_PostedDate) VALUES ({0}, {1}, '{2}', '{3}', '{4}')",
                cmt.ideaId, cmt.personId, cmt.Details, cmt.isAnonymous.ToString().ToLower(), cmt.postedDate);

            try
            {
                var conn = Connect();
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Comment> GetCommentsByIdea(int ideaId)
        {
            var lstCmt = new List<Comment>();
            var query = string.Format("SELECT * FROM Comment WHERE I_ID = {0}", ideaId);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cmt = new Comment();
                    cmt.Id = int.Parse(reader["cmt_ID"].ToString());
                    cmt.ideaId = int.Parse(reader["I_ID"].ToString());
                    cmt.personId = int.Parse(reader["p_ID"].ToString());
                    cmt.Details = reader["cmt_Detail"].ToString();
                    cmt.isAnonymous = bool.Parse(reader["cmt_IsAnonymous"].ToString());
                    cmt.postedDate = Convert.ToDateTime(string.IsNullOrEmpty(reader["cmt_PostedDate"].ToString()) ? "" : reader["cmt_PostedDate"].ToString());

                    lstCmt.Add(cmt);
                }
                conn.Close();
                return lstCmt;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }
        #endregion

        #region Department
        public List<Department> GetAllDepartments()
        {
            var lst = new List<Department>();

            var query = string.Format("SELECT * FROM Department");

            var conn = Connect();

            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tp = new Department();
                    tp.Id = int.Parse(reader["dp_ID"].ToString());
                    tp.Name = reader["dp_Name"].ToString();
                    tp.Details = reader["dp_Description"].ToString();

                    lst.Add(tp);
                }

                conn.Close();
                return lst;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public Department GetDepartmentById(int id)
        {
            var dp = new Department();
            var query = string.Format("SELECT * FROM Department WHERE dp_ID = {0}", id);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dp.Id = int.Parse(reader["dp_ID"].ToString());
                        dp.Name = reader["dp_Name"].ToString();
                        dp.Details = reader["dp_Description"].ToString();
                    }
                    conn.Close();
                    return dp;
                }

                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }
        #endregion


        #region Voting
        public Voting GetIdeaVote(int ideaId, int userId)
        {
            var vote = new Voting();
            var query = string.Format("SELECT * FROM Voting WHERE i_ID = {0} AND p_ID = {1}", ideaId, userId);

            var conn = Connect();
            try
            {
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        vote.IdeaId = int.Parse(reader["i_ID"].ToString());
                        vote.PersonId = int.Parse(reader["p_ID"].ToString());
                        vote.Vote = reader["Vote"].ToString();
                    }

                }
                conn.Close();
                return vote;
            }
            catch (Exception ex)
            {
                conn.Close();
                return null;
            }
        }

        public bool Voting(int ideaId, int userId, bool vote)
        {
            var stt = false;
            var query = string.Format("INSERT INTO Voting(i_ID, p_ID, Vote) VALUES({0}, {1}, {2})", ideaId, userId, vote ? "1" : "0");

            try
            {
                var conn = Connect();
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteVote(int ideaId, int userId)
        {
            var stt = false;
            var query = string.Format("DELETE Voting WHERE i_ID = {0} AND p_ID = {1}", ideaId, userId);

            try
            {
                var conn = Connect();
                conn.Open();

                var cmd = new SqlCommand(query, conn);
                stt = cmd.ExecuteNonQuery() == 1;

                conn.Close();
                return stt;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Statistic Report
        public int CountIdeasByTopic(int tpId)
        {
            var query = string.Format("SELECT TotalCount = count(*) FROM Idea WHERE t_ID = {0}", tpId);
            var conn = Connect();
            var tp = new TopicUI();
            try
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();



                while (reader.Read())
                {
                    tp.TotalIdeas = int.Parse(reader["TotalCount"].ToString());
                }

                conn.Close();
                return tp.TotalIdeas;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        public int TotalIdeas()
        {
            var total = -1;
            var query = string.Format("SELECT TotalCount = count(*) FROM Idea ");
            var conn = Connect();
            try
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();



                while (reader.Read())
                {
                    total = int.Parse(reader["TotalCount"].ToString());
                }

                conn.Close();
                return total;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        public int TotalCmts()
        {
            var total = -1;
            var query = string.Format("SELECT TotalCount = count(*) FROM Comment ");
            var conn = Connect();
            try
            {
                conn.Open();
                var cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();



                while (reader.Read())
                {
                    total = int.Parse(reader["TotalCount"].ToString());
                }

                conn.Close();
                return total;
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
        }

        //        public Dictionary<string, List<string>> DepartmentStatistics()
        //        {
        //            var dp = GetAllDepartments();
        //            var dic = new Dictionary<string, List<string>>();
        //            if (dp != null)
        //            {
        //                foreach (var x in dp)
        //                {
        //                    dic.Add(x.Name, new List<string> {"0", "0" });
        //                }


        //                var query = string.Format(@"Select dp.dp_Name, Count(dp.dp_Name) as total, (Count(dp.dp_Name)* 100 / (Select Count(*) From idea)) as percentage
        //from Department dp
        //full join PersonalDetail p on p.dp_ID = dp.dp_ID
        //full join idea i on i.p_ID = p.p_ID where i.i_ID is not null
        //group by dp.dp_Name");


        //                var conn = Connect();
        //                try
        //                {
        //                    conn.Open();
        //                    var cmd = new SqlCommand(query, conn);
        //                    var reader = cmd.ExecuteReader();

        //                    while (reader.Read())
        //                    {
        //                        var name = reader["dp_Name"].ToString();
        //                        foreach (var x in dic)
        //                        {
        //                            if (x.Key.Equals(name))
        //                            {
        //                                var total = reader["total"].ToString();
        //                                var percentage = reader["percentage"].ToString();
        //                                var val = new List<string> { total, percentage };
        //                                dic[name] = val;
        //                            }
        //                        }
        //                    }

        //                    conn.Close();
        //                    return dic;
        //                }
        //                catch (Exception ex)
        //                {
        //                    conn.Close();
        //                    return null;
        //                }
        //            }
        //            return null;
        //        }

        public List<Department> DepartmentStatistics()
        {
            var dp = GetAllDepartments();
            
            if (dp != null)
            {
                var query = string.Format(@"Select dp.dp_Name, Count(dp.dp_Name) as total, (Count(dp.dp_Name)* 100 / (Select Count(*) From idea)) as percentage
from Department dp
full join PersonalDetail p on p.dp_ID = dp.dp_ID
full join idea i on i.p_ID = p.p_ID where i.i_ID is not null
group by dp.dp_Name");


                var conn = Connect();
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(query, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var name = reader["dp_Name"].ToString();
                        foreach (var x in dp.ToArray())
                        {
                            if (x.Name.Equals(name))
                            {
                                var total = reader["total"].ToString();
                                var percentage = reader["percentage"].ToString();
                                x.TotalIdeas = total;
                                x.IdeaPercentages = percentage;
                            }
                        }
                    }

                    conn.Close();
                    return dp;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return null;
                }
            }
            return null;
        }

        public List<string> AnonymousPercentages()
        {
            var lstData = new List<string> { "0", "0", "0" };
            var lstQuery = new List<string> {
                string.Format(@"Select i_IsAnonymous, (Count(i_IsAnonymous)* 100 / (Select Count(*) From idea)) as percentage
From idea where i_IsAnonymous = 1
group by i_IsAnonymous"),
                string.Format(@"Select cmt_IsAnonymous, (Count(cmt_IsAnonymous)* 100 / (Select Count(*) From comment)) as percentage
From comment where cmt_IsAnonymous = 1
group by cmt_IsAnonymous"),
                string.Format(@"Select distinct ((select count(*) from idea i
left join comment b on b.I_ID = i.i_ID where b.I_ID is null)* 100 / (Select Count(*) From idea)) as percentage
From idea")
        };

            for (int y = 0; y < lstQuery.Count; y++)
            {
                var conn = Connect();
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(lstQuery[y], conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var percentage = reader["percentage"].ToString();
                        lstData[y] = percentage;
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return lstData;
        }
        #endregion

    }
}