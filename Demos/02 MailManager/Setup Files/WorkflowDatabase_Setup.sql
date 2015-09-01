-- =============================================
-- Create database template
-- =============================================
USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'WorkflowStore'
)
DROP DATABASE WorkflowStore
GO

CREATE DATABASE WorkflowStore
GO