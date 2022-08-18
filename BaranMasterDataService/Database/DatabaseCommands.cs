
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BaranMasterDataService.Database
{
    public class DatabaseCommands
    {
        private readonly BaranMasterDataDBPath _baranMasterDataDBPath;
        private const int CORRECT = 4;
        private const int INCORRECT = 0;
        public DatabaseCommands(BaranMasterDataDBPath baranMasterDataDBPath)
        {
            _baranMasterDataDBPath = baranMasterDataDBPath;   
        }
        
        public void updateLogs(List<CNMaterials> cnMaterialsList,int isCorrect)
        {
            SqlConnection connection = new SqlConnection(_baranMasterDataDBPath.DatabasePath);
            CNMaterials cnMaterial = new CNMaterials();
            connection.Open();
            try
            {
                foreach (var item in cnMaterialsList)
                {
                    cnMaterial = item;
                   
                    SqlCommand command = new SqlCommand("updateFSRDate", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@material", SqlDbType.NVarChar).Value = cnMaterial.Material;
                    command.Parameters.AddWithValue("@fsr_date", SqlDbType.DateTime).Value = DateTime.Now;
                    command.ExecuteNonQuery();
                    insertToTransactionLog(cnMaterial, isCorrect);
                }
            }
            catch (Exception ex)
            {
                
                string error = "There is an error while transferring FSRDate";
                insertToErrorLog(cnMaterial, error, ex);
                insertToTransactionLog(cnMaterial,INCORRECT);
            }
            connection.Close();
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();

            }
        }
        public void insertToTransactionLog(CNMaterials cnMaterials,int isCorrect)
        {
            SqlConnection connection = new SqlConnection(_baranMasterDataDBPath.DatabasePath);
            connection.Open();
            try
            {
                
                SqlCommand command = new SqlCommand("insertToTransactionLog", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@material", SqlDbType.NVarChar).Value = cnMaterials.Material;
                int transactionResult = 0;
                string transactionMessage = "transaction successful";
                if (isCorrect == CORRECT)
                {
                    transactionResult = 1;
                    transactionMessage = "transaction successful";
                }
                else
                {
                    transactionResult = 0;
                    transactionMessage = "transaction failed";
                }
                command.Parameters.AddWithValue("@transaction_result", SqlDbType.Int).Value = transactionResult;
                command.Parameters.AddWithValue("@transaction_message", SqlDbType.NVarChar).Value = transactionMessage;
                command.ExecuteNonQuery();

            }
            catch(Exception ex)
            {
                string error = "Error occured while transfering to transaction log";
                DatabaseCommands databaseCommands = new DatabaseCommands(_baranMasterDataDBPath);
                databaseCommands.insertToErrorLog(cnMaterials,error,ex);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
               
            }       
        }
        public void insertToErrorLog(CNMaterials cnMaterials,string error,Exception ex)
        {
            SqlConnection connection = new SqlConnection(_baranMasterDataDBPath.DatabasePath);
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand("insertToErrorLog",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@error_date",SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.AddWithValue("@error_material_code", SqlDbType.NVarChar).Value = cnMaterials.Material;
                command.Parameters.AddWithValue("@error_message", SqlDbType.NVarChar).Value = error + ex.Message + ex.StackTrace;
                command.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
             
                SqlCommand command = new SqlCommand("insertToErrorLog", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@error_date", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.AddWithValue("@error_material_code", SqlDbType.NVarChar).Value = cnMaterials.Material;
                command.Parameters.AddWithValue("@error_message", SqlDbType.NVarChar).Value = "There is an error while inserting to error_log" + e.Message + e.StackTrace;
                command.ExecuteNonQuery();           
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }

            }
        }
        public void insertToErrorLog(CNMaterials cnMaterials, string error)
        {
            SqlConnection connection = new SqlConnection(_baranMasterDataDBPath.DatabasePath);
            connection.Open();
            try
            {

                
                SqlCommand command = new SqlCommand("insertToErrorLog", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@error_date", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.AddWithValue("@error_material_code", SqlDbType.NVarChar).Value = cnMaterials.Material;
                command.Parameters.AddWithValue("@error_message", SqlDbType.NVarChar).Value = error;
                command.ExecuteNonQuery();
              
            }
            catch (Exception e)
            {
              
                SqlCommand command = new SqlCommand("inserToErrorLog", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@error_date", SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.AddWithValue("@error_material_code", SqlDbType.NVarChar).Value = cnMaterials.Material;
                command.Parameters.AddWithValue("@error_message", SqlDbType.NVarChar).Value = "There is an error while inserting to error_log" + e.Message + e.StackTrace;
                command.ExecuteNonQuery();
                

            }

            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }

            }

        }

        public List<CNMaterials> findNullFSRDate()
        {
            SqlConnection connection = new SqlConnection(_baranMasterDataDBPath.DatabasePath);
            CNMaterials cnMaterials = new CNMaterials();
            List<CNMaterials> cnMaterialsObjectList = new List<CNMaterials>();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand("findNullFSRDate",connection);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    CNMaterials cnMaterial = new CNMaterials();
                    cnMaterials = cnMaterial;
                    cnMaterial.Material = reader[0].ToString();
                    cnMaterial.SText = reader[1].ToString();
                    cnMaterial.QUnit = reader[2].ToString();
                    cnMaterial.CNWDate = reader[3].ToString();
                    cnMaterial.DDWDate = reader[4].ToString();
                    cnMaterial.FSWDate = reader[5].ToString();
                    cnMaterial.CNRDate = reader[6].ToString();
                    cnMaterial.DDRDate = reader[7].ToString();
                    cnMaterial.FSRDate = reader[8].ToString();
                    cnMaterialsObjectList.Add(cnMaterial);
                }
                
                reader.Close();
                command.ExecuteNonQuery();
                connection.Close();


            }
            catch(Exception ex)
            {
                string error = "Error occured while finding FSRDate which is NULL";
                insertToErrorLog(cnMaterials,error,ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();

                }
            }
            return cnMaterialsObjectList;
        }
    }
}
