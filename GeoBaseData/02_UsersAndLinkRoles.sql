USE [GeoGrafija]
GO

-- Add Student user
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password]
           ,[Email]
           ,[ParentUserId]
           ,[HiddenLocationsFound]
           ,[CurrentRank]
           ,[OpenedResources]
           ,[OpenedLocationDetails]
           ,[TeacherClassroomDefinition])
     VALUES
           ('student'
           ,'developer'
           ,'student@geografija.students.mk'
           ,null
           ,0
           ,null
           ,0
           ,0
           ,null)
GO


-- Set student user role
INSERT INTO [dbo].[UserRoles]
           ([Roles_RoleID]
           ,[Users_UserID])
     VALUES
           ((Select RoleID From Roles Where RoleName = 'Student')
           ,(Select UserID From Users Where UserName = 'student'))
GO

-- = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
-- = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

-- Add Teacher user
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password]
           ,[Email]
           ,[ParentUserId]
           ,[HiddenLocationsFound]
           ,[CurrentRank]
           ,[OpenedResources]
           ,[OpenedLocationDetails]
           ,[TeacherClassroomDefinition])
     VALUES
           ('teacher'
           ,'developer'
           ,'teacher@geografija.students.mk'
           ,null
           ,0
           ,null
           ,0
           ,0
           ,null)
GO


-- Set teacher user role
INSERT INTO [dbo].[UserRoles]
           ([Roles_RoleID]
           ,[Users_UserID])
     VALUES
           ((Select RoleID From Roles Where RoleName = 'Teacher')
           ,(Select UserID From Users Where UserName = 'teacher'))
GO


-- = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
-- = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 

-- Add Admin user
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password]
           ,[Email]
           ,[ParentUserId]
           ,[HiddenLocationsFound]
           ,[CurrentRank]
           ,[OpenedResources]
           ,[OpenedLocationDetails]
           ,[TeacherClassroomDefinition])
     VALUES
           ('admin'
           ,'developer'
           ,'admin@geografija.students.mk'
           ,null
           ,0
           ,null
           ,0
           ,0
           ,null)
GO


-- Set admin user role
INSERT INTO [dbo].[UserRoles]
           ([Roles_RoleID]
           ,[Users_UserID])
     VALUES
           ((Select RoleID From Roles Where RoleName = 'Admin')
           ,(Select UserID From Users Where UserName = 'admin'))
GO










