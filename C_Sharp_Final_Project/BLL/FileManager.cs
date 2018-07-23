using BOL;
using DAL;
using System;
using System.Linq;


namespace BLL
{
    class FileManager
    {

        /// <summary>
        /// InsertFile insert into DB a FileName 
        /// using a stored procedure that created in SQL
        /// </summary>
        /// <param name="newFile">An object from BOL that will be passed to DB</param> 
        #region  public static void InsertFile(FileModel newFile)


        public static void InsertFile(FileModel newFile)
        {
            try
            {
                using (FilesDBEntities ef = new FilesDBEntities())
                {
                    ef.InsertFiles(newFile.FileName);
                    ef.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        #endregion


        /// <summary>
        /// GetId checks if the FileName already exists
        /// and returns it Id:
        /// 0 if not exist,
        /// current Id if exist
        /// </summary>
        /// <param name="searchFile">Gets an object from BOL</param>
        /// <returns>Returns a status of the</returns>
        #region public static int GetId(FileModel searchFile)


        public static int GetId(FileModel searchFile)
        {
            try
            {
                using (FilesDBEntities ef = new FilesDBEntities())
                {

                    var fileId = ef.Files
                                  .Where((s) => s.FileName == searchFile.FileName)
                                  .Select((s) => s.FileId);

                    if (fileId != null)
                    {
                        return fileId.FirstOrDefault();
                    }

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
        #endregion
    }
}
