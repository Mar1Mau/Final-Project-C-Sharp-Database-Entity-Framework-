# This is my third project during the training period
----
### Subjects:

  - OOP
  - Database
  - Entity Framework
  - N-Tier Model
  
  #### Programming language: C#
  <br>
  

  # An objective was: 

  - Create software to search for files on the computer and store them in a database.
  - The software should get a string from user to search for files and display found files on the screen while the search itself.
  - At the end of each search, the software must keep the search string in the database, the folder in which the user wants to search and the search results.

  ---
<br>
<br>

# Installation

## To create tables please use this script:

Table #1: **Searches**
```SQL
CREATE TABLE Searches
(
     SearchID int primary key identity,
     SearchName nvarchar(50) not null,
	 DirectoryName nvarchar(50)
)
GO
```

Table #2: **Files**
```SQL
CREATE TABLE Files
(
     FileId int primary key identity,
     FileName nvarchar(MAX)
)
GO
```

Bridge Table: **FilesPerSearches**
```SQL
CREATE TABLE FilesPerSearches
(
     SearchId int not null foreign key references Searches(SearchID),
     FileId int not null foreign key references Files(FileId)
)
GO
```

### Create a primary keys for the bridge table

```SQL
ALTER TABLE FilesPerSearches
Add Constraint PK_FilesSearches
Primary Key Clustered (SearchId, FileId)
```

## To create stored procedures please use this script:

SP #1: **InsertSearches**
```SQL
GO
CREATE PROCEDURE InsertSearches (@SearchName nvarchar(50),@DirectoryName nvarchar(50))
AS
INSERT INTO Searches(SearchName,DirectoryName) 
VALUES(@SearchName, @DirectoryName)
```

SP #2: **InsertFiles**
```SQL
GO
CREATE PROCEDURE InsertFiles(@FileName nvarchar(MAX))
AS
INSERT INTO Files(FileName)  
VALUES(@FileName)
```

SP #3: **InsertFilesSearches**
```SQL
GO
CREATE PROCEDURE InsertFilesSearches (@SearchID int, @FileId int)
AS
if not exists (select SearchID,FileId from FilesPerSearches
			  where SearchID = @SearchID and FileId = @FileId )
	BEGIN
		INSERT INTO FilesPerSearches(SearchID,FileId)  
        VALUES(@SearchId,@FileId)
	END
```