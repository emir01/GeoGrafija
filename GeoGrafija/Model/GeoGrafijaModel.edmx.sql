
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/29/2014 09:40:52
-- Generated from EDMX file: E:\Projects\Geografija\GeoGrafija\Model\GeoGrafijaModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GeoGrafija];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LocationTypes_LocationDisplaySettings]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocationTypes] DROP CONSTRAINT [FK_LocationTypes_LocationDisplaySettings];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePrivileges_Privilege]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePrivileges] DROP CONSTRAINT [FK_RolePrivileges_Privilege];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePrivileges_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePrivileges] DROP CONSTRAINT [FK_RolePrivileges_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_Locations_LocationDisplaySettings]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_Locations_LocationDisplaySettings];
GO
IF OBJECT_ID(N'[dbo].[FK_Locations_LocationTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_Locations_LocationTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_Locations_ParentLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_Locations_ParentLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_Rank_ParentRank]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ranks] DROP CONSTRAINT [FK_Rank_ParentRank];
GO
IF OBJECT_ID(N'[dbo].[FK_Locations_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locations] DROP CONSTRAINT [FK_Locations_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_LocationTypes_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LocationTypes] DROP CONSTRAINT [FK_LocationTypes_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Users1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Users1];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentQuizResults_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentQuizResults] DROP CONSTRAINT [FK_StudentQuizResults_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_TeacherParent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Student_TeacherParent];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoles_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRoles_User];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionAnswers_QuizQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Answers] DROP CONSTRAINT [FK_QuestionAnswers_QuizQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_QuizQuestions_Locations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_QuizQuestions_Locations];
GO
IF OBJECT_ID(N'[dbo].[FK_Quizes_Teacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Quizs] DROP CONSTRAINT [FK_Quizes_Teacher];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentQuizResults_Quizes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentQuizResults] DROP CONSTRAINT [FK_StudentQuizResults_Quizes];
GO
IF OBJECT_ID(N'[dbo].[FK_QuizQuestions_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuizQuestions] DROP CONSTRAINT [FK_QuizQuestions_Question];
GO
IF OBJECT_ID(N'[dbo].[FK_QuizQuestions_Quize]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuizQuestions] DROP CONSTRAINT [FK_QuizQuestions_Quize];
GO
IF OBJECT_ID(N'[dbo].[FK_Resources_Locations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_Resources_Locations];
GO
IF OBJECT_ID(N'[dbo].[FK_Resources_LocationTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_Resources_LocationTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_Resources_ResourceTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_Resources_ResourceTypes];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LocationDisplaySettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocationDisplaySettings];
GO
IF OBJECT_ID(N'[dbo].[LocationTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocationTypes];
GO
IF OBJECT_ID(N'[dbo].[Privileges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Privileges];
GO
IF OBJECT_ID(N'[dbo].[ResourceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResourceTypes];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[StudentQuizResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentQuizResults];
GO
IF OBJECT_ID(N'[dbo].[Ranks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ranks];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Answers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Answers];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[dbo].[Quizs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Quizs];
GO
IF OBJECT_ID(N'[dbo].[Resources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resources];
GO
IF OBJECT_ID(N'[dbo].[RolePrivileges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePrivileges];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[QuizQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuizQuestions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LocationDisplaySettings'
CREATE TABLE [dbo].[LocationDisplaySettings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [MapType] nvarchar(50)  NOT NULL,
    [Zoom] nvarchar(50)  NOT NULL,
    [RenderControls] bit  NOT NULL
);
GO

-- Creating table 'LocationTypes'
CREATE TABLE [dbo].[LocationTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(50)  NOT NULL,
    [TypeDescription] nvarchar(max)  NOT NULL,
    [DisplaySettings] int  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [Icon] nvarchar(50)  NOT NULL,
    [Color] nvarchar(10)  NULL
);
GO

-- Creating table 'Privileges'
CREATE TABLE [dbo].[Privileges] (
    [PrivilegeID] int IDENTITY(1,1) NOT NULL,
    [PrivilegeName] nvarchar(50)  NOT NULL,
    [PrivilegeDescription] nvarchar(200)  NULL
);
GO

-- Creating table 'ResourceTypes'
CREATE TABLE [dbo].[ResourceTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Color] nvarchar(12)  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [RoleDescription] nvarchar(200)  NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Lat] float  NOT NULL,
    [Lng] float  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [LocationType] int  NOT NULL,
    [DisplaySettings] int  NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'StudentQuizResults'
CREATE TABLE [dbo].[StudentQuizResults] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuizId] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [DateOpen] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL,
    [PointsTotal] int  NOT NULL,
    [PointsStudent] int  NOT NULL,
    [DetailResult] nvarchar(max)  NULL,
    [CorrectQuestions] int  NULL
);
GO

-- Creating table 'Ranks'
CREATE TABLE [dbo].[Ranks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RankName] nvarchar(100)  NOT NULL,
    [RankImage] nvarchar(400)  NOT NULL,
    [RequiredPoints] int  NOT NULL,
    [ParentRankId] int  NULL,
    [RankOrder] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(20)  NOT NULL,
    [Password] nvarchar(20)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [ParentUserId] int  NULL,
    [HiddenLocationsFound] int  NULL,
    [CurrentRank] int  NULL,
    [OpenedResources] int  NULL,
    [OpenedLocationDetails] int  NULL,
    [TeacherClassroomDefinition] nvarchar(max)  NULL
);
GO

-- Creating table 'Answers'
CREATE TABLE [dbo].[Answers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuestionId] int  NOT NULL,
    [AnswertText] nvarchar(max)  NOT NULL,
    [isCorrectAnswer] bit  NOT NULL,
    [uId] int  NOT NULL,
    [questionUId] int  NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuizId] int  NOT NULL,
    [LocationId] int  NULL,
    [QuestionText] nvarchar(max)  NOT NULL,
    [Points] int  NOT NULL,
    [uId] int  NOT NULL
);
GO

-- Creating table 'Quizs'
CREATE TABLE [dbo].[Quizs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [TypeId] int  NOT NULL,
    [TeacherId] int  NOT NULL,
    [IsRandom] bit  NULL
);
GO

-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TypeId] int  NOT NULL,
    [LocationId] int  NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [LocationTypeId] int  NULL
);
GO

-- Creating table 'RolePrivileges'
CREATE TABLE [dbo].[RolePrivileges] (
    [Privileges_PrivilegeID] int  NOT NULL,
    [Roles_RoleID] int  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Roles_RoleID] int  NOT NULL,
    [Users_UserID] int  NOT NULL
);
GO

-- Creating table 'QuizQuestions'
CREATE TABLE [dbo].[QuizQuestions] (
    [Questions_Id] int  NOT NULL,
    [Quizes_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'LocationDisplaySettings'
ALTER TABLE [dbo].[LocationDisplaySettings]
ADD CONSTRAINT [PK_LocationDisplaySettings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'LocationTypes'
ALTER TABLE [dbo].[LocationTypes]
ADD CONSTRAINT [PK_LocationTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [PrivilegeID] in table 'Privileges'
ALTER TABLE [dbo].[Privileges]
ADD CONSTRAINT [PK_Privileges]
    PRIMARY KEY CLUSTERED ([PrivilegeID] ASC);
GO

-- Creating primary key on [ID] in table 'ResourceTypes'
ALTER TABLE [dbo].[ResourceTypes]
ADD CONSTRAINT [PK_ResourceTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [RoleID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleID] ASC);
GO

-- Creating primary key on [ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'StudentQuizResults'
ALTER TABLE [dbo].[StudentQuizResults]
ADD CONSTRAINT [PK_StudentQuizResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ranks'
ALTER TABLE [dbo].[Ranks]
ADD CONSTRAINT [PK_Ranks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [Id] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [PK_Answers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Quizs'
ALTER TABLE [dbo].[Quizs]
ADD CONSTRAINT [PK_Quizs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [PK_Resources]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Privileges_PrivilegeID], [Roles_RoleID] in table 'RolePrivileges'
ALTER TABLE [dbo].[RolePrivileges]
ADD CONSTRAINT [PK_RolePrivileges]
    PRIMARY KEY CLUSTERED ([Privileges_PrivilegeID], [Roles_RoleID] ASC);
GO

-- Creating primary key on [Roles_RoleID], [Users_UserID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Roles_RoleID], [Users_UserID] ASC);
GO

-- Creating primary key on [Questions_Id], [Quizes_Id] in table 'QuizQuestions'
ALTER TABLE [dbo].[QuizQuestions]
ADD CONSTRAINT [PK_QuizQuestions]
    PRIMARY KEY CLUSTERED ([Questions_Id], [Quizes_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DisplaySettings] in table 'LocationTypes'
ALTER TABLE [dbo].[LocationTypes]
ADD CONSTRAINT [FK_LocationTypes_LocationDisplaySettings]
    FOREIGN KEY ([DisplaySettings])
    REFERENCES [dbo].[LocationDisplaySettings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocationTypes_LocationDisplaySettings'
CREATE INDEX [IX_FK_LocationTypes_LocationDisplaySettings]
ON [dbo].[LocationTypes]
    ([DisplaySettings]);
GO

-- Creating foreign key on [Privileges_PrivilegeID] in table 'RolePrivileges'
ALTER TABLE [dbo].[RolePrivileges]
ADD CONSTRAINT [FK_RolePrivileges_Privilege]
    FOREIGN KEY ([Privileges_PrivilegeID])
    REFERENCES [dbo].[Privileges]
        ([PrivilegeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Roles_RoleID] in table 'RolePrivileges'
ALTER TABLE [dbo].[RolePrivileges]
ADD CONSTRAINT [FK_RolePrivileges_Role]
    FOREIGN KEY ([Roles_RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePrivileges_Role'
CREATE INDEX [IX_FK_RolePrivileges_Role]
ON [dbo].[RolePrivileges]
    ([Roles_RoleID]);
GO

-- Creating foreign key on [DisplaySettings] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [FK_Locations_LocationDisplaySettings]
    FOREIGN KEY ([DisplaySettings])
    REFERENCES [dbo].[LocationDisplaySettings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Locations_LocationDisplaySettings'
CREATE INDEX [IX_FK_Locations_LocationDisplaySettings]
ON [dbo].[Locations]
    ([DisplaySettings]);
GO

-- Creating foreign key on [LocationType] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [FK_Locations_LocationTypes]
    FOREIGN KEY ([LocationType])
    REFERENCES [dbo].[LocationTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Locations_LocationTypes'
CREATE INDEX [IX_FK_Locations_LocationTypes]
ON [dbo].[Locations]
    ([LocationType]);
GO

-- Creating foreign key on [ParentId] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [FK_Locations_ParentLocation]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Locations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Locations_ParentLocation'
CREATE INDEX [IX_FK_Locations_ParentLocation]
ON [dbo].[Locations]
    ([ParentId]);
GO

-- Creating foreign key on [ParentRankId] in table 'Ranks'
ALTER TABLE [dbo].[Ranks]
ADD CONSTRAINT [FK_Rank_ParentRank]
    FOREIGN KEY ([ParentRankId])
    REFERENCES [dbo].[Ranks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Rank_ParentRank'
CREATE INDEX [IX_FK_Rank_ParentRank]
ON [dbo].[Ranks]
    ([ParentRankId]);
GO

-- Creating foreign key on [CreatedBy] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [FK_Locations_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Locations_Users'
CREATE INDEX [IX_FK_Locations_Users]
ON [dbo].[Locations]
    ([CreatedBy]);
GO

-- Creating foreign key on [CreatedBy] in table 'LocationTypes'
ALTER TABLE [dbo].[LocationTypes]
ADD CONSTRAINT [FK_LocationTypes_Users]
    FOREIGN KEY ([CreatedBy])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocationTypes_Users'
CREATE INDEX [IX_FK_LocationTypes_Users]
ON [dbo].[LocationTypes]
    ([CreatedBy]);
GO

-- Creating foreign key on [CurrentRank] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_Rank]
    FOREIGN KEY ([CurrentRank])
    REFERENCES [dbo].[Ranks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Rank'
CREATE INDEX [IX_FK_User_Rank]
ON [dbo].[Users]
    ([CurrentRank]);
GO

-- Creating foreign key on [StudentId] in table 'StudentQuizResults'
ALTER TABLE [dbo].[StudentQuizResults]
ADD CONSTRAINT [FK_StudentQuizResults_Student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentQuizResults_Student'
CREATE INDEX [IX_FK_StudentQuizResults_Student]
ON [dbo].[StudentQuizResults]
    ([StudentId]);
GO

-- Creating foreign key on [ParentUserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_Student_TeacherParent]
    FOREIGN KEY ([ParentUserId])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_TeacherParent'
CREATE INDEX [IX_FK_Student_TeacherParent]
ON [dbo].[Users]
    ([ParentUserId]);
GO

-- Creating foreign key on [Roles_RoleID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_Role]
    FOREIGN KEY ([Roles_RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_UserID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserRoles_User]
    FOREIGN KEY ([Users_UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoles_User'
CREATE INDEX [IX_FK_UserRoles_User]
ON [dbo].[UserRoles]
    ([Users_UserID]);
GO

-- Creating foreign key on [QuestionId] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [FK_QuestionAnswers_QuizQuestions]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionAnswers_QuizQuestions'
CREATE INDEX [IX_FK_QuestionAnswers_QuizQuestions]
ON [dbo].[Answers]
    ([QuestionId]);
GO

-- Creating foreign key on [LocationId] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_QuizQuestions_Locations]
    FOREIGN KEY ([LocationId])
    REFERENCES [dbo].[Locations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuizQuestions_Locations'
CREATE INDEX [IX_FK_QuizQuestions_Locations]
ON [dbo].[Questions]
    ([LocationId]);
GO

-- Creating foreign key on [TeacherId] in table 'Quizs'
ALTER TABLE [dbo].[Quizs]
ADD CONSTRAINT [FK_Quizes_Teacher]
    FOREIGN KEY ([TeacherId])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Quizes_Teacher'
CREATE INDEX [IX_FK_Quizes_Teacher]
ON [dbo].[Quizs]
    ([TeacherId]);
GO

-- Creating foreign key on [QuizId] in table 'StudentQuizResults'
ALTER TABLE [dbo].[StudentQuizResults]
ADD CONSTRAINT [FK_StudentQuizResults_Quizes]
    FOREIGN KEY ([QuizId])
    REFERENCES [dbo].[Quizs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentQuizResults_Quizes'
CREATE INDEX [IX_FK_StudentQuizResults_Quizes]
ON [dbo].[StudentQuizResults]
    ([QuizId]);
GO

-- Creating foreign key on [Questions_Id] in table 'QuizQuestions'
ALTER TABLE [dbo].[QuizQuestions]
ADD CONSTRAINT [FK_QuizQuestions_Question]
    FOREIGN KEY ([Questions_Id])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Quizes_Id] in table 'QuizQuestions'
ALTER TABLE [dbo].[QuizQuestions]
ADD CONSTRAINT [FK_QuizQuestions_Quize]
    FOREIGN KEY ([Quizes_Id])
    REFERENCES [dbo].[Quizs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuizQuestions_Quize'
CREATE INDEX [IX_FK_QuizQuestions_Quize]
ON [dbo].[QuizQuestions]
    ([Quizes_Id]);
GO

-- Creating foreign key on [LocationId] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_Resources_Locations]
    FOREIGN KEY ([LocationId])
    REFERENCES [dbo].[Locations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Resources_Locations'
CREATE INDEX [IX_FK_Resources_Locations]
ON [dbo].[Resources]
    ([LocationId]);
GO

-- Creating foreign key on [LocationTypeId] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_Resources_LocationTypes]
    FOREIGN KEY ([LocationTypeId])
    REFERENCES [dbo].[LocationTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Resources_LocationTypes'
CREATE INDEX [IX_FK_Resources_LocationTypes]
ON [dbo].[Resources]
    ([LocationTypeId]);
GO

-- Creating foreign key on [TypeId] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_Resources_ResourceTypes]
    FOREIGN KEY ([TypeId])
    REFERENCES [dbo].[ResourceTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Resources_ResourceTypes'
CREATE INDEX [IX_FK_Resources_ResourceTypes]
ON [dbo].[Resources]
    ([TypeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------