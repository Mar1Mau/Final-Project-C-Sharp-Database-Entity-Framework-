using BOL;
using DAL;
using System;
using System.IO;



namespace BLL
{
    public class FileSearchManager
    {
        static string defaultDirectory = "c:\\";

        
        #region Generic Delegates

        public static event Action<string> SearchHandler;
        public static event Action<string> ErrorHandler;
        public static event Action<string> ScanFinishedHandler;

        #endregion


        /// <summary>
        /// Functions for searching files
        /// </summary>
        #region Overloading functions

        /// <summary>
        /// StartNewSearch is overloading function.
        /// Starts searching files without a specific directory
        /// </summary>
        /// <param name="searchStr">The name of the file name from the user</param>
        public static void StartNewSearch(string searchStr)
        {
            StartNewSearch(searchStr, defaultDirectory);
        }



        /// <summary>
        /// StartNewSearch is overloading function 
        /// Starts searching files with a specific directory
        /// </summary>
        /// <param name="searchStr">The name of the file name from the user</param>
        /// <param name="searchDrct">The name of the directory from the user</param>
        public static void StartNewSearch(string searchStr, string searchDrct)
        {

            if (searchStr == "")
            {
                ErrorHandler?.Invoke("Please enter a filename to search ");
                return;
            }


            SearchModel searchModel = new SearchModel() { SearchName = searchStr, DirectoryName = searchDrct };

            searchModel.SearchID = SearchManager.GetId(searchModel);

            if (searchModel.SearchID == 0)
            {
                SearchManager.InsertRequest(new SearchModel { SearchName = searchStr, DirectoryName = searchDrct });
                searchModel.SearchID = SearchManager.GetId(searchModel);
            }

            int found = SearchDirectoryAndString(searchModel, searchDrct);

            if (found == 0)
            {
                ErrorHandler?.Invoke("No files found!!!");
            }
            else
            {
                ScanFinishedHandler?.Invoke($"Scan finished. Found {found} files.");
            }

        }
        #endregion


        /// <summary>
        /// SearchDirectoryAndString is a recursive function.
        /// Searches for files in all directories
        /// </summary>
        /// <param name="search">An object that will be scanned if its SearchName exists</param>
        /// <param name="currentDirectory">The point to start searching</param>
        /// <returns>Returns the number of files found</returns>
        #region private static int SearchDirectoryAndString(SearchModel search, string currentDirectory)

        private static int SearchDirectoryAndString(SearchModel search, string currentDirectory)
        {
            int numFilesFound = 0;

            string[] files = new string[0];

            try
            {
                files = Directory.GetFiles(currentDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine($"OOPS : {e.Message}");
            }

            foreach (string filePath in files)
            {
                try
                {

                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);

                    if (fileName.ToLower().Contains(search.SearchName.ToLower()))
                    {
                        numFilesFound++;

                        SearchHandler?.Invoke(filePath);

                        FileModel fileModel = new FileModel { FileName = filePath };

                        fileModel.FileId = FileManager.GetId(fileModel);

                        if (fileModel.FileId == 0)
                        {
                            FileManager.InsertFile(fileModel);
                            fileModel.FileId = FileManager.GetId(fileModel);

                        }

                        InsertIntoFilesPerSearches(search, fileModel);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                foreach (string directory in Directory.GetDirectories(currentDirectory))
                {
                    numFilesFound += SearchDirectoryAndString(search, directory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return numFilesFound;

        }

        #endregion


        /// <summary>
        /// InsertIntoFilesPerSearches insert into DB SearchId and FileId  
        /// using a stored procedure that created in SQL
        /// </summary>
        ///<param name="search">An object from BOL that will be passed to DB</param>
        ///<param name="file">An object from BOL that will be passed to DB</param>
        #region private static void InsertIntoFilesPerSearches(SearchModel search, FileModel file) 

        private static void InsertIntoFilesPerSearches(SearchModel search, FileModel file)
        {
            try
            {
                using (FilesDBEntities ef = new FilesDBEntities())
                {

                    ef.InsertFilesSearches(search.SearchID, file.FileId);
                    ef.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

    }
}

