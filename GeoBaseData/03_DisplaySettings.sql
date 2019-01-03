USE [GeoGrafija]
GO

INSERT INTO [dbo].[LocationDisplaySettings]
           ([Name]
           ,[MapType]
           ,[Zoom]
           ,[RenderControls])
     VALUES
           ('ROADMAP'
           ,'ROADMAP'
           ,5
           ,1)
GO

INSERT INTO [dbo].[LocationDisplaySettings]
           ([Name]
           ,[MapType]
           ,[Zoom]
           ,[RenderControls])
     VALUES
           ('SATELLITE'
           ,'SATELLITE'
           ,5
           ,1)
GO

INSERT INTO [dbo].[LocationDisplaySettings]
           ([Name]
           ,[MapType]
           ,[Zoom]
           ,[RenderControls])
     VALUES
           ('HYBRID'
           ,'HYBRID'
           ,5
           ,1)
GO

INSERT INTO [dbo].[LocationDisplaySettings]
           ([Name]
           ,[MapType]
           ,[Zoom]
           ,[RenderControls])
     VALUES
           ('TERRAIN'
           ,'TERRAIN'
           ,5
           ,1)
GO
