using BOL;
using DAL;
using System;
using System.Linq;


namespace BLL
{
    public class SearchManager
    {

        /// <summary>
        /// InsertRequest insert into DB a SearchName and DirectoryName
        /// using a stored procedure that created in SQL
        /// </summary>
        /// <param name="newSearch">An object from BOL that will be passed to DB</param>
        #region public static void InsertRequest(SearchModel newSearch)

        public static void InsertRequest(SearchModel newSearch)
        {
            try
            {
                using (FilesDBEntities ef = new FilesDBEntities())
                {

                    ef.InsertSearches(newSearch.SearchName, newSearch.DirectoryName);
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
        /// GetId checks if the SearchName and DirectoryName already exists
        /// and returns it Id:
        /// 0 if not exist,
        /// current Id if exist
        /// </summary>
        /// <param name="search">Gets an object from BOL</param>
        /// <returns>Returns a status of the SearchId</returns>
        #region public static int GetId(SearchModel search)

        public static int GetId(SearchModel search)
        {
            try
            {
                using (FilesDBEntities ef = new FilesDBEntities())
                {

                    var searchId = ef.Searches
                                  .Where((s) => s.SearchName == search.SearchName && s.DirectoryName == search.DirectoryName)
                                  .Select((s) => s.SearchID);

                    if (searchId != null)
                    {
                        return searchId.FirstOrDefault();
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
