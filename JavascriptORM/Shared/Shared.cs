using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    /// <summary>
    /// OperationType Class
    /// </summary>
    public static class OperationType
    {
        //add the mappings for operation type and its corresponding sql query text
        public static readonly Dictionary<String, String> Map
            = new Dictionary<String, String>
            {
                {Read, OperationText.Read},
                {Insert, OperationText.Insert},
                {Update, OperationText.Update},
                {Delete, OperationText.Delete}
            };      
        
        public const String Read = "Read";
        public const String Insert = "Insert";
        public const String Update = "Update";
        public const String Delete = "Delete";
    }

    /// <summary>
    /// Class to provide custom exception messages
    /// </summary>
    public static class Exceptions
    {
        public const String Error = "Some error has occured";
    }

    /// <summary>
    /// ColumnValues Class
    /// </summary>
    public class ColumnValues
    {
        public String ColName { get; set; }
        public String ColValue { get; set; }
    }

    /// <summary>
    /// OperationText Class
    /// </summary>
    public static class OperationText
    {
        public const String Read = "SELECT * FROM tbl_contacts ORDER BY id asc";
        public const String Insert = "INSERT INTO tbl_contacts(name, phone, address) VALUES(@name, @phone, @address)";
        public const String Update = "UPDATE tbl_contacts SET name = @name, phone = @phone, address = @address WHERE id = @id";
        public const String Delete = "DELETE FROM tbl_contacts WHERE id = @id";
    }
}
