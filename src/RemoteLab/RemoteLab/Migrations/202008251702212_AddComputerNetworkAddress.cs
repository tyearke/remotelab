namespace RemoteLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComputerNetworkAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Computers", "CustomNetworkAddress", c => c.String(maxLength: 255));

            // P_remotelabdb_startup
            Sql(@"ALTER PROCEDURE [dbo].[P_remotelabdb_startup] 
                	@computer varchar(50),
					@poolname varchar(50),
                    @customnetworkaddress varchar(255) = NULL
            AS
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;
				declare @now datetime
				set @now = getdate()
	
				-- check for existence of pool
				If exists(select * from Pools where PoolName = @poolname)
				Begin
					-- insert or update computer record 
					If exists(select * from Computers where ComputerName = @computer)
					Begin	
						Update Computers set 
						UserName= null,
						Logon = null,
						Reserved = null,
						Pool_PoolName = @poolname,
                        CustomNetworkAddress = @customnetworkaddress
						where ComputerName= @computer
					End
					Else 
					Begin					
						Insert into Computers (ComputerName, UserName, Reserved, Logon, Pool_PoolName, CustomNetworkAddress) values (@computer,null, null, null,@poolname, @customnetworkaddress)
					End

					-- log the event
					exec dbo.[P_remotelabdb_logevent] 'STARTUP','N/A', @computer, @poolname, @now 
				End
				Else -- Pool does not exist
				Begin
					exec dbo.[P_remotelabdb_logevent] 'INVALID POOL','N/A', @computer, @poolname, @now 
				End	
            END");
        }

        public override void Down()
        {
            // P_remotelabdb_startup
            Sql(@"ALTER PROCEDURE [dbo].[P_remotelabdb_startup] 
                	@computer varchar(50),
					@poolname varchar(50)
            AS
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;
				declare @now datetime
				set @now = getdate()
	
				-- check for existence of pool
				If exists(select * from Pools where PoolName = @poolname)
				Begin
					-- insert or update computer record 
					If exists(select * from Computers where ComputerName = @computer)
					Begin	
						Update Computers set 
						UserName= null,
						Logon = null,
						Reserved = null,
						Pool_PoolName = @poolname
						where ComputerName= @computer
					End
					Else 
					Begin					
						Insert into Computers (ComputerName, UserName, Reserved, Logon, Pool_PoolName) values (@computer,null, null, null,@poolname)
					End

					-- log the event
					exec dbo.[P_remotelabdb_logevent] 'STARTUP','N/A', @computer, @poolname, @now 
				End
				Else -- Pool does not exist
				Begin
					exec dbo.[P_remotelabdb_logevent] 'INVALID POOL','N/A', @computer, @poolname, @now 
				End	
            END");

            DropColumn("dbo.Computers", "CustomNetworkAddress");
        }
    }
}
